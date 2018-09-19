# Interaction Bubbles
![](https://img.shields.io/badge/Mod_Version-1.0.0-blue.svg)
![](https://img.shields.io/badge/Built_for_RimWorld-B19-blue.svg)
![](https://img.shields.io/badge/Powered_by_Harmony-1.2.0.1-blue.svg)

---

Shows bubbles when pawns perform a social interaction with the text that would normally only be found in the log.

Bubbles will fade away after a short time but they are linked to the game time so pausing the game will halt the bubble from fading. Hovering over a bubble will make it nearly transparent and they can be clicked through to objects underneath. There is a toggle button in the play settings area to disable bubbles from being shown.

---

##### INSTALLATION
- **[Download the latest release](https://github.com/Jaxe-Dev/Bubbles/releases/latest) and unzip it into your *RimWorld/Mods* folder.**

---

The following base methods are patched with Harmony:
```
Postfix : RimWorld.PlaySettings.DoPlaySettingsGlobalControls
Postfix : RimWorld.MapInterface.MapInterfaceOnGUI_BeforeMainTabs
Postfix : Verse.PlayLog.Add
Prefix  : Verse.Profile.MemoryUtility.ClearAllMapsAndWorld
```

