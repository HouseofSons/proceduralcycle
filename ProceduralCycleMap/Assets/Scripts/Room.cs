﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public static List<Room> Rooms { get; } = new List<Room>();
    public List<Door> Doors { get; private set; } = new List<Door>();
    public int Order { get; private set; }
    public bool HasCycle;
    public List<Vector3Int> GameGridPosition { get; set; }

    public void Awake()
    {
        HasCycle = false;
        Order = Rooms.Count;
    }

    public void Start()
    {

    }

    public void Update()
    {

    }

    public void AddDoor(Room neighbor)
    {
        Doors.Add(new Door(this, neighbor));
    }

    public List<Room> GetNeighbors()
    {
        List<Room> neighbors = new List<Room>();

        foreach(Door d in Door.Doors)
        {
            if(d.RoomOne == this)
            {
                neighbors.Add(d.RoomTwo);
            }
            if (d.RoomTwo == this)
            {
                neighbors.Add(d.RoomOne);
            }
        }
        return neighbors;
    }

    public Vector2Int RoomSize()
    {
        int xMin = GameGridPosition[0].x;
        int xMax = GameGridPosition[0].x;
        int zMin = GameGridPosition[0].z;
        int zMax = GameGridPosition[0].z;

        foreach (Vector3Int v in GameGridPosition)
        {
            xMin = v.x < xMin ? v.x : xMin;
            zMin = v.z < zMin ? v.z : zMin;
            xMax = v.x > xMax ? v.x : xMax;
            zMax = v.z > zMax ? v.z : zMax;
        }
        return new Vector2Int(xMax - xMin + 1, zMax - zMin + 1);
    }

    public Vector3 RoomPosition()
    {
        Vector3 newPosition = Vector3.zero;

        foreach (Vector3Int v in GameGridPosition)
        {
            newPosition += v;
        }
        newPosition /= GameGridPosition.Count;
        newPosition *= ProceduralMapController.ROOM_SCALE;
        return newPosition;
    }
}