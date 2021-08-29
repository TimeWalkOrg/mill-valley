MegaWires is an Editor extension that makes it very easy to add fully dynamic wires, cables or ropes to your games. Draw out your path and select your pole type from one of the 42 included or use your own pole objects and then with one click have full physics controlled wires strung between all the connections, even add a Wind object for even more realism.

More information on this system can be found along with a complete breakdown of every param on our website at www.megafiers.com

Features:
- Super Fast custom Physics
- Works with All versions of Unity
- 42 Pole Objects included
- Full source code
- LOD system to disable wires
- Wind system
- Attach Objects to Wires
- Path Drawing system

Components Included:

MegaWire Wire Window
If you have a scene that already has the objects in it that you want to connect with wires then you can use the MegaWires Window to easily select those objects and build the wires between them. The window is opened by clicking the MegaWire option in the GameObject menu. A dockable window will open, and at the top there is a button called 'Start Picking' if you click this you can then start clicking on objects in your scene that you want to wire together. The order you pick the objects in is the order they will be connected up and that order is displayed in the window for you. When you click the start picking button you will be asked if you want to replace the existing list of objects or add to it, if you click replace then the current list will be cleared and a new one started, clicking Add will mean the new selection will be added to the end of the current list. When you are happy with you selection you click the 'Stop Picking' button.

You can now select the material to be used for the wires that are created as well as a name for the MegaWires object that is about to be created. You can also pick an existing MegaWires object from your scene from which to copy all the settings so you don't need to set all the params again making it very easy to add multiple dynamic wires to your project. When you have that all set you just need to click the 'Create Wire' button for your poles to be connected up with wires. If you are creating your first wire object in the scene and have not selected a 'Copy From' object then the system will not create any actual wires as you will need to add the connection points for the wires, this is easily done in the MegaWires inspector.

MegaWire Window:
Start Picking
Click this button to start selecting objects in the scene to add to the poles list. You will be asked if you want to replace the existing list or add to it.

Stop Picking
The Start picking button will change to a Stop Picking one, click this to end the current selection process and save the list.

Create Wire
This will create a MegaWires object in your scene, it may not actually produce any visible wires just yet it depends if you are copying the settings from an existing MegaWires object or not, if not you can set up the actual wire connections by opening the inspector on the newly created object and adding the connections there.

Name
The name given to the newly created MegaWires object.

Material
The material to use for the wires.

Copy From
You can choose to have the settings from an existing MegaWires object transferred to the new object, this will save setting up of the physics and meshing params as well as the connection points.

Current Selection
Shows the current selection that will be used to create the wires.


MegaWire
This is the core component of the MegaWires system and contains all the params to control the physics, the mesh building and the behaviour of the wire in the scene. This component has a lot of options but most of them can be left on the default values so it is not that daunting. This component would have been added to your scene for you either when you used the Pole Planting system, or when you created a wire with the MegaWire Window.

Mega Wire Params:
Disable All
This will turn off all the updates to all the wires in the scene.

Rebuild
If you have made any changes to the params below clicking this button will rebuild the wire with those new settings.

Warm Physics Time
When you create a wire it may not be in the rest state so when you run your scene the wire will fall into place, if you don't want this to happen you can use the Run Physics button below, this will run the physics simualtion on the wire for the length of time defined here, so depending on how your wire is setup you can increase or decrease this value so that wire is in a settled state when you run your scene.

Run Physics
Click this button to run the physics on the wire for the period of time above.

Open Select Window
Clicking this button will open the helper window where you can re select the poles to use for the wire from the scene. There is a separate help page on the Select Window (link to go here) - Currently this is disabled.

Add Wire
This will add a new wire to the simulation, you can change its settings in the connection section that is described below.

Enabled
You can disable each wire in your scene with this value.

Show Wire
This will turn the mesh for the wire on and off.

Disable on Dist
The wire system has a system built in where you can have it turn of the physics and mesh update for each span based on the distance from the camera. If you check this you will see some grey transparent spheres appear centred on the mid point of each wire span. The size is controlled by the disable dist value below, if on the wire update will turned off if the camera is outside the sphere and turned back on if the camera gets close enough. Note if you move a pole in the scene while the system is running and the span is disabled due to distance it will automatically be woken up so that the wires will react to the movement of the pole.

