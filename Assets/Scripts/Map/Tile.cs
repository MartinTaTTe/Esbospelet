using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    public Transform transform;
    public Map.Type tileType;
    public int x;
    public int y;
    public int rotation;//1-6
    public int prefabId;
    public Map.InfrastructureType infrastructureType;
    public Map.ContentTileType resourceType;

    public Tile(int x, int y, Map.Type tileType, Transform transform, int rotation = 1, Map.InfrastructureType infrastructureType = Map.InfrastructureType.None)
    {
        this.transform = transform;
        this.x = x;
        this.y = y;
        this.tileType = tileType;
        this.rotation = rotation;
        this.infrastructureType = infrastructureType;
    }

    public void SetResourceType(Map.ContentTileType resourceType)
    {
        this.resourceType = resourceType;
    }
}
