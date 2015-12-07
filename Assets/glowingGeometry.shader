Shader "Custom/glowingGeometry" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_GameObjectPosition( "Game Object Position", Vector ) = ( 0, 0, 0, 1 )
		_EdgeThickness( "Edge Thickness", Float ) = 0.1
		[Toggle] _Circular( "Circular", Float ) = 0
	}
	SubShader {
		Pass {
			 Name "mask"
			 ColorMask 0 // don't output colors
			 Stencil {
                Ref 1
                Comp always
                Pass replace
            }

			CGPROGRAM
			#pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"
            
            fixed4 _GameObjectPosition;
            fixed  _Circular;
            fixed  _EdgeThickness;
            
            struct VS_OUT {
				float4 pos:SV_POSITION;
			};
            
            VS_OUT vert( appdata_base v )
            {
            	VS_OUT o;
            	float4 pos = mul( _Object2World, v.vertex );
            	pos -= _GameObjectPosition;
            	
            	// shrink vertex (object) and draw mask; thereby masking out the "inside" of object
            	// unfortunately, edges for circular objects are fundamentally different from edges of otherwise blocky objects
            	// trying to shoehorn one into the other produces ugly artifacts
            	if ( _Circular > 0 ) {
            		fixed len = length( pos.xy );
            		pos.xy *= ( len - _EdgeThickness ) / len;
            	} else {
	            	fixed x = abs( pos.x );
	            	pos.x *= ( ( x - _EdgeThickness ) / x );
	            	fixed y = abs( pos.y );
	            	pos.y *= ( ( y - _EdgeThickness ) / y );
            	}
            	
            	pos += _GameObjectPosition;
            	o.pos = mul(UNITY_MATRIX_VP, pos);
            	return o;
            }
            
            fixed4 frag(VS_OUT i): SV_Target {
            	return float4( 0, 0, 0, 1 );
            }
            
			ENDCG
		}
	
		Pass {
			Name "outline"
			Stencil {
                Ref 1
                Comp notEqual
                Pass keep 
            }
            Blend SrcAlpha OneMinusSrcAlpha
			
			CGPROGRAM
			#pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"
            
            fixed4 _Color;
            
            struct VS_OUT {
				float4 pos:SV_POSITION;
			};
            
            VS_OUT vert( appdata_base v )
            {
            	VS_OUT o;
            	o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
            	return o;
            }
            
            fixed4 frag(VS_OUT i): COLOR {
            	return _Color;
            }
			ENDCG
		}
	} 
	FallBack "Diffuse"
}
