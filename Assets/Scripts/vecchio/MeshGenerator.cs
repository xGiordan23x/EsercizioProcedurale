using UnityEngine;

public static class MeshGenerator
{
    public static MeshData GenerateMesh(float[,] noisemap, float terrainHeight, AnimationCurve terrainCurve, float scale)
    {
        int width = noisemap.GetLength(0);
        int height = noisemap.GetLength(1);
        float topLeftX = (width - 1) / -2f;
        float topLeftZ = (height - 1) / 2f;

        MeshData meshData = new MeshData(width, height);
        int vertexIndex = 0;
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float meshHeight = terrainHeight * scale *terrainCurve.Evaluate( noisemap[(width-1)-x, y]);
                meshData.vertices[vertexIndex] = new Vector3(topLeftX + x, meshHeight, topLeftZ - y);
                meshData.uv[vertexIndex] = new Vector2(((width - 1) - x) / (float)width, y / (float)height);

                if (x < width - 1 && y < height - 1)
                {
                    meshData.AddTriangle(vertexIndex, vertexIndex + 1 + width, vertexIndex + width);
                    meshData.AddTriangle(vertexIndex + width + 1, vertexIndex, vertexIndex + 1);
                }
                vertexIndex++;

            }
            
        }
        return meshData;
    }
}
public class MeshData
{
    public Vector3[] vertices;
    public int[] triangles;
    public Vector2[] uv;
    int triangleindex;

    public MeshData(int meshWidth, int meshHeight)
    {
        vertices = new Vector3[meshWidth * meshHeight];
        triangles = new int[(meshWidth - 1) * (meshHeight - 1) * 6];
        uv = new Vector2[meshWidth * meshHeight];
        triangleindex = 0;


    }
    public void AddTriangle(int a, int b, int c)
    {
        triangles[triangleindex] = a;
        triangles[triangleindex + 1] = b;
        triangles[triangleindex + 2] = c;
        triangleindex += 3;
    }

    public Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;

        mesh.RecalculateNormals();

        return mesh;
    }
}



