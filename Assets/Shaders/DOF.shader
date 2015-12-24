Shader "Custom/DOF" {
	Properties {
		_MainTex ("Screen Texture", 2D) = "white" {}
        _Factor( "Factor", Float ) = 0.1
        _Depth( "Depth", Float ) = 0.0
        _Range( "Range", Float ) = 0.5
	}
	SubShader {
		Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            sampler2D _CameraDepthTexture;
            float4 _MainTex_TexelSize;
            fixed _Factor;
            fixed _Depth;

            struct VS_OUT {
                float4 vertex : SV_POSITION;
                float2 uv     : TEXCOORD0;
                float2 z      : TEXCOORD1;
            };

            VS_OUT vert( appdata_img v )
            {
                VS_OUT o;
               
                o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
                o.uv = v.texcoord;
               
                return o;
            }

            fixed4 frag (VS_OUT i) : COLOR
            {
                float4 original = tex2D( _CameraDepthTexture, i.uv);
                return original;
            }

            ENDCG
        }
	} 
}
