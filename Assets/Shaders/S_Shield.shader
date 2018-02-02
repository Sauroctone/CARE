// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "S_Shield"
{
	Properties
	{
		_FlashPeriod("Flash Period", Range( 1 , 10)) = 8
		_BaseColor("Base Color", Color) = (0.9264706,0.9100512,0.504109,0)
		_FlashColor("Flash Color", Color) = (0.8078432,0.2117647,0.5058824,0)
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "DisableBatching" = "True" }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float3 worldPos;
		};

		uniform float4 _BaseColor;
		uniform float4 _FlashColor;
		uniform float _FlashPeriod;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 temp_output_14_0 = ( _FlashColor * (0.0 + (sin( ( _Time.y * _FlashPeriod ) ) - -1.0) * (1.0 - 0.0) / (1.0 - -1.0)) );
			float4 lerpResult23 = lerp( _BaseColor , temp_output_14_0 , (temp_output_14_0).g);
			o.Albedo = lerpResult23.rgb;
			o.Smoothness = 0.0;
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float clampResult4 = clamp( ( 0.188198 + ( 1.0 * ase_vertex3Pos.y ) ) , 0.0 , 1.0 );
			o.Alpha = clampResult4;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows 

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
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float3 worldPos : TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				fixed3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			fixed4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				float3 worldPos = IN.worldPos;
				fixed3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=13901
-32;448;1906;1004;2197.997;678.8982;1.665593;True;True
Node;AmplifyShaderEditor.SimpleTimeNode;19;-1648.787,-302.1073;Float;False;1;0;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;16;-1606.893,-155.4663;Float;False;Property;_FlashPeriod;Flash Period;0;0;8;1;10;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;-1312.191,-222.6517;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SinOpNode;20;-1164.787,-120.1074;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;3;-1234.928,170.701;Float;False;Constant;_GradientPosition;Gradient Position;0;0;1;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.ColorNode;27;-1167.886,-591.8295;Float;False;Property;_FlashColor;Flash Color;1;0;0.8078432,0.2117647,0.5058824,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.PosVertexDataNode;1;-1267.928,259.701;Float;False;0;0;5;FLOAT3;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.TFHCRemapNode;18;-1020.786,-155.1075;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;-1.0;False;2;FLOAT;1.0;False;3;FLOAT;0.0;False;4;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;2;-946.0717,278.929;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;-826.5001,-409.1083;Float;True;2;2;0;COLOR;0.0;False;1;FLOAT;0.0;False;1;COLOR
Node;AmplifyShaderEditor.RangedFloatNode;13;-1002.695,99.32993;Float;False;Constant;_Float1;Float 1;1;0;0.188198;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.ColorNode;6;-471.32,-61.5576;Float;False;Property;_BaseColor;Base Color;0;0;0.9264706,0.9100512,0.504109,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;12;-703.378,235.2391;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.ComponentMaskNode;24;-560.0599,-224.1089;Float;False;False;True;False;False;1;0;COLOR;0,0,0,0;False;1;FLOAT
Node;AmplifyShaderEditor.LerpOp;23;-236.253,-381.6004;Float;True;3;0;COLOR;0.0;False;1;COLOR;0.0,0,0,0;False;2;FLOAT;0.0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.ClampOpNode;4;-430.7652,244.3728;Float;True;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;10;-387.5891,146.3194;Float;False;Constant;_Float0;Float 0;1;0;0;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;S_Shield;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;False;False;Back;0;0;False;0;0;Transparent;0.5;True;True;0;False;Transparent;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;0;0;0;0;False;2;15;10;25;False;0.5;True;2;SrcAlpha;OneMinusSrcAlpha;0;Zero;Zero;OFF;OFF;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;0;0;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;FLOAT;0.0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;15;0;19;0
WireConnection;15;1;16;0
WireConnection;20;0;15;0
WireConnection;18;0;20;0
WireConnection;2;0;3;0
WireConnection;2;1;1;2
WireConnection;14;0;27;0
WireConnection;14;1;18;0
WireConnection;12;0;13;0
WireConnection;12;1;2;0
WireConnection;24;0;14;0
WireConnection;23;0;6;0
WireConnection;23;1;14;0
WireConnection;23;2;24;0
WireConnection;4;0;12;0
WireConnection;0;0;23;0
WireConnection;0;4;10;0
WireConnection;0;9;4;0
ASEEND*/
//CHKSM=EECF292BBD2AD56461FBD9FB65DBA6FC0193DF9E