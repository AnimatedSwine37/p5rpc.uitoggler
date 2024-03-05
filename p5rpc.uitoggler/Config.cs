using p5rpc.uitoggler.Template.Configuration;
using System.ComponentModel;
using static p5rpc.inputhook.interfaces.Inputs;

namespace p5rpc.uitoggler.Configuration
{
    public class Config : Configurable<Config>
    {
        /*
            User Properties:
                - Please put all of your configurable properties here.

            By default, configuration saves as "Config.json" in mod user config folder.    
            Need more config files/classes? See Configuration.cs

            Available Attributes:
            - Category
            - DisplayName
            - Description
            - DefaultValue

            // Technically Supported but not Useful
            - Browsable
            - Localizable

            The `DefaultValue` attribute is used as part of the `Reset` button in Reloaded-Launcher.
        */

        [DisplayName("Toggle All Button")]
        [Description("The button that you will press to toggle the entire UI")]
        [DefaultValue(Key.F2)]
        public Key ToggleAllButton { get; set; } = Key.F2;

        [DisplayName("Toggle Selected Button")]
        [Description("The button that you will press to toggle any options that are set to \"HideSelectively\"")]
        [DefaultValue(Key.F3)]
        public Key ToggleSelectiveButton { get; set; } = Key.F3;

        [DisplayName("Map")]
        [Description("Whether the map will be hidden")]
        [DefaultValue(HideType.Shown)]
        public HideType MapHide { get; set; } = HideType.Shown;

        [DisplayName("Objective")]
        [Description("Whether the current objective will be hidden")]
        [DefaultValue(HideType.Shown)]
        public HideType ObjectiveHide { get; set; } = HideType.Shown;

        [DisplayName("Message Cursor")]
        [Description("Whether the vibing cursor to the right of messages will be hidden")]
        [DefaultValue(HideType.Shown)]
        public HideType CursorHide { get; set; } = HideType.Shown;

        [DisplayName("Date")]
        [Description("Whether the date, weather, etc on the top left will be hidden")]
        [DefaultValue(HideType.Shown)]
        public HideType DateHide { get; set; } = HideType.Shown;

        [DisplayName("Button Prompts")]
        [Description("Whether the button prompts at the bottom of the screen will be hidden")]
        [DefaultValue(HideType.Shown)]
        public HideType ButtonPromptsHide { get; set; } = HideType.Shown;

        [DisplayName("Debug Mode")]
        [Description("Logs additional information to the console that is useful for debugging.")]
        public bool DebugEnabled { get; set; } = false;

        public enum HideType
        {
            Shown, 
            HideSelectively,
            HideAlways
        }

    }

    /// <summary>
    /// Allows you to override certain aspects of the configuration creation process (e.g. create multiple configurations).
    /// Override elements in <see cref="ConfiguratorMixinBase"/> for finer control.
    /// </summary>
    public class ConfiguratorMixin : ConfiguratorMixinBase
    {
        // 
    }
}