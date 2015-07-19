using UnityEngine;
using System.Collections;

public class Chunk : MonoBehaviour {

	public GameObject BlockPrefab;

	private float[,,] ScalarField = new float[16,16,16];

	// Use this for initialization
	void Start () {
		StartCoroutine(GenerateBlocks());
	}
	
	// Update is called once per frame
	void Update () {

	}

	IEnumerator GenerateBlocks() {
		Debug.Log ("Waiting...");
		yield return new WaitForSeconds(5.0f);
		Debug.Log ("Beginning chunk generation");
		float beginTime = Time.realtimeSinceStartup;
		for (int x=0; x<=15; x++) {
			for(int y=0; y<=15; y++) {
				for (int z=0; z<=15; z++) {
					Debug.Log(string.Format("Creating voxel at {0},{1},{2}", x, y, z));
					ScalarField[x,y,z] = 175;
					yield return new WaitForSeconds(0.001f);
				}
			}
		}
		Debug.Log ("Generation took: " + (Time.realtimeSinceStartup - beginTime));
	}
}
