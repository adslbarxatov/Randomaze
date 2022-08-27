_Changes for v 3.2_:
- If user changes the compilation settings before running the mod, app will:
    - delete all maps following the one on which the player last saved;
    - immediately rebuild the next map after the saved one.
  So, changes will be applied faster, right after the next “jump”;
- Compilation mechanism has been properly incapsulated into its own thread: app interface will not freeze on this operation anymore;
- Removed some obsolete code;
- Command line keys `-n` and `-go` are not required anymore. But they are still available for possible further applications;
- Command line key `-m` replaced with `-x` (ne`x`t). If the map name is specified, it will work as well as `-m` key, but also it will enable the functionality of `-n` and `-go` keys. So, the `-x` key is now the only one that required to switch the app into the “continue playing” mode
- Code for map sections has been completely refactored;
- Adjusted furniture types for some locations;
- Added visual doors for internal walls that have only one visible side
