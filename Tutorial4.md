# **Tutorial 4 - Animation Events**

## 1. Setup a scene

Create a new scene in a Unity 3D project. Rename the scene to AnimationEvents.

## 2. Add Canvas and Folders

Create a *3D Plane* and scale it by *5* on every axis and reset its position to *zero* if different.
Create a new *Canvas*. Set it's `Render Mode` to *World Space* and drag and drop the *Main Camera* to the *Event Camera* fill.
Set it's Width to *580* and Height by *220*. Scale it on the *X* and *Y* by **0.005**.

Create new folders: One *Scripts*, one *Animation*, one *Sounds*.
In *Scripts*, create a new script called *AnimationEventManager*.
In *Sounds*, bring any button sounds you wish to use.
In *Animation*, create a new `Animator Controller`.

## 3. AnimationEventManager Script

We will need three *variables* for this script:

* A `ParticleSystem` type called *particles* that will hold the reference to the Particle System
* A `AudioSource` called *soundSource*, which will hold the reference to our main Audio Source
* A `Animator` called *animator*, to reference the Animator Controller

```c#
	[SerializeField] private ParticleSystem particles;

	private AudioSource soundSource;
	private Animator animator;
```

On `Start` we create our references for the *Audio* and *Animator*:

```c#
	void Start()
	{
		soundSource = GetComponent<AudioSource>();
		animator = GetComponent<Animator>();
	}
```

We Then Create four *methods*. Two of them will toggle our animator booleand *On* and *Off*. One will play the button *sound* and the last one will play *particles*.

```c#
	public void SetBoolOn(string boolName)
	{
		animator.SetBool(boolName, true);
	}

	public void SetBoolOff(string boolName)
	{
		animator.SetBool(boolName, false);
	}

	public void PlaySound(AudioClip clip)
	{
		soundSource.PlayOneShot(clip);
	}

	public void SendParticles()
	{
		particles.Play();
	}
```

## 4. Button and Panel

In our world space canvas, create a new panel. Set a colour if desired, as well as it's transparency.

Create a button. It has to be a child of the canvas, and be placed under the panel ( so it appears at the front of it).
Change it's width to *150* and height to *50*. Reset it's position to *zero*.
Add an `Audio Source` component and uncheck *Play On Awake*.
Add an `Animator` component and drag the *Animator Controller* from your project window to the *Animator* slot.
Add also the `AnimationEventManager` script to the button.

Create a particle system as a child of the button. Position it how you want it to play. I recommend to uncheck *Loop*, uncheck *Play On Awake*, set *Duration* to 0.1 and *Start Lifetime* to 0.5. In the *Emission* tab, set *Rate over Time* to 0 and add a *Burst* of 10 particles.

Drag and Drop the Particle system *GameObject* in the particles slot of the script.

## 5. Animation and Events

Create a new Animation for the button. Call it *Select*. On frame 0, set it's *scale* to 0.9. On frame 15, set it to 1.2 and on frame 40 set to 1 again.

Back on *frame 1*, add an event (third icon next to property). In the *inspector*, you may now choose a function. Scroll all the way down, you should see the `Play Sound` we created earlier. Select it, and chose your *audioclip* as the parameter.

At *frame 05*, add a new event. This time select the `Send Particles` function. This one does not need parameters.

Finally at *frame 40*, add and event and select the function `Set Bool Off`. The *string* parameter to pass is "Pressed".

Before setting the animator controller itself, we can also change the button `OnClick` event. Go back to the button, on the `OnClick`, drag the button itselft in the *Object* slot. Select the `AnimationEventManager.SetBoolOn` for the event, and again pass the *Pressed* string as a parameter.

## 6. Animator Controller

double click on the animator controller to open it. Right click on the back ground and create a new *Empty State*. Click on it to rename it to *Idle*. Right click on it and set it a the default state. drag and drop the animation `Select` in the controller. Set a transition from  *Idle* to *Select*, and back.

On the left part, add a new boolean parameter called "Pressed".

Click on the transition from *Idle* to *Select*, uncheck `Exit Time` and add a condition *Pressed* set to true.

On the transition back, uncheck `Exit Time` as well, and set the condition *Pressed* to false.

## 7. Play and Test

Make sure that the camera can see the canvas and the button and click *Play*.

Click on the button in-game. The button will animate, play a sound and send particles flying around. Hooray.
