using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class RoadBehavior : MonoBehaviour
{
    float squareDimension;
    private TileType tileType;
    private GestionalTile tile;


    [Header("References")]
    public MeshRenderer rend;
    [Header("Street")]
    public GameObject VerticalStripesNorth;
    public GameObject VerticalStripesSouth;
    public GameObject HorizontalStripesEast;
    public GameObject HorizontalStripesWest;
    [Header("CrossRoad")]
    public List<GameObject> lights;
    [Header("Guardrail")]
    public GameObject northGuardrail;
    public GameObject southGuardrail;
    public GameObject eastGuardrail;
    public GameObject westGuardrail;

    public void SetVariables(float squareDimension, GestionalTile tile)
    {
        this.squareDimension = squareDimension;
        rend = GetComponentInChildren<MeshRenderer>();
        this.tile = tile;

        transform.localPosition = new Vector3(transform.localPosition.x + squareDimension / 2, transform.localPosition.y, transform.localPosition.z + squareDimension / 2);

    }
    public void SetType(TileType tileType)
    {
        this.tileType = tileType;
    }

    public void SelectCorrectMesh()
    {
        ActivateAllStripes(false);
        ActivateGuardrails(false, false, false, false);

        if (lights.Count > 0)
        {
            foreach (GameObject tr in lights)
                Destroy(tr);
        }


        switch (tileType)
        {
            case TileType.empty:
                break;

            case TileType.road:

                if (tile.northOccupied || tile.southOccupied)
                {
                    ActivateVerticalStripes(true, true);
                    ActivateGuardrails(false, false,true,true);
                    InstantiateStreetLight(false,false,true,true);
                }
                else if (tile.eastOccupied || tile.westOccupied)
                {
                    ActivateHorizontalStripes(true, true);
                    ActivateGuardrails(true, true, false, false);
                    InstantiateStreetLight(true, true, false, false);
                }


                break;

            case TileType.highway:
                
                //uscite ingressi verticali
                 if (tile.northOccupied && tile.eastOccupied && !tile.westOccupied && !tile.southOccupied && tile.southEastOccupied)
                {
                    ActivateGuardrails(false,true,false,true);
                    ActivateVerticalStripes(true, true);
                    VerticalStripesSouth.transform.localRotation = Quaternion.Euler(0, -45, 0);
                }
                else if (tile.northOccupied && tile.westOccupied && !tile.eastOccupied && !tile.southOccupied && tile.southWestOccupied)
                {
                    ActivateGuardrails(false, true, true, false);
                    ActivateVerticalStripes(true, true);
                    VerticalStripesSouth.transform.localRotation = Quaternion.Euler(0, 45, 0);
                }
                else if (tile.southOccupied && tile.westOccupied && !tile.eastOccupied && !tile.northOccupied && tile.northWestOccupied)
                {
                    ActivateGuardrails(true, false, true, false);
                    ActivateVerticalStripes(true, true);
                    VerticalStripesNorth.transform.localRotation = Quaternion.Euler(0, -45, 0);
                }
                else if (tile.southOccupied && tile.eastOccupied && !tile.westOccupied && !tile.northOccupied && tile.northEastOccupied)
                {
                    ActivateGuardrails(true, false, false, true);
                    ActivateVerticalStripes(true, true);
                    VerticalStripesNorth.transform.localRotation = Quaternion.Euler(0, 45, 0);
                }

                //uscite ingressi orizzontali
                else if (!tile.northOccupied && tile.eastOccupied && !tile.westOccupied && tile.southOccupied && tile.southWestOccupied)
                {
                    ActivateGuardrails(true, false, false, true);
                    ActivateHorizontalStripes(true, true);
                    HorizontalStripesWest.transform.localRotation = Quaternion.Euler(0, -45, 0);
                }
                else if (tile.northOccupied && !tile.westOccupied && tile.eastOccupied && !tile.southOccupied && tile.northWestOccupied)
                {
                    ActivateGuardrails(false, true, false, true);
                    ActivateHorizontalStripes(true, true);
                    HorizontalStripesWest.transform.localRotation = Quaternion.Euler(0, 45, 0);
                }
                else if (tile.southOccupied && tile.westOccupied && !tile.eastOccupied && !tile.northOccupied && tile.southEastOccupied)
                {
                    ActivateGuardrails(true, false, true, false);
                    ActivateHorizontalStripes(true, true);
                    HorizontalStripesEast.transform.localRotation = Quaternion.Euler(0, 45, 0);
                }
                else if (!tile.southOccupied && !tile.eastOccupied && tile.westOccupied && tile.northOccupied && tile.northEastOccupied)
                {
                    ActivateGuardrails(false, true, true, false);
                    ActivateHorizontalStripes(true, true);
                    HorizontalStripesEast.transform.localRotation = Quaternion.Euler(0, -45, 0);
                }

                //curve interne
                else if(!tile.southEastOccupied && tile.northOccupied && tile.northEastOccupied && tile.eastOccupied && tile.southOccupied && tile.southWestOccupied && tile.westOccupied && tile.northWestOccupied)
                {
                    ActivateVerticalStripes(false, true);
                    ActivateHorizontalStripes(true, false);
                    ActivateGuardrails(false, false, false, false);
                }           
                else if (!tile.northWestOccupied && tile.northOccupied && tile.northEastOccupied && tile.eastOccupied && tile.southEastOccupied && tile.southOccupied && tile.southWestOccupied && tile.westOccupied)
                {
                    ActivateVerticalStripes(true, false);
                    ActivateHorizontalStripes(false, true);
                    ActivateGuardrails(false, false, false, false);
                }                
                else if (!tile.southWestOccupied && tile.northOccupied && tile.northEastOccupied && tile.eastOccupied && tile.southEastOccupied && tile.southOccupied && tile.westOccupied && tile.northWestOccupied)
                {
                    ActivateVerticalStripes(false, true);
                    ActivateHorizontalStripes(false, true);
                    ActivateGuardrails(false, false, false, false);
                }                
                else if (!tile.northEastOccupied && tile.northOccupied && tile.eastOccupied && tile.southEastOccupied && tile.southOccupied && tile.southWestOccupied && tile.westOccupied && tile.northWestOccupied)
                {
                    ActivateVerticalStripes(true, false);
                    ActivateHorizontalStripes(true, false);
                    ActivateGuardrails(false, false, false, false);
                }

                // verticale 
                else if (tile.northOccupied && tile.southOccupied && !tile.westOccupied)
                {
                    ActivateVerticalStripes(true, true);
                    ActivateGuardrails(false, false, false, true);
                }
                else if (tile.northOccupied && tile.southOccupied && !tile.eastOccupied)
                {
                    ActivateVerticalStripes(true, true);
                    ActivateGuardrails(false, false, true, false);
                }
                //orizzontale
                else if (tile.eastOccupied && tile.westOccupied && !tile.northOccupied)
                {
                    ActivateHorizontalStripes(true, true);
                    ActivateGuardrails(true, false, false, false);
                }
                else if (tile.eastOccupied && tile.westOccupied && !tile.southOccupied)
                {
                    ActivateHorizontalStripes(true, true);
                    ActivateGuardrails(false, true, false, false);
                }

                //curve
                else if(tile.northOccupied && tile.eastOccupied)
                {                  
                        ActivateVerticalStripes(true, false);
                        ActivateHorizontalStripes(true, false);
                        ActivateGuardrails(false, true, false, true);
                }
                else if (tile.northOccupied && tile.westOccupied)
                {
                    ActivateVerticalStripes(true, false);
                    ActivateHorizontalStripes(false, true);
                    ActivateGuardrails(false, true, true, false);
                }
                else if (tile.southOccupied && tile.eastOccupied)
                {
                    ActivateVerticalStripes(false, true);
                    ActivateHorizontalStripes(true, false);
                    ActivateGuardrails(true, false, false, true);
                }
                else if (tile.southOccupied && tile.westOccupied)
                {
                    ActivateVerticalStripes(false, true);
                    ActivateHorizontalStripes(false, true);
                    ActivateGuardrails(true, false, true, false);
                }

                


                break;

            case TileType.crossRoad:
                ActivateAllStripes(true);
                InstantiateTrafficLight(true, true, true, true);
                break;

            case TileType.tCrossroad:
                if (tile.northOccupied && tile.southOccupied && tile.eastOccupied && !tile.westOccupied)
                {
                    ActivateVerticalStripes(true, true);
                    ActivateHorizontalStripes(true, false);
                    InstantiateTrafficLight(true, true, false, true);
                    InstantiateStreetLight(false, false, true, false);
                    ActivateGuardrails(false, false, false, true);
                }
                else if (tile.northOccupied && tile.southOccupied && !tile.eastOccupied && tile.westOccupied)
                {
                    ActivateVerticalStripes(true, true);
                    ActivateHorizontalStripes(false, true);
                    InstantiateTrafficLight(false, true, true, true);
                    InstantiateStreetLight(false, false, false, true);
                    ActivateGuardrails(false, false, true, false);
                }
                else if (!tile.northOccupied && tile.southOccupied && tile.eastOccupied && tile.westOccupied)
                {
                    ActivateVerticalStripes(false, true);
                    ActivateHorizontalStripes(true, true);
                    InstantiateTrafficLight(true, false, true, true);
                    InstantiateStreetLight(true, false, false, false);
                    ActivateGuardrails(true, false, false, false);
                }
                else if (tile.northOccupied && !tile.southOccupied && tile.eastOccupied && tile.westOccupied)
                {
                    ActivateVerticalStripes(true, false);
                    ActivateHorizontalStripes(true, true);
                    InstantiateTrafficLight(true, true, true, false);
                    InstantiateStreetLight(false, true, false, false);
                    ActivateGuardrails(false, true, false, false);
                }
                break;

            case TileType.turn:

                if (tile.northOccupied && tile.eastOccupied)
                {
                    ActivateVerticalStripes(true, false);
                    ActivateHorizontalStripes(true, false);
                    ActivateGuardrails(false, true, false, true);
                    InstantiateStreetLight(false, true, true, false);
                }
                if (tile.northOccupied && tile.westOccupied)
                {
                    ActivateVerticalStripes(true, false);
                    ActivateHorizontalStripes(false, true);
                    ActivateGuardrails(false, true, true, false);
                    InstantiateStreetLight(false, true, false, true);
                }
                if (tile.southOccupied && tile.eastOccupied)
                {
                    ActivateVerticalStripes(false, true);
                    ActivateHorizontalStripes(true, false);
                    InstantiateStreetLight(true, false, true, false);
                    ActivateGuardrails(true, false, false, true);
                }
                if (tile.southOccupied && tile.westOccupied)
                {
                    ActivateVerticalStripes(false, true);
                    ActivateHorizontalStripes(false, true);
                    ActivateGuardrails(true, false, true, false);
                    InstantiateStreetLight(true, false, false, true);
                }

                break;

            
        }
    }

    #region Stripes
    private void ActivateAllStripes(bool value)
    {
        ActivateVerticalStripes(value, value);
        ActivateHorizontalStripes(value, value);
    }
    private void ActivateHorizontalStripes(bool east, bool west)
    {
        HorizontalStripesEast.transform.localRotation = Quaternion.identity;
        HorizontalStripesWest.transform.localRotation = Quaternion.identity;
        HorizontalStripesEast.SetActive(east);
        HorizontalStripesWest.SetActive(west);
    }
    private void ActivateVerticalStripes(bool north, bool south)
    {
        VerticalStripesSouth.transform.localRotation = Quaternion.identity;
        VerticalStripesNorth.transform.localRotation = Quaternion.identity;
        VerticalStripesNorth.SetActive(north);
        VerticalStripesSouth.SetActive(south);
    }
    #endregion

    #region light
    private void InstantiateTrafficLight(bool northEast, bool northWest, bool southWest, bool southEast)
    {
        Bounds bounds = tile.tileRenderer.bounds;

        if (northEast)
        {
            GameObject temp = Instantiate(tile.gridGenerator.trafficLightPrefab, tile.transform);
            temp.transform.localPosition = new Vector3((bounds.max.x - tile.transform.position.x), tile.squareDimension / 6, (bounds.max.z - tile.transform.position.z));
            temp.transform.localRotation = Quaternion.identity;
            lights.Add(temp);
        }
        if (northWest)
        {
            GameObject temp2 = Instantiate(tile.gridGenerator.trafficLightPrefab,tile.transform);
            temp2.transform.localPosition = new Vector3((bounds.min.x - tile.transform.position.x), tile.squareDimension/6, (bounds.max.z - tile.transform.position.z));
            temp2.transform.localRotation = Quaternion.Euler(0, -90, 0);
            lights.Add(temp2);
        }
        if (southEast)
        {
            GameObject temp3 = Instantiate(tile.gridGenerator.trafficLightPrefab, tile.transform);
            temp3.transform.localPosition = new Vector3((bounds.max.x - tile.transform.position.x), tile.squareDimension /6, (bounds.min.z - tile.transform.position.z));
            temp3.transform.localRotation = Quaternion.Euler(0, 90, 0);
            lights.Add(temp3);
        }
        if (southWest)
        {
            GameObject temp4 = Instantiate(tile.gridGenerator.trafficLightPrefab, tile.transform);
            temp4.transform.localPosition = new Vector3((bounds.min.x - tile.transform.position.x), tile.squareDimension /6, -(bounds.min.z - tile.transform.position.z));
            temp4.transform.localRotation = Quaternion.Euler(0, 180, 0);
            lights.Add(temp4);
        }
    }
    private void InstantiateStreetLight(bool north, bool south, bool west, bool east)
    {
        Bounds bounds = tile.tileRenderer.bounds;

        if (north)
        {
            GameObject temp = Instantiate(tile.gridGenerator.streetlightPrefab, tile.transform);
            temp.transform.localPosition = new Vector3((bounds.max.x - tile.transform.position.x)/2, tile.squareDimension / 4, (bounds.max.z - tile.transform.position.z));           
            temp.transform.localRotation = Quaternion.Euler(0, 180, 0);
            lights.Add(temp);
        }
        if (south)
        {
            GameObject temp2 = Instantiate(tile.gridGenerator.streetlightPrefab, tile.transform);
            temp2.transform.localPosition = new Vector3((bounds.max.x - tile.transform.position.x)/2, tile.squareDimension / 4, (bounds.min.z - tile.transform.position.z));
            temp2.transform.localRotation = Quaternion.identity;
            lights.Add(temp2);
        }
        if (west)
        {
            GameObject temp3 = Instantiate(tile.gridGenerator.streetlightPrefab, tile.transform);
            temp3.transform.localPosition = new Vector3((bounds.min.x - tile.transform.position.x), tile.squareDimension / 4, (bounds.max.z - tile.transform.position.z)/2);
            temp3.transform.localRotation = Quaternion.Euler(0, 90, 0);
            lights.Add(temp3);
        }
        if (east)
        {
            GameObject temp4 = Instantiate(tile.gridGenerator.streetlightPrefab, tile.transform);
            temp4.transform.localPosition = new Vector3((bounds.max.x - tile.transform.position.x), tile.squareDimension / 4, (bounds.max.z - tile.transform.position.z)/2);
            
            temp4.transform.localRotation = Quaternion.Euler(0, -90, 0);
            lights.Add(temp4);
        }
    }

    #endregion



    #region guardrail
    private void ActivateGuardrails(bool north, bool south, bool east, bool west)
        {
        eastGuardrail.SetActive(east);
        westGuardrail.SetActive(west);
        northGuardrail.SetActive(north);
        southGuardrail.SetActive(south);

    }

    internal void ChooseTileToTransform()
    {
        if (tile.isOccupiedRoad)
        {
            // incrocio
            if ((tile.eastOccupied && tile.southOccupied && tile.westOccupied && tile.northOccupied) && (tile.northTile.tileType != TileType.highway && tile.southTile.tileType != TileType.highway && tile.eastTile.tileType != TileType.highway && tile.westTile.tileType != TileType.highway))
            {
                //creo prefab incrocio              
                tile.ChangeMesh(TileType.crossRoad);
            }

            //superstrada 
            else if ((tile.northOccupied && tile.eastOccupied && tile.northEastOccupied) || (tile.southOccupied && tile.eastOccupied && tile.southEastOccupied) || (tile.westOccupied && tile.southOccupied && tile.southWestOccupied) || (tile.northOccupied && tile.westOccupied && tile.northWestOccupied))
            {
                tile.ChangeMesh(TileType.highway);
            }

            // incrocio ttile.
            else if ((tile.northOccupied && tile.southOccupied && (tile.eastOccupied || tile.westOccupied)) || ((tile.eastOccupied && tile.westOccupied) && (tile.northOccupied || tile.southOccupied)))
            {
                tile.ChangeMesh(TileType.tCrossroad);
            }

            //curva
            else if ((tile.northOccupied && tile.eastOccupied) || (tile.northOccupied && tile.westOccupied) || (tile.southOccupied && tile.eastOccupied) || (tile.southOccupied && tile.westOccupied))
            {
                tile.ChangeMesh(TileType.turn);
            }

            //strada
            else if (tile.northOccupied || tile.southOccupied)
            {
                tile.ChangeMesh(TileType.road);
            }
            else if (tile.eastOccupied || tile.westOccupied)
            {
                tile.ChangeMesh(TileType.road);
            }



        }
    }
    #endregion



}