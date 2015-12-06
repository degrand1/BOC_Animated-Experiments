using UnityEngine;
using System.Collections;

public class DynamicBackground : MonoBehaviour {
	private Renderer bg;
	private GameObject player;
	private GameObject ball;
	private float acc;
	private float freq;
	private float dir;

	// Use this for initialization
	void Start () {
		acc = 0.0f;
		dir = 1.0f;
		bg = GetComponent<Renderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		freq = bg.material.GetFloat ("_WubFrequency");
		acc += dir * Time.deltaTime;
		if ( dir > 0 && acc > freq )  dir = -1;
		if ( dir < 0 && acc < 0 )  dir = 1;
		if ( player == null ) player = GameObject.FindGameObjectWithTag( "Player" );
		if ( ball == null ) ball = GameObject.FindGameObjectWithTag( "Ball" );
		if ( player != null ) bg.material.SetVector("_PlayerPosition", Camera.main.WorldToScreenPoint( player.transform.position ) );
		if ( ball != null ) bg.material.SetVector("_BallPosition", Camera.main.WorldToScreenPoint( ball.transform.position ) );
		bg.material.SetFloat("_WubTime", acc );
	}
}
