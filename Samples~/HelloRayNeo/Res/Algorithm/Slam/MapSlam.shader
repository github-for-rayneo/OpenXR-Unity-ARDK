// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/MapSlam"{
	Properties{
		_Color("Color Tint", Color) = (1, 1, 1, 1)
		_MainTex("Main Tex", 2D) = "white" {}
		_AlphaScale("Alpha Scale",Range(0,1)) = 0.5
		_CircleCenterPoint("Circle Info",Vector) = (0,0,100000,5)
	}
		SubShader{
				Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
			pass
				{
					ZWrite On  //开启深度写入
					ColorMask 0  //当ColorMask设置为0时，表示该Pass不写任何颜色通道，不会输出任何颜色
				}

			Pass {
				Tags{"LightMode" = "ForwardBase"}
				ZWrite Off
				Blend SrcAlpha OneMinusSrcAlpha

				CGPROGRAM

				#pragma vertex vert
				#pragma fragment frag

				#include "Lighting.cginc"

				fixed4 _Color;
				sampler2D _MainTex;
				float4 _MainTex_ST;
				float _AlphaScale;
				fixed4 _CircleCenterPoint;

				struct a2v {
					float4 vertex : POSITION;
					float3 normal : NORMAL;
					float4 texcoord : TEXCOORD0;
				};

				struct v2f {
					float4 pos : SV_POSITION;
					float2 uv : TEXCOORD0;
					float3 worldNormal: TEXCOORD1;
					float3 worldPos : TEXCOORD2;
					float3 worldSpacePos : TEXCOORD3;
				};


				v2f vert(a2v v) {
					v2f o;
					o.pos = UnityObjectToClipPos(v.vertex);

					o.uv = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;

					o.worldNormal = UnityObjectToWorldNormal(v.normal);
					o.worldPos = UnityObjectToWorldNormal(v.vertex);
					o.worldSpacePos = mul(unity_ObjectToWorld, v.vertex);
					return o;
				}
	
				fixed4 frag(v2f i) : SV_Target {
					fixed3 worldNormal = normalize(i.worldNormal);
					fixed3 worldLightDir = normalize(UnityWorldSpaceLightDir(i.worldPos));

					fixed4 texColor = tex2D(_MainTex,i.uv);


					fixed3 albedo = texColor.rgb * _Color.rgb;

					fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz * albedo;

					fixed3 diffuse = _LightColor0.rgb * albedo * max(0, dot(worldNormal, worldLightDir));
				
					return  fixed4(ambient+diffuse, _Color.a);
					}
					ENDCG
				}
		}
			FallBack "Specular"
}
