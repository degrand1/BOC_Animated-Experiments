using UnityEngine;
using System.Collections;

public class PostEffects : MonoBehaviour {
	public Shader postFXShader = null;
	public Shader DOFShader = null;

	private Material mat;
	private Material DOFOut;

	// Use this for initialization
	void Start () {
		if (postFXShader)
		{
			mat = new Material( postFXShader );
			mat.name = "PostFXMaterial";
			mat.hideFlags = HideFlags.HideAndDontSave;
		}
		else
		{
			Debug.LogWarning(gameObject.name + ": Post FX Shader is not assigned. Disabling...", this.gameObject);
			enabled = false;
		}

		if ( DOFShader ) {
			Camera.main.depthTextureMode = DepthTextureMode.Depth;
			DOFOut = new Material( DOFShader );
			DOFOut.name = "DOFMaterial";
		}
	}
	
	void OnRenderImage(RenderTexture src, RenderTexture dst)
	{
		if ( DOFShader && DOFOut ) {
			Graphics.Blit( src, dst, DOFOut );
		}

		if (postFXShader && mat)
		{
			Graphics.Blit(src, dst, mat);
		}
		else
		{
			Graphics.Blit(src, dst);
			Debug.LogWarning(gameObject.name + ": Post FX Shader is not assigned. Disabling...", this.gameObject);
			enabled = false;
		}
	}

	void OnDisable()
	{
	}
}
