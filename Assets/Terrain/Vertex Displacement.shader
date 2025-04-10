Shader "Custom/Vertex Displacement"
{
    Properties
    {
        [Header(Textures)]
        _GrassTexture ("Grass Texture", 2D) = "white" {}
        _PathTexture("Path Texture",2D) = "White" {}
        _RiverTexture("River Texture",2D) = "White" {}
        _PathMask("Path Mask",2D) = "White" {}
        _RiverMask("River Mask",2D) = "White" {}

        [Header(Wave Adjustments)]
        _WaveAmplitude("Wave Amplitude",Range(0,20)) = 0
        _WaveSpeed("Wave Speed",float) = 0
        _WaveFrequency("Wave Frequency",float) = 0
        _RiverDepth("River Depth",Range(0,0.005)) = 0
        _RiverSpeed("River Speed",float) = 0

        [Header(Path Intensity)]
        _PathIntensity("Path Intensity", float) = 0

        [Header(River Intensity)]
        _RiverIntensity("River Intensity",float) = 0

        [Header(Path Channel Setter)]
        _ChannelSetter("Path Channel Mult Vals",FLOAT) = (0,0,0)

    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            //ZWrite Off
            Cull Off
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            #define TAU 6.283185307179586

            sampler2D _GrassTexture;
            sampler2D _PathTexture;
            sampler2D _RiverTexture;
            sampler2D _PathMask;
            sampler2D _RiverMask;
            float4 _GrassTexture_ST;
            float4 _PathTexture_ST;
            float4 _RiverTexture_ST;
            float4 _PathMask_ST;
            float4 _RiverMask_ST;
            float _WaveAmplitude;
            float _WaveSpeed;
            float _WaveFrequency;
            float _PathIntensity;
            float _RiverIntensity;
            float3 _ChannelSetter;
            float _RiverDepth;
            float _RiverSpeed;

            void IncreaseIntensity(inout float value,float intensityAmount){
                value = saturate(value * intensityAmount);
            }

            float GenerateWave(float value){
                return cos((value - _Time.y * _WaveSpeed) * TAU * _WaveFrequency) * 2;
            }

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv_grass : TEXCOORD0;
                float2 uv_pathMask : TEXCOORD1;
                float2 uv_path : TEXCOORD2;
                float2 uv_riverMask : TEXCOORD3;
                float2 uv_river : TEXCOORD4;
                float3 worldPos : TEXCOORD5;

                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o; 
                o.worldPos = mul(UNITY_MATRIX_M, v.vertex);

                o.uv_grass = TRANSFORM_TEX(v.uv,_GrassTexture);
                o.uv_pathMask = TRANSFORM_TEX(v.uv, _PathMask);
                o.uv_path = TRANSFORM_TEX(v.uv,_PathTexture);
                o.uv_riverMask = TRANSFORM_TEX(v.uv,_RiverMask);
                o.uv_river = TRANSFORM_TEX(v.uv,_RiverTexture);

                float river = tex2Dlod(_RiverMask,float4(o.uv_riverMask,0,1)).x;
                v.vertex.z = river * (v.vertex.z - _RiverDepth);
               
                IncreaseIntensity(river,_RiverIntensity);
               
                river = saturate((river - 0.9) * 100);
                river = smoothstep(0.1,0.9,river);
                
                float waveX = GenerateWave(v.vertex.x) ;
                float waveY = GenerateWave(v.vertex.y);
                float displacement = waveX * waveY * _WaveAmplitude;

                v.vertex.z -= displacement * river;

                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float2 worldPos = i.worldPos.xy; 
                
                float4 grass = tex2D(_GrassTexture, i.uv_grass);

                float4 path = tex2D(_PathTexture,i.uv_path);
                path = float4(path.r * _ChannelSetter.r, path.g * _ChannelSetter.g, path.b * _ChannelSetter.b, 1);

                float pathMask = tex2D(_PathMask,i.uv_pathMask).x;
                IncreaseIntensity(pathMask,_PathIntensity);

                i.uv_river.x += frac(_Time.y * -1 * _RiverSpeed); 
                i.uv_river.y += frac(_Time.y  * _RiverSpeed);

                float4 mixedGround = lerp(grass,path,pathMask);

                float4 river = tex2D(_RiverTexture,i.uv_river);

                float riverMask = tex2D(_RiverMask,i.uv_riverMask).x;
                IncreaseIntensity(riverMask,_RiverIntensity);
                
                float4 mixedGroundRiver = lerp(mixedGround,river, riverMask);

                return mixedGroundRiver;
            }
            ENDCG
        }
    }
}
