using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Planet : MonoBehaviour {

    public Dictionary<WorldPos, Chunk> Chunks = new Dictionary<WorldPos, Chunk>();

    public GameObject ChunkPrefab;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
