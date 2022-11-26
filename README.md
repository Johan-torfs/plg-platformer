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
- Changed the KenematicObject to only kill vertical velocity when moving vertically

## New features
### Wall jump
When a player is close to a wall, the player can jump away from the wall.
- Made it so the player will always be pushed away from the wall
- The player can control how far he is pushed by holding the directionals
- Updated tilemap to make wall jumping necessary

### Moving platforms
- Made an addition to the PatrolPath to get the local position
- Added a prefab MovingPlatform
- Added PlatformController to make the platform follow a given PatrolPath
- Added the MovingPlatforms to the scene and changed the position of some of the tokens to fit the updated platforms

### Double jump
- Added a double jump
- The player has only one double jump
- The double jump gets reset when the player lands on the ground
- Moved victory point to create a bit more challenge
- Player changes color when he has no double jump available

### Double jump crystals
- Added a different color token that gives the player another double jump
- Added (hidden) areas that require double jump

## Todo
- Add a counter for the tokens (for the collectivists)
- Fix bug when jumping into moving platforms
- Fix bug where when dying without double jump, the player does not get his double jump back until he lands
