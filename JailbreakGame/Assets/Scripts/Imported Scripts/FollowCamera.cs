using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [Tooltip("Drag the object from the hierarchy tab into the slot that you wabt the camera to follow.")]
    public GameObject Target;
    [Tooltip("Set between 0 and 1. Kinda how fast it moves to the target."), Range(0,1)]
    public float LerpVal = 0.8f;

    private bool faceR;

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
        faceR = FindObjectOfType<MainPlayerController>();
    }
    //fixed update runs once per physics frame
    private void FixedUpdate()
    {
        if(Target != null)
        {
            if(faceR)
            {
                Vector3 newPos = Target.transform.position + new Vector3 (3,1,0);
                newPos.z = transform.position.z;
                transform.position = Vector3.Lerp(transform.position, newPos, LerpVal);
            }
            else
            {
                Vector3 newPos = Target.transform.position + new Vector3 (-3,1,0);
                newPos.z = transform.position.z;
                transform.position = Vector3.Lerp(transform.position, newPos, LerpVal);
            }
            //lerp towards the camera to make a smoothing effect in the movement
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
        if(Input.GetKeyDown(KeyCode.Space))
        {
            TriggerShake(0.2f, 0.2f);
        }
    }
}