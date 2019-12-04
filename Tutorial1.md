# **Tutorial 1 - Getting Audio Data**

## 1. Setup a scene

Create a new Unity 3D project. In the scenes folder in the assets list, rename the scene to *Main*

## 2. Add objects and their component

Create an empty *Gameobject* in the *Hierarchy* with right click -> `Create Empty`.
Create a new *Folder* on the *Assets folder* and rename it *Scripts*. Inside, create a new *C#* script and rename it `GetAudioData`.

## 3. GetAudioData script

This script will read the data from a playing audio source and extract data, *the frequencies*, of its clip.
It needs an *AudioSource* component to *function* so we add a `RequireComponent` *attribute* to the class:

```c#
	[RequireComponent (typeof (AudioSource))]
	public class GetAudioData : MonoBehaviour
```

Then create *three* variables:

* An `int` called *numberOfSamples* which divide the full frequencies (20kHz) into this number of slices. Must be a power of 2. Min is 64 and Max 8192. Give it a default value of 1024 and add a `[SerializeField]` attribute to be able to modify it from the inspector
* A `float` array called *samples* that will hold the *data* from each *samples* of the clip. We `serialize` it as well to *see* the result in the inspector
* An `AudioSource` called *audioSource* that will hold reference to the *AudioSource*.

```c#
	[SerializeField] private int numberOfSamples = 1024;
	[SerializeField] private float[] samples;

	private AudioSource audioSource;
```

In the `Start` function, we will cache the reference to the *AudioSource* and initialize the *samples* array:

```c#
	private void Start()
	{
		audioSource = GetComponent<AudioSource>();
		samples = new float[numberOfSamples];
	}
```

For clarity later on, the `Update` function calls a method that we call `GetSpectrumAudioSource`:

```c#
	private void Update()
	{
		GetSpectrumAudioSource();
	}
```

Finally we create the GetSpectrumAudioSource method that grabs the data from the playing clip:

```c#
	private void GetSpectrumAudioSource()
	{
		audioSource.GetSpectrumData(samples, 0, FFTWindow.Blackman);
	}
```
`GetSpectrumData` takes three parameters. The first one is an array of *samples* that we initiated earlier. The second is the channel we are using, 0 being the *left* channel. Finally, the *FFTWindow* reduce leakage of signals across frequency bands.

## 4. Testing

Before pressing **Play**, make sure to attach the script to the *AudioData* GameObject in the hierarchy.
This should also create an *AudioSource* component automatically. Change the clip of the AudioSource to the desired music (one is available on the project folder -> Audio).

Press **Play**, click on the *AudioData* object, expand the *Samples* array. Every entry on the array are now being updated with the *frequency* being played by the *sample*.
