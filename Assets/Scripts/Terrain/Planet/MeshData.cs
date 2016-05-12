using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeshData
{

    public List<Vector3> Verticies = new List<Vector3>();
    public List<int> Tris = new List<int>();
    public List<Vector2> UV = new List<Vector2>();
    public List<Vector3> ColVerticies = new List<Vector3>();
    public List<int> ColTris = new List<int>();

    public MeshData()
    {

    }

    public void AddQuadTriangles()
    {
        Tris.Add(Verticies.Count - 4);
        Tris.Add(Verticies.Count - 3);
        Tris.Add(Verticies.Count - 2);
        Tris.Add(Verticies.Count - 4);
        Tris.Add(Verticies.Count - 2);
        Tris.Add(Verticies.Count - 1);
    }
}
