using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour {
	private Vector3 origin;
	public float shakeMagnitude = .3f;
	public float currentShakeMagnitude = 0f;
	public float shakeDecay = 0.05f;

	void Start() {
		GameManager.instance.onBallBounce += Shake;
	}

	void OnDisable() {
		GameManager.instance.onBallBounce -= Shake;
	}
	
	// Update is called once per frame
	void Update () {
		if ( currentShakeMagnitude > 0 ) {
			transform.position = origin + Random.insideUnitSphere * currentShakeMagnitude;
			currentShakeMagnitude -= shakeDecay;
		}
	}

	void Shake() {
		origin = transform.position;
		currentShakeMagnitude = shakeMagnitude;
	}
}
