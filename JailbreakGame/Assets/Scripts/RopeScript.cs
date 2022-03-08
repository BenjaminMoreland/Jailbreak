using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeScript : MonoBehaviour
{
    public Vector2 desitny;
    public float speed = 1;
    public float distance = 2;
    public GameObject nodePrefab;
    public GameObject player;
    public GameObject LastNode;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        LastNode = transform.gameObject;
    }

    // Update is called once per frame
    void Update()
    {

        transform.position = Vector2.MoveTowards(transform.position, desitny, speed);


        if(transform.position.x != desitny.x && transform.position.y != desitny.y)
        {
            if (Vector2.Distance(player.transform.position, LastNode.transform.position) > distance)
            {
                CreateNode();
            }
        }
    }
    void CreateNode()
    {
        Vector2 pos2Create = player.transform.position - LastNode.transform.position;
        pos2Create.Normalize();
        pos2Create *= distance;
        pos2Create += (Vector2)LastNode.transform.position;
    }
}
