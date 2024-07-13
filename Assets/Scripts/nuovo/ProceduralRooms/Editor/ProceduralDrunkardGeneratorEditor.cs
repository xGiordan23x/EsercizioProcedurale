using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEngine;

[CustomEditor(typeof(ProceduralDrunkardGeneration))]
public class ProceduralDrunkardGeneratorEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

       
        GUILayout.Space(10f);

        var proceduralDrunkardGeneration = (ProceduralDrunkardGeneration)target;


        if (GUILayout.Button("Generate Dungeon"))
        {
            proceduralDrunkardGeneration.GenerateRoomsFromEditor();
            EditorUtility.SetDirty(proceduralDrunkardGeneration);
        }


        if (GUILayout.Button("populate Rooms"))
        {
            proceduralDrunkardGeneration.PopulateRooms();
            EditorUtility.SetDirty(proceduralDrunkardGeneration);
        }



    }
}
