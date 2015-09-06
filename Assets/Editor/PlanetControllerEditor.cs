using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlanetController))]
public class PlanetControllerEditor : Editor {

    private int chunk = 1;

    // Use this for initialization
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PlanetController script = (PlanetController)target;
        if(GUILayout.Button("Add Chunk"))
        {
            if(Application.isPlaying)
            {
                script.GenerateChunk(1, chunk);
                chunk++;
            }
        }
    }
}
