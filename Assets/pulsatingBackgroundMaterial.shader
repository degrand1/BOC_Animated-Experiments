Shader "Custom/pulsatingBackgroundMaterial" {
    Properties {
        _BGColor         ( "Background color", Color ) = ( 0, 0, 0, 1 )
        _BrightLineColor ( "Line color", Color )       = ( .13, 0, .52, 1 )
        _DarkLineColor   ( "Line color 2", Color )     = ( .07, 0, .27, 1 )
	}
	SubShader {
		Pass { // grid lines
			CGPROGRAM

            fixed4 _BGColor;
            fixed4 _BrightLineColor;
            fixed4 _DarkLineColor;
			
	        #pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			
			#include "UnityCG.cginc"

			float4 vert (appdata_base v): POSITION
	        {
	            return mul (UNITY_MATRIX_MVP, v.vertex);
	        }
	        
		    fixed4 frag (float4 pos:VPOS) : SV_Target
		    {
		    	if ( floor( pos.y ) % 40 == 0 || floor( pos.x ) % 40 == 0 )
                    return _BrightLineColor;
                else if ( floor( pos.y ) % 10 == 0 || floor( pos.x ) % 10 == 0 )
                    return _DarkLineColor;
		        else
                    return _BGColor;
		    }
			ENDCG
		}
	}
}
