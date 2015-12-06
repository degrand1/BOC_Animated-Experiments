Shader "Custom/pulsatingBackgroundMaterial" {
    Properties {
		_BGColor         ( "Background color", Color ) = ( 0, 0, 0, 1 )
		_BrightLineColor ( "Line color", Color )       = ( .13, 0, .52, 1 )
		_DarkLineColor   ( "Line color 2", Color )     = ( .07, 0, .27, 1 )
		_PlayerPosition  ( "Player position", Vector ) = ( 0, 0, 0, 1 )
		_BallPosition    ( "Ball position", Vector )   = ( 100, 500, 0, 1 )
		_LightIntensity  ( "Spotlight intensity", Vector ) = ( 1.8, 1.8, 1.8, 1 )
		_Darkness		 ( "Darkness", Vector ) = ( 0.55, 0.55, 0.55, 1 )
		_LightFalloff	 ( "Spotlight falloff", Float ) = 100
		_WarpFactor		 ( "Warp factor", Float ) = 0.05
		_WubTime		 ( "Wub Time (RO)", Float ) = 0
		_WubFrequency    ( "Wub Frequency", Float ) = 0.25
	}
	SubShader {
		Pass {
			CGPROGRAM

			fixed4 _BGColor;
			fixed4 _BrightLineColor;
			fixed4 _DarkLineColor;
			fixed4 _PlayerPosition;
			fixed4 _BallPosition;
			fixed4 _LightIntensity;
			fixed4 _Darkness;
			fixed _LightFalloff;
			fixed _WarpFactor;
			fixed _WubFrequency;
			fixed _WubTime;
			
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			
			#include "UnityCG.cginc"

			struct VS_OUT {
				float4 pos:SV_POSITION;
				float4 clip:TEXCOORD0;
				float4 color:COLOR;
			};

			VS_OUT vert ( appdata_base v )
			{
				VS_OUT o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.clip = ComputeScreenPos( o.pos );
				o.color = float4( 0, 0, 0, 1 );
				return o;
			}

			float4 spotlight ( float4 base, float4 pos, float4 lightPos )
			{
				float gradient = 1 - ( clamp( distance( pos.xy, lightPos.xy ), 0, _LightFalloff ) / _LightFalloff );
				float4 lit = base * lerp( _Darkness, _LightIntensity, gradient );
				return lit;
			}

			float4 warp( float4 pos, fixed wf )
			{
				float4 coords = ( pos - 0.5 ) * 2.0;

				float4 offs = float4(0,0,0,0);
				offs.x = ( 1.0 - coords.y * coords.y) * wf * (coords.x); 
				offs.y = ( 1.0 - coords.x * coords.x) * wf * (coords.y);
				coords += offs;

				return ( coords / 2.0 ) + 0.5;
			}

			float4 wub( float4 pos )
			{
				return warp( pos, lerp( -_WarpFactor, _WarpFactor, _WubTime / _WubFrequency ) );
			}

/*
			float easeInOutQuad( x, t, c ) {
				if ((t/=1/2) < 1) return c/2*t*t;
				return -c/2 * ((--t)*(t-2) - 1);
			},
			*/
			
			fixed4 frag ( VS_OUT i ) : SV_Target
			{
				// wub wub wub
				fixed4 warped = wub( float4( i.clip.xy / i.clip.w, 0, 1 ) );
				fixed4 screen = float4( _ScreenParams.xy * warped.xy, 0, 1 );
				float4 OutColor = float4( 0, 0, 0, 1 );

				// render gridlines
				if ( floor( screen.y ) % 40 == 0 || floor( screen.x ) % 40 == 0 )
					OutColor = _BrightLineColor;
				else if ( floor( screen.y ) % 10 == 0 || floor( screen.x ) % 10 == 0 )
					OutColor = _DarkLineColor;
				else
					OutColor = _BGColor;

				// spotlight on player and ball position
				OutColor = spotlight( OutColor, screen, _PlayerPosition );
				OutColor = spotlight( OutColor, screen, _BallPosition );

				return OutColor;
			}
			ENDCG
		}
	}
}
