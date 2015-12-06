using UnityEngine;
using System.Collections;

public class DeadZone : MonoBehaviour {

	void OnTriggerEnter( Collider other )
	{
		if( other.gameObject.tag == "Ball" )
		{
			GameManager.instance.LoseLife();
			DestroyObject( other.gameObject );
		}
		else if( other.gameObject.tag == "PowerUp" )
		{
			DestroyObject( other.gameObject );
		}
	}
}
