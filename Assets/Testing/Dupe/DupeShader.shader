Shader "Unlit/DupeShader"
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
            #pragma multi_compile_instancing
            #include "UnityCG.cginc"

            uniform sampler2D _MainTex;    
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
                UNITY_VERTEX_OUTPUT_STEREO
            };

            // gpu instancing buffer
            UNITY_INSTANCING_BUFFER_START(Props)
                UNITY_DEFINE_INSTANCED_PROP(float4, _InstanceOffset)
            UNITY_INSTANCING_BUFFER_END(Props)
            
            vertOut vert(vertIn v)
            {
                // grab instance id and offset from buffer
                UNITY_SETUP_INSTANCE_ID(v);
                float4 offset = UNITY_ACCESS_INSTANCED_PROP(Props, _InstanceOffset);
                v.vertex.xyz += offset.xyz;
                
                // map to camera
                vertOut o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            // shade texture
            half4 frag(vertOut i) : SV_Target
            {
                half4 color = tex2D(_MainTex, i.uv);
                // // tint red
                // color.rgb *= float3(1.0, 0.1, 0.1);
                // // darken
                // color.rgb *= 0.1;
                return color;
            }
            ENDCG
        }
    }
}