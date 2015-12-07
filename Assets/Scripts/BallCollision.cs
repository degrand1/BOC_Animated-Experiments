using UnityEngine;
using System.Collections;

public class BallCollision : MonoBehaviour {

	void OnCollisionEnter( Collision other )
	{
		if( other.gameObject.tag == "Ball" )
		{
			Transform BallRenderer = other.transform.GetChild(0);
			Animator animator = BallRenderer.GetComponent<Animator>();
			//if the animation is already playing
			if( animator.GetCurrentAnimatorStateInfo(0).IsName("StartImpact") )
			{
				BallRenderer.GetComponent<Animator>().Play ( "StartImpact", -1, 0f );
			}
			else
			{
				animator.SetTrigger( "StartImpact" );
			}

			Quaternion rotation = BallRenderer.transform.rotation;
			//zero out the rotation in case we're still in the middle of another animation
			BallRenderer.transform.rotation = Quaternion.identity;
			//Rotate the ball about the normal vector of the collision point so the animation looks correct
			float AmountToRotate = Vector3.Angle ( other.contacts[0].normal, Vector3.up );
			other.transform.GetChild(0).Rotate( new Vector3( rotation.x, rotation.y, AmountToRotate ) );
		}
	}
}
