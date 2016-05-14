using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Orbit.Terrain.Planet
{
    public class MeshData
    {

        public List<Vector3> Verticies = new List<Vector3>();
        public List<int> Tris = new List<int>();
        public List<Vector2> UV = new List<Vector2>();
        public List<Vector3> ColVerticies = new List<Vector3>();
        public List<int> ColTris = new List<int>();
        public bool UseRenderDataForCol;

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
            if (UseRenderDataForCol)
            {
                ColTris.Add(ColVerticies.Count - 4);
                ColTris.Add(ColVerticies.Count - 3);
                ColTris.Add(ColVerticies.Count - 2);
                ColTris.Add(ColVerticies.Count - 4);
                ColTris.Add(ColVerticies.Count - 2);
                ColTris.Add(ColVerticies.Count - 1);
            }
        }

        public void AddVertex(Vector3 vertex)
        {
            Verticies.Add(vertex);
            if (UseRenderDataForCol)
            {
                ColVerticies.Add(vertex);
            }
        }

        public void AddTriangle(int tri)
        {
            Tris.Add(tri);
            if (UseRenderDataForCol)
            {
                ColTris.Add(tri - (Verticies.Count - ColVerticies.Count));
            }
        }
    }
}