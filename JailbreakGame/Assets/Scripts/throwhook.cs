using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class throwhook : MonoBehaviour
{
    public GameObject hook;
    GameObject curHook;
    public bool RopeActive;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (RopeActive == false)
            {


                Vector2 destiny = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                curHook = (GameObject)Instantiate(hook, transform.position, Quaternion.identity);

                curHook.GetComponent<RopeScript>().destiny = destiny;

                RopeActive = true;
            }
            else
            {
                Destroy(curHook);
                RopeActive = false;
            }
        }
    }
}