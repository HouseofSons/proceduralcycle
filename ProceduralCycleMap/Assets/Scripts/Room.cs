﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public static List<Room> Rooms { get; } = new List<Room>();
    public List<Door> Doors { get; private set; } = new List<Door>();
    public List<Vector3Int> Path { get; set; } = new List<Vector3Int>();
    public int Order { get; private set; }
    public bool HasCycle;
    public List<Vector3Int> GameGridPosition { get; set; }
    public List<FloorTile> Floor { get; set; }

    public void Awake()
    {
        HasCycle = false;
        Order = Rooms.Count;
        GameGridPosition = new List<Vector3Int>();
        Floor = new List<FloorTile>();
    }

    public void AddDoor(Room neighbor)
    {
        if (NoDoorExists(neighbor))
        {
            Door d = new Door(this, neighbor);
            Doors.Add(d);
            if (this != neighbor)
            {
                neighbor.Doors.Add(d);
            }
        }
    }

    public bool NoDoorExists(Room neighbor)
    {
        if(neighbor == this)
        {
            return true;
        }
        foreach(Door d in Doors)
        {
            if(d.RoomFirst == neighbor || d.RoomSecond == neighbor)
            {
                return false;
            }
        }
        return true;
    }

    public List<Room> GetNeighbors()
    {
        List<Room> neighbors = new List<Room>();

        foreach (Door d in Door.Doors)
        {
            if (d.RoomFirst == this)
            {
                neighbors.Add(d.RoomSecond);
            }
            if (d.RoomSecond == this)
            {
                neighbors.Add(d.RoomFirst);
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

    public void InitializeFloorTiles()
    {
        foreach (Vector3Int v in GameGridPosition)
        {
            Floor.Add(new FloorTile(this,v));
        }
    }

    public FloorTile GetFloorTile(Vector3Int v)
    {
        foreach (FloorTile ft in Floor)
        {
            if (v == ft.GameGridLocation)
            {
                return ft;
            }
        }
        return null;
    }
}