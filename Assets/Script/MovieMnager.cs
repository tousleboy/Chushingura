using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovieMnager : MonoBehaviour
{
    public string movieName;
    Animator animator;
    AudioSource soundPlayer;
    public AudioClip[] sounds;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        soundPlayer = GetComponent<AudioSource>();

        animator.Play(movieName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Sound(int i)
    {
        soundPlayer.PlayOneShot(sounds[i]);
    }
}
