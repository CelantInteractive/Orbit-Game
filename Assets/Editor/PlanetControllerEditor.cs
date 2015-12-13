using UnityEngine;
using UnityEditor;
using Orbit.Terrain;

[CustomEditor(typeof(PlanetController))]
public class PlanetControllerEditor : Editor {

	private int chunk = 0;

	// Use this for initialization
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		PlanetController script = (PlanetController)target;
		if(GUILayout.Button("Add Chunk"))
		{
			if(Application.isPlaying)
			{
				script.CreateChunk(0, chunk);
				chunk++;
			}
		}
	}
}
