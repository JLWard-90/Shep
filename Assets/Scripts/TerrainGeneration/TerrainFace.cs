using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainFace
{
    Mesh mesh;
    int resolution;
    Vector3 localUp = Vector3.up; //Because we are not using a sphere in this project, the local up direction will always be the same as the global up direction
    Vector3 axisA;
    Vector3 axisB;
    ShapeGenerator shapeGenerator;

    public TerrainFace(ShapeGenerator shapeGenerator, Mesh mesh, int resolution)
    {
        this.shapeGenerator = shapeGenerator;
        this.mesh = mesh;
        this.resolution = resolution;
        axisA = new Vector3(localUp.y, localUp.z, localUp.x);
        axisB = Vector3.Cross(localUp, axisA);
    }

    public void ConstructMesh()
    {
        Vector3[] vertices = new Vector3[resolution * resolution];
        int[] triangles = new int[(resolution - 1) * (resolution - 1) * 6];
        int triIndex = 0;
        Vector2[] uv = (mesh.uv.Length == vertices.Length) ? mesh.uv : new Vector2[vertices.Length];
        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++) //Iterate over entire face
            {
                int i = x + y * resolution;
                Vector2 percent = new Vector2(x, y) / (resolution - 1); //Tells us how far through the loop we are
                Vector3 pointOnUnitCube = localUp + (percent.x - .5f) * 2 * axisA + (percent.y - .5f) * 2 * axisB;
                //Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;//used to go from a cube to a sphere
                Vector3 pointOnUnitSphere = pointOnUnitCube; //We are not using a sphere so we do not want to normalise
                float unscaledElevation = shapeGenerator.CalculateUnscaledElevation(pointOnUnitSphere);
                vertices[i] = pointOnUnitSphere * shapeGenerator.GetScaledElevation(unscaledElevation);
                uv[i].y = unscaledElevation;

                if (x != resolution - 1 && y != resolution - 1) //vertex is not along the right or bottom edges
                {
                    triangles[triIndex] = i; //Build the two triangles that make up a square by defining their 6 points (3 per triangle)
                    triangles[triIndex + 1] = i + resolution + 1;
                    triangles[triIndex + 2] = i + resolution;

                    triangles[triIndex + 3] = i;
                    triangles[triIndex + 4] = i + 1;
                    triangles[triIndex + 5] = i + resolution + 1;

                    triIndex += 6;
                }
            }
        }
        mesh.Clear(); //If we update with a lower resolution, then when we set mesh.vertices, mesh.triangles will reference vertices that no longer exist and will throw an error. Therefore we need to clear the mesh before setting the new vertices
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.uv = uv;
    }
}
