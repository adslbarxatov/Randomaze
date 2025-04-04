# ES: Randomaze: user guide
> **ƒ** &nbsp;RD AAOW FDL; 23.09.2024; 21:58



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
| Lamps lighting coefficient | Affects quantity of enabled lamp lights | 1 – 10 | Yes | `LI` | `esrm_inlight` |
| Sun lighting coefficient | Affects outdoor brightness and the type of sky | 1 – 6 | Yes | `LO` | `esrm_outlight` |
| Use neon lighting flag | Allows addition of neon lamps on too dark sections | 0 / 1 | No | `NL` | `esrm_neon` |
| Crates density coefficient | Describes, how much enemies will be replaced with crates with bugs or explosives | 0 – 5 | Yes | `CD` | `esrm_crates` |
| Gravity coefficient | Affects the gravity percentage for all objects on the map | 1 – 20 (10 = 100%) | Yes | `GR` | `esrm_gravity` |
| Button mode | Adds the button(s) that should be found and pressed to open the exit gate | Quantity, 1 / 2 / 3 | No | `BM` | `esrm_button` |
| Map section types | Sets allowed types of map sections (*all*, *only under sky* or *only inside*) | 1 / 2 / 3 | No | `ST` | `esrm_sections` |
| Two floors flag | Adds the second floor to the map with some balconies and enemies | 0 / 1 | Yes | `TF` | `esrm_two_floors` |
| Allow items on the second floor | Permits the generation of items for balconies | 0 / 1 | Yes | `TF` | `esrm_two_floors` |
| Allow monster makers flag | Allows replacement of some monsters with monster makers | 0 / 1 | No | `MM` | `esrm_makers` |
| Map barrier types | Sets allowed types of barriers between map sections (*glass*, *fabric* or *both*) | 1 / 2 / 3 | No | `BT` | `esrm_barriers` |
| Fog coefficient | Affects the fog density percentage on the map | 0 – 10 (0 = 0%, 10 = 100%) | Yes | `FC` | `esrm_fog` |
| Water level | Affects the water level on the map | 0 – 9 (0 = 0%, 5 = 25%) | Yes | `WL` | `esrm_water` |
| Enemies permissions list | Consists of probabilities for monsters on maps | (the line of **probabilities** (**0 – 5**) for assassins, bullchickens, controllers, houndeyes, human grunts, headcrabs, tripmines, barnacles, alien grunts, alien slaves, turrets and zombies) | No | `EP` | `esrm_enemies_list` |
| Items permissions list | Consists of probabilities for weapons and items that will be allowed on maps (both on floors and in crates) | (the line of **probabilities** (**0 – 5**) for healthkits, batteries, grenades, 9mm handguns, satchels, .357 pythons, crossbows, gauss, crowbars, hornetguns, 9mm ARs, shotguns, RPGs, tripmines, snarks, egons and axes) | No | `IP` | `esrm_items_list` |
