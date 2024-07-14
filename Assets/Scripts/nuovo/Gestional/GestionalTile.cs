using UnityEngine;

public enum TileType
{
    empty,
    road,
    highway,
    crossRoad,
    tCrossroad,
    turn,
    grass
}
public class GestionalTile : MonoBehaviour
{
    public float squareDimension;
    public bool isOccupiedRoad;
    public bool isOccupiedGrass;
    public MeshCollider tileCollider;
    public MeshRenderer tileRenderer;
    public GestionalGridGenerator gridGenerator;

    public RoadBehavior roadObjectBehavior;
    public GrassBehavior grassObjectBehavior;
    private GameObject roadObject;
    private GameObject grassObject;
    public TileType tileType;

    public bool northOccupied;
    public bool southOccupied;
    public bool eastOccupied;
    public bool westOccupied;
    public bool northEastOccupied;
    public bool southWestOccupied;
    public bool southEastOccupied;
    public bool northWestOccupied;

    public GestionalTile northTile;
    public GestionalTile southTile;
    public GestionalTile eastTile;
    public GestionalTile westTile;
    public GestionalTile northEastTile;
    public GestionalTile southWestTile;
    public GestionalTile southEastTile;
    public GestionalTile northWestTile;


    private bool isHighway;
    private bool isCrossroad;
    private bool isTCrossroad;
    private bool isTurn;




    public void ChangeMesh(TileType type)
    {
        tileType = type;

        if (roadObjectBehavior == null && tileType == TileType.road)
        {
            roadObject = Instantiate(gridGenerator.roadPrefab, transform);
            roadObject.transform.localScale = new Vector3(squareDimension, squareDimension / 10, squareDimension);
            roadObjectBehavior = roadObject.GetComponent<RoadBehavior>();
            roadObjectBehavior.SetVariables(squareDimension, this);
            isOccupiedRoad = true;
        }
        if (grassObjectBehavior == null && tileType == TileType.grass)
        {
            grassObject = Instantiate(gridGenerator.grassPrefab, transform);
            grassObject.transform.localScale = new Vector3(squareDimension, squareDimension / 10, squareDimension);
            grassObjectBehavior = grassObject.GetComponent<GrassBehavior>();
            grassObjectBehavior.SetVariables(squareDimension, this);
            isOccupiedGrass = true;
        }

        if (roadObjectBehavior != null)
        {
            roadObjectBehavior.SetType(type);
            roadObjectBehavior.SelectCorrectMesh();
        }
        if (grassObjectBehavior != null)
        {
            grassObjectBehavior.SetNature();
        }




        if (type == TileType.empty)
        {
            Destroy(roadObject);
            roadObjectBehavior = null;


            Destroy(grassObject);
            if (grassObjectBehavior != null)
            {
                grassObjectBehavior.DestroyNature();
                grassObjectBehavior = null;
            }
        }



    }
    public void CheckNearTiles()
    {
        if (isOccupiedRoad)
        {
            eastOccupied = CheckEastTile(true);
            northOccupied = CheckNorthTile(true);
            southOccupied = CheckSouthTile(true);
            westOccupied = CheckWestTile(true);
            northWestOccupied = CheckNorthWestTile(true);
            northEastOccupied = CheckNorthEastTile(true);
            southWestOccupied = CheckSouthWestTile(true);
            southEastOccupied = CheckSouthEastTile(true);
        }
        else
        {
            eastOccupied = CheckEastTile(false);
            northOccupied = CheckNorthTile(false);
            southOccupied = CheckSouthTile(false);
            westOccupied = CheckWestTile(false);
            northWestOccupied = CheckNorthWestTile(false);
            northEastOccupied = CheckNorthEastTile(false);
            southWestOccupied = CheckSouthWestTile(false);
            southEastOccupied = CheckSouthEastTile(false);
        }

    }

    public void CLearTile()
    {
        ChangeMesh(TileType.empty);
        northOccupied = false;
        southOccupied = false;
        westOccupied = false;
        eastOccupied = false;
        northWestOccupied = false;
        northEastOccupied = false;
        southWestOccupied = false;
        southEastOccupied = false;


    }




