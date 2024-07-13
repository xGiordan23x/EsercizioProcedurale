using System.Collections.Generic;
using System.Linq;

public class BSPDungeonGenerator
{
    public int dungeonWidth;
    public int dungeonHeight;
    public List<BSPRoomNode> _allRoomNodes;

    public BSPDungeonGenerator(int dungeonWidth, int dungeonHeight)
    {
        this.dungeonWidth = dungeonWidth;
        this.dungeonHeight = dungeonHeight;
        _allRoomNodes = new List<BSPRoomNode>();
    }

    public List<BSPNode> CalculateDungeon(int maxIterations, int minRoomWidth, int minRoomHeight, float roomBottomCornerModifier, float roomTopCornerModifier, int roomOffset, int corridorWidth)
    {
        BinarySpacePartitioner bsp = new BinarySpacePartitioner(dungeonWidth, dungeonHeight);
        _allRoomNodes = bsp.PrepareNodeCollection(maxIterations, minRoomWidth, minRoomHeight);

        List<BSPNode> roomSpaces = BSPStructureHelper.GetlowestLeavesFromGraph(bsp.RootNode);
        List<BSPRoomNode> roomList = BSPRoomGenerator.GenerateRoomInGivenSpace(roomSpaces, roomBottomCornerModifier, roomTopCornerModifier, roomOffset);


        CorridorsGenerator corridorsGenerator = new CorridorsGenerator();
        var corridorList = corridorsGenerator.CreateCorridor(_allRoomNodes, corridorWidth);


        return new List<BSPNode>(roomList).Concat(corridorList).ToList(); ;
    }

}
