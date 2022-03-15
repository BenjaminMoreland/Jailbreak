using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

// Code by: Logan Laurance
// Meant for usage on Chain Clatter Ambience to randomly play it whenever
public class RandLoopPlay : MonoBehaviour
{
    private AudioSource gAud;
    // Start is called before the first frame update
    void Start()
    {
        gAud = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        int randNum = Random.Range(0, 480);
        if(randNum == 55)
        {
            gAud.Play();
        }
    }
}