    private bool CheckEastTile(bool isOccupied)
    {

        if (Physics.Raycast(new Vector3(transform.position.x + squareDimension / 2 + squareDimension, transform.position.y + squareDimension / 2, transform.position.z + squareDimension / 2), Vector3.down, out var hitInfo))
        {
            if (hitInfo.collider.gameObject.TryGetComponent<GestionalTile>(out GestionalTile hittedTile))
            {

                if (hittedTile.roadObjectBehavior != null)
                {
                    hittedTile.westOccupied = isOccupied;
                    eastTile = hittedTile;
                    hittedTile.roadObjectBehavior.ChooseTileToTransform();
                    
                }
                return hittedTile.isOccupiedRoad;

            }
        }
        Debug.LogWarning("Non ho trovato tile a est");
        return false;
    }
    private bool CheckWestTile(bool isOccupied)
    {
        if (Physics.Raycast(new Vector3(transform.position.x + squareDimension / 2 - squareDimension, transform.position.y + squareDimension / 2, transform.position.z + squareDimension / 2), Vector3.down, out var hitInfo))
        {
            if (hitInfo.collider.gameObject.TryGetComponent<GestionalTile>(out GestionalTile hittedTile))
            {
                if (hittedTile.roadObjectBehavior != null)
                {

                    hittedTile.eastOccupied = isOccupied;
                    westTile = hittedTile;
                    hittedTile.roadObjectBehavior.ChooseTileToTransform();
                  
                }

                return hittedTile.isOccupiedRoad;
            }
        }
        Debug.LogWarning("Non ho trovato tile a ovest");
        return false;
    }
    private bool CheckNorthTile(bool isOccupied)
    {
        if (Physics.Raycast(new Vector3(transform.position.x + squareDimension / 2, transform.position.y + squareDimension / 2, transform.position.z + squareDimension / 2 + squareDimension), Vector3.down, out var hitInfo))
        {
            if (hitInfo.collider.gameObject.TryGetComponent<GestionalTile>(out GestionalTile hittedTile))
            {
                if (hittedTile.roadObjectBehavior != null)
                {

                    hittedTile.southOccupied = isOccupied;
                    northTile = hittedTile;
                    hittedTile.roadObjectBehavior.ChooseTileToTransform();
                   
                }


                return hittedTile.isOccupiedRoad;
            }
        }
        Debug.LogWarning("Non ho trovato tile a nord");
        return false;
    }
    private bool CheckSouthTile(bool isOccupied)
    {
        if (Physics.Raycast(new Vector3(transform.position.x + squareDimension / 2, transform.position.y + squareDimension / 2, transform.position.z + squareDimension / 2 - squareDimension), Vector3.down, out var hitInfo))
        {
            if (hitInfo.collider.gameObject.TryGetComponent<GestionalTile>(out GestionalTile hittedTile))
            {
                if (hittedTile.roadObjectBehavior != null)
                {
                    hittedTile.northOccupied = isOccupied;
                    southTile = hittedTile;
                    hittedTile.roadObjectBehavior.ChooseTileToTransform();
                    
                }

                return hittedTile.isOccupiedRoad;
            }
        }
        Debug.LogWarning("Non ho trovato tile a sud");
        return false;
    }
    private bool CheckNorthEastTile(bool isOccupied)
    {

        if (Physics.Raycast(new Vector3(transform.position.x + squareDimension / 2 + squareDimension, transform.position.y + squareDimension / 2, transform.position.z + squareDimension / 2 + squareDimension), Vector3.down, out var hitInfo))
        {
            if (hitInfo.collider.gameObject.TryGetComponent<GestionalTile>(out GestionalTile hittedTile))
            {
                if (hittedTile.roadObjectBehavior != null)
                {
                    hittedTile.southWestOccupied = isOccupied;
                    northEastTile = hittedTile;
                    hittedTile.roadObjectBehavior.ChooseTileToTransform();
                   
                }
                return hittedTile.isOccupiedRoad;

            }
        }
        Debug.LogWarning("Non ho trovato tile a  nord est");
        return false;
    }
    private bool CheckNorthWestTile(bool isOccupied)
    {
        if (Physics.Raycast(new Vector3(transform.position.x + squareDimension / 2 - squareDimension, transform.position.y + squareDimension / 2, transform.position.z + squareDimension / 2 + squareDimension), Vector3.down, out var hitInfo))
        {
            if (hitInfo.collider.gameObject.TryGetComponent<GestionalTile>(out GestionalTile hittedTile))
            {
                if (hittedTile.roadObjectBehavior != null)
                {
                    hittedTile.southEastOccupied = isOccupied;
                    northWestTile = hittedTile;
                    hittedTile.roadObjectBehavior.ChooseTileToTransform();
                    
                }

                return hittedTile.isOccupiedRoad;
            }
        }
        Debug.LogWarning("Non ho trovato tile a nord ovest");
        return false;
    }
    private bool CheckSouthEastTile(bool isOccupied)
    {
        if (Physics.Raycast(new Vector3(transform.position.x + squareDimension / 2 + squareDimension, transform.position.y + squareDimension / 2, transform.position.z + squareDimension / 2 - squareDimension), Vector3.down, out var hitInfo))
        {
            if (hitInfo.collider.gameObject.TryGetComponent<GestionalTile>(out GestionalTile hittedTile))
            {
                if (hittedTile.roadObjectBehavior != null)
                {
                    hittedTile.northWestOccupied = isOccupied;
                    southEastTile = hittedTile;
                    hittedTile.roadObjectBehavior.ChooseTileToTransform();
                   
                }

                return hittedTile.isOccupiedRoad;
            }
        }
        Debug.LogWarning("Non ho trovato tile a soud est");
        return false;
    }
    private bool CheckSouthWestTile(bool isOccupied)
    {
        if (Physics.Raycast(new Vector3(transform.position.x + squareDimension / 2 - squareDimension, transform.position.y + squareDimension / 2, transform.position.z + squareDimension / 2 - squareDimension), Vector3.down, out var hitInfo))
        {
            if (hitInfo.collider.gameObject.TryGetComponent<GestionalTile>(out GestionalTile hittedTile))
            {
                if (hittedTile.roadObjectBehavior != null)
                {
                    hittedTile.northEastOccupied = isOccupied;
                    southWestTile = hittedTile;
                    hittedTile.roadObjectBehavior.ChooseTileToTransform();
                   
                }

                return hittedTile.isOccupiedRoad;
            }
        }
        Debug.LogWarning("Non ho trovato tile a sud ovest");
        return false;
    }




}
