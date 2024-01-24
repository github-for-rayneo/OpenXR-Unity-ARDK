Shader "Scene01/Skybox"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Tags{"Queue"="Background"}
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            //命名的标准
            float4 _MainTex_HDR;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                
                #if UNITY_REVERSED_Z
                o.pos.z=o.pos.w*0.0000001f;
                #else
                o.pos.z=o.pos.w*0.9999999f;
                #endif
                //控制裁剪面使之在Clip space范围内
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                //解码HDR
                half3 col_hdr=DecodeHDR(col,_MainTex_HDR);
                return half4(col_hdr,1.0);
            }
            ENDCG
        }
    }
}
