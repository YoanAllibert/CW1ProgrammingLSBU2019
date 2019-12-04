# **Tutorial 2 - Numeric Springing**

## 1. Setup a scene

Create a new scene in a Unity 3D project. Rename the scene to NumericSpring.

## 2. Add test objects and their component

Create a *3D Plane* and scale it by *5* on every axis and reset its position to *zero* if different.
Create a *3D Cube*, set it’s position to (0, 0.5, 0). Add a `Rigidbody` component to it from the inspector, then from it on the `Interpolate` setting select `Interpolate`.
Create a new *Folder* on the *Assets folder* and rename it *Scripts*.

## 3. Jump Script

Create a new *script* and rename it *Jump*
This script apply a vertical force the the `Rigidbody` of the cube.
Create *two* variables:

* A `Rigidbody` type called *rb* that will hold the reference to our rigidbody
* A `float` called *jumpForce*, initialized at *6f*. use The `[SerializeField]` attribute to modify it from the *inspector*

```c#
	private Rigidbody rb;
	[SerializeField] private float jumpForce = 6f;
```

In the `Start` function, we will cache the reference to the rigidbody:

```c#
	void Start()
	{
		rb = GetComponent<Rigidbody>();
	}
```

In the `Update` function, we check for player input of the *Spacebar* key with `Input.GetKeyDown`.
If it returns *true*, we use `AddForce` on the `Rigidbody`, with a ForceMode of *Impulse*.

```c#
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
		}
	}
```

## 4. Numeric Springing

Create a new *script* called *NumericSpringing*. This will be responsible for calculating a numeric value through an *Easer Out Elastic Function* over time.
We need *two Serialized* variables:

* A `Vector3` called *modifScale*, which will multiply our `localScale` by a certain amount
* A `float` called *duration* to specify the time to animate our scale

Also we will create a `Vector3` *originalScale* that will cache our original scale, and a constant `float` for two times *Pi* called *twoPI*.

```c#
	[SerializeField] private Vector3 modifScale;
	[SerializeField] private float duration;

	private Vector3 originalScale;
	private static float twoPI = 2f * Mathf.PI;
```

Our `Start` function will simply cache our current scale:

```c#
	private void Start()
	{
		originalScale = transform.localScale;
	}
```

In our `Update`, we check for player *Spacebar* input, then start our `Coroutine` to change our scale.
A `coroutine` is a particuliar method that can be *paused* in the middle of its execution and started again.
First we *stop* all Coroutines, so we don't apply it twice.
Then we multiply our `localScale` by the factor multiplier from *modifScale* using the `Vector3.Scale` method.
The `Coroutine` that will be called need *four arguments*. The first is the *transform* to be affected, then the starting scale (our current modified *localScale*), then the *endScale* (the original shape) and finally the duration.

```c#
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			StopAllCoroutines();
			transform.localScale = Vector3.Scale(transform.localScale, modifScale);
			StartCoroutine(EaseScaleVector(transform, transform.localScale, originalScale, duration));
		}
	}
```
Then we create the actual `Coroutine`.
The logic of it is to have a variable *t* that keep track of the time passed, then create a `while` loop until *duration* has been reached.
Inside this loop, we calculate the next value for the scale using the *easer method* that we will create next, then apply it to the localScale, wait for a frame using `yield return null;`, increment the time *t* and start the loop again.
When the loop end, we set the *localScale* to the *endScale*, in case the scale has not been reached by the loop.

```c#
	IEnumerator EaseScaleVector(Transform trans, Vector3 startScale, Vector3 endScale, float duration)
	{
		float t = 0f;
		while (t < duration)
		{
			Vector3 newOne = new Vector3(
				ElasticEaseOutFull(t, startScale.x, endScale.x - startScale.x, duration),
				ElasticEaseOutFull(t, startScale.y, endScale.y - startScale.y, duration),
				ElasticEaseOutFull(t, startScale.z, endScale.z - startScale.z, duration)
				);

			trans.localScale = newOne;
			yield return null;
			t += Time.deltaTime;
		}

		trans.localScale = endScale;
	}
```

As you can see, the *ElasticEase* method use *four* parameters:

* t - the current time position(a value from 0 -> d)
* b - the initial value
* c - the amount of change(end - b)
* d - the total amount of time the ease should take

The mathematics behind this method represent an elastic easing out, as demonstrated on this page:
[https://easings.net/en#easeOutElastic](https://easings.net/en#easeOutElastic "Easing Functions")

During the coroutine, we *feed* each parameters of our localScale to the *Ease* method, and get the value back each frame it is running.

```c#
	public static float ElasticEase(float t, float b, float c, float d)
	{
		float s;
		if (t == 0f) return b;
		if ((t /= d) == 1) return b + c;

		s = (d * 0.3f) / 4;
		return (c * (float)Mathf.Pow(2, -10 * t) * (float)Mathf.Sin((t * d - s) * twoPI / (d * 0.3f)) + c + b);
	}
```

## 5. Testing

Before pressing **Play**, make sure to add both scripts to the cube.
On the `Jump` script, the force should be a positive value, *6* is recommended.
On the `NumericSpringing` script, set the duration to *2*, and the *modif Scale* Vector3 to 0.4, 1.4, 0.4.
Feel free to experiment with those values for interesting results.

Press **Play**, Press the *Space bar*, and gasp as the cube looks like a soft object being send into the air. 