Disable Dist
The distance from the camera beyond which the wire update will be disabled.

Disable on InVisible
You can also have the system turn off the wire updates if a span stops becoming visible to the camera.

Physics Params
Clicking this will open up the params that describe the physics simulation of the wire.

Mesh Params
Clicking this will open up the params that describe how the wire is turned into a mesh.

Connections
Clicking this will show you the connections for the poles that are used, you can tweak the values and delete connections that are no longer required.

Hide Spans
Clicking this will hide the span objects in the hierarchy so helping keeping your project hierarchy a little cleaner.

Show Gizmos
Click this to show the masses used to build your wires along with the springs that connect those masses.

Gizmo Color
The color of the spheres that show the disable on distance value.

Span Connections
Click this to show the connections params for all the wires in the object, by un checking the boxes you can have wires detach and attach themselves from the pole connection points.

Physics Params:
These values control the physics simulation used to model the wires. Some of these values can cause the physics to explode, if that happens just changed the value back and hit the Rebuild Physics button above. Most of the time the default values should serve you well.

Masses
How many masses the simulation will use to simulate each wire, the more masses you have the more accurate the simulation will but will also use more CPU time (though not a lot) you can get convincing simulations with 3 masses so it is up to you as to what you need, just play with the value until you are happy.

Mass
The total mass for each wire, the lighter the wire the more easily it will be effected by wind and gravity, you should change this value to reflect the size of the wire and length and the material it is made off, but again tweak until you get a look you are happy with.

Mass Random
If this value i snot 0 then a random amount between +- the value set here will be added to the masses, this will help to stop each wire looking and behaving exactly like its neighbours, so wires with random mass will hang slightly differently etc.

Spring
The spring rate for the springs that connect the masses, increase this to make the wire stiffer, this value can cause the system to explode depending on the damp rate, the mass and the time step of the system. Reset the value and click Rebuild Physics if the system goes wonky.

Damp
The damping rate for the connecting springs, this along with the spring rate above control the amount of stretch for a given force on the wire.

Stretch
This value adds a pre stretch value to the wire, this is an easy way to make the wire sag less for a given spring rate or you can make a wire longer than the span so it will fall on to the floor.

Gravity
The gravity value used by the physics, you should not need to change this.

Aero Drag
This value controls how quickly the wire masses will slow down due to movement through the air, if you find your wires are a bit to twitchy you can reduce this a little.

Length Constraints
This will add extra constraints to the system to make the wires very stiff.

Stiff Springs
If you dont need super stiff wires you can add in extra stiffness springs, this will make the system act more like a metal cable as opposed to a rope.

Stiff Rate
The spring rate for the stiffness springs.

Stiff Damp
The damping rate for the stiffness springs.

Do Collisions
The system has a very simple collision system where you can define a floor height value, with this option checked the system will do collisions with that floor value for you.

Floor
The height of the floor to use in the collisions check.

Wind Params
This will open up the wind params. Described below.

Advanced Params
This will open up the advanced options params. Described below.

Wind Params:
These params control how the wires interact with any wind.
Wind Src
The wire can either be effected by the global wind strength and direction values or you can choose a Wind object that will effect your wire. The wind object gives a better control over the wind with turbulence and noise values. If no object is selected here then the global wind values will be used.

Wind Dir
The global wind direction.

Wind Frc
The global wind strength.

Wind Effect
This value can be used to reduce or increase the effect of any wind on your wire, so if your wire is in a more sheltered area you can reduce this value, or if your wire is higher up on a hill you can increase this value.

Advanced Params:
These value control some of the finer points of the physics system as well as providing more options to increase the performance of multiple wires in a complex scene.

Time Step
The time step used by the physics system to simulate the wires. The larger this value the less CPU time the system will take but it can also cause the system to go wrong depending on the spring, mass and damping rates used. When you have your wires working you can try increasing this value little by little to the highest value you can before the system messes up to get the best performance you can from the system.

