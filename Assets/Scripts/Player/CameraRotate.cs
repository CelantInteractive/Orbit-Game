using UnityEngine;
using System.Collections;

public class CameraRotate : MonoBehaviour
{

	public Transform target;
	public float distance = 2.0f;

	public float xSpeed = 180.0f;
	public float ySpeed = 90.0f;

	public float yMinLimit = 0.0f;
	public float yMaxLimit = 80f;

	public float speed = 0.14f;

	private float x = 0.0f;
	private float y = 0.0f;


	void Start()
	{
		var angles = transform.eulerAngles;
		x = angles.y;
		y = angles.x;

		// Make the rigid body not change rotation
		Rigidbody rigidbody = GetComponent<Rigidbody>();
		if (rigidbody)
		{
			rigidbody.freezeRotation = true;
		}

		var rotation = Quaternion.Euler(y, x, 0);
		var position = rotation * new Vector3(0, 0, -distance) + target.position;

		transform.rotation = rotation;
		transform.position = position;
	}

	void LateUpdate()
	{
		var d = Input.GetAxis("Mouse ScrollWheel");
		if (d > 0f)
		{
			distance -= 0.5f;
		}
		else if (d < 0f)
		{
			distance += 0.5f;
		}

		if (target && Input.GetMouseButton(0))
		{
			x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
			y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

			y = ClampAngle(y, yMinLimit, yMaxLimit);
		}

		distance = Mathf.Clamp(distance, 5f, 100f);

		var rotation = Quaternion.Euler(y, x, 0);
		var position = rotation * new Vector3(0, 0, -distance) + target.position;

		transform.rotation = rotation;
		transform.position = position;
	}

	static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360)
		angle += 360;
		if (angle > 360)
		angle -= 360;
		return Mathf.Clamp(angle, min, max);
	}
}