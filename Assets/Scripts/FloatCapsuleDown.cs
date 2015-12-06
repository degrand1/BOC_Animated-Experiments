using UnityEngine;
using System.Collections;

public class FloatCapsuleDown : MonoBehaviour {

	public float speed = 1.0f;
	
	void Update () {
		float yPosition = transform.position.y - Time.deltaTime * speed;
		transform.position = new Vector3( transform.position.x, yPosition, transform.position.z );
	}
}
