﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Orbit.Terrain.Planet
{
    public class Planet : MonoBehaviour
    {

        public Dictionary<WorldPos, Chunk> Chunks = new Dictionary<WorldPos, Chunk>();

        public GameObject ChunkPrefab;

		public int Radius = 16;

		public bool GenerationComplete = false;

        // Use this for initialization
        void Start()
        {
			int ChunkCount = Mathf.CeilToInt(Radius / Chunk.CHUNK_SIZE);

			for (int x = -ChunkCount; x < ChunkCount; x++)
            {
				Debug.Log (string.Format("Planet creation is on iteration x:{0}", x));
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
			newChunkObject.transform.localPosition = new Vector3 (x, y, z);

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
						if (( pos.x-center.x ) ^2 + (pos.y-center.y) ^2 + (pos.z-center.z) ^ 2 < radius^2)
                        if (yi <= 16)
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

			Debug.Log (string.Format ("Finished chunk generation for x:{0}, y:{1}, z:{2}", worldPos.x, worldPos.y, worldPos.z));
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