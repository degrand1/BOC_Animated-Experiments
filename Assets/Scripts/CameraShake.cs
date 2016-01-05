using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour {
	private Vector3 origin;
	public float shakeMagnitudeMultiplier = 0.01f;
	private float currentShakeMagnitudeX = 0f;
	private float currentShakeMagnitudeY = 0f;
	public float shakeDampener = 0.8f;

	void Start() {
		GameManager.instance.onBallBounce += ShakeOnBounce;
	}

	void OnDestroy() {
		GameManager.instance.onBallBounce -= ShakeOnBounce;
	}
	
	// Update is called once per frame
	void Update () {
		if ( currentShakeMagnitudeX > 0.001 || currentShakeMagnitudeY > 0.001 ) {
			transform.position = origin + new Vector3( Random.value * Mathf.Sin( currentShakeMagnitudeX )
																						   , Random.value * Mathf.Sin( currentShakeMagnitudeY )
																							 , 0 );
			currentShakeMagnitudeX *= shakeDampener;
			currentShakeMagnitudeY *= shakeDampener;
		}
	}

	void ShakeOnBounce() {
		origin = transform.position;
		GameObject ball = GameObject.FindGameObjectWithTag ("Ball");
		currentShakeMagnitudeX = shakeMagnitudeMultiplier * ball.GetComponent<Rigidbody>().velocity.x;
		currentShakeMagnitudeY = shakeMagnitudeMultiplier * ball.GetComponent<Rigidbody>().velocity.y;
	}
}
