using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BinarySpacePartitioner 
{
    BSPRoomNode _rootNode;
    public BSPRoomNode RootNode =>_rootNode;

    public BinarySpacePartitioner(int dungeonWidth, int dungeonHeight)
    {
        _rootNode = new BSPRoomNode(Vector2Int.zero,new Vector2Int(dungeonWidth, dungeonHeight),null,0);
    }

    public List<BSPRoomNode> PrepareNodeCollection(int maxIterations, int minRoomWidth, int minRoomHeight)
    {
        Queue<BSPRoomNode> graph = new Queue<BSPRoomNode>();
        List<BSPRoomNode> resultList = new List<BSPRoomNode>();

        graph.Enqueue(_rootNode);
        resultList.Add(_rootNode);
        int iterations = 0;

        while (iterations < maxIterations && graph.Count > 0 ) 
        { 
            iterations++;
            var currentNode = graph.Dequeue();

            if(currentNode.width >= minRoomWidth*2  || currentNode.height >= minRoomHeight * 2)
            {
                SlitTheSpace(currentNode,resultList,minRoomWidth,minRoomHeight,graph);
            }
        }

        return resultList;
    }

    private void SlitTheSpace(BSPRoomNode currentNode, List<BSPRoomNode> resultList, int minRoomWidth, int minRoomHeight, Queue<BSPRoomNode> graph)
    {
        BSPLine dividingLine = GetDividingLineSpace(currentNode.BottomLeftAreaCorner,currentNode.TopRightAreaCorner,minRoomWidth,minRoomHeight);
        BSPRoomNode node1 = null;
        BSPRoomNode node2 = null;

        switch (dividingLine.Orientation)
        {
            case BSPOrientation.Horizontal:
                node1 = new BSPRoomNode(currentNode.BottomLeftAreaCorner, new Vector2Int(currentNode.TopRightAreaCorner.x , dividingLine.Coordinate.y),currentNode,currentNode.TreeLayerIndex+1);
                node2 = new BSPRoomNode(new Vector2Int(currentNode.BottomLeftAreaCorner.x, dividingLine.Coordinate.y), currentNode.TopRightAreaCorner, currentNode, currentNode.TreeLayerIndex + 1);
                break;

            case BSPOrientation.Vertical:
                node1 = new BSPRoomNode(currentNode.BottomLeftAreaCorner, new Vector2Int(dividingLine.Coordinate.x, currentNode.TopRightAreaCorner.y),currentNode,currentNode.TreeLayerIndex+1);
                node2 = new BSPRoomNode(new Vector2Int(dividingLine.Coordinate.x, currentNode.BottomLeftAreaCorner.y),currentNode.TopRightAreaCorner,currentNode,currentNode.TreeLayerIndex+1);

                break;
        }

        AddNodesToCollection(resultList, graph, node1);
        AddNodesToCollection(resultList, graph, node2);

    }

    private void AddNodesToCollection(List<BSPRoomNode> resultList,Queue<BSPRoomNode> graph, BSPRoomNode node)
    {
        resultList.Add(node);
        graph.Enqueue(node);
    }

    private BSPLine GetDividingLineSpace(Vector2Int bottomLeftAreaCorner, Vector2Int topRightAreaCorner, int minRoomWidth, int minRoomHeight)
    {
        BSPOrientation orientation;
        bool heightStatus = (topRightAreaCorner.y - bottomLeftAreaCorner.y) > minRoomHeight * 2;
        bool wifthStatus = (topRightAreaCorner.x - bottomLeftAreaCorner.x) > minRoomWidth * 2;

        if(heightStatus && wifthStatus)
        {
            orientation = (BSPOrientation)UnityEngine.Random.Range(0, 2);
        }

        else if(heightStatus)
        {
            orientation = BSPOrientation.Horizontal;
        }

        else 
        {
            orientation =BSPOrientation.Vertical;
        }

        return new BSPLine(orientation, GetCoordinatesForOrientation(orientation, bottomLeftAreaCorner, topRightAreaCorner, minRoomWidth, minRoomHeight));
    }
    private Vector2Int GetCoordinatesForOrientation(BSPOrientation orientation, Vector2Int bottomLeftAreaCorner, Vector2Int topRightAreaCorner, int minRoomWidth, int minRoomHeight)
    {
        Vector2Int coordinates = Vector2Int.zero;

        switch (orientation)
        {
            case BSPOrientation.Horizontal:
                coordinates = new Vector2Int(0,UnityEngine.Random.Range(bottomLeftAreaCorner.y + minRoomHeight , topRightAreaCorner.y - minRoomHeight));
                break;


            case BSPOrientation.Vertical:
                coordinates = new Vector2Int(UnityEngine.Random.Range(bottomLeftAreaCorner.x + minRoomWidth, topRightAreaCorner.x - minRoomWidth),0);
                break;
        }


        return coordinates;
    }
}
