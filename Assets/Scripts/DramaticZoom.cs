using UnityEngine;
using System.Collections;

public class DramaticZoom : MonoBehaviour {
	private Rigidbody ballRigidBody;
	private GameObject ball;
	private float acc = 1;
	private Vector3 originalPos;
	private float originalSize;
	private bool predict = false;

	public float duration = 1;

	[Range(0.0f, 1.0f)]
	public float dramaticZoom = 0.2f;

	[Range(0.0f, 1.0f)]
	public float minTimeScale = 0.2f;

	// Use this for initialization
	void Start () {
		acc = duration;
		originalPos = transform.position;
		ball = GameObject.FindGameObjectWithTag ("Ball");
		if (ball != null) {
			ballRigidBody = ball.GetComponent<Rigidbody> ();
		}
		originalSize = Camera.main.orthographicSize;

		if ( GameManager.instance.numBricks == 1 ) activateTrajectoryPrediction();
		else GameManager.instance.onFinalBrick += activateTrajectoryPrediction;
	}

	void activateTrajectoryPrediction() {
		predict = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (ball == null) {
			ball = GameObject.FindGameObjectWithTag ("Ball");
			if ( ball != null ) {
				ballRigidBody = ball.GetComponent<Rigidbody> ();
			}
		}
		if (acc < duration) {
			acc += Time.deltaTime / Time.timeScale;
			float eased = Easing.Sine.InOut (acc / duration); // between 0 and 1
			float inflect = Mathf.Abs (eased - 0.5f); // map 0 -> 1 monotonic function to 0.5 -> 0 -> 0.5 function
			float weight = inflect * (1 - minTimeScale) / 0.5f + minTimeScale; // map 0.5 -> 0 -> 0.5 to 1 -> minTimeScale -> 1
			weight = Mathf.Clamp (weight, 0, 1);
			Time.timeScale = weight;
			transform.position = Vector3.Lerp ( new Vector3( ball.transform.position.x, ball.transform.position.y, transform.position.z )
			                                  , originalPos
			                                  , weight );
			Camera.main.orthographicSize = Mathf.Lerp( originalSize * dramaticZoom, originalSize, weight );
		} else {
			Camera.main.orthographicSize = originalSize;
		}

		if ( acc >= duration && WillHitBrick() ) {
			acc = 0;
		}
	}

	bool WillHitBrick() {
		if (!predict) {
			return false;
		}

		if ( GameManager.instance.numBricks < 1 ) {
			predict = false;
			return false;
		}

		Collider brickCollider = GameObject.FindGameObjectWithTag( "Brick" ).GetComponent<Collider>();

		if (brickCollider != null) {
			Ray ray = new Ray (ball.transform.position, ballRigidBody.velocity);
			float dist;
			Bounds bb = brickCollider.bounds;
			bb.Expand( bb.size );
			bool hits = bb.IntersectRay(ray, out dist);
			return hits && dist <= ballRigidBody.velocity.magnitude * duration / 4;
		} else {
			return false;
		}
	}
}
