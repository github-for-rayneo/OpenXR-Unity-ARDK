// Made with Amplify Shader Editor v1.9.0.1
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Diamond"
{
	Properties
	{
		_RefractTex("RefractTex", CUBE) = "white" {}
		_RefractionIndex("RefractionIndex", Range( 0 , 10)) = 1
		_ColorOverlay("Color Overlay", Color) = (1,1,1,0)
		_ReflectTex("ReflectTex", CUBE) = "white" {}
		_ReflectionIndex("ReflectionIndex", Range( 0 , 10)) = 1
		_RimPower("Rim Power", Range( 0 , 10)) = 3.235294
		_RimScale("RimScale", Range( 0 , 10)) = 1
		_RimBias("RimBias", Range( 0 , 1)) = 1
		_RimColor("RimColor", Color) = (1,1,1,0)

	}
	
	SubShader
	{
		
		
		Tags { "RenderType"="Opaque" "Queue"="Geometry" }
	LOD 100
		
		
		Pass
		{
			Name "First"
			Blend Off
			ZWrite On
			ZTest LEqual
			Cull Front
			CGPROGRAM

			

			#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
				//only defining to not throw compilation error over Unity 5.5
				#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
			#endif
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing
			#include "UnityCG.cginc"
			#define ASE_NEEDS_FRAG_WORLD_POSITION


			struct appdata
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				float3 ase_normal : NORMAL;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			
			struct v2f
			{
				float4 vertex : SV_POSITION;
				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
					float3 worldPos : TEXCOORD0;
				#endif
				float4 ase_texcoord1 : TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			uniform float4 _ColorOverlay;
			uniform samplerCUBE _RefractTex;
			uniform samplerCUBE _ReflectTex;
			uniform float _RefractionIndex;

			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				float3 ase_worldNormal = UnityObjectToWorldNormal(v.ase_normal);
				o.ase_texcoord1.xyz = ase_worldNormal;
				
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord1.w = 0;
				float3 vertexValue = float3(0, 0, 0);
				#if ASE_ABSOLUTE_VERTEX_POS
					vertexValue = v.vertex.xyz;
				#endif
				vertexValue = vertexValue;
				#if ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif
				o.vertex = UnityObjectToClipPos(v.vertex);

				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
					o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				#endif
				return o;
			}
			
			fixed4 frag (v2f i ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
				fixed4 finalColor;
				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
					float3 WorldPosition = i.worldPos;
				#endif
				float3 ase_worldViewDir = UnityWorldSpaceViewDir(WorldPosition);
				ase_worldViewDir = normalize(ase_worldViewDir);
				float3 ase_worldNormal = i.ase_texcoord1.xyz;
				float3 temp_output_9_0 = reflect( -ase_worldViewDir , ase_worldNormal );
				float4 texCUBENode6 = texCUBE( _ReflectTex, temp_output_9_0 );
				float4 temp_output_13_0 = ( _ColorOverlay * texCUBE( _RefractTex, temp_output_9_0 ) * texCUBENode6 * _RefractionIndex );
				
				
				finalColor = temp_output_13_0;
				return finalColor;
			}
			ENDCG
		}
		
		Pass
		{
			Name "Second"
			Blend One One
			ZWrite On
			ZTest LEqual
			Cull Back
			
			CGPROGRAM

			

			#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
				//only defining to not throw compilation error over Unity 5.5
				#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
			#endif
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing
			#include "UnityCG.cginc"
			#define ASE_NEEDS_FRAG_WORLD_POSITION


			struct appdata
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				float3 ase_normal : NORMAL;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			
			struct v2f
			{
				float4 vertex : SV_POSITION;
				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
					float3 worldPos : TEXCOORD0;
				#endif
				float4 ase_texcoord1 : TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			uniform float4 _ColorOverlay;
			uniform samplerCUBE _RefractTex;
			uniform samplerCUBE _ReflectTex;
			uniform float _RefractionIndex;
			uniform float _ReflectionIndex;
			uniform float _RimPower;
			uniform float _RimScale;
			uniform float _RimBias;
			uniform float4 _RimColor;

			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				float3 ase_worldNormal = UnityObjectToWorldNormal(v.ase_normal);
				o.ase_texcoord1.xyz = ase_worldNormal;
				
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord1.w = 0;
				float3 vertexValue = float3(0, 0, 0);
				#if ASE_ABSOLUTE_VERTEX_POS
					vertexValue = v.vertex.xyz;
				#endif
				vertexValue = vertexValue;
				#if ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif
				o.vertex = UnityObjectToClipPos(v.vertex);

				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
					o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				#endif
				return o;
			}
			
			fixed4 frag (v2f i ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
				fixed4 finalColor;
				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
					float3 WorldPosition = i.worldPos;
				#endif
				float3 ase_worldViewDir = UnityWorldSpaceViewDir(WorldPosition);
				ase_worldViewDir = normalize(ase_worldViewDir);
				float3 ase_worldNormal = i.ase_texcoord1.xyz;
				float3 temp_output_9_0 = reflect( -ase_worldViewDir , ase_worldNormal );
				float4 texCUBENode6 = texCUBE( _ReflectTex, temp_output_9_0 );
				float4 temp_output_13_0 = ( _ColorOverlay * texCUBE( _RefractTex, temp_output_9_0 ) * texCUBENode6 * _RefractionIndex );
				float dotResult23 = dot( ase_worldViewDir , ase_worldNormal );
				float clampResult24 = clamp( dotResult23 , 0.0 , 1.0 );
				float temp_output_25_0 = ( 1.0 - clampResult24 );
				float4 temp_output_19_0 = ( temp_output_13_0 + ( texCUBENode6 * _ReflectionIndex * temp_output_25_0 ) );
				float saferPower26 = abs( temp_output_25_0 );
				
				
				finalColor = ( temp_output_19_0 + ( temp_output_19_0 * ( ( ( pow( saferPower26 , _RimPower ) * _RimScale ) + _RimBias ) * _RimColor ) ) );
				return finalColor;
			}
			ENDCG
		}
		
	}
	CustomEditor "ASEMaterialInspector"
	
	Fallback Off
}
/*ASEBEGIN
Version=19001
1847.667;304;2257;990;1380.551;234.7917;1;True;True
Node;AmplifyShaderEditor.CommentaryNode;28;-2348.987,194.7632;Inherit;False;1247.222;489.2047;Fresnel;7;21;22;23;24;25;27;26;;1,1,1,1;0;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;21;-2286.987,244.7632;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldNormalVector;22;-2298.987,422.7634;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.CommentaryNode;11;-1562.94,-567.979;Inherit;False;617.8348;436.2016;R=reflect(-V,N);4;9;8;7;10;;1,1,1,1;0;0
Node;AmplifyShaderEditor.DotProductOpNode;23;-2035.988,327.7633;Inherit;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;7;-1512.94,-517.979;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ClampOpNode;24;-1771.988,336.7634;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;27;-1657.766,567.968;Inherit;False;Property;_RimPower;Rim Power;5;0;Create;True;0;0;0;False;0;False;3.235294;0;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;25;-1582.013,335.7025;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldNormalVector;8;-1463.303,-314.7774;Inherit;True;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.NegateNode;10;-1302.25,-425.8898;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;30;-1155.874,616.6572;Inherit;False;Property;_RimScale;RimScale;6;0;Create;True;0;0;0;False;0;False;1;0;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;26;-1361.766,383.968;Inherit;True;True;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ReflectOpNode;9;-1167.105,-353.2592;Inherit;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;5;-859.6937,-467.0828;Inherit;True;Property;_RefractTex;RefractTex;0;0;Create;True;0;0;0;False;0;False;-1;c6372c6d552909744935f84b7add2edf;b0224cd6f0a65c74ebcd03de8d02eed7;True;0;False;white;LockedToCube;False;Object;-1;Auto;Cube;8;0;SAMPLERCUBE;;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;6;-867.0999,-162.6;Inherit;True;Property;_ReflectTex;ReflectTex;3;0;Create;True;0;0;0;False;0;False;-1;c49ef192a57a5af42a728163b2468c8a;c49ef192a57a5af42a728163b2468c8a;True;0;False;white;LockedToCube;False;Object;-1;Auto;Cube;8;0;SAMPLERCUBE;;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;17;-856.262,-266.1718;Inherit;False;Property;_RefractionIndex;RefractionIndex;1;0;Create;True;0;0;0;False;0;False;1;0;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;12;-803.5073,-660.9681;Inherit;False;Property;_ColorOverlay;Color Overlay;2;0;Create;True;0;0;0;False;0;False;1,1,1,0;0.9474079,1,0.1462264,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;33;-831.4627,570.4315;Inherit;False;Property;_RimBias;RimBias;7;0;Create;True;0;0;0;False;0;False;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;18;-867.262,56.82825;Inherit;False;Property;_ReflectionIndex;ReflectionIndex;4;0;Create;True;0;0;0;False;0;False;1;0;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;29;-897.5444,284.4082;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;32;-514.0486,222.5394;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;36;-450.9088,452.0023;Inherit;False;Property;_RimColor;RimColor;8;0;Create;True;0;0;0;False;0;False;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;-465.9076,-513.7682;Inherit;False;4;4;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;20;-468.9082,-114.0486;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;37;-226.4725,258.3455;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;19;-344.262,-282.1718;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;35;-191.3656,-66.71036;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;34;-27.73535,-81.75174;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;16;174.1989,-104.4131;Float;False;False;-1;2;ASEMaterialInspector;100;12;New Amplify Shader;d6c708ecc827c484ca5a08195799c171;True;Second;0;1;Second;2;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;False;False;0;False;True;4;1;False;;1;False;;0;1;False;;0;False;;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;False;False;False;False;False;False;False;False;False;False;True;1;False;;True;3;False;;False;False;False;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;15;-267.6,-454.2;Float;False;True;-1;2;ASEMaterialInspector;100;12;Diamond;d6c708ecc827c484ca5a08195799c171;True;First;0;0;First;2;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;False;False;0;False;True;0;1;False;;0;False;;0;1;False;;0;False;;False;False;False;False;False;False;False;False;False;False;False;True;True;1;False;;False;False;False;False;False;False;False;False;False;False;False;True;1;False;;True;3;False;;False;False;False;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;0;2;True;True;False;;False;0
WireConnection;23;0;21;0
WireConnection;23;1;22;0
WireConnection;24;0;23;0
WireConnection;25;0;24;0
WireConnection;10;0;7;0
WireConnection;26;0;25;0
WireConnection;26;1;27;0
WireConnection;9;0;10;0
WireConnection;9;1;8;0
WireConnection;5;1;9;0
WireConnection;6;1;9;0
WireConnection;29;0;26;0
WireConnection;29;1;30;0
WireConnection;32;0;29;0
WireConnection;32;1;33;0
WireConnection;13;0;12;0
WireConnection;13;1;5;0
WireConnection;13;2;6;0
WireConnection;13;3;17;0
WireConnection;20;0;6;0
WireConnection;20;1;18;0
WireConnection;20;2;25;0
WireConnection;37;0;32;0
WireConnection;37;1;36;0
WireConnection;19;0;13;0
WireConnection;19;1;20;0
WireConnection;35;0;19;0
WireConnection;35;1;37;0
WireConnection;34;0;19;0
WireConnection;34;1;35;0
WireConnection;16;0;34;0
WireConnection;15;0;13;0
ASEEND*/
//CHKSM=F8F281D8A8724AE3166B26B1D9BDFFE7D109FF82