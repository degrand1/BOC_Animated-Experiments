using UnityEngine;
using System.Collections;

public class DramaticZoom : MonoBehaviour {
	private GameObject Ball;
	private float acc = 1;
	private Vector3 start;

	public float duration = 1;

	[Range(0.0f, 1.0f)]
	public float minTimeScale = 0.2f;

	// Use this for initialization
	void Start () {
		start = transform.position;
		Ball = GameObject.FindGameObjectWithTag ("Ball");
	}
	
	// Update is called once per frame
	void Update () {
		if ( acc < 1 ) {
			acc += Time.deltaTime;
			float interp = Easing.Quadratic.InOut( acc/duration ); // between 0 and 1
			float inflect = Mathf.Abs( interp-0.5f ); // map 0 -> 1 monotonic function to 0.5 -> 0 -> 0.5 function
			float final = inflect * ( 1-minTimeScale ) / 0.5f + minTimeScale; // map 0.5 -> 0 -> 0.5 to 1 -> minTimeScale -> 1
			Time.timeScale = Mathf.Clamp( final, 0, 1 );
		}

		// for debug use only DELETEME
		if (Input.GetKey (KeyCode.LeftShift) && acc >= 1) {
			acc = 0;
		}
	}
}
