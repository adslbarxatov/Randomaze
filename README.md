# Randomaze v 4.10.2

ES: Randomaze – The maps generator

#

**ES: Randomaze** is the maps generator for the mod with the same name.
It was the child project of the [ESHQ mod](https://moddb.com/mods/eshq) for Half-Life part 1.
Now it is stand-alone project

This tool will be started by the game engine automatically, every time it will need a new map

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

- `-go` [*legacy*]: aborts start of the game (only map building will be performed).
- `-n` [*legacy*]: forces the app to generate the next map even if the required map already exists.
- `-x <map_name>`: compares specified map (currently saved map expected) with the last created one. Also enables `-go` and `-n` keys.

### From the settings window

- ***Maze size coefficient***. Range: `1 – 8`. `Random` flag (here and below) initiates the random selection.
- ***Enemies density coefficient***. Range: `1 – 8`. Affects quantity of enemies and crates on the map.
- ***Items density coefficient***. Range: `1 – 8`. Affects quantity of collectable items / weapons on the map.
- ***Walls density coefficient***. Range: `1 – 12`. The larger the value, the fewer “branches” the maze will have.
  `12` means almost a snake-shaped corridor, `1` is for a hangar with a few walls.
- ***Crates density coefficient***. Range: `1 – 5`. Describes, how much enemies will be replaced with crates with bugs or explosives.
- ***Lighting coefficient***. Range: `1 – 5`. Affects quantity of enabled lights and the type of the sky.
- ***Gravity coefficient***. Range: `10 – 200%`. Affects the gravity percentage for all objects on the map.
- ***Map section types***. Supports “all types”, “only under sky” and “only inside the room” variants.
- ***Enemies permissions line***. Consists of flags of monsters that will be allowed on maps.
- ***Items permissions line***. Consists of flags of weapons and items that will be allowed on maps (both on floors and in crates).
- ***Button mode flag***. Adds the button to the map. This button must be found and pressed to open the exit gate.
- ***Allow monster makers flag***. Allows replacement of some monsters with monster makers on the map. Their triggering and spreading will also be random.
- ***Two floors flag***. Adds the second floor to the map with some balconies and enemies. Also makes barnacles available.
- ***Allow items on the second floor***. Permit the generation of items for balconies.
- ***Allow explosive crates*** and ***Allow crates with items***. Enable or disable corresponding types of crates.

&nbsp;



## Requirements

- Windows 7 or newer;
- Installed [ESRM mod](https://moddb.com/mods/esrm);
- [Microsoft .NET Framework 4.8](https://go.microsoft.com/fwlink/?linkid=2088631).

Interface languages: en_us.

&nbsp;



## [Development policy and EULA](https://adslbarxatov.github.io/ADP)

This Policy (ADP), its positions, conclusion, EULA and application methods
describes general rules that we follow in all of our development processes, released applications and implemented ideas.
***It must be acquainted by participants and users before using any of laboratory’s products.
By downloading them, you agree and accept this Policy!***
