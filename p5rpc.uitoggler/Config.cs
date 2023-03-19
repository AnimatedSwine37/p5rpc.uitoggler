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

        [DisplayName("Toggle Button")]
        [Description("The button that you will press to toggle the UI")]
        [DefaultValue(Key.F2)]
        public Key ToggleButton { get; set; } = Key.F2;

        [DisplayName("Hide Map")]
        [Description("If enabled the map will be hidden")]
        [DefaultValue(false)]
        public bool HideMap { get; set; } = false;

        [DisplayName("Hide Objective")]
        [Description("If enabled the current objective will be hidden")]
        [DefaultValue(false)]
        public bool HideObjective { get; set; } = false;

        [DisplayName("Hide Cursor")]
        [Description("If enabled the vibing cursor to the right of messages will be hidden")]
        [DefaultValue(false)]
        public bool HideCursor { get; set; } = false;

        [DisplayName("Hide Date")]
        [Description("If enabled the date, weather, etc on the top left will be hidden")]
        [DefaultValue(false)]
        public bool HideDate { get; set; } = false;

        [DisplayName("Hide Button Prompts")]
        [Description("If enabled button prompts at the bottom of the screen will be hidden")]
        [DefaultValue(false)]
        public bool HideButtonPrompts { get; set; } = false;

        [DisplayName("Debug Mode")]
        [Description("Logs additional information to the console that is useful for debugging.")]
        public bool DebugEnabled { get; set; } = false;

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