using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ProceduralDrunkardGeneration : MonoBehaviour
{
    [SerializeField] List<GameObject> roomPrefabList;
    [SerializeField] int roomCount;
    [SerializeField] float minRoomDistance;
    [SerializeField] Transform startPivot;
    
    public bool clearRoomOnNewGeneration;

    List<GameObject> _spawnedRooms;
    List<Vector3> _validPositions;
    Vector3 _currentPosition;


    private void Awake()
    {
        _currentPosition = startPivot.position;
        _validPositions = new List<Vector3>();
    }
    private void Start()
    {
        if(clearRoomOnNewGeneration && _spawnedRooms.Count > 0)
        {
            ClearSpawnedRooms();
            
        }
        GenerateRooms();
        PopulateRooms();


    }

    public void PopulateRooms()
    {
        _spawnedRooms.Select(x => x.GetComponent<RoomHandler>()).ToList().ForEach(r => r.Initialize());
    }

    private void ClearSpawnedRooms()
    {
        if (_spawnedRooms.Count > 0)
        {
            foreach (var item in _spawnedRooms)
            {
                DestroyImmediate(item);
            }
        }
        _spawnedRooms.Clear();
        _validPositions.Clear();
        _currentPosition = startPivot.position;
    }

    private void GenerateRooms()
    {
        if (_spawnedRooms == null)
            _spawnedRooms = new List<GameObject>();

        else if (clearRoomOnNewGeneration)
        {
            //TODO
            _spawnedRooms.Clear();
        }

        for (int i = 0; i < roomCount; i++)
        {
            
            Vector3 validPosition = GetValidPosition();
            GameObject newRoom = Instantiate(roomPrefabList[UnityEngine.Random.Range(0,roomPrefabList.Count)],validPosition,Quaternion.identity);
            _spawnedRooms.Add(newRoom);
            _validPositions.Add(validPosition);
        }
    }

    private Vector3 GetValidPosition()
    {
       
        do
        {
            _currentPosition = GetDrunkardPosition(_currentPosition,(int)minRoomDistance);
        }
        while (!IsPositionValid(_currentPosition));

        return _currentPosition;
    }

    public static Vector3 GetDrunkardPosition(Vector3 currentPos, int minDistance)
    {
        Vector3 direction;
        int distance;

        do
        {
            direction = new Vector3(UnityEngine.Random.Range(-1, 2), 0, UnityEngine.Random.Range(-1, 2));
            distance = UnityEngine.Random.Range((int)-minDistance, (int)minDistance);
        }
        while (direction.magnitude == 0);

        currentPos += direction * distance;
        return currentPos;

    }

    private bool IsPositionValid(Vector3 testPosition)
    {
        if ((_validPositions.Contains(testPosition)))
            return false;
        
        foreach (var position in _validPositions)
        {
            if(Vector3.Distance(position,testPosition) < minRoomDistance)
             return false;
        }

        return true;
    }


#if UNITY_EDITOR

    public void GenerateRoomsFromEditor()
    {
        if (clearRoomOnNewGeneration)
        {
            ClearSpawnedRooms();
        }

       GenerateRooms();
    }

#endif


}
