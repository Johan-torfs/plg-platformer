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
