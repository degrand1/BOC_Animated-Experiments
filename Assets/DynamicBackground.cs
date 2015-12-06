using UnityEngine;
using System.Collections;

public class DynamicBackground : MonoBehaviour {
	private Renderer bg;
	private GameObject player;
	private GameObject ball;

	// Use this for initialization
	void Start () {
		bg = GetComponent<Renderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		if ( player == null ) player = GameObject.FindGameObjectWithTag( "Player" );
		if ( ball == null ) ball = GameObject.FindGameObjectWithTag( "Ball" );
		if ( player != null ) bg.material.SetVector("_PlayerPosition", Camera.main.WorldToScreenPoint( player.transform.position ) );
		if ( ball != null ) bg.material.SetVector("_BallPosition", Camera.main.WorldToScreenPoint( ball.transform.position ) );
	}
}
