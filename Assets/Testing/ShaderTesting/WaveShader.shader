//UNITY_SHADER_NO_UPGRADE

Shader "Unlit/WaveShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Pass
		{
			Cull Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			uniform sampler2D _MainTex;	

			struct vertIn
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct vertOut
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			// Implementation of the vertex shader
			vertOut vert(vertIn v)
			{
				// oscillate up and down
				//float4 displacement = float4(0.0f, sin(_Time.y), 0.0f, 0.0f);

				float4 displacement = float4(0.0f, 0.0f, 0.0f, 0.0f);
				v.vertex += displacement;
				
				// moving sin wave
				//v.vertex.y += sin(v.vertex.x + _Time.x*10);

				// amplitude halved
				//v.vertex.y += sin(v.vertex.x + _Time.x*10)/2;

				// faster
				//v.vertex.y += sin(v.vertex.x + _Time.x*100);

				// increase with time (slows then increases?)
				//v.vertex.y += sin(v.vertex.x + pow(_Time.x, _Time.x)*100);

				// moves relative to scnene camera
				v.vertex.y += sin(v.vertex.x + mul(UNITY_MATRIX_MVP, v.vertex));

				vertOut o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			// Implementation of the fragment shader
			fixed4 frag(vertOut v) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, v.uv);
				return col;
			}
			ENDCG
		}
	}
}