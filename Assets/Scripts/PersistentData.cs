using UnityEngine;
using System.Collections;

public class PersistentData : MonoBehaviour {

	public int lives = 3;
	
	void Start () {
		DontDestroyOnLoad(gameObject);
	}
}
