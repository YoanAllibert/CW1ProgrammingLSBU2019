using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem particles;

    private AudioSource soundSource;
    private Animator animator;
    
    void Start()
    {
        soundSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }


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
}
