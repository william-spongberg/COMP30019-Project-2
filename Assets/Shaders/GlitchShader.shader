Shader "Unlit/GlitchShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _GlitchIntensity ("Glitch Intensity", Range(0, 3)) = 1.5
        _GlitchSpeed ("Glitch Speed", Range(0, 10)) = 1.0
        _DitherAmount ("Dither Amount", Range(0, 1)) = 0.1
    }
    SubShader
    {
        Pass
        {
            Cull Off
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float _GlitchIntensity;
            float _GlitchSpeed;
            float _DitherAmount;

            struct vertIn
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };
            struct vertOut
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 screenPos : TEXCOORD1;
                UNITY_VERTEX_OUTPUT_STEREO
            };
            
            // pseudo random number generator
            // from https://docs.unity3d.com/Packages/com.unity.shadergraph@6.9/manual/Random-Range-Node.html
            void Unity_RandomRange_float(float2 Seed, float Min, float Max, out float Out)
            {
                float randomno =  frac(sin(dot(Seed, float2(12.9898, 78.233)))*43758.5453);
                Out = lerp(Min, Max, randomno);
            }

            // dithering function
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
                Out = In - DITHER_THRESHOLDS[index] * _DitherAmount;
            }

            // gpu instance offset
            UNITY_INSTANCING_BUFFER_START(Props)
                UNITY_DEFINE_INSTANCED_PROP(float4, _InstanceOffset)
            UNITY_INSTANCING_BUFFER_END(Props)

            vertOut vert(vertIn v)
            {
                // apply instance offset from gpu buffer
                UNITY_SETUP_INSTANCE_ID(v);
                float4 offset = UNITY_ACCESS_INSTANCED_PROP(Props, _InstanceOffset);
                v.vertex.xyz += offset.xyz;

                // randomly offset x axis
                float time = _Time.y * _GlitchSpeed;
                float randomOffsetX;
                Unity_RandomRange_float(v.vertex.xy + time, -_GlitchIntensity, _GlitchIntensity, randomOffsetX);
                v.vertex.x += randomOffsetX;
                
                // map vertexes to camera space + screen position
                vertOut o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.screenPos = ComputeScreenPos(o.vertex);
                return o;
            }

            half4 frag(vertOut i) : SV_Target
            {
                float redOffsetX, redOffsetY, greenOffsetX, greenOffsetY, blueOffsetX, blueOffsetY;
                float time = _Time.y * _GlitchSpeed;
                
                // randomise offset coords - unique seeds for each rgb x/y channel
                Unity_RandomRange_float(float2(time, 0.0), -_GlitchIntensity, _GlitchIntensity, redOffsetX);
                Unity_RandomRange_float(float2(0.0, time), -_GlitchIntensity, _GlitchIntensity, redOffsetY);
                Unity_RandomRange_float(float2(time + 1.0, 0.0), -_GlitchIntensity, _GlitchIntensity, greenOffsetX);
                Unity_RandomRange_float(float2(0.0, time + 1.0), -_GlitchIntensity, _GlitchIntensity, greenOffsetY);
                Unity_RandomRange_float(float2(time + 2.0, 0.0), -_GlitchIntensity, _GlitchIntensity, blueOffsetX);
                Unity_RandomRange_float(float2(0.0, time + 2.0), -_GlitchIntensity, _GlitchIntensity, blueOffsetY);

                // apply the offsets to uv coords
                float2 uv = i.uv;
                float2 redOffset = uv + float2(redOffsetX, redOffsetY);
                float2 greenOffset = uv + float2(greenOffsetX, greenOffsetY);
                float2 blueOffset = uv + float2(blueOffsetX, blueOffsetY);

                // grab colour from texture at offset coords
                half4 color;
                color.r = tex2D(_MainTex, redOffset).r;
                color.g = tex2D(_MainTex, greenOffset).g;
                color.b = tex2D(_MainTex, blueOffset).b;
                color.a = tex2D(_MainTex, uv).a;

                // randomly discard all pixels for flickering effect
                float randomAlpha;
                Unity_RandomRange_float(uv + time, 0.0, 1.0, randomAlpha);
                if (randomAlpha < 0.1) {
                    discard;
                }

                // dither pixels for crt effect
                half4 ditheredColor;
                Unity_Dither_float4(color, i.screenPos, ditheredColor);

                half4 colour = ditheredColor;

                return colour;
            }
            ENDCG
        }
    }
}