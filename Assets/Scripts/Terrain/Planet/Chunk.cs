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
                    if ((x == 0 || x == 16) || (y == 0 || y == 16) || (z == 0 || z == 16))
                    {
                        Blocks[x, y, z] = new BlockAir();
                        continue;
                    }
                    if (Random.value > 0.5f)
                    {
                        Blocks[x, y, z] = new BlockAir();
                    } else
                    {
                        Blocks[x, y, z] = new Block();
                    }
                }
            }
        }
        
        UpdateChunk();
    }

    void Update()
    {

    }

    public Block GetBlock(int x, int y, int z)
    {
        Debug.Log(string.Format("Fetching x:{0}, y:{1}, z:{2}", x, y, z));
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
        Filter.mesh.RecalculateNormals();

        Coll.sharedMesh = null;
        Mesh mesh = new Mesh();
        mesh.vertices = meshData.ColVerticies.ToArray();
        mesh.triangles = meshData.ColTris.ToArray();
        mesh.RecalculateNormals();

        Coll.sharedMesh = mesh;
    }
}
