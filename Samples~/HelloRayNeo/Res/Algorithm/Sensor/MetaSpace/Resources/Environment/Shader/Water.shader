// Made with Amplify Shader Editor v1.9.0.1
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Water"
{
	Properties
	{
		_ReflectionTex("ReflectionTex", 2D) = "white" {}
		_WaterNoise("WaterNoise", Float) = 0.5
		_WaterNormal("WaterNormal", 2D) = "white" {}
		_NormalIntensity("NormalIntensity", Float) = 0
		_NormalTiling("NormalTiling", Float) = 5
		_Speed("Speed", Float) = 2
		_SecondaryTiling("SecondaryTiling", Float) = 1.5
		_SecondarySpeed("SecondarySpeed", Float) = -1
		_UnderWaterlTiling("UnderWaterlTiling", Float) = 5
		_UnderWater("UnderWater", 2D) = "white" {}
		_Depth("Depth", Float) = 0
		_FresnelOffset("FresnelOffset", Float) = 0
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "UnityStandardUtils.cginc"
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float3 worldPos;
			float3 viewDir;
			INTERNAL_DATA
			float4 screenPos;
			float3 worldNormal;
		};

		struct SurfaceOutputCustomLightingCustom
		{
			half3 Albedo;
			half3 Normal;
			half3 Emission;
			half Metallic;
			half Smoothness;
			half Occlusion;
			half Alpha;
			Input SurfInput;
			UnityGIInput GIData;
		};

		uniform sampler2D _UnderWater;
		uniform float _UnderWaterlTiling;
		uniform sampler2D _WaterNormal;
		uniform float _SecondaryTiling;
		uniform float _NormalTiling;
		uniform float _Speed;
		uniform float _SecondarySpeed;
		uniform float _NormalIntensity;
		uniform float _Depth;
		uniform sampler2D _ReflectionTex;
		uniform float _WaterNoise;
		uniform float _FresnelOffset;


		inline float4 ASE_ComputeGrabScreenPos( float4 pos )
		{
			#if UNITY_UV_STARTS_AT_TOP
			float scale = -1.0;
			#else
			float scale = 1.0;
			#endif
			float4 o = pos;
			o.y = pos.w * 0.5f;
			o.y = ( pos.y - o.y ) * _ProjectionParams.x * scale + o.y;
			return o;
		}


		inline half4 LightingStandardCustomLighting( inout SurfaceOutputCustomLightingCustom s, half3 viewDir, UnityGI gi )
		{
			UnityGIInput data = s.GIData;
			Input i = s.SurfInput;
			half4 c = 0;
			float3 ase_worldPos = i.worldPos;
			float2 temp_output_11_0 = ( (ase_worldPos).xz / _NormalTiling );
			float temp_output_14_0 = ( _Time.y * 0.1 * _Speed );
			float3 WaterNormal33 = BlendNormals( UnpackScaleNormal( tex2D( _WaterNormal, ( ( _SecondaryTiling * temp_output_11_0 ) + ( temp_output_14_0 * _SecondarySpeed ) ) ), _NormalIntensity ) , tex2D( _WaterNormal, ( temp_output_11_0 + temp_output_14_0 ) ).rgb );
			float2 paralaxOffset102 = ParallaxOffset( _Depth , _Depth , i.viewDir );
			float4 UnderWaterColor93 = tex2D( _UnderWater, ( ( ( (ase_worldPos).xy / _UnderWaterlTiling ) + ( (WaterNormal33).xz * 0.1 ) ) + paralaxOffset102 ) );
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float4 unityObjectToClipPos37 = UnityObjectToClipPos( ase_vertex3Pos );
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_grabScreenPos = ASE_ComputeGrabScreenPos( ase_screenPos );
			float4 ase_grabScreenPosNorm = ase_grabScreenPos / ase_grabScreenPos.w;
			float4 ReflectionColor43 = tex2D( _ReflectionTex, ( ( ( (WaterNormal33).xz / ( 1.0 + unityObjectToClipPos37.w ) ) * _WaterNoise ) + (ase_grabScreenPosNorm).xy ) );
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float dotResult83 = dot( ase_worldNormal , ase_worldViewDir );
			float clampResult84 = clamp( dotResult83 , 0.0 , 1.0 );
			float clampResult109 = clamp( ( ( 1.0 - clampResult84 ) + _FresnelOffset ) , 0.0 , 1.0 );
			float4 lerpResult86 = lerp( UnderWaterColor93 , ReflectionColor43 , clampResult109);
			c.rgb = lerpResult86.rgb;
			c.a = 1;
			return c;
		}

		inline void LightingStandardCustomLighting_GI( inout SurfaceOutputCustomLightingCustom s, UnityGIInput data, inout UnityGI gi )
		{
			s.GIData = data;
		}

		void surf( Input i , inout SurfaceOutputCustomLightingCustom o )
		{
			o.SurfInput = i;
			o.Normal = float3(0,0,1);
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf StandardCustomLighting keepalpha fullforwardshadows noforwardadd 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float4 screenPos : TEXCOORD1;
				float4 tSpace0 : TEXCOORD2;
				float4 tSpace1 : TEXCOORD3;
				float4 tSpace2 : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				o.screenPos = ComputeScreenPos( o.pos );
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.viewDir = IN.tSpace0.xyz * worldViewDir.x + IN.tSpace1.xyz * worldViewDir.y + IN.tSpace2.xyz * worldViewDir.z;
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				surfIN.screenPos = IN.screenPos;
				SurfaceOutputCustomLightingCustom o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputCustomLightingCustom, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=19001
18;38.66667;1706.667;1005.667;-2532;-1868.862;1;True;True
Node;AmplifyShaderEditor.CommentaryNode;32;411.9738,76.09934;Inherit;False;1810.076;888.1146;Comment;19;7;8;13;16;10;9;25;29;14;11;24;27;15;28;22;6;31;33;111;WorldNormal;0.6096476,0.8423722,0.9433962,1;0;0
Node;AmplifyShaderEditor.WorldPosInputsNode;7;532.0865,234.1669;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SwizzleNode;8;728.9739,231.9146;Inherit;False;FLOAT2;0;2;2;3;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;16;668.4966,818.2766;Inherit;False;Property;_Speed;Speed;5;0;Create;True;0;0;0;False;0;False;2;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;10;645.3381,637.2287;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;13;668.22,722.7713;Inherit;False;Constant;_Float1;Float 1;3;0;Create;True;0;0;0;False;0;False;0.1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;9;542.1,404.1727;Inherit;False;Property;_NormalTiling;NormalTiling;4;0;Create;True;0;0;0;False;0;False;5;10;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;870.2202,641.7714;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;25;955.8273,126.0992;Inherit;False;Property;_SecondaryTiling;SecondaryTiling;6;0;Create;True;0;0;0;False;0;False;1.5;2.31;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;11;966.3331,363.1149;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;29;972.9426,849.2136;Inherit;False;Property;_SecondarySpeed;SecondarySpeed;7;0;Create;True;0;0;0;False;0;False;-1;0.67;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;1151.225,243.0527;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;1174.045,717.9975;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;15;1168.22,545.7715;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;28;1343.77,301.5298;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;111;1252.654,134.8254;Inherit;False;Property;_NormalIntensity;NormalIntensity;3;0;Create;True;0;0;0;False;0;False;0;3.34;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;22;1511.281,159.8114;Inherit;True;Property;_WaterNormalScondary;WaterNormalScondary;2;0;Create;True;0;0;0;False;0;False;-1;None;20c69599743d33c44813391f0e92365c;True;0;False;bump;Auto;True;Instance;6;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;42;-1051.608,-278.7566;Inherit;False;1301.11;1240.193;Comment;10;40;34;35;21;2;41;18;3;20;5;ReflectionColor;1,1,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;6;1506.386,451.6519;Inherit;True;Property;_WaterNormal;WaterNormal;2;0;Create;True;0;0;0;False;0;False;-1;None;43e4e07b82800b44e84a070a96e5ff2f;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BlendNormalsNode;31;1834.717,312.3391;Inherit;False;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;40;-1084.351,-81.67899;Inherit;False;675.9104;354.7909;Comment;4;38;37;39;36;当前距离越远法线强度越弱;0.990566,0.6868548,0.8388591,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;106;633.5615,2392.171;Inherit;False;1658.588;989.7305;Comment;9;101;92;103;104;96;102;105;79;93;UnderWater;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;101;699.5285,2806.352;Inherit;False;573.4014;315;使用Waternormal 对水底进行扰动;4;98;99;97;100;;1,1,1,1;0;0
Node;AmplifyShaderEditor.PosVertexDataNode;36;-1034.352,68.61979;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;33;1977.792,509.314;Inherit;False;WaterNormal;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;92;683.5615,2442.171;Inherit;False;636.2466;338.2581;世界空间的UV坐标采样;4;88;89;90;91;;1,1,1,1;0;0
Node;AmplifyShaderEditor.WorldPosInputsNode;90;733.5615,2494.423;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.GetLocalVarNode;97;749.5285,2858.088;Inherit;False;33;WaterNormal;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;34;-626.9978,-341.8567;Inherit;True;33;WaterNormal;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;39;-753.5157,-31.67912;Inherit;False;Constant;_Float0;Float 0;7;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.UnityObjToClipPosHlpNode;37;-808.9576,71.31202;Inherit;False;1;0;FLOAT3;0,0,0;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SwizzleNode;35;-427.7762,-340.1217;Inherit;True;FLOAT2;0;2;2;3;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;87;2199.88,2422.103;Inherit;False;913.2988;412.6763;Comment;6;81;82;83;84;85;110;Fresnel;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;91;737.575,2654.429;Inherit;False;Property;_UnderWaterlTiling;UnderWaterlTiling;13;0;Create;True;0;0;0;False;0;False;5;15;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SwizzleNode;88;930.4489,2492.171;Inherit;False;FLOAT2;0;1;2;3;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;99;855.93,2994.352;Inherit;False;Constant;_Float3;Float 3;14;0;Create;True;0;0;0;False;0;False;0.1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SwizzleNode;100;959.9299,2856.352;Inherit;False;FLOAT2;0;2;2;3;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;38;-580.5005,63.6046;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;89;1167.808,2623.371;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;98;1110.929,2884.352;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;41;-301.3598,110.3248;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;21;-446.9939,332.9828;Inherit;False;Property;_WaterNoise;WaterNoise;1;0;Create;True;0;0;0;False;0;False;0.5;0.3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;104;1222.272,3121.736;Inherit;False;Property;_Depth;Depth;15;0;Create;True;0;0;0;False;0;False;0;1.4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;82;2249.88,2646.78;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.GrabScreenPosition;2;-613.3757,579.4818;Inherit;False;0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;103;1077.73,3193.901;Inherit;False;Tangent;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldNormalVector;81;2255.282,2472.103;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleAddOpNode;96;1393.619,2731.386;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DotProductOpNode;83;2510.838,2605.614;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ParallaxOffsetHlpNode;102;1398.605,3108.626;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;-190.6561,305.1426;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SwizzleNode;3;-322.6142,581.4818;Inherit;True;FLOAT2;0;1;2;3;1;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ClampOpNode;84;2686.876,2604.841;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;20;-33.80718,563.2499;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;105;1615.772,2860.65;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;110;2768.567,2741.336;Inherit;False;Property;_FresnelOffset;FresnelOffset;16;0;Create;True;0;0;0;False;0;False;0;0.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;85;2837.669,2604.857;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;5;-49.30031,744.6302;Inherit;True;Property;_ReflectionTex;ReflectionTex;0;0;Create;False;0;0;0;False;0;False;-1;None;;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;79;1728.912,2794.94;Inherit;True;Property;_UnderWater;UnderWater;14;0;Create;True;0;0;0;False;0;False;-1;None;3187606e765bc90488a539bddc5c1f7a;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;43;-10.69046,1091.34;Inherit;False;ReflectionColor;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;93;2050.816,2873.909;Inherit;False;UnderWaterColor;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;108;3022.177,2597.648;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;64;3124.116,2403.877;Inherit;False;43;ReflectionColor;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;109;3235.45,2594.94;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;95;3119.784,2313.525;Inherit;False;93;UnderWaterColor;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;63;489.8071,1056.212;Inherit;False;1721.538;659.6326;Comment;17;46;45;47;48;55;56;51;57;52;59;54;58;60;44;61;62;78;SpecColor;1,0.4168184,0.08962262,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;77;597.0004,1780.173;Inherit;False;1218.468;494.8004;Comment;9;68;73;69;74;72;71;70;75;76;视线范围的线性渐变控制;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;115;2686.219,1930.323;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;118;2474.224,2200.881;Inherit;False;Constant;_Float5;Float 5;17;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;55;919.6358,1424.962;Inherit;False;Property;_SpecSmoothness;SpecSmoothness;8;0;Create;True;0;0;0;False;0;False;0.1;0.472;0.02;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;46;592.001,1174.429;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.DistanceOpNode;70;972.001,1930.173;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;117;2688.563,2103.669;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;51;1153.336,1216.962;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;68;714.0004,1830.173;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.PowerNode;54;1444.336,1296.962;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;76;1644.47,2000.904;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;56;1039.336,1518.962;Inherit;False;Constant;_Float2;Float 2;8;0;Create;True;0;0;0;False;0;False;256;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;62;2197.955,1439.752;Inherit;False;SpecColor;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;74;1109.401,2158.973;Inherit;False;Property;_SpecStart;SpecStart;12;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;60;1834.176,1424.418;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;61;1695.042,1563.344;Inherit;False;Property;_Ks;Ks;10;0;Create;True;0;0;0;False;0;False;1;1.07;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;120;3044.624,1978.344;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;66;3467.155,2165.324;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;44;913.0786,1123.212;Inherit;False;33;WaterNormal;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ColorNode;59;1432.587,1503.844;Inherit;False;Property;_SpecTint;SpecTint;9;0;Create;True;0;0;0;False;0;False;0.990566,0.9858935,0.9858935,0;1,0.9386792,0.9912399,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;71;1297.002,2109.174;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;57;1238.336,1445.962;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;75;1473.71,2006.797;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;72;1281.002,1930.174;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;65;3167.635,2192.691;Inherit;False;62;SpecColor;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;58;1654.987,1345.444;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;86;3348.682,2373.624;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;119;2866.593,2012.31;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;78;2037.861,1549.956;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;45;539.807,1345.224;Inherit;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldSpaceCameraPos;69;641.0004,2020.174;Inherit;True;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;73;1074.001,1850.173;Inherit;False;Property;_SpecEnd;SpecEnd;11;0;Create;True;0;0;0;False;0;False;0;97.35;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;112;2200.657,1866.958;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldSpaceCameraPos;113;2150.304,2042.262;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.DistanceOpNode;114;2475.395,1978.344;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;116;2556.21,1835.451;Inherit;False;Constant;_Float4;Float 4;17;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;47;854.3358,1270.962;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.NormalizeNode;48;980.3358,1263.962;Inherit;False;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMaxOpNode;52;1297.336,1188.962;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;3680.584,2165.04;Float;False;True;-1;2;ASEMaterialInspector;0;0;CustomLighting;Water;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;;0;False;;False;0;False;;0;False;;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;18;all;True;True;True;True;0;False;;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;2;15;10;25;False;0.5;True;0;0;False;;0;False;;0;0;False;;0;False;;0;False;;0;False;;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;;-1;0;False;;0;0;0;False;0.1;False;;0;False;;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;8;0;7;0
WireConnection;14;0;10;0
WireConnection;14;1;13;0
WireConnection;14;2;16;0
WireConnection;11;0;8;0
WireConnection;11;1;9;0
WireConnection;24;0;25;0
WireConnection;24;1;11;0
WireConnection;27;0;14;0
WireConnection;27;1;29;0
WireConnection;15;0;11;0
WireConnection;15;1;14;0
WireConnection;28;0;24;0
WireConnection;28;1;27;0
WireConnection;22;1;28;0
WireConnection;22;5;111;0
WireConnection;6;1;15;0
WireConnection;31;0;22;0
WireConnection;31;1;6;0
WireConnection;33;0;31;0
WireConnection;37;0;36;0
WireConnection;35;0;34;0
WireConnection;88;0;90;0
WireConnection;100;0;97;0
WireConnection;38;0;39;0
WireConnection;38;1;37;4
WireConnection;89;0;88;0
WireConnection;89;1;91;0
WireConnection;98;0;100;0
WireConnection;98;1;99;0
WireConnection;41;0;35;0
WireConnection;41;1;38;0
WireConnection;96;0;89;0
WireConnection;96;1;98;0
WireConnection;83;0;81;0
WireConnection;83;1;82;0
WireConnection;102;0;104;0
WireConnection;102;1;104;0
WireConnection;102;2;103;0
WireConnection;18;0;41;0
WireConnection;18;1;21;0
WireConnection;3;0;2;0
WireConnection;84;0;83;0
WireConnection;20;0;18;0
WireConnection;20;1;3;0
WireConnection;105;0;96;0
WireConnection;105;1;102;0
WireConnection;85;0;84;0
WireConnection;5;1;20;0
WireConnection;79;1;105;0
WireConnection;43;0;5;0
WireConnection;93;0;79;0
WireConnection;108;0;85;0
WireConnection;108;1;110;0
WireConnection;109;0;108;0
WireConnection;115;0;116;0
WireConnection;115;1;114;0
WireConnection;70;0;68;0
WireConnection;70;1;69;0
WireConnection;117;0;114;0
WireConnection;117;1;118;0
WireConnection;51;0;44;0
WireConnection;51;1;48;0
WireConnection;54;0;52;0
WireConnection;54;1;57;0
WireConnection;76;0;75;0
WireConnection;62;0;78;0
WireConnection;60;0;58;0
WireConnection;60;1;61;0
WireConnection;120;0;119;0
WireConnection;66;0;86;0
WireConnection;66;1;65;0
WireConnection;71;0;73;0
WireConnection;71;1;74;0
WireConnection;57;0;55;0
WireConnection;57;1;56;0
WireConnection;75;0;72;0
WireConnection;75;1;71;0
WireConnection;72;0;73;0
WireConnection;72;1;70;0
WireConnection;58;0;54;0
WireConnection;58;1;59;0
WireConnection;86;0;95;0
WireConnection;86;1;64;0
WireConnection;86;2;109;0
WireConnection;119;0;116;0
WireConnection;119;1;117;0
WireConnection;78;0;60;0
WireConnection;78;1;76;0
WireConnection;114;0;112;0
WireConnection;114;1;113;0
WireConnection;47;0;46;0
WireConnection;47;1;45;0
WireConnection;48;0;47;0
WireConnection;52;0;51;0
WireConnection;0;13;86;0
WireConnection;0;15;86;0
ASEEND*/
//CHKSM=9B671114A054EBFCE48AE3614A438481A40CEB04