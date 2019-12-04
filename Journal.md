# **Journal Tutorials**

## *Tutorial 1: Getting Audio Data*

### 15/10/19

The *frequency* available to for humans to hear are from about 20 Hz to 20 kHz (20 000 Hz). The notion of *samples* is to divide this large amount into a shorter list that will check the points at a certain *interval*. The number of samples therefore is to check at every point in time the *amplitude* over the sampled *frequency*. a Higher number means more precision but run slower. For our tutorial, a sample count at 1024 is a good choice. 

Samples can be seen as *frequency resolution*, and calculate the *relative amplitude* at their point. With a 1024 array, we have a resolution of 23.4 hertz, calculated by the total *frequency* range (24kHz) divided by the sample count: 24 000 / 1024. In this tutorial we sample the relative amplitude every 23.4 hertz.

The FFTWindow parameter stands for `Fast Fourier Transfer Window`, FFT being an *algorythm* to convert a signal from *original* domain to *frequency* domain. Unity have some predefined *FFTWindow* to help reduce leakage of signal across the frequency band.

## *Tutorial 2: Numeric Springing*

### 28/10/19

`What I did not know was that the *amplitude* gets lower the higher the frequency, meaning from 20Hz to 60Hz, we get more amplitude than from 1000Hz to 2000Hz and it keeps getting`

I Spent some time looking into numeric *Ease* functions, I couldn't wrap my head around how to use them to change a value over a set time. I found a repositery containing a great list of Ease functions: [Space puppy Easing](https://github.com/lordofduct/spacepuppy-unity-framework/blob/master/SpacepuppyBase/Tween/Easing.cs "Easing functions"). I used a slightly modified version of his mathematics *Elastic Out* ease. Most of the functions can be visualized on this website: [https://easings.net](https://easings.net "https://easings.net").

I changed the parameters to keep only four of them: *Current Time, Initial Value, Amount of Change, Duration*. It took some time before I got the first result, apply this *Ease* on a `Vector3 position` to get a spring movement of an object.

From there it was a couple of changes to apply it to the scale. The problem was that it would apply the same value to all three *parameters* of the `localScale`. I had to divide the scale in a new *variable*, apply each value through the *springing* function, then apply the `Vector3` as the new `localScale`.

I added the `StopAllCoroutines();` call, otherwise if the *Coroutine* was called again, *first* it would be using the wrong starting scale, *second* the first coroutine would keep going until it stopped, and only then the second one would use its current value, resulting in a wrong scaling.

Finally for easy use, I added a `modifScale` variable modifiable from the inspector as a factor to the `localScale` for better control.

## *Tutorial 3: Dolly Zoom*

### 07/11/19

To Achieve a *Dolly Zoom* effect, I took the information on the *Wikipedia* page: [en.wikipedia.org/wiki/Dolly_zoom](https://en.wikipedia.org/wiki/Dolly_zoom "Dolly Zoom"). The page shows the main mathematical function that factors the *width* (the constant size that the camera will be able to keep still) with the *distance* (from camera to object in focus). the third argument passed is the *FOV*, Field of view, that will be calculated every frame using *distance* and *width*.

So there is two *functions* that derive from there.

First the calculation for the frustrum *width*. It takes the initial *distance* and the initial *FOV*. By rearranging the distance formula, we get:  
`width = distance x 2tan(1/2 FOV)`  

Then every frame we will recalculate the *distance*, then calculate the FOV through the same rearranged formula:  
`FOV = 2 x Atan(width * 0.5f / distance)`

The effect is best achieved with a front facing camera rather than a top down view of the object.

## *Tutorial 4: Animation Events*

### 18/11/19

I could not get the proper size to work for world scale canvas. I was using a small size (5 by 5) with a regular scale of 1. That meant one 5 pixels where as big as 1 world unity, a meter. The solution was the opposite approach, a high size (what your UI in pixel would need to be, I chose $580 \times 220$) and reduce the scale to 0.005. That would insure a world space witdh of 2.9 meters ($580 \times 0.005$) and a height of 1.1 meters ($220 \times 0.005$).

My first event was calling a built-in `SetTrigger` method on the animator, but as I wanted to use a *boolean*, I change it to a custom script method. An animation event can only call method with a maximum of *1* argument, so that's why it can call a Trigger (with only it's name) and not a `SetBool` (which use *name* + *true* or *false*). I created four methods, two to set the animator boolean to true and false, one to play a sound, one to play a particle effect. I set the boolean *ON* on the button click function in the *inspector*, it plays the *Pressed* animation, in the animation I have *three* events: Playing sound, playing particles, setting the boolean back to false.
