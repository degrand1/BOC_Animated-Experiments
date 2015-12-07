using UnityEngine;
using System.Collections;

public class EmpowerPlayer : MonoBehaviour {

	public float increaseLengthTime = 10.0f;

	private float currentTime = 0.0f;
	private PowerUp currentPower = PowerUp.NONE;
	private bool powerActive = false;
	private Animator anim;

	void Start()
	{
		anim = GameObject.FindGameObjectWithTag( "Player" ).GetComponent<Animator>();
	}

	void SetAnimation( string Name, bool Value )
	{
		if( anim == null )
		{
			anim = GameObject.FindGameObjectWithTag( "Player" ).GetComponent<Animator>();
		}
		anim.SetBool( Name, Value );
	}

	public void GrantPower( PowerUp powerUp )
	{
		if( currentPower == powerUp )
		{
			//Just reset the current time if we get the same power up twice
			currentTime = 0.0f;
			return;
		}
		switch( powerUp )
		{
		case PowerUp.NONE:
			Debug.LogError( "Trying to grant a power with the NONE type" );
			break;
		case PowerUp.INCREASE_LENGTH:
			currentTime = 0.0f;
			powerActive = true;
			currentPower = powerUp;
			SetAnimation( "IncreaseLength", true );
			break;
		}
	}

	void Reset()
	{
		powerActive = false;
		currentTime = 0.0f;
		currentPower = PowerUp.NONE;
	}

	void Update()
	{
		if( !powerActive ) return;

		currentTime += Time.deltaTime;
		switch( currentPower )
		{
		case PowerUp.NONE:
			break;
		case PowerUp.INCREASE_LENGTH:
			if( currentTime >= increaseLengthTime )
			{
				Reset();
				SetAnimation ( "IncreaseLength", false );
			}
			break;
		}
	}
}
