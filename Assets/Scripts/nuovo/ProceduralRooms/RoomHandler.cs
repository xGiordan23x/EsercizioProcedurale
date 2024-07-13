using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomHandler : MonoBehaviour
{
    [Header("Self References")]
    [SerializeField] MeshCollider tileableFloor;

    [Header("Asset References")]
    [SerializeField] WorldTileHandler worldTileHandlerPrefab;
    [SerializeField] List<RoomObjectHandler> roomObjectsPrefabList;

    [Header("Settings")]
    [SerializeField] int minObjectCount;
    [SerializeField] int maxObjectCount;
    [SerializeField] int placementTries;

    public WorldTileHandler[,] roomTiles;

   


    public void Initialize()
    {
        Vector3 floorSize = tileableFloor.bounds.size;
        Vector3 floorMin = tileableFloor.bounds.min;
        Vector3 tileSize = worldTileHandlerPrefab.size;

        int tilesX = Mathf.FloorToInt(floorSize.x / tileSize.x);
        int tilesZ = Mathf.FloorToInt(floorSize.z / tileSize.z);

        roomTiles = new WorldTileHandler[tilesX, tilesZ];

        for (int x = 0; x <tilesX; x++)
        {
            for(int z =0; z < tilesZ; z++)
            {
                Vector3 tilePosition = floorMin + new Vector3(tileSize.x * x + tileSize.x /2 , 0, tileSize.z * z + tileSize.z /2);
                var newTile = Instantiate(worldTileHandlerPrefab,tilePosition,Quaternion.identity);
                newTile.transform.SetParent(transform);
                roomTiles[x,z] = newTile;
            }       
        }

        
    }

    
}
