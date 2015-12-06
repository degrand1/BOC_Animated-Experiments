using UnityEngine;
using System.Collections;

public class DeleteAfterElapsedTime : MonoBehaviour {

	public float destroyTime = 1f;

	void Start () {
		Destroy (gameObject, destroyTime);
	}
}
