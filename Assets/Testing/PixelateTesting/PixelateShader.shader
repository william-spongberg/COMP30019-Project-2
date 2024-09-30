// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/PixelateShader"
{
    // pixelate randomly using white noise
    // ? could displace vertexes randomly?
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

            // from https://docs.unity3d.com/Packages/com.unity.shadergraph@6.9/manual/Random-Range-Node.html
            void Unity_RandomRange_float(float2 Seed, float Min, float Max, out float Out)
            {
                float randomno =  frac(sin(dot(Seed, float2(12.9898, 78.233)))*43758.5453);
                Out = lerp(Min, Max, randomno);
            }
            
            // from https://docs.unity3d.com/Packages/com.unity.shadergraph@6.9/manual/Dither-Node.html
            void Unity_Dither_float4(float4 In, float4 ScreenPosition, out float4 Out)
            {
                float2 uv = ScreenPosition.xy * _ScreenParams.xy;
                float DITHER_THRESHOLDS[16] =
                {
                    1.0 / 17.0,  9.0 / 17.0,  3.0 / 17.0, 11.0 / 17.0,
                    13.0 / 17.0,  5.0 / 17.0, 15.0 / 17.0,  7.0 / 17.0,
                    4.0 / 17.0, 12.0 / 17.0,  2.0 / 17.0, 10.0 / 17.0,
                    16.0 / 17.0,  8.0 / 17.0, 14.0 / 17.0,  6.0 / 17.0
                };
                uint index = (uint(uv.x) % 4) * 4 + uint(uv.y) % 4;
                Out = In - DITHER_THRESHOLDS[index];
            }
            

            vertOut vert(vertIn v)
			{
                float4 displacement = float4(0.0f, 0.0f, 0.0f, 0.0f);
				v.vertex += displacement;

                vertOut o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
            }

            
            fixed4 frag(vertOut v) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, v.uv);
                // randomise pixelation
                // float random;
                // Unity_RandomRange_float(_Time + v.uv, 0.0, 1.0, random);
                // if (random > 0.5)
                // {
                //     col = fixed4(0.0, 0.0, 0.0, 1.0);
                // }

                // dither
                float4 dithered;
                Unity_Dither_float4(col, v.vertex, dithered);
                col = dithered;

				return col;
			}
            ENDCG
        }
    }
}