# **Tutorial 3 - Dolly Zoom**

## 1. Setup a scene

Create a new scene in a Unity 3D project. Rename the scene to DollyZoom.

## 2. Add test objects

Create a *3D Plane* and scale it by *5* on every axis and reset its position to *zero* if different.
Create a few *3D Cube*, place them around the plane. make sure there is on central, it will be our focus object. Scale the others randomly and assign them colours if you fancy.
Create a new *Folder* on the *Assets folder* and rename it *Scripts*.

## 3. DollyZoom Script

Create a new *script* and rename it *DollyZoom*.
We will need three *variables* for this script:

* A `Transform` type called *target* that will hold the reference to the object we want to keep in focus
* A `Camera` called *cam*, which will hold the reference to our main camera component
* A `float` called *initialFrustrumWidth*, it will be calculated on *Awake* and contain the initial size width of the camera frustrum

```c#
	[SerializeField] private Transform target;

	private Camera cam;
	private float initialFrustrumWidth;
```

## 4. Initialisation

In the `Awake` function, we will cache the reference to the camera, get the initial distance from the camera to the object, then use this distance to calculate the initial width frustrum. the function `CalculateFrustrumWidth` will be created after.

```c#
	void Awake()
	{
		cam = GetComponent<Camera>();

		float initialDistance = Vector3.Distance(transform.position, target.position);
		initialFrustrumWidth = CalculateFrustrumWidth(initialDistance);
	}
```

To calculate the initial frustrum *width*, we use the formula from the wikipedia page: [en.wikipedia.org/wiki/Dolly_zoom](https://en.wikipedia.org/wiki/Dolly_zoom "Dolly Zoom"). after rearranging the equation, we obtain the following result. Note that we pass the initial distance previously calculated, and this is calculated only once, as we keep the initial width value as a reference.

```c#
	private float CalculateFrustrumWidth(float distance)
	{
		return (2f * distance * Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad));
	}
```

## 5. Step calculations

In the `Update` function, we check first for player input of the *Horizontal* axis with `Input.GetAxis`, and place the result in a `Translate` function, multiplied to the forward direction and a arbitrary chosen value for the speed (here 5f). We also multiply by `Time.deltaTime` to make it *framerate* independant.
Then we calculate two values each steps:

* The current distance to the *target*, using the same `Vector3.Distance` used previously
* The new *FOV* for the camera using the same equation but rearranged once again as we now know the *width*

```c#
	void Update()
	{
		transform.Translate(Input.GetAxis("Vertical") * Vector3.forward * Time.deltaTime * 5f);

		float curentDistance = Vector3.Distance(transform.position, target.position);
		cam.fieldOfView = CalculateFieldOfView(initialFrustrumWidth, curentDistance);
	}
```

The only *function* left to create is calculating the current frustrum:

```c#
	private float CalculateFieldOfView(float width, float distance)
	{
		return (2f * Mathf.Atan(width * 0.5f / distance) * Mathf.Rad2Deg);
	}
```

## 6. Testing

Before pressing **Play**, add the created script on the main camera in the *hierarchy*.
Populate the *target* variable with the central cube on the scene.  
For best result, place the camera at the same height  as the cube, directly facing it, at a reasonable distance (think 12 to 15 units).

Press **Play**, Press the *W* and *S* (or *up and down* arrows). you will notice the central cube keeping the same scale and focus while the background and side cube get distorted by the effect.
