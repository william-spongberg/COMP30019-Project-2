Shader "Custom/URP/PixelatePostProcess"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _PixelSize ("Pixel Size", Float) = 0.01
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Overlay" }
        Pass
        {
            Name "PixelatePass"
            Tags { "LightMode"="UniversalForward" }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _PixelSize;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.pos = TransformObjectToHClip(v.vertex);
                o.uv = TransformUV(v.uv, _MainTex_ST);
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv;
                uv = floor(uv / _PixelSize) * _PixelSize;
                return SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv);
            }
            ENDCG
        }
    }
}