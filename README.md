# p5rpc.uitoggler

Makes it so you can toggle the UI with the press of a button. By default the button to do this is F2 but this can be configured to any button on a standard keyboard in the mod's configuration in Reloaded. (This mod is also on [GameBanana](https://gamebanana.com/mods/412136))

This makes use of my [P5R Input Hook](https://github.com/AnimatedSwine37/p5rpc.inputhook) library which should automatically install when you install this mod. If you're interested in making mods that use of or alter the game's inputs check it out.
Because of this only keyboards are supported currently and only press to toggle, not hold to hide. When/if I update the input library to support these behaviours I'll also update this mod to do so.

## Caveats (Please Read!)
When the UI is hidden some things in the game will not advance such as dialogue and menus. Temporarily turning the UI back on will allow things to advance correctly so if your game appears to be frozen try turning the UI back on and it should fix the problem. I may work on changing this in the future (in particular for event dialogue) although for now, it should be easy enough to live with.

Also, toggling the UI when in menus can cause some visual oddities like blank screens, I don't plan on changing this since that's sort of what you should expect from hiding a menu when you're in it.

This mod toggles the UI almost everywhere, one exception that I have found is the battle UI. Although later on, I might add this. The reason for it being missing is that it is rendered from a different function (that I haven't looked into yet) compared to almost every other UI element.
If you find anything else that isn't toggled please let me know and I may add it later as well.
