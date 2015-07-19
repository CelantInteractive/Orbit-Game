using UnityEngine;
using System.Collections;

public class SurfaceDamage : MonoBehaviour {
	
	public float resistance = 1.0f;

	public float spreading = 1.0f;

	public GameObject particle;

	double lowestTime = 99999;

	Vector3[] debugVerts = new Vector3[0];

	Vector3 hitPos = Vector3.zero;

	public void Destruct(DestructorBase destructorScript) {

		Debug.Log(Time.time + " - Destruct called");

		float destructorScriptForce = destructorScript.force;

		if(destructorScriptForce > resistance) {
			float impactRange = destructorScript.impactRange;
			impactRange *= spreading;

			hitPos = destructorScript.hitPos;

			MeshFilter meshFilter = (MeshFilter)gameObject.GetComponent (typeof(MeshFilter));

			Mesh mesh = meshFilter.mesh;

			Vector3[] verticies = mesh.vertices;
			Vector3[] normals = mesh.normals;

			debugVerts = verticies;

			bool isChanged = false;

			for(int i = 0; i < verticies.Length; i++) {

				float force = destructorScriptForce;
				float distance = Vector3.Distance(hitPos, transform.TransformPoint(verticies[i])); 

				if(distance < lowestTime) {
					lowestTime = distance;
				}

				if(distance <= impactRange) {
					force = force * ((impactRange - distance) / impactRange) - resistance;
					if(force > 0) {
						if(particle && !isChanged) {
							Instantiate(particle, verticies[i], Quaternion.FromToRotation(Vector3.up, normals[i]));
						}
						verticies[i] -= normals[i] * force;
						isChanged = true;
					}
				}
			}
			Debug.Log(lowestTime + " <= " + impactRange);

			if(isChanged) {
				mesh.vertices = verticies;
				mesh.RecalculateBounds();

				MeshCollider meshCollider = (MeshCollider)gameObject.GetComponent (typeof(MeshCollider));
				meshCollider.sharedMesh = mesh;
			}
		}
	}

	void OnDrawGizmos() {
			Gizmos.color = Color.green;
			Gizmos.DrawSphere(hitPos, 0.01f);

		for(int i = 0; i < debugVerts.Length; i++) {
			Gizmos.color = Color.red; 
			Gizmos.DrawSphere(transform.TransformPoint(debugVerts[i]), 0.01f);
		}
	}
}
