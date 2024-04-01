using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow_Player : MonoBehaviour
{

    //public Transform player;
    //private Vector3 offset = new Vector3(0, 7.5f, -10); //offset from the player

  
    //void Start()
    //{

    //    if (player == null)
    //    {
    //        // Try to find the player by tag
    //        FindPlayer();
    //    }
    //    transform.position = player.position + offset;
    //}

    //void LateUpdate()
    //{
    //    if (player != null)
    //    {
    //        //logic for camera to "follow" player
    //        Vector3 desiredPosition = player.position + offset;
    //        desiredPosition.x = 0;
    //        transform.position = Vector3.Lerp(transform.position, desiredPosition, 0.1f);
    //    } else {
    //        FindPlayer();
    //    }

    //}

    //void FindPlayer()
    //{
    //    GameObject playerGameObject = GameObject.FindGameObjectWithTag("Player");
    //    if (playerGameObject != null)
    //    {
    //        player = playerGameObject.transform;
    //    }
    //}
}
