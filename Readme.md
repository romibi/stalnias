Stalnias the Game
=================

This Project will become the Stalnias Game to the Story by `<insert correct alias>` from [easterwoodsound.ch (german)](http://easterwoodsound.ch/de/schreiben).

The Game Code will be Open Source as well as some Demo Content. Everything else is undecided.

##Todo (incomplete list, only basic stuff for now):
- [ ] Tile Map
	- [x] Textures only
		- [x] generate mesh(-es) (hard coded)
		- [x] map generating from data objects
		- [x] map loading from file (See more below ↓)
	- [x] Collision Boxes generating
	- [ ] …
- [ ] Player
	- [x] movement
	- [x] animation
	- [ ] …
- [ ] Database Support (not finally decided yet what exactly)
	- [ ] Offline Mode: files (does it work in WebGL?)
		- [x] load map from tmx & images
		- [ ] level select
		- [ ] teleport to different map
		- [ ] write support
		- [ ] update from online
	- [ ] Online Mode: REST update service (with mysql/postgresql in php? node?)
		- [ ] check if configured/supported
		- [ ] connect & load to local
		- [ ] write support (login? how?)
	- [ ] savegame in playerprefs ?
- [ ] Objects and NPCs
	- [ ] 1 Tile sized objects
	- [ ] multi Tile sized objects
	- [ ] objects you can "hide" behind
	- [ ] interaction
	- [ ] simple AI
	- [ ] …
- [ ] UI
	- [ ] Dialogs
	- [ ] Inventory
	- [ ] Menu
- [ ] Map Editor
	- [ ] …
- [ ] Style the game (change/create textures)
- [ ] Create Story (perhaps outside this source code)
- [ ] …

## Credits / Used Tutorials: (this will later be moved to the game credits)
- [quill18creates's "Unity 3d Tutorial: Tile Maps" Tutorial](https://youtu.be/bpB4BApnKhM?list=PLbghT7MmckI4qGA0Wm_TZS8LVrqS47I9R)
- [rm2kdev's "Unity 2D RPG Tutorial"](https://youtu.be/XZDjkQ8wEd0?list=PL_4rJ_acBNMH3SExL3yIOzaqj5IP5CJLC)
- Some Textures from [Liberated Pixel Cup (LPC) Base Assets (sprites & map tiles) on OpenGameArt.Org](http://opengameart.org/content/liberated-pixel-cup-lpc-base-assets-sprites-map-tiles)
