using p5rpc.inputhook.interfaces;
using p5rpc.uitoggler.Configuration;
using p5rpc.uitoggler.Template;
using Reloaded.Hooks.Definitions;
using Reloaded.Hooks.Definitions.Enums;
using Reloaded.Hooks.ReloadedII.Interfaces;
using Reloaded.Memory.SigScan.ReloadedII.Interfaces;
using Reloaded.Memory.Sources;
using Reloaded.Mod.Interfaces;
using static p5rpc.inputhook.interfaces.Inputs;
using IReloadedHooks = Reloaded.Hooks.ReloadedII.Interfaces.IReloadedHooks;

namespace p5rpc.uitoggler
{
    /// <summary>
    /// Your mod logic goes here.
    /// </summary>
    public class Mod : ModBase // <= Do not Remove.
    {
        /// <summary>
        /// Provides access to the mod loader API.
        /// </summary>
        private readonly IModLoader _modLoader;

        /// <summary>
        /// Provides access to the Reloaded.Hooks API.
        /// </summary>
        /// <remarks>This is null if you remove dependency on Reloaded.SharedLib.Hooks in your mod.</remarks>
        private readonly IReloadedHooks? _hooks;

        /// <summary>
        /// Provides access to the Reloaded logger.
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Entry point into the mod, instance that created this class.
        /// </summary>
        private readonly IMod _owner;

        /// <summary>
        /// Provides access to this mod's configuration.
        /// </summary>
        private Config _configuration;

        /// <summary>
        /// The configuration of the currently executing mod.
        /// </summary>
        private readonly IModConfig _modConfig;

        private IMemory _memory;

        private nuint _shouldHidePtr;

        private bool _shouldHide;

        private IAsmHook _renderHudHook;

        private IInputHook _inputHook;

        private Dictionary<string, IAsmHook> _uiHooks = new();

        private List<IAsmHook> _buttonPromptHooks = new();

        public Mod(ModContext context)
        {
            _modLoader = context.ModLoader;
            _hooks = context.Hooks;
            _logger = context.Logger;
            _owner = context.Owner;
            _configuration = context.Configuration;
            _modConfig = context.ModConfig;


            // For more information about this template, please see
            // https://reloaded-project.github.io/Reloaded-II/ModTemplate/

            // If you want to implement e.g. unload support in your mod,
            // and some other neat features, override the methods in ModBase.

            // TODO: Implement some mod logic
            Utils.Initialise(_logger, _configuration);

            var inputController = _modLoader.GetController<IInputHook>();
            if (inputController == null || !inputController.TryGetTarget(out _inputHook))
            {
                Utils.LogError("Unable to access input hooks, please make sure you have p5rpc.inputhooks installed. Aborting initialisation");
                return;
            }

            var startupScannerController = _modLoader.GetController<IStartupScanner>();
            if (startupScannerController == null || !startupScannerController.TryGetTarget(out var startupScanner))
            {
                Utils.LogError("Unable to access startup scanner, please make sure you have Reloaded.Memory.Sigscan installed. Aborting initialisation");
                return;
            }

            _memory = new Memory();
            _shouldHidePtr = _memory.Allocate(1);
            Utils.LogDebug($"Allocated should hide to 0x{_shouldHidePtr:X}");

            startupScanner.AddMainModuleScan("48 83 EC 28 B9 0C 00 00 00", result =>
            {
                if (!result.Found)
                {
                    Utils.LogError("Unable to find render hud function, toggling will not work");
                    return;
                }
                Utils.LogDebug($"Found render hud function at 0x{result.Offset + Utils.BaseAddress:X}");
                string[] function =
                {
                    "use64",
                    "push rax",
                    $"lea rax, [qword 0x{_shouldHidePtr:X}]",
                    $"cmp byte [rax], 0",
                    "pop rax",
                    "je endHook",
                    "ret",
                    "label endHook"
                };
                _renderHudHook = _hooks.CreateAsmHook(function, result.Offset + Utils.BaseAddress, AsmHookBehaviour.ExecuteFirst).Activate();
            });

            // Hooks for individual things
            foreach (var sig in Signatures.FunctionSigs)
            {
                startupScanner.AddMainModuleScan(sig.Signature, result =>
                {
                    if (!result.Found)
                    {
                        Utils.LogError($"Unable to find code for {sig.Name}");
                        return;
                    }
                    Utils.LogDebug($"Found {sig.Name} at 0x{result.Offset + Utils.BaseAddress + sig.Offset:X}");
                    string[] function =
                    {
                        "use64",
                        $"{sig.OriginalCode}",
                    };
                    var hook = _hooks.CreateAsmHook(function, result.Offset + Utils.BaseAddress + sig.Offset, AsmHookBehaviour.DoNotExecuteOriginal).Activate();
                    var property = typeof(Config).GetProperty($"Hide{sig.Name}");
                    // Disable the hookPair if the config for it isn't enabled
                    if (property != null && property.GetValue(_configuration) is bool value && !value)
                        hook.Disable();
                    _uiHooks.Add(sig.Name, hook);
                });
            }

            // Hooks for inline stuff (just button prompts rn)
            foreach (var sig in Signatures.ButtonPromptSigs)
            {
                startupScanner.AddMainModuleScan(sig.Signature, result =>
                {
                    if (!result.Found)
                    {
                        Utils.LogError($"Unable to find code for {sig.Name}");
                        return;
                    }
                    Utils.LogDebug($"Found {sig.Name} at 0x{result.Offset + Utils.BaseAddress + sig.Offset:X}");
                    string[] function =
{
                        "use64",
                        $"{sig.OriginalCode}",
                    };
                    var hook = _hooks.CreateAsmHook(function, result.Offset + Utils.BaseAddress + sig.Offset, AsmHookBehaviour.DoNotExecuteOriginal).Activate();
                    if (!_configuration.HideButtonPrompts)
                        hook.Disable();
                    _buttonPromptHooks.Add(hook);
                });
            }

            _inputHook.OnInput += OnInput;
        }

        private void OnInput(List<Key> inputs)
        {
            if (inputs.Contains(_configuration.ToggleButton))
            {
                _shouldHide = !_shouldHide;
                Utils.LogDebug($"Turning UI {(_shouldHide ? "off" : "on")}");
                _memory.Write(_shouldHidePtr, _shouldHide);
            }
        }

        #region Standard Overrides
        public override void ConfigurationUpdated(Config configuration)
        {
            // Enable or disable normal hooks
            foreach (var hookPair in _uiHooks)
            {
                var name = hookPair.Key;
                var hook = hookPair.Value;
                var property = typeof(Config).GetProperty($"Hide{name}");
                if (property != null && property.GetValue(configuration) is bool enabled)
                {
                    if (enabled)
                        hook.Enable();
                    else
                        hook.Disable();
                }
            }

            // Enable or disable button prompt hooks
            if (_configuration.HideButtonPrompts != configuration.HideButtonPrompts)
            {
                foreach (var hook in _buttonPromptHooks)
                {
                    if (configuration.HideButtonPrompts)
                        hook.Enable();
                    else
                        hook.Disable();
                }
            }

            _configuration = configuration;
            _logger.WriteLine($"[{_modConfig.ModId}] Config Updated: Applying");
        }
        #endregion

        #region For Exports, Serialization etc.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null enabled when exiting constructor. Consider declaring as nullable.
        public Mod() { }
#pragma warning restore CS8618
        #endregion
    }
}