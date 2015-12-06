using UnityEngine;
using System.Collections;

public class MovePlayer : MonoBehaviour {

	public float MovementSpeed = 1.0f;
	public float MaxVelocity = 100f;
	public Renderer playerRenderer;

	private float leftBoundary = -12.9f;
	private float rightBoundary = 13.9f;
	private GameObject leftWall;
	private Renderer leftWallRenderer;
	private GameObject rightWall;
	private Renderer rightWallRenderer;
	private MoveBall BallData;
	private Vector3 originalScale;
	private Renderer localRenderer;

	void Start()
	{
		if( leftWall == null )
		{
			leftWall = GameObject.FindGameObjectWithTag( "LeftWall" );
		}
		leftWallRenderer = leftWall.GetComponent<Renderer>();
		if( rightWall == null )
		{
			rightWall = GameObject.FindGameObjectWithTag( "RightWall" );
		}
		rightWallRenderer = rightWall.GetComponent<Renderer>();
		localRenderer = gameObject.GetComponent<Renderer>();
		CalculateBoundaries();
		if( BallData == null )
		{
			GameObject Ball = GameObject.FindGameObjectWithTag( "Ball" );
			BallData = Ball.GetComponent<MoveBall>();
		}
		originalScale = playerRenderer.transform.localScale;
	}

	void CalculateBoundaries()
	{
		//The boundaries are determined by the position of the left/right wall offset by the width of the wall and the player
		leftBoundary = leftWall.transform.position.x + leftWallRenderer.bounds.extents.x + localRenderer.bounds.extents.x;
		rightBoundary = rightWall.transform.position.x - rightWallRenderer.bounds.extents.x - localRenderer.bounds.extents.x;
	}

	void Update () {
		CalculateBoundaries();
		//float xPos = transform.position.x + (Input.GetAxisRaw("Horizontal")*MovementSpeed);
		float xPos = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
		xPos = Mathf.Clamp( xPos, leftBoundary, rightBoundary);
		float velocity = Mathf.Abs(transform.position.x - xPos);
		float unitVelocity = velocity/MaxVelocity;
		float xScale = Mathf.Lerp ( 1f, 5f/3f, unitVelocity );
		float yScale = Mathf.Lerp ( 0.7f, 1f, 1f - unitVelocity );
		float xValue = originalScale.x*xScale;
		playerRenderer.transform.localScale = new Vector3( xValue, originalScale.y*yScale, 1 );
		transform.position = new Vector3( Mathf.Clamp( xPos, leftBoundary, rightBoundary), transform.position.y, transform.position.z );
	}
}
