using UnityEngine;
using System.Collections;

public class MoveBall : MonoBehaviour {

	public Vector2 initialBallVelocity = new Vector2( 700f, 500f );
	public string fireBallKey = "Fire1";
	public float shrinkTimeAfterCollision = 2.0f;

	private Rigidbody rigidBody;
	private bool ballIsActive;
	private float shrinkBallTimer;

	//Shrink the ball after a collision occurs
	public void ShrinkBall()
	{
		if( shrinkBallTimer == 0 )
		{
			shrinkBallTimer = shrinkTimeAfterCollision;
		}
	}

	public bool GetIsBallActive()
	{
		return ballIsActive;
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
			transform.localScale = new Vector3( 1.0f, 1.0f, 1.0f );
			rigidBody.AddForce( initialBallVelocity );
		}
		else if( !ballIsActive )
		{
			transform.localScale = new Vector3( 1f/transform.parent.localScale.x, 
			                                    1f/transform.parent.localScale.y, 
			                                    1f/transform.parent.localScale.z );
		}
		if( shrinkBallTimer != 0 )
		{
			shrinkBallTimer -= Time.deltaTime;
			if( shrinkBallTimer <= 0 )
			{
				shrinkBallTimer = 0f;
			}
		}
	}
}
