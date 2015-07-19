using UnityEngine;
using System.Collections;

public class TempCrosshair : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	void OnGUI(){
		GUI.Box(new Rect(Screen.width/2,Screen.height/2, 10, 10), "");

	}

	void OnDrawGizmos() {

	}
	
	// Update is called once per frame
	void Update () {
		Vector3 forward = transform.TransformDirection(Vector3.forward) * 10;
		Debug.DrawRay(transform.position, forward, Color.red);
	}
}
