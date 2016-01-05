Shader "Custom/bloom" {
    Properties {
        _MainTex ("Screen Texture", 2D) = "white" {}
        _HighlightThreshold( "Highlight Threshold", Float) = 0.4
        _Intensity("Intensity", Float) = 1.1

        _DOFFactor( "DOFFactor", Range( 0.0, 1.0 ) ) = 0.0
        _DOFDepth( "DOFDepth", Float ) = 0.0
        _DOFRange( "DOFRange", Float ) = 0.5
    }

    SubShader {
        Pass {
            ZTest Always Cull Off ZWrite Off
            Blend Off
            Name "DOF horizontal pass"

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma fragmentoption ARB_precision_hint_fastest
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            sampler2D _CameraDepthTexture;
            float4 _MainTex_TexelSize;

            fixed _DOFFactor;
            fixed _DOFRange;
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

            fixed4 DOF_horizontal( fixed4 incolor, fixed2 uv )
            {
                float4 depth = tex2D( _CameraDepthTexture, uv );

                if ( distance( depth.x, _DOFDepth ) > _DOFRange ) {
                    float4 blurred = float4( 0, 0, 0, 1 );
                    blurred += tex2D( _MainTex, uv - _MainTex_TexelSize.xy * half2( 4.0 * _DOFFactor, 0.0 ) ) * 0.06;
                    blurred += tex2D( _MainTex, uv - _MainTex_TexelSize.xy * half2( 3.0 * _DOFFactor, 0.0 ) ) * 0.09;
                    blurred += tex2D( _MainTex, uv - _MainTex_TexelSize.xy * half2( 2.0 * _DOFFactor, 0.0 ) ) * 0.12;
                    blurred += tex2D( _MainTex, uv - _MainTex_TexelSize.xy * half2( 1.0 * _DOFFactor, 0.0 ) ) * 0.15;
                    blurred += incolor * 0.16;
                    blurred += tex2D( _MainTex, uv + _MainTex_TexelSize.xy * half2( 1.0 * _DOFFactor, 0.0 ) ) * 0.15;
                    blurred += tex2D( _MainTex, uv + _MainTex_TexelSize.xy * half2( 2.0 * _DOFFactor, 0.0 ) ) * 0.12;
                    blurred += tex2D( _MainTex, uv + _MainTex_TexelSize.xy * half2( 3.0 * _DOFFactor, 0.0 ) ) * 0.09;
                    blurred += tex2D( _MainTex, uv + _MainTex_TexelSize.xy * half2( 4.0 * _DOFFactor, 0.0 ) ) * 0.06;
                    return blurred;
                }

                return incolor;
            }

            fixed4 frag (VS_OUT i) : COLOR
            {
                float4 incolor = tex2D( _MainTex, i.uv);
                return DOF_horizontal( incolor, i.uv );
            }
           
            ENDCG
        }

        GrabPass {}

        Pass {
            ZTest Always Cull Off ZWrite Off
            Blend Off
            Name "DOF vertical pass"

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma fragmentoption ARB_precision_hint_fastest
            #include "UnityCG.cginc"

            sampler2D _GrabTexture;
            sampler2D _CameraDepthTexture;
            float4 _GrabTexture_TexelSize;

            fixed _DOFFactor;
            fixed _DOFRange;
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

            fixed4 DOF_vertical( fixed4 incolor, fixed2 uv )
            {
                float4 depth = tex2D( _CameraDepthTexture, uv );

                if ( distance( depth.x, _DOFDepth ) > _DOFRange ) {
                    float4 blurred = float4( 0, 0, 0, 1 );
                    blurred += tex2D( _GrabTexture, uv - _GrabTexture_TexelSize.xy * half2( 0.0, 4.0 * _DOFFactor ) ) * 0.06;
                    blurred += tex2D( _GrabTexture, uv - _GrabTexture_TexelSize.xy * half2( 0.0, 3.0 * _DOFFactor ) ) * 0.09;
                    blurred += tex2D( _GrabTexture, uv - _GrabTexture_TexelSize.xy * half2( 0.0, 2.0 * _DOFFactor ) ) * 0.12;
                    blurred += tex2D( _GrabTexture, uv - _GrabTexture_TexelSize.xy * half2( 0.0, 1.0 * _DOFFactor ) ) * 0.15;
                    blurred += incolor * 0.16;
                    blurred += tex2D( _GrabTexture, uv + _GrabTexture_TexelSize.xy * half2( 0.0, 1.0 * _DOFFactor ) ) * 0.15;
                    blurred += tex2D( _GrabTexture, uv + _GrabTexture_TexelSize.xy * half2( 0.0, 2.0 * _DOFFactor ) ) * 0.12;
                    blurred += tex2D( _GrabTexture, uv + _GrabTexture_TexelSize.xy * half2( 0.0, 3.0 * _DOFFactor ) ) * 0.09;
                    blurred += tex2D( _GrabTexture, uv + _GrabTexture_TexelSize.xy * half2( 0.0, 4.0 * _DOFFactor ) ) * 0.06;
                    return blurred;
                }

                return incolor;
            }

            fixed4 frag (VS_OUT i) : COLOR
            {
                float4 incolor = tex2D( _GrabTexture, i.uv);
                return DOF_vertical( incolor, i.uv );
            }
           
            ENDCG
        }

        GrabPass {}

        Pass {
            ZTest Always Cull Off ZWrite Off
            Blend One One
            Name "Bloom pass"

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma fragmentoption ARB_precision_hint_fastest
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;

            sampler2D _GrabTexture;
            float4 _GrabTexture_TexelSize;

            fixed _Intensity;
            fixed _HighlightThreshold;

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
                o += highlight( tex2D( _GrabTexture, float2( uv.x - 3.0 * pixelw, uv.y ) ) ) * 0.09;
                o += highlight( tex2D( _GrabTexture, float2( uv.x - 1.5 * pixelw, uv.y ) ) ) * 0.15;
                o += incolor * 0.16;
                o += highlight( tex2D( _GrabTexture, float2( uv.x + 1.5 * pixelw, uv.y ) ) ) * 0.15;
                o += highlight( tex2D( _GrabTexture, float2( uv.x + 3.0 * pixelw, uv.y ) ) ) * 0.09;
   
                // y taps - interpolate
                fixed pixelh = 1.0/_ScreenParams.y;
                o += highlight( tex2D( _GrabTexture, float2( uv.x, uv.y - 3.0 * pixelh ) ) ) * 0.09;
                o += highlight( tex2D( _GrabTexture, float2( uv.x, uv.y - 1.5 * pixelw ) ) ) * 0.15;
                o += incolor * 0.16;
                o += highlight( tex2D( _GrabTexture, float2( uv.x, uv.y + 1.5 * pixelh ) ) ) * 0.15;
                o += highlight( tex2D( _GrabTexture, float2( uv.x, uv.y + 3.0 * pixelh ) ) ) * 0.09;

                return o * _Intensity + incolor;
            }

            fixed4 frag (VS_OUT i) : COLOR
            {
                float4 incolor = tex2D( _GrabTexture, i.uv);
                return bloom( incolor, i.uv );
            }
           
            ENDCG
        }
    }
}
