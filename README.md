# MillValley1920
Unity model of TimeWalk for Mill Valley, CA c.1920


## Changes done by Worth Technology Pvt. Ltd. 

#### Common reworks

- The terrain was not leveled inside most of the buildings and hence was occluding the floor surfaces, and other interiors. The buildings were also in-air in certain cases and were not placed properly. 
- Revived the greenery of the then possible era. Trees and bushes have been placed & arranged strategically as and where needed. 
- Most of the parts of the town are now well-connected with the Electric Transformers for a visually realistic electricity supply across the town. 
- Added Mesh/Box Colliders to each model including buildings, vehicles, trains, and other objects ( if applicable ).  

###### What’s upcoming here : 

- `We are planning to add floral objects, since they are missing as of now.`
- `Small gardens outside a few buildings or maybe in backyards of few would look good.`
- `The roads of later years like 1973 or 2015 are not in proper level and need to be placed precisely on the terrain. Currently, the player at times fails to even come on the road, specially when coming from vacant greener parts of terrain.`

<hr>

#### A better Night Life

- Street Poles have been added to light up the entire town. Atleast one pole can be spotted outside each building. The best part: **They turn ON at night.** :)
- During the night, The Trains now have warm-yellow headlights and red-color tail lamps. 
- The Platform at Depot has hanging light from the ceiling ( will soon increase the intensity and number of hanging lights). 
- While all other street lights are yellow in color, the Depot has white ones to light it up exclusively. 

###### What’s upcoming here:

- `Will attempt to add more varieties of lights, that closely resemble those in the older photographs of the mill valley town. `
- `Will light-up the interiors of the buildings with ceiling/panel lights, table lamps, and chandeliers. `

<hr>

#### Records-Building

- Reworked on creating components of windows and the 3 different types of gables. 
- Recolored the windows & gables for better visual appeal. 
- Created a proper wooden frame for the main door as earlier the door was at a significant height above the floor. 
- Created a wooden door set to replace the existing main door. 
- Replaced the backdoor as well. 
- Wood-Paneled the gables and windows from inside as well. 
- Added Texture to floor and a dual-tone texture to the ceiling. 
- Added two-step stair at the backdoor ( interior + exterior ) as it was at a significant height from the floor. 
- Edited existing texture colors and transparency levels of the Glass windows. 
- Placed at correct location, target year, and in precise orientation, as on Map.  
- Other minor tweaks & fixes. 

###### What’s upcoming here: 

- `We will light up the model in night mode.`
- `Would love to have old pics of interiors of this building to take a headstart.`

<hr>

#### Depot. 

- The keyframes of the **Baggage & Express** Train have been revised to ensure that the train doesn’t fly out of terrain, and just oscillates to & fro between the depot and the other marked end. 
- The same train has been aligned to stick to the railway track and not appear as a flying-on-wheels train :) 
- As already mentioned, both the trains have headlamps and taillamps that light up at night automatically. The trains do have colliders as well. 
- Tweaking Character animations to stick to platform and not fly in air. 

###### What’s upcoming here:

- `Will animate the **1880-1929 Train - Northwestern Pacific** train by animating revolutions of wheels along with a keyframe for its locomotion.`
- `Will add a few more character-passengers to the trains.`
- `Will improve the visual appeal of the platform and its roof.`

<hr>

#### Browns Building

- Improved & enhanced the building textures, amongst other rework. Please find the previous building’s snapshot [ here ](old-browns.jpg) & the transformed model’s snapshot [ here ](new-browns.jpg). 

<hr>

#### Hub-Theatre

- Fixed the issue of theatre interiors visible from the outer booth, as visible [ here ](hub-theatre-interiors-visible.jpg)
- Fixed the black screen issue and removed the unnecessary black object, that was earlier visible outside.
- Fixed the script to play the movie inside the theatre. The movie didnt play earlier. 
- Added a few characters as audience.  
- Other minor tweaks and fixes.

###### What’s upcoming here:
- `Will introduce another movie show here, that would play in night mode.`
- `Will time the reactions of audience ( currently, the lady continuously keeps clapping ).`

<hr>

#### Hiker’s Retreat

- Added to the correct target year, and at correct location in actual orientation. 

###### What’s upcoming here:
- `Will create interiors once the old photographs are available of the same.`
- `Can plan a timed event of Hikers coming around the building.`

<hr>

#### O’Shaughnessy Building

- Earlier, the fingers of the piano player moved in-air. Fixed his position and orientation so that he actually plays the piano now :)
- The light used here was causing glare and reflections. Fixed. 

###### What’s upcoming here:

- `The piano player can be animated to play actual notes, as of the music. A simpler music can be chosen for this. It would be a great visual appeal.`

<hr>

@tedbarnett: We Thank you for giving us an opportunity to contribute to this project. We look forward to having your views about our work. 

<hr>

To set up the demo for VR, walkthrough, or flythrough mode, enable the appropriate object under the "TimeWalk Controls" object.

To enable VR
- disable "Fly-through control" and "Walk-through control" (uncheck top checkbox in Inspector)
- enable "Oculus VR control" (note: HTC Vive in progress)
- open menu Edit/Project Settings/Player
- under "XR Settings", make sure "Virtual Reality Supported" is checked
- under "XR Settings/Virtual Reality SDKs", drag the "Oculus" line to the top of the list (above "None")
- that should do it.  Press PLAY to see it working on the Oculus headset

To enable Fly-through
- disable "Oculus VR control" and "Walk-through control" (uncheck top checkbox in Inspector)
- enable "Fly-through control"
- open menu Edit/Project Settings/Player
- under "XR Settings", make sure "Virtual Reality Supported" is UN-checked
- Press PLAY to start

To enable Walk-through
- disable "Oculus VR control" and "Fly-through control" (uncheck top checkbox in Inspector)
- enable "Walk-through control"
- open menu Edit/Project Settings/Player
- under "XR Settings", make sure "Virtual Reality Supported" is UN-checked
- Press PLAY to start




