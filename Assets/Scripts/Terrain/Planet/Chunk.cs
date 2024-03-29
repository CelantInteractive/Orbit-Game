﻿using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Threading;

namespace Orbit.Terrain.Planet
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshCollider))]
    public class Chunk : MonoBehaviour
    {
        Block[,,] Blocks = new Block[CHUNK_SIZE, CHUNK_SIZE, CHUNK_SIZE];

        public bool NeedsUpdate = true;

        public Planet Planet;

        public WorldPos Pos;

        public bool GenerationComplete = false;

        public const int CHUNK_SIZE = 16;

        MeshFilter Filter;

        MeshCollider Coll;

        void Start()
        {
            Filter = gameObject.GetComponent<MeshFilter>();
            Coll = gameObject.GetComponent<MeshCollider>();
        }

        void Update()
        {
            if (NeedsUpdate && Planet.GenerationComplete)
            {
                NeedsUpdate = false;
                UpdateChunk();
            }
        }

        void UpdateChunk()
        {
            MeshData meshData = new MeshData();
            for (int x = 0; x < CHUNK_SIZE; x++)
            {
                for (int y = 0; y < CHUNK_SIZE; y++)
                {
                    for (int z = 0; z < CHUNK_SIZE; z++)
                    {
                        meshData = Blocks[x, y, z].Blockdata(this, x, y, z, meshData);
                    }
                }
            }
            RenderMesh(meshData);
        }

        void RenderMesh(MeshData meshData)
        {
            Filter.mesh.Clear();
            Filter.mesh.vertices = meshData.Verticies.ToArray();
            Filter.mesh.triangles = meshData.Tris.ToArray();
            Filter.mesh.uv = meshData.UV.ToArray();
            Filter.mesh.RecalculateNormals();

            Coll.sharedMesh = null;
            Mesh mesh = new Mesh();
            mesh.vertices = meshData.ColVerticies.ToArray();
            mesh.triangles = meshData.ColTris.ToArray();
            mesh.RecalculateNormals();

            Coll.sharedMesh = mesh;
        }

        public Block GetBlock(int x, int y, int z)
        {
            if (InRange(x) && InRange(y) && InRange(z))
                return Blocks[x, y, z];
            return Planet.GetBlock(Pos.x + x, Pos.y + y, Pos.z + z);
        }

        public void SetBlock(int x, int y, int z, Block block)
        {
            if (InRange(x) && InRange(y) && InRange(z))
            {
                Blocks[x, y, z] = block;
            }
            else
            {
                Planet.SetBlock(Pos.x + x, Pos.y + y, Pos.z + z, block);
            }
        }

        public static bool InRange(int index)
        {
            if (index < 0 || index >= CHUNK_SIZE)
                return false;

            return true;
        }
    }
}