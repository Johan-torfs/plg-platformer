# Platformer Microgame Improvements
This 2D platformer uses the 2D platformer microgame template from Unity as a base. Upon that game I made several improvements, each improvement is listed below.

## Improvements
### General movement
- Added a acceleration timer to limit the time the player can hold down the jump button
- Increased the gravity modifier
- Increased the max speed
- Increased the jump take off speed
- Also increased the jump deceleration

### Jumping next to wall
When a player was stood next to a wall, he would be unable to jump sometimes. This was caused by the horizontal movement killing the vertical velocity when it detected a collider in its path.
- Changed the KinematicObject to only kill vertical velocity when moving vertically

## New features
### Wall jump
When a player is close to a wall, the player can jump away from the wall.
- Made it so the player will always be pushed away from the wall
- The player can control how far he is pushed by holding the directionals
- Updated tilemap to make wall jumping necessary
- Changed the force with which the player gets pushed away

### Moving platforms
- Made an addition to the PatrolPath to get the local position
- Added a prefab MovingPlatform
- Added PlatformController to make the platform follow a given PatrolPath
- Added the MovingPlatforms to the scene and changed the position of some of the tokens to fit the updated platforms
- Changed platforms so they use a sprite renderer and box collider, instead of moving a tilemap for every platform
- Made a Draw method to draw each platform based on a given width
- If the player is standing on a moving platform, the player will move along with the platform

### Double jump
- Added a double jump
- The player has only one double jump
- The double jump gets reset when the player lands on the ground
- Moved victory point to create a bit more challenge
- Player changes color when he has no double jump available

### Double jump crystals
- Added a different color token that gives the player another double jump
- Added (hidden) areas that require double jump

### Token counter
- Added a counter on the UI to let the player know how many tokens they have collected and more importantly... missed
- Excluded jump crystals from being counted towards the token total

### Checkpoints
- Added checkpoints that replace the spawnpoint when the player passes through
- The checkpoint trigger gets destroyed once triggered, so the player can only trigger it once

## Todo
- Fix bug when jumping into moving platforms 
  - Most likely due to the wall jump, seeing as this happens when close to a wall on one side and a platform on the other
- Write a controller to draw the platform sprites instead of drawing them for each platform seperately
- When the platform turns, the player unintentialy exits the idle animation
- Make visuals for the checkpoints
- Make it so the player cannot spam jump into a wall
