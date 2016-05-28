using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using LibNoise.Unity;
using LibNoise.Unity.Generator;
using CoherentNoise.Generation;
using CoherentNoise;

namespace Orbit.Terrain.Planet
{
    public class Planet : MonoBehaviour
    {

        public Dictionary<WorldPos, Chunk> Chunks = new Dictionary<WorldPos, Chunk>();

        public GameObject ChunkPrefab;

        public int Radius = 16;

        [HideInInspector]
        public bool GenerationComplete = false;

        [Header("Procedural generation settings")]
        public int Seed = 0;
        public float Scale = 1f;
        public double Frequency = 1.0;
        public double Lacunarity = 2.0;
        public double Persistence = 0.5;
        public int OctaveCount = 6;
        private ModuleBase PerlinGenerator;

        // Use this for initialization
        void Start()
        {
            PerlinGenerator = new Perlin();

            float random = UnityEngine.Random.Range(-99999f, 99999f);

            Debug.Log(string.Format("Accepted x:{0}, y:{1}, z:{2} and got {3}", random, random, random, PerlinGenerator.GetValue(new Vector3(random, random, random))));

            int ChunkCount = Mathf.CeilToInt(Radius / Chunk.CHUNK_SIZE);

            for (int x = -ChunkCount; x < ChunkCount; x++)
            {
                Debug.Log(string.Format("Planet creation is on iteration x:{0}", x));
                for (int y = -ChunkCount; y < ChunkCount; y++)
                {
                    for (int z = -ChunkCount; z < ChunkCount; z++)
                    {
                        CreateChunk(x * 16, y * 16, z * 16);
                    }
                }
            }

            GenerationComplete = true;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void CreateChunk(int x, int y, int z)
        {
            WorldPos worldPos = new WorldPos(x, y, z);

            GameObject newChunkObject = Instantiate(ChunkPrefab, Vector3.zero, Quaternion.Euler(Vector3.zero)) as GameObject;
            newChunkObject.transform.parent = gameObject.transform;
            newChunkObject.transform.localPosition = new Vector3(x, y, z);

            Chunk newChunk = newChunkObject.GetComponent<Chunk>();

            newChunk.Pos = worldPos;
            newChunk.Planet = this;

            Chunks.Add(worldPos, newChunk);

            for (int xi = 0; xi < 16; xi++)
            {
                for (int yi = 0; yi < 16; yi++)
                {
                    for (int zi = 0; zi < 16; zi++)
                    {
                        double heightModifier = PerlinGenerator.GetValue(new Vector3(xi + 0.01f, yi + 0.01f, zi + 0.01f));

                        Debug.Log(string.Format("Accepted x:{0}, y:{1}, z:{2} and got {3}", xi + 0.01f, yi, zi, heightModifier));
                        Debug.Log(heightModifier);
                        if (heightModifier == 0)
                        {
                            throw new Exception ();
                        }
                        if (Vector3.Distance(new Vector3(x + xi, y + yi, z + zi), Vector3.zero) < Radius - (heightModifier * Scale))
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

            Debug.Log(string.Format("Finished chunk generation for x:{0}, y:{1}, z:{2}", worldPos.x, worldPos.y, worldPos.z));
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
                chunk.SetBlock(x - chunk.Pos.x, y - chunk.Pos.y, z - chunk.Pos.z, block);
                chunk.NeedsUpdate = true;
            }
        }
    }
}