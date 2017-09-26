# MillValley1920
Unity model of TimeWalk for Mill Valley, CA c.1920

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
