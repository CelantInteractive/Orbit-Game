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
	}
}