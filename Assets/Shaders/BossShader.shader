Shader "Custom/MinecraftPortalEffectShader"
{
    Properties
    {
        // custom multipliers + colour
        _MainTex ("Main Texture", 2D) = "white" {}
        _DepthMultiplier ("Depth Multiplier", Range(0.1, 10)) = 1.0
        _ParallaxStrength ("Parallax Strength", Range(0, 1)) = 0.1
        _GlowColor ("Glow Color", Color) = (1, 1, 1, 1)
        _PulseSpeed ("Pulse Speed", Range(0, 10)) = 1.0
        _GlowIntensity ("Glow Intensity", Range(0, 1)) = 0.5
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

            struct appdata
            {
                float4 vertex : POSITION;
                float3 worldPos : TEXCOORD1;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float _DepthMultiplier;
            float _ParallaxStrength;
            float4 _GlowColor;
            float _PulseSpeed;
            float _GlowIntensity;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // fixed texture, doesn't move with camera (portal effect)
                float2 uv = i.worldPos.xy;

                // parallax effect
                uv.x += (i.worldPos.z / _DepthMultiplier) * _ParallaxStrength;
                uv.y += (i.worldPos.x / _DepthMultiplier) * _ParallaxStrength;
                fixed4 baseColor = tex2D(_MainTex, uv);

                // pulsating subtle glow
                float glow = abs(sin(_Time.y * _PulseSpeed)) * _GlowIntensity;
                fixed4 glowColor = _GlowColor * glow;

                return lerp(baseColor, glowColor, glow);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}