# ER: Randomaze: user guide
> **ƒ** &nbsp;RD AAOW FDL; 9.12.2023; 19:10



### Page contents

- [General information](#general-information)
- [Environment](#environment)
- [Available settings](#available-settings)
- [Download links](https://moddb.com/mods/esrm)

---

### General information

**ES: Randomaze** is the maps generator for the [mod with the same name](https://moddb.com/mods/esrm).
It was the child project of the [ESHQ mod](https://moddb.com/mods/eshq) for Half-Life part 1.
Now it is stand-alone project.

This tool will be started by the game engine automatically, every time it will need a new map.

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

- `-x <map_name>`: compares specified map (currently saved map expected) with the last created one.
- `-s <property_alias> <property_value>`: sets the specified property to the value without starting the interface.
- `-r`: requests the immediate recompilation of the next map (after the saved one). It corresponds to `esrm_rebuild` command in the game console.

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
| Enemies permissions line | Consists of flags of monsters that will be allowed on maps | (the line of letters for `a`ssassins, `b`ullchickens, `c`ontrollers, hound`e`yes, human `g`runts, `h`eadcrabs, trip`m`ines, bar`n`acles, alien g`r`unts, alien `s`laves, `t`urrets, `z`ombies) | No | `EP` | `esrm_enemies_line` |
| Items permissions line | Consists of flags of weapons and items that will be allowed on maps (both on floors and in crates) | (the line of letters for health`k`its, `b`atteries, `g`renades, `9` mm handguns, `s`atchels, .`3`57 pythons, `c`rossbows, ga`u`ss, cro`w`bars, `h`ornetguns, 9 mm A`R`s, sh`o`tguns, R`P`Gs, `t`ripmines, s`n`arks, `e`gons, `a`xes) | No | `IP` | `esrm_items_line` |
| Button mode flag | Adds the button that should be found and pressed to open the exit gate | 0 / 1 | No | `BM` | `esrm_button` |
| Allow monster makers flag | Allows replacement of some monsters with monster makers | 0 / 1 | No | `MM` | `esrm_makers` |
| Two floors flag | Adds the second floor to the map with some balconies and enemies | 0 / 1 | Yes | `TF` | `esrm_two_floors` |
| Allow items on the second floor | Permits the generation of items for balconies | 0 / 1 | Yes | `TF` | `esrm_two_floors` |
| Allow explosive crates | Enables explosive crates | 0 / 1 | No | `XC` | `esrm_expl_crates` |
| Allow crates with items | Enables crates with items and bugs | 0 / 1 | No | `IC` | `esrm_item_crates` |
