using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Threading;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class Chunk : MonoBehaviour
{
    Block[,,] Blocks;

    public bool NeedsUpdate = true;

    public static int CHUNK_SIZE = 16;

    MeshFilter Filter;

    MeshCollider Coll;

    void Start()
    {
        Filter = gameObject.GetComponent<MeshFilter>();
        Coll = gameObject.GetComponent<MeshCollider>();

        // Boilerplate shit to make it work as a demo
        Blocks = new Block[CHUNK_SIZE, CHUNK_SIZE, CHUNK_SIZE];
        for (int x = 0; x < CHUNK_SIZE; x++)
        {
            for (int y = 0; y < CHUNK_SIZE; y++)
            {
                for (int z = 0; z < CHUNK_SIZE; z++)
                {
                    Blocks[x, y, z] = new BlockAir();
                }
            }
        }

        Blocks[3, 5, 2] = new Block();
        UpdateChunk();
    }

    void Update()
    {

    }

    public Block GetBlock(int x, int y, int z)
    {
        return Blocks[x, y, z];
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
    }
}
