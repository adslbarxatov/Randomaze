# Randomaze v 3.0

ES: Randomaze – The maps generator

#

**ES: Randomaze** is the maps generator for the sub-mod with the same name.
It is the child project of the [ESHQ mod](https://moddb.com/mods/eshq) for Half-Life part 1.

Modification uses its own fork of [Xash3d engine](https://github.com/adslbarxatov/xash3d-for-ESHQ).
This tool will be started by the engine automatically, every time it will need a new map.
It may be started directly: after building of the map it will start the game.

&nbsp;



## Environment

The tool requires next compilation tools, directories and files for work:
- `.\hlcsg.exe`;
- `.\hlbsp.exe`;
- `.\hlvis.exe`;
- `.\rlrad.exe`;
- `.\maps` (directory);
- `..\valve\eshq.wad`.

&nbsp;



## Available settings

### From the command line

- `-go`: aborts start of the game (only map building will be performed).
- `-m <map_name>`: compares specified map (currently saved map expected) with the last created.
- `-n`: forces the app to generate the next map even if the required map already exists.

### From the settings window

- ***Maze size coefficient***. Range: `1 – 8`. `Random` flag (here and below) initiates the random selection.
- ***Enemies density coefficient***. Range: `1 – 8`. Affects quantity of enemies and crates on the map.
- ***Items density coefficient***. Range: `1 – 8`. Affects quantity of collectable items / weapons on the map.
- ***Walls density coefficient***. Range: `1 – 12`. The larger the value, the fewer “branches” the maze will have.
  `12` means almost a snake-shaped corridor, `1` is for a hangar with a few walls.
- ***Crates density coefficient***. Range: `1 – 5`. Describes, how much enemies will be replaced with crates with bugs or explosives.
- ***Enemies permissions line***. Consists of flags of monsters that will be allowed on maps.
- ***Button mode flag***. Adds the button to the map. This button must be found and pressed to open the exit gate.

&nbsp;



## Requirements

- Windows XP or newer;
- Installed [ESHQ mod](https://moddb.com/mods/eshq);
- [Microsoft .NET Framework 4.0](https://microsoft.com/en-us/download/details.aspx?id=17718).

Interface languages: en_us.

&nbsp;



## [Development policy and EULA](https://adslbarxatov.github.io/ADP)

This Policy (ADP), its positions, conclusion, EULA and application methods
describes general rules that we follow in all of our development processes, released applications and implemented ideas.
***It must be acquainted by participants and users before using any of laboratory’s products.
By downloading them, you agree and accept this Policy!***
