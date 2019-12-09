# **Behaviour Component - Audio Visualizer**

## Resources

This *Audio Visualizer* is based on a tutorial by **Peer Play** available here: [https://www.youtube.com/watch?v=4Av788P9stk](youtube.com/watch?v=4Av788P9stk "Audio Visualization - Unity/C# Tutorial").

The test scene contains *sprites* and *audio* from the *Star Fox* game series.

## Concept

This *Audio Visualizer* is to be used as a *UI element*. It reacts to any played audio sound it is connected to, showing *audio bands* reacting to the *frequency* of the speech.

## How to use

You simply need to drag the `SpeechVisualizer` prefab from the *Project* window into a canvas in the *Hierarchy*. for the size and position, you simply need to modify the `Rect Transform` component on the *SpeechVisualizer* adjusting the *scale* and *position*.

On the `AudioData` script component, fill the *Source Audio* with a GameObject containing the `AudioSource` to visualize.
You can change the `Scale` variable to scale up the effects as desired (can be done in real time).