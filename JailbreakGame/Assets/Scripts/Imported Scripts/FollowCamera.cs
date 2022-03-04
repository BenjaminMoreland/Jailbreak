using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [Tooltip("Drag the object from the hierarchy tab into the slot that you wabt the camera to follow.")]
    public GameObject Target;
    [Tooltip("Set between 0 and 1. Kinda how fast it moves to the target."), Range(0,1)]
    public float LerpVal = 0.8f;

    [HideInInspector]
    private bool faceR;
    [HideInInspector]
    private float moveInput;

    float ShakeTime = 0;
    float ShakeMagnitude = 0;
    //call this function to make the screen shake
    public void TriggerShake(float time, float amount)
    {
        if (ShakeTime < time)
        {
            ShakeTime = time;
        }
        if(ShakeMagnitude < amount)
        {
            ShakeMagnitude = amount;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Code by: Logan Laurance
        // Grabs the facingRight boolean variable from the controller to start synced
        faceR = FindObjectOfType<MainPlayerController>();
    }
    //fixed update runs once per physics frame
    private void FixedUpdate()
    {
        if (Target != null)
        {
            // Code by: Logan Laurance
            // If the player is facing right, it will pan over to the right to show more of that area, and vice versa if the player is facing left
            if(faceR)
            {
                Vector3 newPos = Target.transform.position + new Vector3 (4,1.75f,0);
                newPos.z = transform.position.z;
                transform.position = Vector3.Lerp(transform.position, newPos, LerpVal);
            }
            else if(!faceR)
            {
                Vector3 newPos = Target.transform.position - new Vector3 (4,-1.75f,0);
                newPos.z = transform.position.z;
                transform.position = Vector3.Lerp(transform.position, newPos, LerpVal);
            }
        }

        if (ShakeTime > 0)
        {
            ShakeTime -= Time.deltaTime;
            Vector3 randDir = Random.insideUnitCircle * ShakeMagnitude;
            transform.position += randDir;
        }
        else
        {
            ShakeMagnitude = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Code by: Logan Laurance
        // Checks if either the D key or A key are pressed, and updates the faceR variable so the camera is synced with the player
        if(Input.GetKeyDown(KeyCode.D))
        {
            faceR = true;
        }
        if(Input.GetKeyDown(KeyCode.A))
        {
            faceR = false;
        }
        // Test shake key, will find a use for this at some point
        if(Input.GetKeyDown(KeyCode.Space))
        {
            TriggerShake(0.2f, 0.2f);
        }
    }
}