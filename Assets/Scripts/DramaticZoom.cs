using UnityEngine;
using System.Collections;

public class DramaticZoom : MonoBehaviour {
	private GameObject Ball;
	private float acc = 1;
	public float duration = 1;
	[Range(0.0f, 1.0f)] public float minTimeScale = 0.2f;

	// Use this for initialization
	void Start () {
		Ball = GameObject.FindGameObjectWithTag ("Ball");
	}
	
	// Update is called once per frame
	void Update () {
		if ( acc < 1 ) {
			acc += Time.deltaTime;
			float interp = Easing.Quadratic.InOut( acc/duration ); // between 0 and 1
			Time.timeScale = Mathf.Clamp( Mathf.Abs( interp-0.5f ) * ( 1-minTimeScale ) / 0.5f + minTimeScale, 0, 1 );
		}

		// for debug use only DELETEME
		if (Input.GetKey (KeyCode.LeftShift) && acc >= 1) {
			acc = 0;
		}
	}
}
