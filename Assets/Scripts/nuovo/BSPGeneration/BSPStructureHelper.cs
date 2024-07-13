using System;
using System.Collections.Generic;
using UnityEngine;

public static class BSPStructureHelper
{


    public static List<BSPNode> GetlowestLeavesFromGraph(BSPNode rootNode)
    {
        if (rootNode.ChildrenNodeList.Count == 0)
            return new List<BSPNode>() { rootNode };

        Queue<BSPNode> nodesToCheck = new();
        List<BSPNode> resultList = new();

        foreach (BSPNode childNode in rootNode.ChildrenNodeList)
        {
            nodesToCheck.Enqueue(childNode);
        }

        while (nodesToCheck.Count > 0)
        {
            var currentNode = nodesToCheck.Dequeue();
            if (currentNode.ChildrenNodeList.Count == 0)
            {
                resultList.Add(currentNode);
            }
            else
            {
                foreach (BSPNode childNode in currentNode.ChildrenNodeList)
                {
                    nodesToCheck.Enqueue(childNode);
                }
            }
        }

        return resultList;
    }

    public static Vector2Int GenerateBottomLeftCorner(Vector2Int bottomLeftAreaCorner, Vector2Int topRightAreaCorner, float roomBottomCornerModifier, int offset)
    {
        int minX = bottomLeftAreaCorner.x + offset;
        int maxX = topRightAreaCorner.x - offset;

        int minY = bottomLeftAreaCorner.y + offset;
        int maxY = topRightAreaCorner.y - offset;


        return new Vector2Int(
            UnityEngine.Random.Range(minX,(int)(minX +(maxX - minX) * roomBottomCornerModifier)),
            UnityEngine.Random.Range(minY, (int)(minY + (maxY - minY) * roomBottomCornerModifier)));
    }

    public static Vector2Int GenerateTopRightCorner(Vector2Int bottomLeftAreaCorner, Vector2Int topRightAreaCorner, float roomTopCornerModifier, int offset)
    {
        int minX = bottomLeftAreaCorner.x + offset;
        int maxX = topRightAreaCorner.x - offset;

        int minY = bottomLeftAreaCorner.y + offset;
        int maxY = topRightAreaCorner.y - offset;

        return new Vector2Int(
           UnityEngine.Random.Range((int)(minX + (maxX - minX) * roomTopCornerModifier),maxX),
           UnityEngine.Random.Range((int)(minY + (maxY - minY) * roomTopCornerModifier),maxY));
    }

    public  static Vector2Int CalculateMiddlePoint(Vector2Int v1, Vector2Int v2)
    {
       Vector2 sum = v1+v2;
        Vector2 tempVector = sum / 2;
        return new Vector2Int((int)tempVector.x, (int)tempVector.y);
    }
}
