using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinCondition : MonoBehaviour
{
    public FollowPlayer friend;
    public string Win = "WinEnding";
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && friend.free)
        {
            SceneManager.LoadScene(Win);
        }
    }
}
