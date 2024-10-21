Shader "Unlit/BossShader"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _TextureSize ("Texture Size", Range(0, 25)) = 1.0
        _Alpha ("Alpha", Range(0, 1)) = 1.0
        _GlowColour ("Glow Color", Color) = (1, 1, 1, 1)
        _GlowIntensity ("Glow Intensity", Range(0, 1)) = 0.5
        _PulseSpeed ("Pulse Speed", Range(0, 10)) = 1.0
        _Offset ("Chromatic Aberration", Vector) = (0.01, 0.01, 0, 0)
        _TextureStretch ("Texture Stretch", Vector) = (1.0, 1.0, 0, 0)
        _TextureOffset ("Random Offset", Vector) = (0.0, 0.0, 0, 0)
    }
    SubShader
    {
        Pass
        {
            // needed for alpha transparency
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float _TextureSize;
            float4 _GlowColour;
            float _GlowIntensity;
            float _PulseSpeed;
            float _Alpha;
            float4 _Offset;
            float4 _TextureStretch;
            float4 _TextureOffset;

            struct vertIn
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
            };

            struct vertOut
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
                float4 screenPos : TEXCOORD2;
            };

            vertOut vert (vertIn v)
            {
                vertOut o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.screenPos = ComputeScreenPos(o.vertex);
                return o;
            }

            // pseudo random number generator
            // from https://docs.unity3d.com/Packages/com.unity.shadergraph@6.9/manual/Random-Range-Node.html
            void Unity_RandomRange_float(float2 Seed, float Min, float Max, out float Out)
            {
                float randomno =  frac(sin(dot(Seed, float2(12.9898, 78.233)))*43758.5453);
                Out = lerp(Min, Max, randomno);
            }

            half4 frag (vertOut i) : SV_Target
            {
                // get slight random multiplier
                float random;
                Unity_RandomRange_float(i.worldPos.xz, 0.9, 1.1, random);

                // fixed texture, doesn't move with camera (portal effect)
                float2 uv = i.screenPos.xy / i.screenPos.w * _TextureSize;

                // distort the texture
                uv *= _TextureStretch.xy * random;
                uv += _TextureOffset.xy * random;

                // chromatic aberration offsets
                float2 redOffset = uv + _Offset.xy * random;
                float2 blueOffset = uv - _Offset.xy * random;
                half4 redColour = tex2D(_MainTex, redOffset);
                half4 blueColour = tex2D(_MainTex, blueOffset);
                half4 baseColour = tex2D(_MainTex, uv);
                half4 textureColour = half4(redColour.r, baseColour.g, blueColour.b, 1.0);

                // pulsing subtle glow
                half4 glow = abs(sin(_Time.y * _PulseSpeed)) * _GlowIntensity;
                half4 glowColour = _GlowColour * glow;

                // transition between texture colour and glow colour
                half4 colour = lerp(textureColour, glowColour, glow);

                // apply alpha transparency
                colour.a *= _Alpha;

                return colour;
            }
            ENDCG
        }
    }
}