using UnityEngine;
using System.Collections;

public class BallAnimation : MonoBehaviour {

	public void ResetBallRotation()
	{
		transform.rotation = Quaternion.identity;
	}
}
