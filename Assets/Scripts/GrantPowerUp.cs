using UnityEngine;
using System.Collections;

public class GrantPowerUp : MonoBehaviour {

	public PowerUp capsulePower = PowerUp.NONE;
	
	private EmpowerPlayer empowerPlayer;

	void Start()
	{
		GameObject player = GameObject.FindGameObjectWithTag( "Player" );
		empowerPlayer = player.GetComponent<EmpowerPlayer>();
	}

	void OnTriggerEnter( Collider other )
	{
		if( other.gameObject.tag == "Player" )
		{
			empowerPlayer.GrantPower( capsulePower );
			DestroyObject(gameObject);
		}
	}
}
