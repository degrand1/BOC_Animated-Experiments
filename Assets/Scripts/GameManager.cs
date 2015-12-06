using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// A singleton class used to keep track of gameover logic, load subsequent levels, and store persistent data

public class GameManager : MonoBehaviour {
	
	public GameObject deathParticles;
	public GameObject player;
	public string nextLevel;
	public float loadNextLevelDelay = 1.0f;
	public float reviveDelay = 1.0f;
	public AudioClip deathSound;
	public AudioClip gameOverSound;
	public AudioClip[] songs;
	public static GameManager instance = null;

	private int bricks;
	private int startingLives;
	private float originalDestroyTime;
	private float gameOverTime = 6.0f;
	private GameObject playerClone;
	private AudioSource audio;
	private PersistentData data = null;
	private Text livesText;

	void PlaySong()
	{
		if( audio == null )
		{
			audio = GetComponent<AudioSource>();
		}
		int randClip = Random.Range (0, songs.Length);
		audio.clip = songs[randClip];
		audio.Play();
	}

	public delegate void BallBounceListener();
	public event BallBounceListener onBallBounce;

	void Awake() 
	{
		if( instance == null )
		{
			instance = this;
			PlaySong();
		}
		else if( instance != this )
		{
			Destroy(gameObject);
		}
	}

	void Setup()
	{
		//Count all the bricks in the scene
		GameObject[] brickObjects = GameObject.FindGameObjectsWithTag( "Brick" );
		bricks = 0;
		foreach( GameObject brickObject in brickObjects )
		{
			Brick brickComponent = brickObject.GetComponent<Brick>();
			if( brickComponent.canBreak )
			{
				bricks++;
			}
		}
		playerClone = Instantiate( player, transform.position, Quaternion.identity ) as GameObject;
		//livesText is part of the Canvas, which gets deleted between scenes
		livesText = GameObject.FindGameObjectWithTag( "LivesText" ).GetComponent<Text>();
		livesText.text = "Lives: " + data.lives;
	}

	void Start()
	{
		data = gameObject.GetComponent<PersistentData>();
		originalDestroyTime = deathParticles.GetComponent<DeleteAfterElapsedTime>().destroyTime;
		startingLives = data.lives;
		Setup();
	}

	void OnLevelWasLoaded( int Level)
	{
		if( instance == this )
		{
			Setup();
			switch( Level )
			{
			case 0:
				nextLevel = "levelTwo";
				break;
			case 1:
				nextLevel = "levelOne";
				break;
			default:
				Debug.LogError( "Loaded an unknown level." );
				break;
			}
		}
	}

	void LoadNextLevel()
	{
		Application.LoadLevel( nextLevel );
	}

	void LoadLevelOne()
	{
		deathParticles.GetComponent<DeleteAfterElapsedTime>().destroyTime = originalDestroyTime;
		Application.LoadLevel( "levelOne" );
		data.lives = startingLives;
	}

	void PlayGameOverSound()
	{
		audio.PlayOneShot(gameOverSound);
	}

	void RevivePlayer()
	{
		playerClone = Instantiate( player, transform.position, Quaternion.identity ) as GameObject;
	}

	public AudioSource GetAudioSource()
	{
		return audio;
	}

	public void DestroyBrick()
	{
		bricks--;
		if( bricks < 1 )
		{
			Invoke ( "LoadNextLevel", loadNextLevelDelay );
		}
	}

	public void LoseLife()
	{
		data.lives--;
		livesText.text = "Lives: " + data.lives;
		if( data.lives < 1 )
		{
			Invoke ( "PlayGameOverSound", reviveDelay );
			//Make the playe wallow in defeat with a longer death animation
			deathParticles.GetComponent<DeleteAfterElapsedTime>().destroyTime = gameOverTime+reviveDelay;
			//Add the revive delay since we want to wait until the death sound ends
			Invoke ( "LoadLevelOne", gameOverTime+reviveDelay );
		}
		else
		{
			Invoke ( "RevivePlayer", reviveDelay );
			deathParticles.GetComponent<DeleteAfterElapsedTime>().destroyTime = originalDestroyTime;
		}
		Instantiate( deathParticles, playerClone.transform.position, Quaternion.identity );
		Destroy( playerClone );
		audio.PlayOneShot( deathSound );
	}

	public void BallBounced()
	{
		if ( onBallBounce != null ) onBallBounce();
	}
}
