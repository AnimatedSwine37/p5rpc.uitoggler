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
            // Apply settings from configuration.
            // ... your code here.
            _configuration = configuration;
            _logger.WriteLine($"[{_modConfig.ModId}] Config Updated: Applying");
        }
        #endregion

        #region For Exports, Serialization etc.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public Mod() { }
#pragma warning restore CS8618
        #endregion
    }
}