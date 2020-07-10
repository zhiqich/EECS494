using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InteractiveOrgan : MonoBehaviour {
	public enum MovementType {Idle, Slow, Fast}
	public enum InteractionMode {None, Wheel, Cam}
	
	private MovementType movementType = MovementType.Idle;
	private InteractionMode interactionMode = InteractionMode.None;

	bool pitchFixed = false; // avoid pitch setter getting called when we reach the right tempo (crackles sound)

	public GameObject turningWheel;
	public GFMachine gearFactoryMachine;
	public GameObject animationObject;
	public Transform camBox;
	public ParticleSystem goodSpeedParticles;
	public ParticleSystem toFastParticles;
	

	private Vector3? firstClick = null;
	private AudioSource aud;
	private Animation anim;
	
	// Use this for initialization
	void Start () {
		aud = GetComponent<AudioSource>();
		aud.pitch = 0f;
		aud.Play();
		
		anim = animationObject.GetComponent<Animation>();
		anim["Dancing"].wrapMode = WrapMode.Loop;
		anim["DancingSlow"].wrapMode = WrapMode.Loop;
		anim["Idle"].wrapMode = WrapMode.Loop;

        Sparkle(toFastParticles, false);
        Sparkle(goodSpeedParticles, false);

    }

    // Update is called once per frame
    private float prevAngle = 0f;
	
	public float curSpeed = 0f;
	public float speedDamping = 0.5f;
	public bool ccw = false;
	private string currentAnim = "Idle";
	
	public float degPerSec = 0f;
	Vector3 buttonDownAt = Vector3.zero;
	
	private void HandleInteraction()
	{
		float headingAngle = 0f;
		if (Input.GetMouseButton(0))
		{
			// calculate direction and distance from mouse to wheel
			Vector3 direction = Input.mousePosition - Camera.main.WorldToScreenPoint(turningWheel.transform.position);
			float distance = direction.magnitude;
			direction.Normalize();
			
			// Rotate camera
			if ((interactionMode == InteractionMode.None && distance > 200f) || interactionMode == InteractionMode.Cam)
			{
				interactionMode = InteractionMode.Cam;
				if (firstClick == null)
				{
					firstClick = camBox.localRotation.eulerAngles;
					buttonDownAt = Input.mousePosition;
				}
				Vector3 camRot = firstClick.Value;
				camRot.y = firstClick.Value.y + (((Input.mousePosition.x - buttonDownAt.x) / Screen.width) * 360f);
				camBox.localRotation = Quaternion.Euler(0f, camRot.y, 0f);
			}
			else
			{
				interactionMode = InteractionMode.Wheel;

				if (firstClick == null)
					firstClick = turningWheel.transform.localRotation.eulerAngles;
			
				headingAngle = -Mathf.Rad2Deg * Mathf.Atan2(direction.y, direction.x);
				turningWheel.transform.localRotation = Quaternion.Euler(firstClick.Value.x, firstClick.Value.y, headingAngle);
			}
		}
		else 
		{
			firstClick = null;
			interactionMode = InteractionMode.None;
		}
		
		if (interactionMode == InteractionMode.Wheel)
		{
			ccw = headingAngle < prevAngle;
			float angle = Mathf.Max(headingAngle, prevAngle) - Mathf.Min(headingAngle, prevAngle);
			if (angle > 180f)
				angle -= 360f;
			degPerSec = angle / Time.deltaTime * (ccw ? 1 : -1);
			prevAngle = headingAngle;
		}
		else 
			degPerSec = 0f;
	}

	void Update () {
		HandleInteraction();	
		curSpeed += (degPerSec / 360f);
		gearFactoryMachine.Step(curSpeed);
		curSpeed -= curSpeed * (speedDamping*Time.deltaTime);

		float speed = Mathf.Abs(curSpeed);
		
		if (speed < 0.1f)
			curSpeed = 0f;
		 
		#region Manage pipes animation state thresholds
		float animSpeed = 1.0f;
		if (speed > 30f)
		{
			if (speed < 150f)
			{
				movementType = MovementType.Slow;
				animSpeed = speed / 70f;
			}
			else
			{
				movementType = MovementType.Fast;
				animSpeed = speed / 200f;
			}
			
			// Specify sweetspots
			aud.volume = speed > 75f ? 1.0f : Mathf.Clamp01((speed - 50f) / (75f-50f)); // fadeout from 75->50
			if (speed > 150f && speed < 200f)
			{
				if (!pitchFixed)
				{
					pitchFixed = true;
					aud.pitch = ccw ? -1f : 1f;
					Sparkle(goodSpeedParticles, !ccw);
				}
			}
			else 
			{
				pitchFixed = false;
				aud.pitch = -curSpeed / 170f; 
				Sparkle(goodSpeedParticles, false);
			}
			
		}
		else 
		{
			movementType = MovementType.Idle;
			animSpeed = 1f;
			Sparkle(goodSpeedParticles, false);
		}

        Sparkle(toFastParticles, speed >= 200f);

        #endregion

        SetAnim(animSpeed);
	}

	private void Sparkle(ParticleSystem particles, bool emit)
	{ 
        if (emit)
        {
            if (!particles.isPlaying)
                particles.Play();
        }
        else
        {
            if (particles.isPlaying)
                particles.Stop();
        }
    }
	
	private void SetAnim(float animSpeed)
	{
		string prevAnim = currentAnim;
		switch (movementType)
		{
			case MovementType.Slow: currentAnim = "DancingSlow"; break;
			case MovementType.Fast: currentAnim = "Dancing"; break;
			case MovementType.Idle: currentAnim = "Idle"; break;
		}

		// Animation changed ? Crossfade to new animation
		if (prevAnim != currentAnim)
		{
			anim.CrossFade(currentAnim, 1f);			
		}
		
		if (anim[currentAnim] != null)
			anim[currentAnim].normalizedSpeed = animSpeed;
		else 
			Debug.Log("anim: "+currentAnim+" == null");
			
	}
}
