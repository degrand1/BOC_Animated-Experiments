﻿using UnityEngine;
using System;

public enum PowerUp {
	NONE,
	INCREASE_LENGTH,
};

public class Brick : MonoBehaviour {

	public GameObject brickParticles;
	public GameObject powerCapsule;
	public AudioClip impactSound;
	public AudioClip unbreakableSound;
	public AudioClip stillHoldingSound;
	public int hitsToBreak = 1;
	public bool canBreak = true;
	public PowerUp brickPowerUp = PowerUp.NONE;
	public static int bricksLeft = 0;

	void Start()
	{
		bricksLeft++;
		if (GameObject.FindGameObjectsWithTag ("Brick").Length == 1) {
			GameManager.instance.PenultimateBrickDestroyed ();
		}
	}

	void OnCollisionEnter( Collision other)
	{
		if( other.gameObject.tag == "Ball" )
		{
			GameManager.instance.BallBounced();
			if( !canBreak )
			{
				GameManager.instance.GetAudioSource().PlayOneShot( unbreakableSound );
			}
			else
			{
				hitsToBreak--;
				if( hitsToBreak < 1 )
				{
					Instantiate(brickParticles, transform.position, Quaternion.identity);
					if( brickPowerUp != PowerUp.NONE )
					{
						Instantiate(powerCapsule, transform.position, powerCapsule.transform.rotation);
						powerCapsule.GetComponent<GrantPowerUp>().capsulePower = brickPowerUp;
					}
					GameManager.instance.DestroyBrick();
					GameManager.instance.GetAudioSource().PlayOneShot( impactSound );
					Destroy(gameObject);

					bricksLeft--;
					if ( bricksLeft == 1 ) {
						GameManager.instance.PenultimateBrickDestroyed();
					}
				}
				else
				{
					GameManager.instance.GetAudioSource().PlayOneShot( stillHoldingSound );
				}
			}
		}
	}
}