Time Mult
Depending on wht you need from the simulation your wires may be updating too fast or too slow in your scene for your liking, you can control the look and feel of the simulation by changing this value, increasing this value will changes to happen quicker making the wires seem lighter etc. Decreasing the value will make the wires seem heavier. Lower values will reduce the CPU use of the physics simulation.

Start Time
If you have Disable on Distance system enabled then when you scene starts distant wires may not update depending on their distance from the camera, if this value is non zero then the wires will be woken up for this length of time from the scene starting so they can settle into place etc. You can also just use the 'Run Physics' option above to get your wires into a pre warmed state.

Awake Time
If you have the wires being disable on distance from the camera they will still be woken up if a pole they are connected to is moved in the scene, this value says how long the wires will stay active after they have been woken in that way.

Frame Wait
If you have a lot of wires in your scene you can stagger the updates over set frames, if this value is non zero then the system will wait that number of game frames before it will update the system. So a value of 4 will mean the system is updated only every 4th frame. This will slow the look of the simulation down as it is only being updated 1 frame in 4, you can counter this by setting the Time Mult value above to 4 top compensate.

Frame Num
If you are using the frame wait option, you can set this value to control which frame the wire will update on, so for example if you have 3 MegaWire objects in your scene you can set Frame Wait on each to 3 and the set the Frame Num to 0, 1 and 2 on the 3 wires, then only one wire will be updated on any frame.

Constraint Iters
This determines how may times the constraints are calculated per frame, if you are not using Length Constraints then this value has little effect, again the lower the value the less CPU time the system will use.

Mesh Params:
These values control how the wire simulation is turned into a mesh that is displayed in your scene.

Material
The material to use for the wires.

Sides
How many sides the wire mesh will have. The higher the value the smoother the rope will look up close, but most of the time your wires will only need a very low value here, 4 is a good general value for wires you move close to, if your wire are in the distance or your scene is 2D then setting this to 2 will make flat wires but will still look good in most situations.

Segments
This is how many segments the wire has been split into, this value is calculated from the Segs Per Unit value below.

Segs Per Unit
This controls how many segments a wire mesh will have, this is a per unit length of the wire, so if a value of 1 is used then the mesh will have vertices every 1 unit of its length so if the wire is 15 units long it will have 15 divisions. Adjust this value to get a look you are happy with, it will depend on how close you will be to your wires in the scene as well as how many masses you have used for your wires. The system by default uses a cubic interpolation between masses so it will try and generate a nice curve for you.

Strands
You can elect to have the system build multi stranded twisted wires if you need that, this value says how many strands the wire will be built from.

Offset
If you have more than one strand you can select how far apart they are placed.

Strand Radius
The radius of each strand making up the wire.

Twist
How much the multistranded wire is being twisted along its length.

Twist Per Unit
This defines how many degrees of twist are applied to multi stranded wires per unit length.

Gen UV
Says whether UV coords are generated, you may not need uv mapping if your wires are quite a way in the distance, turning this off will speed up the meshing system a little.

UV Twist
You can ask the system to twist the uvs along the wire length, you may need this if your wire texture does not already have a twisted look to it.

UV Tile X
How often the uv is tiled in the x direction.

UV Tile Y
How often the uv is tiled in the y direction.

Linear Interp
By default the system uses cubic interpolation between the masses which gives a nice smooth curve to the wire, you may not need this so you can opt to use the faster linear interpolation instead, in most cases you will not notice much difference.

Calc Bounds
Ask the system to recalc the bounds info for the wires, if your poles are not moving much you wont need this.

Calc Tangents
If you are using a bump shader on your wires you will need to turn this on, this is quite a slow option so it is not recommended.

Vertex Count
This will show you how many vertices each span of the wire is using.

Connections:
This section shows you all the wire connections the system is using, you can alter the values here and adjust wire radius values, or delete connections that are not needed any more. You can add a connection by clicking the Add Wire button at the top of the inspector.

Radius
The radius of the wire that will use this connection.

Out Offset
The location of the connection point of the wire leaving the object.

In Offset
The location of the connection point of the wire entering the object.

Delete
Delete this connection.

Span Connections:
This sections allows you to disconnect a wire from a pole and or re connect it again. If the simulation is running and you uncheck a box the wire will fall to the ground. Checking the box again and the wire will attach itself again to the pole. There is a check box for every connection point on a pole and a section for every span in the whole wire object.

