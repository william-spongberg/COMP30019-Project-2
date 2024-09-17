// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/RandomiseShader"
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
            

            vertOut vert(vertIn v)
			{
                float4 displacement = float4(0.0f, 0.0f, 0.0f, 0.0f);
				v.vertex += displacement;

                float random;
                Unity_RandomRange_float(_Time + v.vertex.x, 0.0, 1.0, random);
                v.vertex.x += random;
                Unity_RandomRange_float(_Time + v.vertex.y, 0.0, 1.0, random);
                v.vertex.y += random;
                Unity_RandomRange_float(_Time + v.vertex.z, 0.0, 1.0, random);
                v.vertex.z += random;

                vertOut o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
            }

            
            fixed4 frag(vertOut v) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, v.uv);
				return col;
			}
            ENDCG
        }
    }
}