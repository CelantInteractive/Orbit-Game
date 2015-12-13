using UnityEngine;
using System.Collections.Generic;
using System.Threading;

namespace Orbit.Terrain
{
	public class PlanetController : MonoBehaviour
	{
		public float Scale = 1f;
		public int Seed = 0;
		public bool DisplayVerts = false;
		public GameObject ChunkPrefab = null;

		private Dictionary<Vector3, GameObject> Chunks = new Dictionary<Vector3, GameObject>();

		private NoiseGenerator noise;

		void Start()
		{
			if (Seed == 0)
			{
				System.Random r = new System.Random();

				Seed = r.Next(int.MaxValue);
				NoiseGenerator.Seed = Seed;
			}
		}

		public Chunk CreateChunk(int x, int z)
		{
			Vector3 pos = new Vector3(x, 0, z);

			if (Chunks.ContainsKey(pos))
			{
				return Chunks[new Vector3(x, 0, z)].GetComponent<Chunk>();
			}

			GameObject chunk = Instantiate(ChunkPrefab) as GameObject;
			chunk.transform.parent = transform;
			chunk.transform.position = transform.TransformPoint(new Vector3(x * 16, 0, z * 16));

			Chunk chunkScript = chunk.GetComponent<Chunk>();
			chunkScript.GenerateChunk(x, z, Scale);
			chunkScript.GenerateMesh();

			Chunks.Add(pos, chunk);
			return chunkScript;
		}

		public byte[,,] GetChunk(Vector3 pos)
		{
			GameObject chunk = null;

			if (!Chunks.ContainsKey(pos))
			{
				return null;
			}

			chunk = Chunks[pos];

			return chunk.GetComponent<Chunk>().GetChunkData();
		}

		void OnDrawGizmosSelected()
		{
			if (Application.isPlaying && DisplayVerts)
			{
				foreach (KeyValuePair<Vector3, GameObject> chunk in Chunks)
				{
					byte[,,] chunkData = GetChunk(chunk.Key);
					Vector3 position = chunk.Key;

					for (int x = 0; x < chunkData.GetLength(0); x++)
					{
						for (int y = 0; y < chunkData.GetLength(1); y++)
						{
							for (int z = 0; z < chunkData.GetLength(2); z++)
							{
								switch (chunkData[x, y, z])
								{
									case 0:
										break;
									case 1:
										Gizmos.color = Color.yellow;
										Gizmos.DrawSphere(transform.TransformPoint(new Vector3(x + (position.x * 16), y + (position.y * 16), z + (position.z * 16))), 0.1f);
										break;
									case 255:
										//Gizmos.color = Color.red;
										//Gizmos.DrawSphere(transform.TransformPoint(new Vector3(x + (position.x * 16), y + (position.y * 16), z + (position.z * 16))), 0.1f);
										break;
								}
							}
						}
					}
				}
			}
		}
	}
}