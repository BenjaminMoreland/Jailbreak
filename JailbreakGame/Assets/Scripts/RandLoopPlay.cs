using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

// Code by: Logan Laurance
// Meant for usage on Chain Clatter Ambience to randomly play it whenever
public class RandLoopPlay : MonoBehaviour
{
    private AudioSource gAud;
    public AudioClip chainAud;
    // Start is called before the first frame update
    void Start()
    {
        gAud = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //default playback method for testing
        if (Input.GetKeyDown(KeyCode.P))
        {
            PlaySound();
        }
    }

    public void PlaySound()
    {
        int randomPlay = Random.Range(1, 2);

        if(randomPlay == 2)
        {
            gAud.PlayOneShot(chainAud);
        }
    }
}
