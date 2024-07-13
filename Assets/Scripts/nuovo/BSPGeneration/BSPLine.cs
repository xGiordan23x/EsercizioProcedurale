using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BSPOrientation 
{ 
    Horizontal =0,
    Vertical = 1

 
}

public class BSPLine 
{ 
    BSPOrientation _orientation;
    public BSPOrientation Orientation {  get { return _orientation; } }    
    Vector2Int _coordinates;
    public Vector2Int Coordinate { get { return _coordinates; } }


    

    public BSPLine(BSPOrientation orientation, Vector2Int coordinates)
    {
        _orientation = orientation;
        _coordinates = coordinates;
    }
}

    

