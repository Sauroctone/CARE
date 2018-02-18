// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "S_rock_shader"
{
	Properties
	{
		_BaseColor("Base Color", Color) = (0.1838235,0.1770653,0.1770653,0)
		_GradientMultiplier("Gradient Multiplier", Range( 0 , 0.1)) = 0.1396763
		_ShadeColor("Shade Color", Color) = (0.06812285,0.06812285,0.07352942,0)
		_Smoothness("Smoothness", Range( 0 , 1)) = 0.08379545
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "DisableBatching" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float3 worldPos;
		};

		uniform float4 _BaseColor;
		uniform float4 _ShadeColor;
		uniform float _GradientMultiplier;
		uniform float _Smoothness;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float temp_output_4_0 = ( _GradientMultiplier * ase_vertex3Pos.z );
			float clampResult5 = clamp( temp_output_4_0 , 0.0 , 1.0 );
			float4 lerpResult2 = lerp( _BaseColor , _ShadeColor , ( 1.0 - clampResult5 ));
			o.Emission = lerpResult2.rgb;
			o.Smoothness = _Smoothness;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=13901
57;418;1906;1004;2126.749;632.1167;1.368861;True;True
Node;AmplifyShaderEditor.RangedFloatNode;6;-1671.608,40.31636;Float;False;Property;_GradientMultiplier;Gradient Multiplier;1;0;0.1396763;0;0.1;0;1;FLOAT
Node;AmplifyShaderEditor.PosVertexDataNode;3;-1656.629,167.2213;Float;False;0;0;5;FLOAT3;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;4;-1356.496,124.1855;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0,0,0;False;1;FLOAT
Node;AmplifyShaderEditor.ClampOpNode;5;-1150.908,118.3233;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.ColorNode;1;-975.1629,-261.073;Float;False;Property;_BaseColor;Base Color;0;0;0.1838235,0.1770653,0.1770653,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.OneMinusNode;12;-975.5699,186.5088;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.ColorNode;7;-1012.069,-58.38291;Float;False;Property;_ShadeColor;Shade Color;2;0;0.06812285,0.06812285,0.07352942,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;58;-1474.577,431.5139;Float;False;Property;_SnowLevel;Snow Level;7;0;0;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;57;-960.5614,322.7165;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.ClampOpNode;56;-752.0565,309.4479;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.LerpOp;2;-663.8906,-38.34608;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.ColorNode;11;-615.1376,-233.6026;Float;False;Property;_SnowColor;Snow Color;4;0;0.8983564,0.9089906,0.9117647,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.TFHCCompareLower;33;-768.1212,1061.514;Float;False;4;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.NormalVertexDataNode;32;-1574.321,922.6228;Float;False;0;5;FLOAT3;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.WorldNormalVector;25;-1573.338,1075.829;Float;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.ClampOpNode;48;-1173.702,934.7853;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.3;False;2;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;53;-1128.637,1343.814;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;-0.1;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;31;-1558.844,1316.028;Float;False;Property;_ClampMax;Clamp Max;6;0;0;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.ClampOpNode;27;-1259.818,1108.846;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;8;-245.9214,230.8566;Float;False;Property;_Smoothness;Smoothness;3;0;0.08379545;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;52;-1176.715,1461.922;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;-0.01;False;1;FLOAT
Node;AmplifyShaderEditor.TFHCCompareWithRange;49;-843.6865,1606.054;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;4;FLOAT;0.7;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;30;-1558.844,1218.912;Float;False;Property;_ClampMin;Clamp Min;5;0;0;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.LerpOp;55;-377.6032,1013.074;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0.0,0,0,0;False;2;FLOAT;0.0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.LerpOp;9;-282.8051,-7.421618;Float;True;3;0;COLOR;0.0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0.0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;85,-46;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;S_rock_shader;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;Back;0;0;False;0;0;Opaque;0.5;True;True;0;False;Opaque;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;0;0;0;0;False;2;15;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;OFF;OFF;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;0;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;FLOAT;0.0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;4;0;6;0
WireConnection;4;1;3;3
WireConnection;5;0;4;0
WireConnection;12;0;5;0
WireConnection;57;0;3;3
WireConnection;57;1;58;0
WireConnection;56;0;57;0
WireConnection;2;0;1;0
WireConnection;2;1;7;0
WireConnection;2;2;12;0
WireConnection;33;0;32;1
WireConnection;33;1;32;3
WireConnection;33;2;48;0
WireConnection;48;0;4;0
WireConnection;53;0;32;3
WireConnection;27;0;32;3
WireConnection;27;1;30;0
WireConnection;27;2;31;0
WireConnection;52;0;32;1
WireConnection;49;0;32;3
WireConnection;49;1;53;0
WireConnection;49;2;52;0
WireConnection;55;0;11;0
WireConnection;55;1;2;0
WireConnection;55;2;33;0
WireConnection;9;0;2;0
WireConnection;9;1;11;0
WireConnection;9;2;56;0
WireConnection;0;2;2;0
WireConnection;0;4;8;0
ASEEND*/
//CHKSM=2D3091FB4DFE8E51503A56BF15CDE9AD9EFF45CF