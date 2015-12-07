using UnityEngine;
using System.Collections;

public class MoveBall : MonoBehaviour {

	public Vector2 initialBallVelocity = new Vector2( 700f, 500f );
	public string fireBallKey = "Fire1";
	public TrailRenderer trail;

	private Rigidbody rigidBody;
	private bool ballIsActive;
	

	public bool GetIsBallActive()
	{
		return ballIsActive;
	}

	void Awake () {
		rigidBody = GetComponent<Rigidbody>();
	}

	void Update () {
		if( Input.GetButtonDown (fireBallKey) && !ballIsActive )
		{
			//Unparent the ball so it will move on its own
			transform.parent = null;
			ballIsActive = true;
			rigidBody.isKinematic = false;
			transform.localScale = new Vector3( 1.0f, 1.0f, 1.0f );
			rigidBody.AddForce( initialBallVelocity );
			trail.time = 0.5f;
		}
	}
}
