using UnityEngine;
using System.Collections;

public class BounceBall : MonoBehaviour {

	public float minBounceStrength = 1.0f;
	public float maxBounceStrength = 3.0f;
	public float minXSpeed = 3.0f;
	public float maxXSpeed = 10.0f;
	public float minYSpeed = 3.0f;
	public float maxYSpeed = 10.0f;
	public AudioClip impactSound;

	float AbsoluteClamp( float value, float min, float max )
	{
		if( value > -min && value < min )
		{
			value = value < 0 ? -min : min;
		}
		else if( value < -max || value > max )
		{
			value = value < 0 ? -max : max;
		}

		return value;
	}

	void OnCollisionEnter( Collision other )
	{
		if( other.gameObject.tag == "Ball" )
		{
			Vector3 reflectedVector = Vector3.Reflect( other.relativeVelocity, other.contacts[0].normal );
			reflectedVector *=Random.Range (minBounceStrength,maxBounceStrength);
			reflectedVector.x = AbsoluteClamp( reflectedVector.x, minXSpeed, maxXSpeed );
			reflectedVector.y = AbsoluteClamp( reflectedVector.y, minYSpeed, maxYSpeed );
			other.transform.GetComponent<Rigidbody>().velocity = reflectedVector;
			if( gameObject.tag == "Player" )
			{
				other.gameObject.GetComponent<MoveBall>().ShrinkBall();
			}
			else // only broadcast ball bounced if it didn't hit paddle
			{
				GameManager.instance.BallBounced();
			}
			if( impactSound != null )
			{
				GameManager.instance.GetAudioSource().PlayOneShot( impactSound );
			}
		}
	}
}
