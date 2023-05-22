<div align="center">

![](https://github.com/D4KU/unity-geometry-nodes/blob/main/Media%7E/Barn.gif)
<br/>
*A barn procedurally constructed from a single cube*

</div>

This is an extension to the [Unity Visual Scripting](https://unity.com/features/unity-visual-scripting)
package, containing the following new nodes to build objects procedurally:

| Node Name | Description |
| --------- | ----------- |
| Array     | Create a variable amount of copies from a template object and positions them with a given offset |
| Mirror    | Create a copy of an object with inverted scale on one or multiple axes |
| Merge     | Group objects to let the next node process them as one unit |
| Activate  | Set an object (in-)active |
| Position  | Set the local position of an object |
| Rotation  | Set the local rotation of an object |
| Scale     | Set the local scale of an object |
| Look At   | Rotate an object so that it points its forward axis to a given point |

Some nodes sound so simple that you may wonder why you shouldn't just use the
nodes built into the *Visual Scripting* package to do something like setting a
position. The answer is that nodes of this package can revert changes they
have done, allowing you to tear down parts or the entirety of your creations
and reconstruct them with different input parameters. This is possible both in
Edit and Play Mode, allowing you to iterate quickly.


# Installation

In your project folder, simply add this to the dependencies inside `Packages/manifest.json`:

`"com.d4ku.geometry-nodes": "https://github.com/D4KU/unity-geometry-nodes.git"`

Alternatively, you can:
* Clone this repository
* In Unity, go to `Window` > `Package Manager` > `+` > `Add Package from disk`
* Select `package.json` at the root of the package folder

After installation, under `Edit` > `Project Settings` > `Visual Scripting`
press the button to (re-)generate your node library. You should now see the
*Geometry Nodes* category when adding new nodes in the *Script Graph* window.

# Usage

Next to a *Script Machine* and *Variables* component, add a *Geometry Machine*
component. Geometry Nodes are triggered by a *Geometry Node Entry* node,
marking the starting point of the object construction. If the graph referenced
in your *Script Machine* component contains such an entry node, a button named
*Initialize Geometry Nodes* appears inside your *Variables* component. Press
it and the Geometry Nodes inside the graph are executed. Press it again and
their changes are reverted.

# Known issues

The *Visual Scripting* package is not designed to be executed in Edit mode.
Should your Geometry Nodes be initialized, avoid to use the Undo (Ctrl + Z)
function of Unity. It can lead to nodes disappearing. Clear (a.k.a.
uninitialize) the nodes first via the button in the *Variables* component
before undoing changes in your graph.
