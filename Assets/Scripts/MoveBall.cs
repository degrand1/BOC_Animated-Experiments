using UnityEngine;
using System.Collections;

public class MoveBall : MonoBehaviour {

	public Vector2 initialBallVelocity = new Vector2( 700f, 500f );
	public string fireBallKey = "Fire1";
	public float shrinkTimeAfterCollision = 2.0f;

	private Rigidbody rigidBody;
	private bool ballIsActive;
	private float shrinkBallTimer;
	private Vector3 originalScale;
	private Vector3 originalVelocity;

	//Shrink the ball after a collision occurs
	public void ShrinkBall()
	{
		if( shrinkBallTimer == 0 )
		{
			shrinkBallTimer = shrinkTimeAfterCollision;
			/*
			originalScale = transform.localScale;
			originalVelocity = transform.GetComponent<Rigidbody>().velocity;
			transform.localScale -= new Vector3( transform.localScale.x/2, transform.localScale.y/2, 0 );
			transform.GetComponent<Rigidbody>().velocity -= new Vector3( originalVelocity.x/2, originalVelocity.y/2, 0 );
			*/
		}
	}

	void Awake () {
		rigidBody = GetComponent<Rigidbody>();
		shrinkBallTimer = 0f;
	}

	void Update () {
		if( Input.GetButtonDown (fireBallKey) && !ballIsActive )
		{
			//Unparent the ball so it will move on its own
			transform.parent = null;
			ballIsActive = true;
			rigidBody.isKinematic = false;
			rigidBody.AddForce( initialBallVelocity );
		}
		if( shrinkBallTimer != 0 )
		{
			shrinkBallTimer -= Time.deltaTime;
			if( shrinkBallTimer <= 0 )
			{
				shrinkBallTimer = 0f;
				/*
				transform.localScale = originalScale;
				transform.GetComponent<Rigidbody>().velocity = originalVelocity;
				*/
			}
		}
	}
}
