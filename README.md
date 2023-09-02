# Randomaze v 5.0

ES: Randomaze – The maps generator

#

**ES: Randomaze** is the maps generator for the [mod with the same name](https://moddb.com/mods/esrm).
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

- `-go` (*legacy*): aborts start of the game (only map building will be performed).
- `-n` (*legacy*): forces the app to generate the next map even if the required map already exists.
- `-x <map_name>`: compares specified map (currently saved map expected) with the last created one. Also enables `-go` and `-n` keys.
- `-s <property_alias> <property_value>`: sets the specified property to the value without starting the interface.
- `-r`: requests the immediate recompilation of the next map (after the saved one). Corresponds to `esrm_rebuild` command in the game console.

### From the settings window and game console

*Random flag* (here and below) initiates the random selection.

| Property | Description | Range | Supports random flag | Alias | Game console command |
|-|-|-|-|-|-|
| Maze size coefficient | Sets the size of the map | 1 – 8 | No | `MS` | `esrm_size` |
| Enemies density coefficient | Sets the enemies density | 1 – 8 | Yes | `DF` | `esrm_enemies` |
| Items density coefficient | Affects quantity of collectable items / weapons | 1 – 8 | Yes | `ID` | `esrm_items` |
| Walls density coefficient | The larger the value, the fewer “branches” the maze will have | 1 – 12 | No | `WD` | `esrm_walls` |
| Crates density coefficient | Describes, how much enemies will be replaced with crates with bugs or explosives | 1 – 5 | Yes | `CD` | `esrm_crates` |
| Lighting coefficient | Affects quantity of enabled lights and the type of the sky | 1 – 6 | Yes | `LG` | `esrm_light` |
| Gravity coefficient | Affects the gravity percentage for all objects on the map | 1 – 20 (10 = 100%) | Yes | `GR` | `esrm_gravity` |
| Map section types | Sets allowed types of map sections (*all*, *only under sky* or *only inside*) | 0 / 1 / 2 | No | `ST` | `esrm_sections` |
| Enemies permissions line | Consists of flags of monsters that will be allowed on maps | (string value) | No | (not implemented) | (not implemented) |
| Items permissions line | Consists of flags of weapons and items that will be allowed on maps (both on floors and in crates) | (string value) | No | (not implemented) | (not implemented) |
| Button mode flag | Adds the button that should be found and pressed to open the exit gate | 0 / 1 | No | `BM` | `esrm_button` |
| Allow monster makers flag | Allows replacement of some monsters with monster makers | 0 / 1 | No | `MM` | `esrm_makers` |
| Two floors flag | Adds the second floor to the map with some balconies and enemies | 0 / 1 | Yes | `TF` | `esrm_two_floors` |
| Allow items on the second floor | Permit the generation of items for balconies | 0 / 1 | Yes | `TF` | `esrm_two_floors` |
| Allow explosive crates | Enables explosive crates | 0 / 1 | No | `XC` | `esrm_expl_crates` |
| Allow crates with items | Enables crates with items and bugs | 0 / 1 | No | `IC` | `esrm_item_crates` |

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
