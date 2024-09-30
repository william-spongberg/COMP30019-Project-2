Shader "Hidden/PostProcessShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always


        // We take this outside the usual Pass to use CGINCLUDE to say we use the cg scripting language and will include what in the block in now possibly multiple pass blocks in the file (I assume).
        CGINCLUDE
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            Texture2D _MainTex;
            SamplerState point_clamp_sampler;
            float4 _MainTex_TexelSize;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
        ENDCG
        
        // Pass 0: Point clamp sample, Dithering, and Color Quantisation
        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            float _Spread;
            int _BayerLevel;
            int _RedColorCount, _GreenColorCount, _BlueColorCount;

            // Pre-constructed Bayer Treshhold maps.
            static const int bayer2[2 * 2] = {
                0, 2,
                3, 1
            };

            static const int bayer4[4 * 4] = {
                0, 8, 2, 10,
                12, 4, 14, 6,
                3, 11, 1, 9,
                15, 7, 13, 5
            };

            static const int bayer8[8 * 8] = {
                0, 32, 8, 40, 2, 34, 10, 42,
                48, 16, 56, 24, 50, 18, 58, 26,  
                12, 44,  4, 36, 14, 46,  6, 38, 
                60, 28, 52, 20, 62, 30, 54, 22,  
                3, 35, 11, 43,  1, 33,  9, 41,  
                51, 19, 59, 27, 49, 17, 57, 25, 
                15, 47,  7, 39, 13, 45,  5, 37, 
                63, 31, 55, 23, 61, 29, 53, 21
            };

            
            // !!! LESSON LEARNT Cg IS REALLY FUCKING WEIRD AND DOESN'T HAVE POINTERS OR SWITCH CASE
            // Small Extension!! How bayer maps
            // Function that accesses the value at the x,y coordinates of a certain Bayer Threshhold Map determined by the input Bayer Level.
            // float GetBayerN(int x, int y, int level) {
            //     // We obtain n = actual bayer level
            //     int n = pow(2, level + 1);
                
            //     // Convert into coordinates of threshhold map value.
            //     int xThresh = x % n;
            //     int yThresh = y % n;

            //     // Obtain the treshhold map value for the given n = Bayer level
            //     // Divide by n^2 to get the value into the range [0, 1] then subtract 0.5 (reduce effect strength?)
            //     return float(*bayerMaps[_BayerLevel][(xThresh) + (yThresh) * n]) * (1.0f / (float) pow(n, 2)) - 0.5f;
                
            //     // Incase bayerMaps doesn't work.
            //     // switch (level) {   
            //     //     case 0:
            //     //         // Bayer2
            //     //         return float(bayer2[(x % 2) + (y % 2) * 2]) * (1.0f / 4.0f) - 0.5f;
            //     //     case 1:
            //     //         // Bayer4
            //     //         return float(bayer4[(x % 4) + (y % 4) * 4]) * (1.0f / 16.0f) - 0.5f;
            //     //     case 2:
            //     //         // Bayer8
            //     //         return float(bayer8[(x % 8) + (y % 8) * 8]) * (1.0f / 64.0f) - 0.5f;
            //     // }
                
            // }

            // Get Bayer Map value functions for each level, gonna just use this instead of the above mistake.
            float GetBayer2(int x, int y) {
                return float(bayer2[(x % 2) + (y % 2) * 2]) * (1.0f / 4.0f) - 0.5f;
            }

            float GetBayer4(int x, int y) {
                return float(bayer4[(x % 4) + (y % 4) * 4]) * (1.0f / 16.0f) - 0.5f;
            }

            float GetBayer8(int x, int y) {
                return float(bayer8[(x % 8) + (y % 8) * 8]) * (1.0f / 64.0f) - 0.5f;
            }

            fixed4 frag (v2f i) : SV_Target
            {  
                // A lot of this is copied from Acerolla's GitHub, so credit to him (the process and Shaderlab/CG scripting wouldn't make sense without him).
                // Expansions upon the origional shader will be declared as such.

                // Sample the screen using point clamp, to get the "pixel art" look
                float4 col = _MainTex.Sample(point_clamp_sampler, i.uv);

                // Get a pixel's position on screen but multiplying the uv coord by the width/height of the screen.
                int x = i.uv.x * _MainTex_TexelSize.z;
                int y = i.uv.y * _MainTex_TexelSize.w;

                // Get the dithering noise for this pixel for the selected Bayer Level
                float bayerValues[3] = { GetBayer2(x, y), GetBayer4(x, y), GetBayer8(x, y) };
                float bayerNoise = bayerValues[_BayerLevel];
                
                // Now we have the amount of dither noise a given pixel has, finally we multiply by a given spread value and add it to the origional pixel.
                float4 output = col + _Spread * bayerNoise;
                
                // Apply color quantization, splitting each color into a pallete of n colors.
                output.r = floor((_RedColorCount - 1.0f) * output.r + 0.5) / (_RedColorCount - 1.0f);
                output.g = floor((_GreenColorCount - 1.0f) * output.g + 0.5) / (_GreenColorCount - 1.0f);
                output.b = floor((_BlueColorCount - 1.0f) * output.b + 0.5) / (_BlueColorCount - 1.0f);

                return output;
            }
            ENDCG
        }
        
        // ! UNUSED - BEEN PUT IN ANOTHER SHADER BUT CAN BE ACCESSED AS DIFFERENT PASS HERE
        // Pass 1: Just use Point clamp sampling
        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            fixed4 frag (v2f i) : SV_Target
            {
                // Sample the screen using point clamp, to get the "pixel art" look
                float4 col = _MainTex.Sample(point_clamp_sampler, i.uv);
                return col;
            }
            ENDCG
        }
    }
}
