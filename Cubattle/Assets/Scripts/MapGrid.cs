﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGrid
{
    public static Block[,,] Blocks { get; private set; }
    public static int BlockSize { get; private set; }

    public MapGrid(int gridSize, int blockSize)
    {
        Blocks = new Block[gridSize, gridSize, gridSize];
        BlockSize = blockSize;
    }

    public static bool UpdateGridLocation(Block b, Vector3 location)
    {
        Vector3Int nl = new Vector3Int(Mathf.RoundToInt(location.x), Mathf.RoundToInt(location.y), Mathf.RoundToInt(location.z));

        if (nl.x % BlockSize == 0 && nl.y % BlockSize == 0 && nl.z % BlockSize == 0)
        {
            Vector3Int newLocation = new Vector3Int(nl.x / 4, nl.y / 4, nl.z / 4);

            if (newLocation != b.MapGridLocation)
            {
                Blocks[Mathf.RoundToInt(b.MapGridLocation.x), Mathf.RoundToInt(b.MapGridLocation.y), Mathf.RoundToInt(b.MapGridLocation.z)] = null;
                b.PrevMapGridLocation = b.MapGridLocation;
                b.MapGridLocation = newLocation;
                Blocks[newLocation.x, newLocation.y, newLocation.z] = b;
                return true;
            }
        }
        return false;
    }

    public static Block[] GridLocationHasNeighbors(Vector3Int mapGridLocation)
    {
        //+X,-X,+Y,-Y,+Z,-Z
        Block[] neighbors = new Block[6];

        Vector3Int[] neighborCheck = new Vector3Int[6];

        neighborCheck[0] = new Vector3Int(mapGridLocation.x + 1, mapGridLocation.y, mapGridLocation.z);
        neighborCheck[1] = new Vector3Int(mapGridLocation.x - 1, mapGridLocation.y, mapGridLocation.z);
        neighborCheck[2] = new Vector3Int(mapGridLocation.x, mapGridLocation.y + 1, mapGridLocation.z);
        neighborCheck[3] = new Vector3Int(mapGridLocation.x, mapGridLocation.y - 1, mapGridLocation.z);
        neighborCheck[4] = new Vector3Int(mapGridLocation.x, mapGridLocation.y, mapGridLocation.z + 1);
        neighborCheck[5] = new Vector3Int(mapGridLocation.x, mapGridLocation.y, mapGridLocation.z - 1);

        for (int i = 0; i < neighborCheck.Length; i++)
        {
            if (!(neighborCheck[i].x < 0 || neighborCheck[i].x > LevelManager.GridSize - 1 ||
                neighborCheck[i].y < 0 || neighborCheck[i].y > LevelManager.GridSize - 1 ||
                neighborCheck[i].z < 0 || neighborCheck[i].z > LevelManager.GridSize - 1))
            {
                neighbors[i] = Blocks[neighborCheck[i].x, neighborCheck[i].y, neighborCheck[i].z];
            }
        }
        return neighbors;
    }
}
