Shader "RayNeo/DualeyeDiffrentDisplay"
{
    Properties
    {
        [PerRendererData]
        _MainTex("Sprite Texture", 2D) = "white" {}

        _Hor("Is Horizontal Filp", Range(0, 1)) = 0
        _Ver("Is Vertical Filp", Range(0, 1)) = 0

        _StencilComp("Stencil Comparison", Float) = 8
        _Stencil("Stencil ID", Float) = 0
        _StencilOp("Stencil Operation", Float) = 0
        _StencilWriteMask("Stencil Write Mask", Float) = 255
        _StencilReadMask("Stencil Read Mask", Float) = 255

        _ColorMask("Color Mask", Float) = 15
        

        [Toggle(_True)]_IsStereo("IsStereoEye", float) = 1
        [Toggle(_True)]_IsRight("IsRight", float) = 1
        _CanvasGroup("Canvas Group", Range(0, 1)) = 1
    }

        SubShader
        {
            Tags
            {
                "Queue" = "Transparent"
                "RenderType" = "Transparent"
            }

            Stencil
            {
                Ref[_Stencil]
                Comp[_StencilComp]
                Pass[_StencilOp]
                ReadMask[_StencilReadMask]
                WriteMask[_StencilWriteMask]
            }

            Cull Off
            Lighting Off
            ZWrite Off
            ZTest[unity_GUIZTestMode]
            Fog { Mode Off }
            Blend SrcAlpha OneMinusSrcAlpha
            ColorMask[_ColorMask]

            Pass
            {
                CGPROGRAM

                #pragma vertex vert
                #pragma fragment frag

                sampler2D _MainTex;
                float4 _MainTex_ST;
                int _Type;
                int _Hor;
                int _Ver;
                bool _IsStereo;
                bool _IsRight;
                float _CanvasGroup;

                #include "UnityCG.cginc"

                struct appdata
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f
                {
                    float4 vertex : SV_POSITION;
                    half4 uv : TEXCOORD0;
                    UNITY_VERTEX_OUTPUT_STEREO
                };

                v2f vert(appdata v)
                {
                    v2f o;

                    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv.xy = TRANSFORM_TEX(v.uv.xy, _MainTex);

                    o.uv.x = (1 - v.uv.x) * _Hor + v.uv.x * (1 - _Hor);
                    o.uv.y = (1 - v.uv.y) * _Ver + v.uv.y * (1 - _Ver);

                    //float2 uv = o.uv.xy;

                    //uv.x = _IsStereo != 0 ? uv.x / 2 + unity_StereoEyeIndex * 0.5 : uv.x;

                    //o.uv.xy = uv;

                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);

                    fixed4  col = tex2D(_MainTex, i.uv);


                    col.a *= _CanvasGroup; // 使用Canvas Group的透明度信息
                    col.a *= _IsRight ? (unity_StereoEyeIndex == 0) : !(unity_StereoEyeIndex == 0);
                    return col;
                }

                ENDCG
            }
        }
}