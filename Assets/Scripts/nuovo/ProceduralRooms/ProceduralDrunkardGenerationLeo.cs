using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class ProceduralDrunkardGenerationLeo : MonoBehaviour
{
    [SerializeField] List<GameObject> roomPrefabs = new List<GameObject>();
    [SerializeField] int roomCount = 10;
    [SerializeField, Min(1)] float minRoomDistance = 1f;
    [SerializeField] Transform startPivot;
    [SerializeField] bool clearRoomOnNewGeneration = true;

    [SerializeField] List<GameObject> spawnedRooms = new();
    [SerializeField] List<Vector3> validPositions = new();
    Vector3 currentPosition;

    private void Awake()
    {
        currentPosition = startPivot.position;
    }

    private void Start()
    {
        if (clearRoomOnNewGeneration)
        {
            ClearRoomsSpawned();
        }

        GenerateRooms();
        PopulateRooms();
    }

    private void ClearRoomsSpawned()
    {
        bool immediate = false;

#if UNITY_EDITOR
        immediate = true;
#endif
        foreach (GameObject room in spawnedRooms)
        {
            if (immediate)
                DestroyImmediate(room);
            else
                Destroy(room);
        }
        spawnedRooms.Clear();
        validPositions.Clear();
        currentPosition = startPivot.position;
    }

    private void GenerateRooms()
    {
        if (spawnedRooms == null)
            spawnedRooms = new();
        else if (clearRoomOnNewGeneration)
            spawnedRooms.Clear();

        for (int i = 0; i < roomCount; i++)
        {
            Vector3 validPosition = GetValidPosition();
            GameObject newRoom = Instantiate(roomPrefabs[Random.Range(0, roomPrefabs.Count)], validPosition, Quaternion.identity);

            spawnedRooms.Add(newRoom);
            validPositions.Add(validPosition);
        }
    }

    private Vector3 GetValidPosition()
    {
        do
        {
            currentPosition = GetDrunkardPosition(currentPosition, (int)minRoomDistance);
        } while (!IsPositionValid(currentPosition));

        return currentPosition;
    }

    public static Vector3 GetDrunkardPosition(Vector3 currentPos, int minDistance)
    {
        Vector3 direction;
        int distance;

        do
        {
            direction = new Vector3Int(Random.Range(-1, 2), 0, Random.Range(-1, 2));
            distance = Random.Range(-minDistance, minDistance);
        } while (direction.magnitude == 0);

        currentPos += direction * distance;
        return currentPos;
    }

    private bool IsPositionValid(Vector3 testPosition)
    {
        if (validPositions.Contains(testPosition))
            return false;

        foreach (Vector3 validPosition in validPositions)
        {
            if (Vector3.Distance(testPosition, validPosition) < minRoomDistance)
                return false;
        }

        return true;
    }


    private void PopulateRooms()
    {
        spawnedRooms.Select(x => x.GetComponent<RoomHandler>()).ToList().ForEach(r => r.Initialize());
    }


#if UNITY_EDITOR
    public void GenerateFromEditor()
    {
        if (clearRoomOnNewGeneration)
        {
            ClearRoomsSpawned();
        }

        GenerateRooms();
    }
#endif

}
