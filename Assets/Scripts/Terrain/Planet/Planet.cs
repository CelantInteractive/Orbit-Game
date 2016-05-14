using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Orbit.Terrain.Planet
{
    public class Planet : MonoBehaviour
    {

        public Dictionary<WorldPos, Chunk> Chunks = new Dictionary<WorldPos, Chunk>();

        public GameObject ChunkPrefab;

        // Use this for initialization
        void Start()
        {
            for (int x = -2; x < 2; x++)
            {
                for (int y = -1; y < 1; y++)
                {
                    for (int z = -1; z < 1; z++)
                    {
                        CreateChunk(x * 16, y * 16, z * 16);
                    }
                }
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void CreateChunk(int x, int y, int z)
        {
            WorldPos worldPos = new WorldPos(x, y, z);

            GameObject newChunkObject = Instantiate(ChunkPrefab, new Vector3(x, y, z), Quaternion.Euler(Vector3.zero)) as GameObject;

            Chunk newChunk = newChunkObject.GetComponent<Chunk>();

            newChunk.Pos = worldPos;
            newChunk.Planet = this;

            Chunks.Add(worldPos, newChunk);

            for (int xi = 0; xi < 16; x++)
            {
                for (int yi = 0; yi < 16; yi++)
                {
                    for (int zi = 0; zi < 16; zi++)
                    {
                        if (yi <= 7)
                        {
                            SetBlock(x + xi, y + yi, z + zi, new Block());
                        }
                        else
                        {
                            SetBlock(x + xi, y + yi, z + zi, new BlockAir());
                        }
                    }
                }
            }
        }

        public Chunk GetChunk(int x, int y, int z)
        {
            WorldPos pos = new WorldPos();
            float multiplier = Chunk.CHUNK_SIZE;
            pos.x = Mathf.FloorToInt(x / multiplier) * Chunk.CHUNK_SIZE;
            pos.y = Mathf.FloorToInt(y / multiplier) * Chunk.CHUNK_SIZE;
            pos.z = Mathf.FloorToInt(z / multiplier) * Chunk.CHUNK_SIZE;

            Chunk containerChunk = null;
            Chunks.TryGetValue(pos, out containerChunk);

            return containerChunk;
        }

        public Block GetBlock(int x, int y, int z)
        {
            Chunk containerChunk = GetChunk(x, y, z);
            if (containerChunk != null)
            {
                Block block = containerChunk.GetBlock(
                    x - containerChunk.Pos.x,
                    y - containerChunk.Pos.y,
                    z - containerChunk.Pos.z);

                return block;
            }
            else
            {
                return new BlockAir();
            }
        }

        public void SetBlock(int x, int y, int z, Block block)
        {
            Chunk chunk = GetChunk(x, y, z);

            if (chunk != null)
            {
                chunk.SetBlock(x - chunk.Pos.x, y - chunk.Pos.y, z - chunk.Pos.y, block);
                chunk.NeedsUpdate = true;
            }
        }
    }
}