Start
Disconnect the start of the wire.

End
Disconnect the end of the wire.

MegaWire Wind
Wires can either be effected by a global wind strength and direction or you can add Wind objects to the scene, you can set the strength and direction as well as turblence values and variations in strength and direction over time. Each wire object can select a wind objec that will effect it.

The MegaWires Wind component allows you to add multiple areas of wind to your scene that will effect your wires. Each set of wires in the scene can either use the global wind strength and direction value or can be pointed at a MegaWires Wind object in the scene. Each wind component allows you to define strength and direction as well as variations in the strength and direction over time plus an option to add turbulence to the wind.

To create a Wind object in the scene either add the MegaWiresWind script to an object or go to the GameObject/Create Other/MegaWire/Wind menu.

Wind Params:
Below is a breakdown of each param in the MegaWires Wind component.

Direction
The direction the wind would be blowing in if not effected by turbulence or direction variation.

Decay
How quickly the strength of the wind falls off from the centre of the wind object. This allows you to only influence a section of wires as opposed to the whole length.

Strength
The strength of the wind.

Type
The wind system currently supports two types of wind, planar which is the normal wind blowing in one direction, similar to say how a directional light works. The other option is Spherical so the wind force acts outwards from the centre, so here direction has no effect as the direction is calculated as from the mass of the wire to the wind position.

Turbulence
Instead of consistent direction and force for the wind you can ask to add some turbulence effects in, so for any given point in space the wind direction and strength could be very different depending on the values you set below. This gives a much more natural feel to the effect the wind as on any wires.

Frequency
How quickly the turbulence effect changes.

Scale
How much the wind direction and strength is effected by the turbulence.

Strength Noise
As well as turbulence you can add noise to the strength value so that you can have the strength fall and rise in a natural way over time.

Strength Scale
How much the strength can change over time.

Strength Freq
How quickly the strength value will change. High values means very quick changes, values near 0 will be more natural slower changes.

Dir Noise
As fpr the strength the direction of the wind can be changed over time.

Dir Scale
How much the wind direction can change over time, so a value of say 20 here will only allow a +-20 degree change in direction, 180 could mean the wind could come from any direction.

Dir Freq
How quickly the wind direction changes, lower values mean a slower change in direction.

Display Gizmo
The system has a gizmo to help you visualise the wind flow in the scene. Click this to turn it on. When on you will see a bunch of lines coming from boxes, this indicates the direction of the wind and the strength. There is a position handle you can use to change the gizmo position in the scene.

Gizmo Size
How big the gizmo should be, as the wind can vary at any point in the world you may want to make the gizmo cover your entire set of wires, so you can adjust the size here.

Divs
How many sample points the wind display is broken up into.

Giz Scale
You can use this to adjust the length of the lines from the gizmo.

Gizmo Col
The color of the wind gizmo.

MegaWire Pole List
In the GameObject/Create Other/MegaWire section you can select the Plant Poles object, this will create a helper object in the scene which allows you to lay down waypoints in your scene and then select a pole type, the helper will then automatically position poles along the path and connect up all the wires for you. You can control various aspects of the pole planting such as spacing, angle variation, conform to the ground etc. This is the quickest way to add poles and wires to your scenes.

When you create the object a few waypoints are created for you which you can move around as you like. If you need more you can either click the Add Point button in the inspector which will add a new point to the end, of if you click the green box that appears half way between way points while you have the 'A' key held down a new waypoint will be created at that location. You can delet waypoints by clicking the '-' button by the point in the inspector.

Plant Poles Params:
Add Waypoint
Click this button to add a new waypoint to the end of the current path.

Waypoint List
This shows the positions of the current way points in the path.

Delete
Delete the last point in the list, alternatively by the side of each way point there is a small button which can be used to delete that particular way point.

Start
When you create the poles along your path you do not have to have them start at the beginning of the path use this value to move the start point along the path.

Length
You can also use a portion of the path by setting this value to less than one.

Spacing
This value controls the approximate distance between each pole when it is placed on the path.

Closed
You may want your path to form a closed loop, if so check this box and the path will be closed for you and any wires added will also form a complete loop.

