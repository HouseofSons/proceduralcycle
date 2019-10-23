﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public static List<Block> Blocks = new List<Block>();
    public Vector3Int CurrentMapGridLocation { get; set; }

    public Block[] FaceBlocks = new Block[4];
    public bool Cloned { get; set; }

    protected virtual void Start()
    {
        Blocks.Add(this);
        MapGrid.InitializeGridLocation(this);
    }

    void Update()
    {

    }

    public void UpdateBlockColliders()
    {
        if (this.GetType() == typeof(InsideBlock))
        {
            bool[] sidesEnabled = MapGrid.GridLocationHasBlockNeighbors(this, CurrentMapGridLocation);

            for (int i = 0; i < sidesEnabled.Length; i++)
            {
                this.gameObject.transform.GetChild(i).GetComponent<MeshCollider>().enabled = !sidesEnabled[i];//inside
                //Debug.Log(this.gameObject.name + " wall: " + this.gameObject.transform.GetChild(0).GetChild(i).name + " bool: " + false);
            }
        }
    }

    public static void UpdateAllColliders()
    {
        foreach (Block b in Blocks)
        {
            b.UpdateBlockColliders();
        }
    }
}