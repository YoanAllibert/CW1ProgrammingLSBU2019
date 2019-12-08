# **Tutorial 1 - Smooth Camera Follow**

## 1. Setup a scene

Create a new Unity 3D project. In the scenes folder in the assets list, rename the only scene to Main.

## 2. Add test objects and their component

Create a *3D Plane* and scale it by *5* on every axis and reset its position to *zero* if different.
Create a *3D Sphere*, set it’s position to (0, 0.5, 0). Add a `Rigidbody` component to it from the inspector, then from it on the `Interpolate` setting select `Interpolate`.
Create a new *Folder* on the *Assets folder* and rename it *Scripts*.

## 3. Movement script

Create a new *script* and rename it *PlayerController*
This script will check the player *Input* every frame, and apply a force to the gameobject related to input.
Create *four* variables:

* A `float` called *speed* which will affect the force applied, give it a default value of 10 and add a `[SerializeField]` attribute to be able to modify it from the inspector
* A `Rigidbody` type called *rb* that will hold the reference to our rigidbody
* Two `floats` for our *Vertical* and *Horizontal* inputs

```c#
	[SerializeField] private float speed = 10f;
	private Rigidbody rb;
	private float verticalInput;
	private float horizontalInput;
```

In the `Start` function, we will cache the reference to the rigidbody:

```c#
	void Start()
	{
		rb = GetComponent<Rigidbody>();
	}
```

The `Update` function is run *once* per frame. Inside we will get the value of our respective inputs and store them in the previously created *variables*. The `Input.GetAxis` create a float ranging from -1 to 1.

```c#
	private void Update()
	{
		verticalInput = Input.GetAxis("Vertical");
		horizontalInput = Input.GetAxis("Horizontal");
	}
```

Finally on a `FixedUpdate`, we will apply the force on our *rigidbody*.

* The X axis is our Horizontal movement, scale with desired speed. We also multiply by `Time.deltaTime` to ensure a consistent force unaffected by framerate
* The Y axis is left at 0, as we do not move the player vertically
* The Z axis is similar to the X, only affected by Vertical inputs

```c#
private void FixedUpdate()
	{
		rb.AddForce(horizontalInput * speed * Time.deltaTime, 0f, verticalInput * speed * Time.deltaTime, ForceMode.Impulse);
	}
```

`FixedUpdate` is called on a fixed time step unrelated to frames rendering and is suited to work on the *physics* engine.
Additionally, we use a `ForceMode.Impulse` to add *instant* force to the rigidbody using its *mass*, resulting in a more responsive input feel.

The script is now done, return to Unity and attach the script to the sphere with a drag and drop.

## 4. Camera Script

Create a new script called `CameraFollow` and open it.
First create three variables with the `[SerializeField]` attribute:

* objectToFollow will be a `transform` type of the position to follow
* A float smoothTime, set at 0.3f by default, the parameter that will affect the time of transition
* A Vector3 offset, to position the camera at a certain distance

Create a *fourth variable*, a `private Vector3` velocity, starting at *zero*. This will be used by our smoothing function to control its current velocity.

```c#
	[SerializeField] private Transform objectToFollow;
	[SerializeField] private float smoothTime = 0.3f;
	[SerializeField] private Vector3 offset;
	 
	private Vector3 velocity = Vector3.zero;
```

We now simply have to create a `SmoothDamp` function. We do that in a `LateUpdate`, as the camera movement do not need *priority* over other calculations.
The `SmoothDamp` function is a *sigmoid interpolation*, meaning it as a *smooth* fade in/out, for a more pleasing look interpolation. It takes *four* parameters:

* A starting position, we pass it the current position
* A destination, which is the object to follow plus the offset distance
* Its velocity, pass with the `ref` keyword. The function use it on it’s own, without our input
* The smooth time, the approximate time it will need to reach the destination

```c#
	void LateUpdate()
	{
		transform.position = Vector3.SmoothDamp(transform.position, objectToFollow.position + offset, ref velocity, smoothTime);
	}
```

Back into Unity, drag the CameraFollow script onto the main camera.

## 5. Testing

Before pressing **Play**, make sure to populate the `CameraFollow` script attached to the camera.
*Drag and Drop* the Sphere onto the objectToFollow.
Change the offset to a position away from the sphere. I recommend *(0, 3.5, -5)*
Also set the rotation of the camera to a more top down view, such as *(20, 0, 0)*

Press **Play**, move the sphere using *arrow keys or WASD*. The camera will follow the player in a *smooth* and *pleasing* way.
