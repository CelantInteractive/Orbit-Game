using UnityEngine;
using System.Collections;

public class WeaponDestructor : DestructorBase {

	public float range = 100.0f;
	public float fireRate = 0.05f;
	public float damage = 5.0f;
	public int bulletsPerClip = 40;
	public int clips = 20;
	public float reloadTime = 0.5f;
	public ParticleEmitter hitParticles;
	public Renderer muzzleFlash;
	
	public int bulletsLeft = 0;
	private float nextFireTime = 0.0f;
	private int m_LastFrameShot = -1;
	
	void Start ()
	{
		// We don't want to emit particles all the time, only when we hit something.
		if (hitParticles)
			hitParticles.emit = false;
		bulletsLeft = bulletsPerClip;
	}

	void Update() {
		if(Input.GetKeyDown("left ctrl")) {
			FireOneShot();
		}
	}
	
	void LateUpdate()
	{
		if (muzzleFlash)
		{
			// We shot this frame, enable the muzzle flash
			if (m_LastFrameShot == Time.frameCount)
			{
				muzzleFlash.transform.localRotation = Quaternion.AngleAxis(Random.value, Vector3.forward);
				muzzleFlash.enabled = true;
				
				if (GetComponent<AudioSource>())
				{
					if (!GetComponent<AudioSource>().isPlaying)
						GetComponent<AudioSource>().Play();
					GetComponent<AudioSource>().loop = true;
				}
			}
			// We didn't, disable the muzzle flash
			else
			{
				muzzleFlash.enabled = false;
				enabled = false;
				
				// Play sound
				if (GetComponent<AudioSource>())
				{
					GetComponent<AudioSource>().loop = false;
				}
			}
		}
	}
	
	void Fire ()
	{
		if (bulletsLeft == 0)
			return;
		
		// If there is more than one bullet between the last and this frame
		// Reset the nextFireTime
		if (Time.time - fireRate > nextFireTime)
			nextFireTime = Time.time - Time.deltaTime;
		
		// Keep firing until we used up the fire time
		while( nextFireTime < Time.time && bulletsLeft != 0)
		{
			FireOneShot();
			nextFireTime += fireRate;
		}
	}
	
	void FireOneShot ()
	{
		Vector3 direction = transform.TransformDirection(Vector3.forward);
		RaycastHit hit;
		
		// Did we hit anything?
		if (Physics.Raycast (transform.position, direction, out hit, range))
		{
			// Apply a force to the rigidbody we hit
			if (hit.rigidbody)
				hit.rigidbody.AddForceAtPosition(force * direction, hit.point);
			
			// Place the particle system for spawing out of place where we hit the surface!
			// And spawn a couple of particles
			/*if (hitParticles)
        {
            hitParticles.transform.position = hit.point;
            hitParticles.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
            hitParticles.Emit();
        }*/
			
			// Send a damage message to the hit object
			hitPos = hit.point;
			hit.collider.SendMessageUpwards("ApplyDamage", damage, SendMessageOptions.DontRequireReceiver);
			hit.collider.SendMessageUpwards("Destruct", this, SendMessageOptions.DontRequireReceiver);
		}
		
		bulletsLeft--;
		
		// Register that we shot this frame,
		// so that the LateUpdate function enabled the muzzleflash renderer for one frame
		m_LastFrameShot = Time.frameCount;
		enabled = true;
		
		// Reload gun in reload Time       
		if (bulletsLeft == 0)
			Reload();          
	}
	
	IEnumerator Reload () {
		
		// Wait for reload time first - then add more bullets!
		yield return new WaitForSeconds(reloadTime);
		
		// We have a clip left reload
		if (clips > 0)
		{
			clips--;
			bulletsLeft = bulletsPerClip;
		}
	}
	
	int GetBulletsLeft () {
		return bulletsLeft;
	}
}