# **Journal Tutorials**

## **Tutorial 1: Smooth Camera Follow**

### 15/10/19

The first iteration of the movement script was changing the velocity of the `gameobject` every fixed steps, resulting in a sharp continuous movement. Changing it to adding force to the `rigidbody` made it more smooth and natural, better suited to a physical object.

I also changed the `ForceMode` for `Impulse`, to get better reaction feel of input.
And with that, I was ready to script my camera.

The camera script used first a `Vector3.Lerp`, however changing it to a `SmoothDamp` interpolation yield a more natural result. `Smoothdamp` follow a *Sigmoid* function, with softer fades in/out, whereas *Lerp* is Linear all the way.

The camera follow the object to it’s center, so I added and *offset* variable, modifiable from the *inspector*, to stay at a distance.

Finally, the camera movement was fluid but the object appeared to jitter, selecting `Interpolate` in the Moving object’s Rigidbody setting smoothed it’s movement perfectly.

## **Tutorial 2: Numeric Springing**

### 28/10/19

I Spent some time looking into numeric *Ease* functions, I couldn't wrap my head around how to use them to change a value over a set time. I found a repositery containing a great list of Ease functions: [Space puppy Easing](https://github.com/lordofduct/spacepuppy-unity-framework/blob/master/SpacepuppyBase/Tween/Easing.cs "Easing functions"). I used a slightly modified version of his mathematics *Elastic Out* ease. Most of the functions can be visualized on this website: [https://easings.net](https://easings.net "https://easings.net").

I changed the parameters to keep only four of them: *Current Time, Initial Value, Amount of Change, Duration*. It took some time before I got the first result, apply this *Ease* on a `Vector3 position` to get a spring movement of an object.

From there it was a couple of changes to apply it to the scale. The problem was that it would apply the same value to all three *parameters* of the `localScale`. I had to divide the scale in a new *variable*, apply each value through the *springing* function, then apply the `Vector3` as the new `localScale`.

I added the `StopAllCoroutines();` call, otherwise if the *Coroutine* was called again, *first* it would be using the wrong starting scale, *second* the first coroutine would keep going until it stopped, and only then the second one would use its current value, resulting in a wrong scaling.

Finally for easy use, I added a `modifScale` variable modifiable from the inspector as a factor to the `localScale` for better control.

## **Tutorial 3: Dolly Zoom**

### 07/11/19

To Achieve a *Dolly Zoom* effect, I took the information on the *Wikipedia* page: [en.wikipedia.org/wiki/Dolly_zoom](https://en.wikipedia.org/wiki/Dolly_zoom "Dolly Zoom"). The page shows the main mathematical function that factors the *width* (the constant size that the camera will be able to keep still) with the *distance* (from camera to object in focus). the third argument passed is the *FOV*, Field of view, that will be calculated every frame using *distance* and *width*.

So there is two *functions* that derive from there.

First the calculation for the frustrum *width*. It takes the initial *distance* and the initial *FOV*. By rearranging the distance formula, we get:  
`width = distance x 2tan(1/2 FOV)`  

Then every frame we will recalculate the *distance*, then calculate the FOV through the same rearranged formula:  
`FOV = 2 x Atan(width * 0.5f / distance)`

The effect is best achieved with a front facing camera rather than a top down view of the object.

## **Tutorial 4: Animation Events**

### 18/11/19

I could not get the proper size to work for world scale canvas. I was using a small size (5 by 5) with a regular scale of 1. That meant one 5 pixels where as big as 1 world unity, a meter. The solution was the opposite approach, a high size (what your UI in pixel would need to be, I chose $580 \times 220$) and reduce the scale to 0.005. That would insure a world space witdh of 2.9 meters ($580 \times 0.005$) and a height of 1.1 meters ($220 \times 0.005$).

My first event was calling a built-in `SetTrigger` method on the animator, but as I wanted to use a *boolean*, I change it to a custom script method. An animation event can only call method with a maximum of *1* argument, so that's why it can call a Trigger (with only it's name) and not a `SetBool` (which use *name* + *true* or *false*). I created four methods, two to set the animator boolean to true and false, one to play a sound, one to play a particle effect. I set the boolean *ON* on the button click function in the *inspector*, it plays the *Pressed* animation, in the animation I have *three* events: Playing sound, playing particles, setting the boolean back to false.

## **Behaviour Component: Audio Visualizer**

### 08/12/19

The *frequency* available to for humans to hear are from about 20 Hz to 20 kHz (20 000 Hz). The notion of *samples* is to divide this large amount into a shorter list that will check the points at a certain *interval*. The number of samples therefore is to check at every point in time the *amplitude* over the sampled *frequency*. A higher number means more precision but run slower. For our test scene, a sample count at 1024 is a good choice.

Samples can be seen as *frequency resolution*, and calculate the *relative amplitude* at their point. With a 1024 array, we have a resolution of 23.4 hertz, calculated by the total *frequency* range (24kHz) divided by the sample count: 24 000 / 1024. In this tutorial we sample the relative amplitude every 23.4 hertz.

The FFTWindow parameter stands for `Fast Fourier Transfer Window`, FFT being an *algorythm* to convert a signal from *original* domain to *frequency* domain. Unity have some predefined *FFTWindow* to help reduce leakage of signal across the frequency band.

As I wanted to adapt the audio visualizer to only showing *three* bands, I had to recalculate the number of samples to use for every frequency band. I settled for this calculation:

```c#
    int sampleCount = (int)Mathf.Pow(3.5f, i) * 50;
```

As calculated before, every samples hold the *amplitude* over 23.4 hertz. So the first **band** will hold 50 samples(50 \* 3.5^0) which cover 1170 hertz (50 \* 23.4). The second **band** hold 175 samples (50 \* 3.5^1) which is a range of 4095 hertz. Finally the third **band** hold 612 samples, ranging 14 332 hertz.

After looping through each *band* to calculate the average frequency, we multiply to a *scale* to be able to adjust the size in real time if needed.
The movement of the *bands* was a little jarring at this point, so we added a *buffer* to smooth the movement. We now hold *three* values adjusting every frames that we can use.

I decided to use a *slider* to visualize the values as it is easy to access and scale accordingly. I made sure to remove the background, the handle slide, and to uncheck *interactable*. Instead of rotating the GameObject, we can select the *direction* to *Bottom to Top*. To have the slider completely disappearing at zero, we can set its `width` to zero.

From there, we only need to *duplicate* the slider twice more, making it a child of an empty *UI GameObject*, adding the *AudioData* and filling the audio source variable.