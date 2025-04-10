Shader "Custom/Vertex Displacement"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _WaveAmplitude("Wave Amplitude",Range(0,1)) = 0
        _CosMultiplyVal("Cos Multiply Val",float) = 0

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

           sampler2D _MainTex;
           float4 _MainTex_ST;
           float _WaveAmplitude;
           float _CosMultiplyVal;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normals : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float3 normals : TEXCOORD1;
                float4 vertex : SV_POSITION;
            };


            v2f vert (appdata v)
            {
                v2f o; 
                float waveOne = cos((v.uv.y - _Time.y * 0.02) * TAU * _CosMultiplyVal);
                float waveTwo = cos((v.uv.x - _Time.y * 0.02) * TAU *_CosMultiplyVal);
                v.vertex.z = waveOne * waveTwo  * _WaveAmplitude; 
                o.normals = UnityObjectToWorldNormal(v.normals);
                //v.vertex.y = vertPos;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv,_MainTex);
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                //fixed4 col = tex2D(_MainTex, i.uv);
                //float offsetX = cos(i.uv.y * TAU * 8) * 0.01;
                float col = cos((i.uv.y - _Time.y * 0.02) * TAU * _CosMultiplyVal) * 0.5 + 0.5;
                //float col = abs(frac(i.uv.x * 5) * 2 - 1;
                return col;
            }
            ENDCG
        }
    }
}
