_Changes for v 3.3_:
- Implemented the ***map lighting coefficient***. It will affect the quantity of active lights inside the compartments and the type of the sky outside;
- Implemented the filter for the map section type. Now you can select only “under sky” or only “inside the room” maps;
- Added the restriction for size-walls coefficients pair: too large mazes with too small walls density are exceeding the compiler’s limitations;
- Some code refactoring has been applied;
- Empty crates can now rarely spawn some useful stuff that is not available in other sources;
- Settings saving now works properly;
- Loading settings from the configuration file is not available anymore;
- A path to the exit is now literally highlighted;
- If user changes the compilation settings before running the mod, app will:
    - delete all maps following the one on which the player last saved;
    - immediately rebuild the next map after the saved one.
  So, changes will be applied faster, right after the next “jump”;
- Compilation mechanism has been properly incapsulated into its own thread: app interface will not freeze on this operation anymore;
- Command line keys `-n` and `-go` are not required anymore. But they are still available for possible further applications;
- Command line key `-m` replaced with `-x` (ne`x`t). If the map name is specified, it will work as well as `-m` key, but also it will enable the functionality of `-n` and `-go` keys. So, the `-x` key is now the only one that required to switch the app into the “continue playing” mode
