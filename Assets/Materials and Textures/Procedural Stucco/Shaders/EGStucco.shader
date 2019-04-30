// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Ethan Gross/EGStucco"
{
	Properties
	{
		EG_Stucco_metalRough("EG_Stucco_metalRough", 2D) = "white"{}
		EG_Stucco_roughness("EG_Stucco_roughness", 2D) = "white"{}
		[HideInInspector] __dirty( "", Int ) = 1
		EG_Stucco_normal("EG_Stucco_normal", 2D) = "white"{}
		EG_Stucco_basecolor("EG_Stucco_basecolor", 2D) = "white"{}
		_heightStrength("heightStrength", Range( 0 , 1)) = 0.1843704
		_roughness("roughness", Range( 0.5 , 1)) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		_TessValue( "Max Tessellation", Range( 1, 32 ) ) = 30
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#pragma target 4.6
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc tessellate:tessFunction nolightmap 
		struct Input
		{
			float2 uv_texcoord;
		};

		struct appdata
		{
			float4 vertex : POSITION;
			float4 tangent : TANGENT;
			float3 normal : NORMAL;
			float4 texcoord : TEXCOORD0;
			float4 texcoord1 : TEXCOORD1;
			float4 texcoord2 : TEXCOORD2;
			float4 texcoord3 : TEXCOORD3;
			fixed4 color : COLOR;
			UNITY_VERTEX_INPUT_INSTANCE_ID
		};

		uniform sampler2D EG_Stucco_normal;
		uniform float4 EG_Stucco_basecolor_ST;
		uniform sampler2D EG_Stucco_basecolor;
		uniform sampler2D EG_Stucco_metalRough;
		uniform sampler2D EG_Stucco_roughness;
		uniform float _roughness;
		uniform float _heightStrength;
		uniform float _TessValue;

		float4 tessFunction( )
		{
			return _TessValue;
		}

		void vertexDataFunc( inout appdata v )
		{
			float4 uvEG_Stucco_basecolor = float4(v.texcoord * EG_Stucco_basecolor_ST.xy + EG_Stucco_basecolor_ST.zw, 0 ,0);
			float4 EG_Stucco_roughness1 = tex2Dlod( EG_Stucco_roughness, uvEG_Stucco_basecolor);
			v.vertex.xyz += ( ( EG_Stucco_roughness1 * _heightStrength ) * float4( v.normal , 0.0 ) ).rgb;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uvEG_Stucco_basecolor = i.uv_texcoord * EG_Stucco_basecolor_ST.xy + EG_Stucco_basecolor_ST.zw;
			float3 EG_Stucco_normal1 = UnpackNormal( tex2D( EG_Stucco_normal, uvEG_Stucco_basecolor) );
			o.Normal = EG_Stucco_normal1;
			float4 EG_Stucco_basecolor1 = tex2D( EG_Stucco_basecolor, uvEG_Stucco_basecolor);
			o.Albedo = EG_Stucco_basecolor1.rgb;
			float4 EG_Stucco_metalRough1 = tex2D( EG_Stucco_metalRough, uvEG_Stucco_basecolor);
			float4 EG_Stucco_roughness1 = tex2D( EG_Stucco_roughness, uvEG_Stucco_basecolor);
			float4 temp_output_14_0 = ( EG_Stucco_roughness1 * _roughness );
			o.Metallic = ( EG_Stucco_metalRough1 * temp_output_14_0 ).r;
			o.Smoothness = ( 1.0 - temp_output_14_0 ).r;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=7003
144;44;1440;851;1842.386;561.3482;1.720004;True;True
Node;AmplifyShaderEditor.RangedFloatNode;3;-1428.732,121.2065;Float;False;Property;_roughness;roughness;1;0;1;0.5;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;9;-634.2568,329.686;Float;False;Property;_heightStrength;heightStrength;0;0;0.1843704;0;1;FLOAT
Node;AmplifyShaderEditor.SubstanceSamplerNode;1;-1155.406,-314.5428;Float;True;Property;_SubstanceSample0;Substance Sample 0;2;0;c5b84a5b4ed364b978a306f78c30364e;0;True;0;FLOAT2;0,0;False;COLOR;FLOAT3;COLOR;COLOR;COLOR
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;-163.5139,183.3956;Float;False;0;COLOR;0,0,0,0;False;1;FLOAT;0.0,0,0,0;False;COLOR
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;-868.194,21.90225;Float;False;0;COLOR;0,0,0,0;False;1;FLOAT;0,0,0,0;False;COLOR
Node;AmplifyShaderEditor.NormalVertexDataNode;4;-165.7139,289.5289;Float;False;FLOAT3;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.OneMinusNode;33;-540.1715,134.8038;Float;False;0;COLOR;0.0;False;COLOR
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;5;215.0843,275.4897;Float;True;0;COLOR;0.0,0,0;False;1;FLOAT3;0,0,0,0;False;COLOR
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;-167.5959,32.56648;Float;False;0;COLOR;0.0,0,0,0;False;1;COLOR;0,0,0,0;False;COLOR
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;621.2975,19.38095;Float;False;True;6;Float;ASEMaterialInspector;0;Standard;Ethan Gross/EGStucco;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;0;False;0;0;Opaque;0.5;True;True;0;False;Opaque;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;True;1;30;10;25;False;0.729;True;0;Zero;Zero;0;Zero;Zero;Add;Add;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;Relative;0;;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;13;OBJECT;0.0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False
WireConnection;10;0;1;2
WireConnection;10;1;9;0
WireConnection;14;0;1;2
WireConnection;14;1;3;0
WireConnection;33;0;14;0
WireConnection;5;0;10;0
WireConnection;5;1;4;0
WireConnection;13;0;1;4
WireConnection;13;1;14;0
WireConnection;0;0;1;0
WireConnection;0;1;1;1
WireConnection;0;3;13;0
WireConnection;0;4;33;0
WireConnection;0;11;5;0
ASEEND*/
//CHKSM=A7E17CA92451A40E3D4EF55A7C62E7163011CCF1