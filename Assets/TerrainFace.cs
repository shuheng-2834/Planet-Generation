using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class TerrainFace
{
    private ShapeGenerator shapeGenerator;
    private readonly Mesh mesh;
    // 分辨率
    private readonly int resolution;
    private readonly Vector3 localUp;

    private readonly Vector3 axisA;
    private readonly Vector3 axisB;

    public TerrainFace(ShapeGenerator shapeGenerator,Mesh mesh, int resolution, Vector3 localUp)
    {
        this.shapeGenerator = shapeGenerator;
        this.resolution = resolution;
        this.localUp = localUp;
        this.mesh = mesh;

        axisA = new Vector3(localUp.y, localUp.z, localUp.x);
        // 叉乘，构建一个坐标系
        axisB = Vector3.Cross(localUp, axisA);
    }

    public void ConstructMesh() {
        // 三角形顶点数组
        Vector3[] vertices = new Vector3[resolution * resolution];
        // 三角形索引数组
        int[] triangles = new int[(resolution - 1) * (resolution - 1) * 6];
        int triIndex = 0;

        for (int y = 0; y < resolution; y++) {
            for (int x = 0; x < resolution; x++) {
                int i = x + y * resolution;
                Vector2 percent = new Vector2(x, y) / (resolution - 1);
                Vector3 pointOnUnitCube = localUp + (percent.x - .5f) * 2 * axisA + (percent.y - .5f) * 2 * axisB;
                Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;
                vertices[i] = shapeGenerator.CalculatePointOnPlanet(pointOnUnitSphere);

                if (x != resolution - 1 && y != resolution - 1) {
                    triangles[triIndex] = i;
                    triangles[triIndex + 1] = i + resolution + 1;
                    triangles[triIndex + 2] = i + resolution;

                    triangles[triIndex + 3] = i;
                    triangles[triIndex + 4] = i + 1;
                    triangles[triIndex + 5] = i + resolution + 1;

                    triIndex += 6;
                }
            }
        }
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}
