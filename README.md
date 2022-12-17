# cs135Final
The project is about a shooting range and the main objective is to hit as many targets.
Targets are randomly spawned so the user has to quickly react to it. Users are given a set
amount of magnizines with limited bullets in them. It is up to them to hit as many targets as
possible given their ammunition.


XR Origin
XR Origin is a player controller for our project, it uses the XR origin preset and our custom
hand models. The XR origin script tracks head movement and moves the main camera. The
Left and right hand model has an XR controller script that tracks the controller movement and
spawn the correct hand model. The hands use the XR Direct interactor, this allows the player to
interact with objects the the XR interactorable scripts.

The handgun is made up of 3 main parts: the gun, the body, the slider and the magazine
location. The gun body has a XR Grab Interactable that allows the player to grab the gun.
When grabbing the gun with the left hand, the orientation is flipped compared to the right
hand. Here we added a custom function that uses quaternion inverse to help adjust graboffset


The target object has a box collider for collision handling, when hit by a bullet, it will break in
pieces by spawning all the broken pieces prefabs along with simulating gravity.