Pole Obj
This is where you select the pole type to use for this set of poles, you should select a pole prefab that has had the wire connection points already defined. If you select a pole object that has no connections then no wires will be created when you click the Add Wires check box, but you can still use that object and then add the connection points later in the Mega Wire inspector.

Offset
You can ask the system to plant the poles offset to one side or the other of the path, so for example you may have laid your path along the centre line of a road, you can then use the offset value to have the poles planted a set distance either side of the path so lining the side of the road.

Rotate
The system will plant the poles at right angles to the direction of the path, for some pole types this may not be the correct orientation, in that case you can alter the rotation used by changing this value.

Conform
Checking this button will ask the system to snap the poles to any surface that is underneath the pole, this could be a terrain or a series of mesh colliders

Upright
If the poles are told to conform to the surface they are over the poles will by default point vertically up from the surface, this maybe what you want but you may want the poles to point perfectly up, if so you can use this slider to say how vertical you want the poles to be.

Wire Material
This is where you select the material to be applied to any wires that are created for the planted poles.

Copy Wire
If you already have a MegaWires object in your scene with the various params set just the way you want them then you can pick that object here and the settings from that wire will be copied to the new one for you.

Add Wires
Click this box to have the system build the wire objects for you.

Reverse Wire
Sometimes you may require the poles to be created in the other direction, for example some poles at in and out connections in different places, and depending on the way your path is built they may be added around the wrong way by default you can use the rotate value to fix this or just click the reverse wire box to have pole planted from the other end.

Wire Size Mult
When the wires are built they will use the radius values defined in the connection points, you can though easily increase or decrease that size by changing this value, this is useful if you are creating wires that are quite far from the camera and where they wires may be too thin to be seen clearly, you can increase the size to make them stand out a little better.

Stretch
If you wires are created too droopy or too stiff you can adjust that by changing the pre stretch value, a smaller value here will mean the wires have been pulled tighter when they are hung, a larger value means they were hung with a little slackness.

Seed
When using the variation values below the random seed value here is used, you can change this value to get a different set of variations until you get one you are happy with.

Position Variation
The system can be asked to vary the positioning of the poles from their intended position by the values here.

Rotate Variation
The system can be asked to vary the rotation of the poles from their intended rotation by the values here, this is useful for adding a more rustic look to your poles instead of them all being perfectly upright.

Spacing Variation
You can also vary the spacing between your poles.

Gizmo Params
Click this to open the various params that control the display of the planting gizmos.

Gizmo Params:
These params control how the gizmos for way points are displayed.
Show Gizmo
Click this to have the waypoints or pole spacing gizmos displayed.

Show Type
You can ask the system to either show you the way points for the path, or the pole spacing distances, or both. Select the one you need.

Units
You can select the units the distances are displayed in here, by default the distances are displayed in meters with one unity unit equalling 1m, you can change the scale below.

Units Scale
By default the distances are display with 1 Unity unit equalling 1m, you can change that ratio here if your scene works of a different scaling.

Arrow Width
You can control the width of the various measurement arrow heads with this value.

Arrow Length
You can control the length various measurement arrow heads with this value.

Arrow Offset
How far the measurement arrows are display from the points they measure.

Vert Start
The measurement vertical line distance from the actual line.

Vert Length
The max length of the vertical measuring line.

Dash Dist
The length of the dash on the measurement lines.

Line Color
The color of the main lines.

Arrow Color
The color of the arrows.

Other Color
Color of the vertical measurement lines.

Dash Color
The second dash color.


MegaWire Attach
This component allows you to attach other game objects to any wire created by the system, useful for say positioning birds on wires that will need to move along with the wires in the wind.

To use just add the MegaWireAttach component to the object you want to hang on a wire and then select the wire object in the scene. The Alpha value will control the position along the wire.

Attach Params:
Wire
The MegaWire object in the scene you want to hang this object on.

Alpha
How far along the length of the wire object you want to hang your object, 0 is the very start, 1 the very end.

Strand
If your selected wire object has multiple strands you can choose the right strand by changing this value.

Offset
An offset value to use to help position your object correctly.

Align
Ask the system to align the object along the wire, if your wire sags a lot then you may want to turn this on.

