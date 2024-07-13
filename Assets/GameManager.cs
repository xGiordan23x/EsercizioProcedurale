using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Grid Generator")]
    [SerializeField] GestionalGridGenerator gridGenerator;

    [Header("Camera settings")]
    [SerializeField] Camera mainCamera;
    [SerializeField] float cameraSpeed;
    [SerializeField] float fovSpeed;
    [SerializeField] float minFov;
    [SerializeField] float maxFov;
    [SerializeField] GameObject cameraTarget;


    private TileType selectedType;

    private void Start()
    {
        mainCamera.transform.position = new Vector3(gridGenerator.gridWidth / 2, gridGenerator.gridHeight > gridGenerator.gridWidth ? gridGenerator.gridHeight : gridGenerator.gridWidth, gridGenerator.gridHeight / 2);
        GameObject gridCenter = new GameObject("GridCenter");
        cameraTarget = gridCenter;
        gridCenter.transform.position = new Vector3(gridGenerator.gridWidth / 2, gridGenerator.gameObject.transform.position.y, gridGenerator.gridHeight / 2);
        selectedType = TileType.road;
    }

    private void Update()
    {
        //Camera control rotella
        if (Input.GetMouseButton(2))
        {
            //Rotate
            mainCamera.transform.RotateAround(cameraTarget.transform.position, mainCamera.transform.up, Input.GetAxis("Mouse X") * cameraSpeed);
            mainCamera.transform.RotateAround(cameraTarget.transform.position, mainCamera.transform.right, Input.GetAxis("Mouse Y") * cameraSpeed);

            //Zoom
            float fov = mainCamera.fieldOfView;
            fov += Input.GetAxis("Mouse ScrollWheel") * fovSpeed;
            fov = Mathf.Clamp(fov, minFov, maxFov);
            mainCamera.fieldOfView = fov;
        }


        //change tIles clic sinistro
        if (Input.GetMouseButton(0))
        {
            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray.origin, ray.direction, out var hitInfo))
            {
                if (hitInfo.collider.gameObject.TryGetComponent<GestionalTile>(out GestionalTile hittedTile) && !hittedTile.isOccupiedRoad && !hittedTile.isOccupiedGrass)
                {
                    
                    hittedTile.ChangeMesh(selectedType);
                    hittedTile.CheckNearTiles();

                    if(hittedTile.roadObjectBehavior != null){
                        hittedTile.roadObjectBehavior.ChooseTileToTransform();
                    }
                    if (hittedTile.grassObjectBehavior != null)
                    {
                        hittedTile.grassObjectBehavior.SetNature();
                    }
                }
            }
        }

        //eliminaTile clic destro
        if (Input.GetMouseButton(1))
        {
            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray.origin, ray.direction, out var hitInfo))
            {
                if (hitInfo.collider.gameObject.TryGetComponent<GestionalTile>(out GestionalTile hittedTile) && (hittedTile.isOccupiedRoad || hittedTile.isOccupiedGrass))
                {
                    hittedTile.isOccupiedRoad = false;
                    hittedTile.isOccupiedGrass = false;
                    hittedTile.ChangeMesh(TileType.empty);
                    hittedTile.CheckNearTiles();
                   
                }
            }

        }

        //cambio Materiali
        if (Input.GetKeyDown(KeyCode.Alpha1)){
            selectedType = TileType.grass;
            Debug.LogWarning("grass type selected");
        }

        //cambio Materiali
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedType = TileType.road;
            Debug.LogWarning("road type selected");
        }

        //Export
        

    }
}
