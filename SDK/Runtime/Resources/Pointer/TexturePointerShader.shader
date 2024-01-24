Shader "FFALCON/TexturePointerShader"
{  
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_DistanceInMeters("DistanceInMeters", Range(0.0, 10000.0)) = 2.0
		_ScaleValue("ScaleValue", Range(0, 999)) = 1.5

	}
SubShader
{
		Tags { "Queue" = "Overlay" "IgnoreProjector" = "True" "RenderType" = "Transparent" }

	LOD 100

	Pass
	{
		 Blend SrcAlpha OneMinusSrcAlpha, OneMinusDstAlpha One
			AlphaTest Off
	  Cull Back
	  Lighting Off
	  ZWrite Off
	  ZTest Always
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		// make fog work
		#pragma multi_compile_fog

		#include "UnityCG.cginc"

		struct appdata
		{
			float4 vertex : POSITION;
			float2 uv : TEXCOORD0;
		};

		struct v2f
		{
			float2 uv : TEXCOORD0;
			UNITY_FOG_COORDS(1)
			float4 vertex : SV_POSITION;
		};

		sampler2D _MainTex;
		float4 _MainTex_ST;
		uniform float _DistanceInMeters;
		uniform float _ScaleValue;


		v2f vert(appdata v)
		{
			float3 vert_out = float3(v.vertex.x * _ScaleValue, v.vertex.y * _ScaleValue, _DistanceInMeters);
			v2f o;
			o.vertex = UnityObjectToClipPos(vert_out);
			o.uv = TRANSFORM_TEX(v.uv, _MainTex);
			return o;
		}

		fixed4 frag(v2f i) : SV_Target
		{
			// sample the texture
			fixed4 col = tex2D(_MainTex, i.uv);

			return col;
		}
	ENDCG
}
}
}
