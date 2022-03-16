using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeFriend : MonoBehaviour
{
    public FollowPlayer friend;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            friend.render.enabled = true;
            friend.free = true;
        }
    }
}
