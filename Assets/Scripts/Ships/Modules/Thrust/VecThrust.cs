using UnityEngine;
using System.Collections;

public class VecThrust : MonoBehaviour {

	public enum ShipAxis {XAxisPos, XAxisNeg, YAxisPos, YAxisNeg, ZAxisPos, ZAxisNeg};
	public ShipAxis shipAxis;
	public float POWAAHHHH = 1F;

	private Rigidbody ship;

	// Use this for initialization
	void Start () {
		ship = transform.parent.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(GameInput.GetKey(Controls.SHIP_X_AXIS_POS))
		{
			ship.AddForce(-transform.forward * POWAAHHHH);
		}
	}
}
