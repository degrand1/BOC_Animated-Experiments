Shader "Custom/bloom" {
	Properties {
		_MainTex ("Screen Texture", 2D) = "white" {}
        _HighlightThreshold( "Highlight Threshold", Float) = 0.4
		_Intensity("Intensity", Float) = 1.1

        _DOFFactor( "DOFFactor", Float ) = 0.1
        _DOFDepth( "DOFDepth", Float ) = 0.0
        _DOFRange( "DOFRange", Float ) = 0.5
	}
	SubShader {
		Pass {
            Name "Bloom"

			CGPROGRAM
	        #pragma vertex vert
	        #pragma fragment frag
	        #include "UnityCG.cginc"

	        sampler2D _MainTex;
            sampler2D _CameraDepthTexture;
	        float4 _MainTex_TexelSize;

	        fixed _Intensity;
            fixed _HighlightThreshold;
            fixed _DOFFactor;
            fixed _DOFDepth;

	        struct VS_OUT {
	            float4 vertex : SV_POSITION;
	            float2 uv     : TEXCOORD0;
	        };
	       
	        VS_OUT vert( appdata_img v )
            {
	            VS_OUT o;
	           
	            o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
	            o.uv = v.texcoord;
	           
	            return o;
	        }

            float4 highlight( float4 pix )
            {
                float avg = ( pix.x + pix.y + pix.z ) / 3.0;
                return pix * smoothstep( _HighlightThreshold - 0.1, _HighlightThreshold + 0.1, avg );
            }

            fixed4 bloom( fixed4 incolor, fixed2 uv )
            {
	        	float4 o = float4( 0, 0, 0, 1 );
                fixed pixelw = 1.0/_ScreenParams.x;

	        	// x taps - interpolate
				o += highlight( tex2D( _MainTex, float2( uv.x - 3.0 * pixelw, uv.y ) ) ) * 0.09;
				o += highlight( tex2D( _MainTex, float2( uv.x - 1.5 * pixelw, uv.y ) ) ) * 0.15;
	            o += incolor * 0.16;
				o += highlight( tex2D( _MainTex, float2( uv.x + 1.5 * pixelw, uv.y ) ) ) * 0.15;
				o += highlight( tex2D( _MainTex, float2( uv.x + 3.0 * pixelw, uv.y ) ) ) * 0.09;
   
	            // y taps - interpolate
                fixed pixelh = 1.0/_ScreenParams.y;
				o += highlight( tex2D( _MainTex, float2( uv.x, uv.y - 3.0 * pixelh ) ) ) * 0.09;
				o += highlight( tex2D( _MainTex, float2( uv.x, uv.y - 1.5 * pixelw ) ) ) * 0.15;
	            o += incolor * 0.16;
				o += highlight( tex2D( _MainTex, float2( uv.x, uv.y + 1.5 * pixelh ) ) ) * 0.15;
				o += highlight( tex2D( _MainTex, float2( uv.x, uv.y + 3.0 * pixelh ) ) ) * 0.09;

	            return o * _Intensity + incolor;
            }

            fixed4 DOF( fixed4 incolor, fixed2 uv )
            {
                float4 depth = tex2D( _CameraDepthTexture, uv );
                return incolor;
            }
	        
	        fixed4 frag (VS_OUT i) : COLOR
	        {
                float4 original = tex2D( _MainTex, i.uv);
	        	return bloom( original, i.uv );
	        }
	       
	        ENDCG
        }
	}
}
