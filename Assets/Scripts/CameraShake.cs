using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour {
	private Vector3 origin;
	public float shakeMagnitude = .25f;
	private float currentShakeMagnitude = 0f;
	public float shakeDampener = 0.9f;
	public float bounceMultiplierBallMaxSpeed = 25.0f;

	void Start() {
		GameManager.instance.onBallBounce += ShakeOnBounce;
	}

	void OnDestroy() {
		GameManager.instance.onBallBounce -= ShakeOnBounce;
	}
	
	// Update is called once per frame
	void Update () {
		if ( currentShakeMagnitude > 0 ) {
			transform.position = origin + Random.insideUnitSphere * currentShakeMagnitude;
			currentShakeMagnitude *= shakeDampener;
		}
	}

	void ShakeOnBounce() {
		origin = transform.position;
		GameObject ball = GameObject.FindGameObjectWithTag ("Ball");
		currentShakeMagnitude = shakeMagnitude * ball.GetComponent<Rigidbody>().velocity.magnitude / bounceMultiplierBallMaxSpeed;
	}
}
