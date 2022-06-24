# Randomaze v 1.3

ES: Randomaze – The maps generator

#

**ES: Randomaze**  is the maps generator for the sub-mod with the same name.
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
- `-c`: cleans *maps*, *save* and *sprites* directories.
- `-m <map_name>`: compares specified map (currently saved map expected) with the last created.
  Skips building of the new map if the next map is already available.

### From `Randomaze.cfg` file

- ***Maze size coefficient***. Range: 1 – 8.
- ***Difficulty coefficient***. Range: 1 – 8. Affects quantities of enemies and items / weapons on the map.
- ***Walls density coefficient***. Range: 1 – 12. The larger the value, the fewer “branches” the maze will have.
  *12* means almost a snake-shaped corridor, *1* is for a hangar with a few walls.
- ***Enemies permissions line***. Consists of key letters of enemies (`a`ssassins, `b`ullchickens, `g`runts, `h`eadcrabs,
  alien g`r`unts, alien `s`laves, `t`urrets, `z`ombies). Missing letter means forbidden enemy.

&nbsp;



## Requirements

- Windows XP or newer;
- [Microsoft .NET Framework 4.0](https://microsoft.com/en-us/download/details.aspx?id=17718).

Interface languages: en_us.

&nbsp;



## [Development policy and EULA](https://adslbarxatov.github.io/ADP)

This Policy (ADP), its positions, conclusion, EULA and application methods
describes general rules that we follow in all of our development processes, released applications and implemented ideas.
***It must be acquainted by participants and users before using any of laboratory’s products.
By downloading them, you agree and accept this Policy!***