Rotate
An extra rotation value to help you orientate your object as you need it.

MegaWire Hanger
Like the attach system this allows you to attach any object to a wire but this has the benfit that it will effect the wire, you can set the weight of the object and the wire will sag under the load, this is great for adding hanging signs etc from your wires.

To use just add the MegaWireHanger component to the object you want to hang on a wire and then select the wire object in the scene. The Alpha value will control the position along the wire. The best use for this is to hang an object which has a rigid body attached via a joint, the rigid body will then swing naturally on the wire.

Hanger Params:
Wire
The MegaWire object in the scene you want to hang this object on.

Alpha
How far along the length of the wire object you want to hang your object, 0 is the very start, 1 the very end.

Strand
If your selected wire object has multiple strands you can choose the right strand by changing this value.

Offset
An offset value to use to help position your object correctly.

Align
Ask the system to align the object along the wire, if your wire sags a lot then you may want to turn this on.

Rotate
An extra rotation value to help you orientate your object as you need it.

Weight
How heavy the attached object is, this value will cause the wire to sag accordingly.

MegaWire Connection Helper
The poles that come with MegaWires have their wire connection points already defined, but if you would like to use your own poles or wire holders then you can use this helper component to define the connection points and wire radius locations on your object, you can then save that as a prefab and use it with the Pole Planter system for quickly defining you wiring. You just need to click the Add Wire button for a connection point to be added, you can then use the position handle to locate the point on your object where you want the wire to attach. There are connection points for wires coming into the object and for where they leave the object, if they are the same point then just set the values to the same. The radius value will tell the system how thick the wire that uses that point should be. You can override this value later one if you need to.

Connection Helper Params
Show Connections
Show the coonections point on the object, this will also enable the positioning handles so you can more easily position the locations.

Add Wire
Click this button to add a new wire connection point to the object.

Radius
The thickness of the wire that will connect this point.

Out Offset
The location of the connection point where the wire leaves the object.

In Offset
The location of the connection point where the wire enters the object.

Delete
Delete the connection from the object.

Version History:
v1.21
Add MegaWireOrigin component which can be added to MegaWire object to track any movement of the object and update the physics, useful for Floating Origing projects.

v1.20
MegaWires made compatible with Unity 2019
MegaWires made compatible with Unity 2020

v1.19
Added Rotation value to Mesh Params so you can easily rotate the wire mesh if needed.
MegaWires made fully compatible with Unity 2018

v1.18
MegaWires made fully Unity 2017 compatible

v1.17
MegaWires made fully Unity 5.6 compatible

v1.16
Fixed a bug where poles were not being deleted correctly if the Plant Pole object name was changed.

v1.15
Adding a wire object that was made into a prefab to a scene will now work correctly.

v1.14
Updated for all depreciated methods etc for Unity 5.3 and 5.4

v1.13
The Run Physics button will now work on a wire system with a single span.

v1.12
Fixed bug of poles not being deleted when a pole object with no wire connections is selected.
Fixed an exception when building poles with no wires attached.

v1.11
Small optimization to the wire physics.

v1.10
Added new collision option to use raycasts for better collisions with the world
Added layer mask option to raycast collsion to choose layers to hit.
Added offset value for the raycast collision system to allow you to raise the wires of hit surfaces.

v1.09
Uploaded Unity 5 version with final fixes.

v1.08
Optimised the physics code for nearly double the speed.
Made changes for Unity 5.

v1.07
Added support for using MegaFlow sources as wind inputs MegaWireFlowWind.

v1.06
Fixed the bug that stopped changes in the stretch value on the plant poles being passed to the actual wires.

v1.05
Fixed warnings about obsolete methods in Unity 4.5 and 4.6

v1.04
Fixed the bug that stopped the poles being planted on MegaShape splines.
Added PDF docs file.

v1.03
Wire window now shows arrows and spacing details.
Pole Planter now preserves wire settings if you change any settings on exisiting wire.
Pole Planter will only delete Wire objects from the children list so you can add your own child objects without them getting destroyed if you update the Pole Planter settings
Objects attached to planted poles will now work correctly if plant poles values change.

V1.0
First release