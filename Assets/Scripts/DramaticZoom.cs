using UnityEngine;
using System.Collections;

public class DramaticZoom : MonoBehaviour {
	private GameObject Ball;
	private float acc = 1;
	public float duration = 1;

	// Use this for initialization
	void Start () {
		Ball = GameObject.FindGameObjectWithTag ("Ball");
	}
	
	// Update is called once per frame
	void Update () {
		if ( acc < 1 ) {
			acc += Time.deltaTime;
			Time.timeScale = Mathf.Clamp( Mathf.Abs( Easing.Quadratic.InOut( acc/duration ) - 0.5f ) + 0.5f, 0, 1 );
		}

		// for debug use only DELETEME
		if (Input.GetKey (KeyCode.LeftShift) && acc >= 1) {
			acc = 0;
		}
	}
}
