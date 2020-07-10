Getting started
***************

-----------------------------------------------------------------------------------------------------------------

Please read our tutorials.pdf (included in this package and available on the website)

Some quickstart instructions:
From the main menu, choose GameObject > Create Other > Gear Factory > Gear.
You should see a procedurally created gear in your scene.
From the main menu, choose GameObject > Create Other > Gear Factory > Machine.
In your hierarchy select the Machine.
From your hierarchy drag the Gear we just created into the "Powered Gear" property of the Machine inspector.
Click Play and watch your first Gear doing a 360 !

Browse to: http://www.atomiccrew.com/gearfactory/ for more information and documentation!


-----------------------------------------------------------------------------------------------------------------

Changelog:

Version 1.9 (03-06-2019):

- Unity 2019 version
- Runtime to .NET 4.x Equivalent
- New feature to reduce vertices in the inner radius by setting the Inner Minimum Vertex Distance
- New optimizer that reduces overlapping vertices with shared normals 
- Interactive information to give realtime feedback on amount of vertex reduction done by optimizer
- Moved GUI overlay to upper left (for better workflow and to prevent DPI scaling issues)
- Fixed bug with mesh cache when removing gears
- Fixed bug with undo/redo handling
- Fixed bug showing wrong number of faces
- Fixed bug filling center when inner radius equals 0
- Fixed bug in button when finished adding multiple gears
- Fixed bug when calculating mouse position on high dpi screens

Version 1.8 (05-02-2018):

- Replaced deprecated GUI stuff
- Marble demo is back

Version 1.7 (03-04-2017):

- cleaned up the project
- removed ray of gears example
- simplified the simple example
- replaced old particle system in organ demo
- bugfix for missing mesh when creating prefab
- all in unity editor generated meshes are now saved in the unity assetdatabase
- inverse tree traversal recalculates hierarchy and allows to change powered gear at runtime
- bugfix for error when cloning gear (int32 parse error)
- bugfix in alignment positioning on size calculation
- new alignment features in GFGear 
- alignment parameter helper editor indicators on GFGear
- new Script Example scene that will showcase all generic script examples
- GFGear now has currentSpeed property (in degrees per second).
- GFGear has ratio (the traditional gear ratio: local ratio in relation to gear that powers it) property. 
- GFGear has machine ratio property (global ratio in relation to the gear that is powered by the machine).
- GFGear inspector has information pane that shows realtime current speed, ratio's.
- GFMachine and GFGear have SetSpeedByGear and SetSpeed methods which alters the machine speed based on a desired gear speed (see ChangePoweredGear script in the script examples scene).
  This way you can change the speed of a gear without changing the powered gear.

Version 1.6 (26-06-2016):
- Bugfix for framerate drop in editor when aligning using auto teeth or auto radius with parent.

Version 1.5 (13-04-2015):
- Unity 5 version with converted examples (some were removed due to feature deprecation by Unity) and all warnings fixed.

Version 1.4 (16-01-2014):
- Changed all new properties to serialized properties (for multiobject edit, undo and prefab overrides)
- Minor bugfix in normal calculation
- Optimized inspector code
- Optimized inspector presentation
- Better calculation of batching info 
- New gear type: ring gears
- Auto switch to ring gear when innerradius exceeds radius
- Auto swap teeth properties when switching to ring gear
- Option to switch off drawing outer edge in 3d
- New option to twist the outside too
- Planetary gears example


Version 1.3 (17-06-2013):

- Bugfix for synced speed
- Tip angle offset
- Valley angle offset
- Improved new normal calculation (GenerateNormals function deprecated)
- Split vertices based on configurable angle (great hard and soft edges)
- UV box mapping
- UV offset and tiling configurable in 3 dimensions
- Fixed to keep original rotation when setting "driven by"-property
- Interactive differential example 
- Twist option to twist a 3d gear
- New interactive organ example
- Static batching indicator in GFGearGen information block in inspector gives you an indication if the gear is below unity's vertex threshold for static batching
- Autoset number of teeth on new gear now disabled by default (auto enabled when gear gets cloned) to improve user workflow
- performance test example added for quick proofing if a device is capable for a user defined amount of gears (and also a great simple example of how to instantiate gears by just a few lines of code)
- Reduced overhead and cpu cycles
- Tested for mobile
- All examples converted for Unity Free users

Version 1.2 (23-01-2013):

- GFGearGen: “Align Radius With Parent” added. It will be checked by default and will appear if you uncheck the “Align Teeth With Parent”.
- GFMachine: Documented the step method for interactive use of the machine.
- Bugfixes and performance optimalization

Version 1.1 (19-11-2012):
- Auto align feature now supports difference in radius
- Some fields converted to slider input control for more convenience and preventing wrong values
	
- Included tutorial scenes
- Included tutorial document
- Unity 3d version 4 is now supported
- Gears initially get a default diffuse material assigned

Version 1.0:
- Initial release
