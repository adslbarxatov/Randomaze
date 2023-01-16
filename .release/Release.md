_Changes for v 4.6_:
- If turret enemy is enabled, app will randomly select the entity from turret, miniturret and sentry for its location;
- App now able to repeat the map generation if the previous try exceeded the maximum of engine entities quantity; app will also adjust the generation settings to prevent this situation on the next try;
- App will not start if one copy of it is already running. It allow us to prevent multiple calls on game reloadings;
- Added an extra localization check;
- Implemented the ability to randomize the “two floors” and “allow items for balconies” flags;
- Fixed the backward teleportation bug;
- Light bulbs have been “sunk” into the ceiling (for better experience with low gravity coefficients);
- The height of the second floor has been decreased; climbing on balconies is more comfortable now
