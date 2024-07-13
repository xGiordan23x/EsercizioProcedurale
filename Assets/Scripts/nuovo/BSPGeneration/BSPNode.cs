using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public abstract class BSPNode
{
    public Vector2Int BottomLeftAreaCorner {  get; set; }
    public Vector2Int BottomRightAreaCorner { get; set; }
    public Vector2Int TopLeftAreaCorner { get; set; }
    public Vector2Int TopRightAreaCorner { get; set; }
    public BSPNode ParentNode { get; set; }
    public int TreeLayerIndex { get; set; }

    List<BSPNode> _childrenNodeList;
    public List<BSPNode> ChildrenNodeList { get { return _childrenNodeList; } }

    protected BSPNode(BSPNode parent) 
    {
        ParentNode = parent;
        _childrenNodeList = new List<BSPNode>();

        if(ParentNode != null)
        {
            ParentNode.ChildrenNodeList.Add(this);
        }
    }
}
