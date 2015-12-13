using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Threading;

public class Chunk : MonoBehaviour
{

	private byte[,,] ChunkData = new byte[16, 64, 16];

	private NoiseGenerator noise;

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	public void GenerateChunk(int xpos, int zpos, float scale)
	{
		noise = new NoiseGenerator();

		int StartX = 16 * (xpos);
		int StartZ = 16 * (zpos);

		Debug.Log("Beginning chunk generation");

		for (int z = 0; z < 16; z++)
		{
			for (int x = 0; x < 16; x++)
			{
				double Random = noise.Noise(StartX + x, StartZ + z) * scale;
				int Height = Mathf.Clamp((int)((Random + 1) * 32), 0, 64);

				ChunkData[x, Height, z] = 1;
				ChunkData[x, 0, z] = 255;
			}
		}

		noise = null;

		Debug.Log("Chunk finished generation");
	}

	public void GenerateMesh()
	{
		Mesh mesh = new Mesh();
		mesh.Clear();

		List<Vector3> verts = CalculateVerts();
		int[] tris = null;

		Thread newThread = new Thread(() => { tris = CalculateTris(); });
		newThread.Start();
		newThread.Join(1000);

		foreach (Vector3 vert in verts)
		{
			Debug.LogFormat("X:{0}, Y:{1}, Z:{2}", vert.x, vert.y, vert.z);
		}

		Debug.Log("Verts: " + verts.Count);
		Debug.Log("Tris: " + tris.Length);

		Vector2[] uvs = new Vector2[verts.Count];
		for (var i = 0; i < uvs.Length; i++)
		{
			//Give UV coords X,Z world coords
			uvs[i] = new Vector2(verts[i].x, verts[i].z);
		}

		mesh.vertices = verts.ToArray();
		mesh.triangles = tris;
		mesh.uv = uvs;
		mesh.RecalculateNormals();
		GetComponent<MeshFilter>().mesh = mesh;
	}

	private List<Vector3> CalculateVerts()
	{
		List<Vector3> verts = new List<Vector3>();

		for (int x = 0; x < 16; x++)
		{
			for (int z = 0; z < 16; z++)
			{
				for (int y = 0; y < ChunkData.GetLength(1); y++)
				{
					if (ChunkData[x, y, z] == 0 || ChunkData[x, y, z] == 255)
					{
						continue;
					}
					verts.Add(new Vector3(x, y, z));
				}
			}
		}

		return verts;
	}

	private int[] CalculateTris()
	{

		int[] triangles = new int[16 * 16 * 6];
		for (int ti = 0, vi = 0, z = 0; z < 15; z++, vi++)
		{
			for (int x = 0; x < 15; x++, ti+= 6, vi++)
			{
				triangles[ti] = vi;
				triangles[ti + 1] = vi + 1;
				triangles[ti + 2] = vi + 16;
				triangles[ti + 3] = vi + 16 + 1;
				triangles[ti + 4] = vi + 16;
				triangles[ti + 5] = vi + 1;
			}
		}

		return triangles;
	}

	public byte[,,] GetChunkData()
	{
		return ChunkData;
	}
}
