_Changes for v 6.1.5_:
- `Engine`: debris of destructible object now reacts to the force of the weapon used;
- `Engine`: added the stand-alone material for fabric textures;
- `Engine`: fixed incorrect startup angle and behavior of `trigger_camera`;
- `Engine`: fixed some memory leaks in previously updated code;
- `Compiler`: fixed some old limits;
- `Randomaze`: fixed inability to spawn Barney if the second button was found before collecting all the rats;
- `Randomaze`: access to map file has been properly refactored;
- Implemented the crates balance instead of flags for crates with items and crates with explosives. Its edges (+3 and -3) allow you to disable corresponding types of crates (items or explosives);
- The crate density value can now be set to zero to disable crates altogether
