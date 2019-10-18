﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public static List<PlayerCamera> playerCameras = new List<PlayerCamera>();
    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        playerCameras.Add(this);
    }

    // Update is called once per frame
    void Update()
    {
        FollowPlayer();
    }

    public void AssignPlayer(Player p)
    {
        player = p;
    }

    private void FollowPlayer()
    {
        if (System.Math.Abs(Quaternion.Angle(player.transform.rotation, this.transform.rotation)) > Mathf.Epsilon)
        {
            this.transform.position = Vector3.Lerp(this.transform.position, player.transform.GetChild(0).transform.position, 5 * Time.deltaTime);
        }
        else
        {
            this.transform.position = player.transform.GetChild(0).transform.position;
            if (player.RotatingPlayerInPlace)
            {//Ends Player.RotatePlayer Coroutine
                player.RotatingPlayerInPlace = false;
            }
            if (player.RotatingMap)
            {//Ends Player.RotatingMap Coroutine
                player.RotatingMap = false;
            }
        }
        this.transform.LookAt(player.transform.position);
    }
}