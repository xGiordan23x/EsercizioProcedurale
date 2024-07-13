using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BSPRoomNode : BSPNode
{
    public int width { get => TopRightAreaCorner.x - BottomLeftAreaCorner.x; }
    public int height { get =>TopRightAreaCorner.y - BottomLeftAreaCorner.y; }
    public BSPRoomNode(Vector2Int bottomLeftAreaCorner, Vector2Int topRightAreaCorner,BSPNode parent,int layerIndex) : base(parent)
    {
        BottomLeftAreaCorner = bottomLeftAreaCorner;
        TopRightAreaCorner = topRightAreaCorner;
        BottomRightAreaCorner = new Vector2Int(TopRightAreaCorner.x, BottomLeftAreaCorner.y);
        TopLeftAreaCorner = new Vector2Int(BottomLeftAreaCorner.x, TopRightAreaCorner.y);
        TreeLayerIndex = layerIndex;
    }
}
