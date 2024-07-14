using System.Collections.Generic;
using System.IO;
using UnityEditor;
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
        CenterCameraToGrid(gridGenerator);
        selectedType = TileType.road;
    }

    private void CenterCameraToGrid(GestionalGridGenerator gridGenerator)
    {
        
        mainCamera.transform.position = new Vector3(gridGenerator.gridWidth / 2, gridGenerator.gridHeight > gridGenerator.gridWidth ? gridGenerator.gridHeight : gridGenerator.gridWidth, gridGenerator.gridHeight / 2);
        mainCamera.transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
        GameObject gridCenter = new GameObject("GridCenter");
        cameraTarget = gridCenter;
        gridCenter.transform.position = new Vector3(gridGenerator.gridWidth / 2, gridGenerator.gameObject.transform.position.y, gridGenerator.gridHeight / 2);
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

                    if (hittedTile.roadObjectBehavior != null)
                    {
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
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
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
        if (Input.GetKeyDown(KeyCode.E))
        {
            string filePath = EditorUtility.SaveFilePanel("Export tileMap", "Export current TileMAp in JSon", "tempMap1" + ".Json", "Json");
            string fileContent = "";

            Dictionary<GestionalTile, List<GestionalTile>> currentMap = gridGenerator.worldSectionTileMap;

            fileContent += $"'{gridGenerator.gridWidth}','{gridGenerator.gridHeight}','{gridGenerator.tileCount}','{gridGenerator.innerTileCount}'\n";

            foreach (GestionalTile bigTile in currentMap.Keys)
            {
                List<GestionalTile> smallTileList = new List<GestionalTile>();
                if (currentMap.TryGetValue(bigTile, out smallTileList))
                {
                    foreach (GestionalTile smallTile in smallTileList)
                    {
                        fileContent += $"'{bigTile.name}','{smallTile.name}','{smallTile.isOccupiedGrass}','{smallTile.isOccupiedRoad}'\n";
                    }
                }
            }



            File.WriteAllText(filePath, fileContent);

        }

        //Import
        if (Input.GetKeyDown(KeyCode.I))
        {
            string filePath = EditorUtility.OpenFilePanel("Import tileMap", "This will import a new tileMap of .Json", "Json");

            if (!string.IsNullOrWhiteSpace(filePath))
            {

                gridGenerator.DestroyTileMap();
                string[] lines = File.ReadAllLines(filePath);

                string firstLine = lines[0];
                lines[0] = "";

                bool wordStarted = false;
                List<string> words = new List<string>();
                string word = null;

                foreach (char c in firstLine)
                {
                    if ((c == '\u0027') && wordStarted == false)
                    {
                        wordStarted = true;
                    }
                    else if ((c == '\u0027') && wordStarted == true)
                    {
                        wordStarted = false;
                        words.Add(word);
                        word = null;
                    }

                    if (wordStarted && c != '\u0027')
                    {
                        word += c;
                    }
                }

                gridGenerator.gridWidth = int.Parse(words[0]);
                gridGenerator.gridHeight = int.Parse(words[1]);
                gridGenerator.tileCount = int.Parse(words[2]);
                gridGenerator.innerTileCount = int.Parse(words[3]);
                gridGenerator.InitializeGrid();
                CenterCameraToGrid(gridGenerator);

                words.Clear();
                word = null;
                wordStarted = false;

                foreach (string line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    wordStarted = false;
                    words = new List<string>();
                    word = null;

                    foreach (char c in line)
                    {
                        if ((c == '\u0027') && wordStarted == false)
                        {
                            wordStarted = true;
                        }
                        else if ((c == '\u0027') && wordStarted == true)
                        {
                            wordStarted = false;
                            words.Add(word);
                            word = null;
                        }

                        if (wordStarted && c != '\u0027')
                        {
                            word += c;
                        }
                    }

                    List<GestionalTile> result = gridGenerator.GetSmallTileListFromBigTileName(words[0]);
                    if (result != null)
                    {
                        foreach (GestionalTile tile in result)
                        {
                            if (tile.gameObject.name == words[1])
                            {
                                tile.isOccupiedGrass = bool.Parse(words[2]);
                                tile.isOccupiedRoad = bool.Parse(words[3]);

                                if (tile.isOccupiedRoad)
                                {
                                    tile.ChangeMesh(TileType.road);
                                    tile.CheckNearTiles();
                                }
                                else if (tile.isOccupiedGrass)
                                {
                                    tile.ChangeMesh(TileType.grass);
                                    tile.CheckNearTiles();
                                }



                            }
                        }
                    }

                    words.Clear();
                }


             




            }
        }
    }
}
