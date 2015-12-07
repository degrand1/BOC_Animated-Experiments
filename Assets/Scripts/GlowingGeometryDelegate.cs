using UnityEngine;
using System.Collections;

public class GlowingGeometryDelegate : MonoBehaviour {
	private Material m;

	// Use this for initialization
	void Start () {
		m = GetComponent<Renderer>().material;
	}
	
	// Update is called once per frame
	void Update () {
		if (m != null) {
			m.SetVector ( "_GameObjectPosition", transform.position );
		}
	}
}
