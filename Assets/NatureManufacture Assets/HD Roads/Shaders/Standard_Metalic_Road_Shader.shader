Shader "NatureManufacture Shaders/Standard Metalic Road Material"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_TextureSample1("Second Road Noise Mask", 2D) = "white" {}
		_SecondRoadNoiseMaskPower("Second Road Noise Mask Power", Range( 0 , 10)) = 0.1
		_MainRoadColor("Main Road Color", Color) = (1,1,1,1)
		_MainRoadBrightness("Main Road Brightness", Float) = 1
		_MainTex("Main Road Albedo_T", 2D) = "white" {}
		_SecondRoadNoiseMaskTreshold("Second Road Noise Mask Treshold", Range( 0 , 10)) = 1
		_MainRoadAlphaCutOut("Main Road Alpha CutOut", Range( 0 , 2)) = 1
		_BumpMap("Main Road Normal", 2D) = "bump" {}
		_BumpScale("Main Road BumpScale", Range( 0 , 5)) = 0
		_MetalicRAmbientOcclusionGHeightBEmissionA("Main Road Metallic (R) Ambient Occlusion (G) Height (B) Smoothness (A)", 2D) = "white" {}
		_MainRoadMetalicPower("Main Road Metalic Power", Range( 0 , 2)) = 0
		_MainRoadAmbientOcclusionPower("Main Road Ambient Occlusion Power", Range( 0 , 1)) = 1
		_MainRoadSmoothnessPower("Main Road Smoothness Power", Range( 0 , 2)) = 1
		_SecondRoadColor("Second Road Color", Color) = (1,1,1,1)
		_SecondRoadBrightness("Second Road Brightness", Float) = 1
		_TextureSample3("Second Road Albedo_T", 2D) = "white" {}
		[Toggle(_IGNORESECONDROADALPHA_ON)] _IgnoreSecondRoadAlpha("Ignore Second Road Alpha", Float) = 0
		_SecondRoadAlphaCutOut("Second Road Alpha CutOut", Range( 0 , 2)) = 1
		_SecondRoadNormal("Second Road Normal", 2D) = "bump" {}
		_SecondRoadNormalScale("Second Road Normal Scale", Range( 0 , 5)) = 0
		_SecondRoadNormalBlend("Second Road Normal Blend", Range( 0 , 1)) = 0.8
		_SecondRoadMetallicRAmbientocclusionGHeightBSmoothnessA("Second Road Metallic (R) Ambient occlusion (G) Height (B) Smoothness (A)", 2D) = "white" {}
		_SecondRoadMetalicPower("Second Road Metalic Power", Range( 0 , 2)) = 1
		_SecondRoadAmbientOcclusionPower("Second Road Ambient Occlusion Power", Range( 0 , 1)) = 1
		_SecondRoadSmoothnessPower("Second Road Smoothness Power", Range( 0 , 2)) = 1
		_DetailMask("DetailMask (A)", 2D) = "white" {}
		_DetailAlbedoMap("DetailAlbedoMap", 2D) = "black" {}
		_DetailAlbedoPower("Main Road Detail Albedo Power", Range( 0 , 2)) = 0
		_Float2("Second Road Detail Albedo Power", Range( 0 , 2)) = 0
		_DetailNormalMap("DetailNormalMap", 2D) = "bump" {}
		_DetailNormalMapScale("Main Road DetailNormalMapScale", Range( 0 , 5)) = 0
		_Float1("Second Road DetailNormalMapScale", Range( 0 , 5)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
		[Header(Forward Rendering Options)]
		[ToggleOff] _SpecularHighlights("Specular Highlights", Float) = 1.0
		[ToggleOff] _GlossyReflections("Reflections", Float) = 1.0
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "AlphaTest+0" }
		Cull Back
		ZTest LEqual
		Offset  -2 , 0
		CGPROGRAM
		#include "UnityStandardUtils.cginc"
		#pragma target 3.0
		#pragma shader_feature _SPECULARHIGHLIGHTS_OFF
		#pragma shader_feature _GLOSSYREFLECTIONS_OFF
		#pragma shader_feature _IGNORESECONDROADALPHA_ON
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			fixed2 uv_texcoord;
			float4 vertexColor : COLOR;
		};

		uniform fixed _BumpScale;
		uniform sampler2D _BumpMap;
		uniform float4 _BumpMap_ST;
		uniform fixed _DetailNormalMapScale;
		uniform sampler2D _DetailNormalMap;
		uniform sampler2D _DetailAlbedoMap;
		uniform float4 _DetailAlbedoMap_ST;
		uniform sampler2D _DetailMask;
		uniform float4 _DetailMask_ST;
		uniform fixed _SecondRoadNormalScale;
		uniform sampler2D _SecondRoadNormal;
		uniform float4 _SecondRoadNormal_ST;
		uniform fixed _SecondRoadNormalBlend;
		uniform fixed _Float1;
		uniform sampler2D _TextureSample1;
		uniform float4 _TextureSample1_ST;
		uniform fixed _SecondRoadNoiseMaskPower;
		uniform fixed _SecondRoadNoiseMaskTreshold;
		uniform fixed _MainRoadBrightness;
		uniform sampler2D _MainTex;
		uniform float4 _MainTex_ST;
		uniform fixed4 _MainRoadColor;
		uniform fixed _DetailAlbedoPower;
		uniform fixed _SecondRoadBrightness;
		uniform sampler2D _TextureSample3;
		uniform float4 _TextureSample3_ST;
		uniform fixed4 _SecondRoadColor;
		uniform fixed _Float2;
		uniform sampler2D _MetalicRAmbientOcclusionGHeightBEmissionA;
		uniform float4 _MetalicRAmbientOcclusionGHeightBEmissionA_ST;
		uniform fixed _MainRoadMetalicPower;
		uniform sampler2D _SecondRoadMetallicRAmbientocclusionGHeightBSmoothnessA;
		uniform float4 _SecondRoadMetallicRAmbientocclusionGHeightBSmoothnessA_ST;
		uniform fixed _SecondRoadMetalicPower;
		uniform fixed _MainRoadSmoothnessPower;
		uniform fixed _SecondRoadSmoothnessPower;
		uniform fixed _MainRoadAmbientOcclusionPower;
		uniform fixed _SecondRoadAmbientOcclusionPower;
		uniform fixed _MainRoadAlphaCutOut;
		uniform fixed _SecondRoadAlphaCutOut;
		uniform float _Cutoff = 0.5;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_BumpMap = i.uv_texcoord * _BumpMap_ST.xy + _BumpMap_ST.zw;
			fixed3 tex2DNode4 = UnpackScaleNormal( tex2D( _BumpMap, uv_BumpMap ) ,_BumpScale );
			float2 uv_DetailAlbedoMap = i.uv_texcoord * _DetailAlbedoMap_ST.xy + _DetailAlbedoMap_ST.zw;
			float2 uv_DetailMask = i.uv_texcoord * _DetailMask_ST.xy + _DetailMask_ST.zw;
			fixed4 tex2DNode481 = tex2D( _DetailMask, uv_DetailMask );
			float3 lerpResult479 = lerp( tex2DNode4 , BlendNormals( tex2DNode4 , UnpackScaleNormal( tex2D( _DetailNormalMap, uv_DetailAlbedoMap ) ,_DetailNormalMapScale ) ) , tex2DNode481.a);
			float2 uv_SecondRoadNormal = i.uv_texcoord * _SecondRoadNormal_ST.xy + _SecondRoadNormal_ST.zw;
			fixed3 tex2DNode535 = UnpackScaleNormal( tex2D( _SecondRoadNormal, uv_SecondRoadNormal ) ,_SecondRoadNormalScale );
			float3 lerpResult570 = lerp( lerpResult479 , tex2DNode535 , _SecondRoadNormalBlend);
			float3 lerpResult617 = lerp( tex2DNode535 , BlendNormals( lerpResult570 , UnpackScaleNormal( tex2D( _DetailNormalMap, uv_DetailAlbedoMap ) ,_Float1 ) ) , tex2DNode481.a);
			float2 uv_TextureSample1 = i.uv_texcoord * _TextureSample1_ST.xy + _TextureSample1_ST.zw;
			float clampResult673 = clamp( pow( ( min( min( min( tex2D( _TextureSample1, uv_TextureSample1 ).r , tex2D( _TextureSample1, ( uv_TextureSample1 * float2( 0.5,0.5 ) ) ).r ) , tex2D( _TextureSample1, ( uv_TextureSample1 * float2( 0.2,0.2 ) ) ).r ) , tex2D( _TextureSample1, ( uv_TextureSample1 * float2( 0.36,0.35 ) ) ).r ) * _SecondRoadNoiseMaskPower ) , _SecondRoadNoiseMaskTreshold ) , 0 , 1 );
			float4 appendResult665 = (fixed4(( ( i.vertexColor / float4( 1,1,1,1 ) ).r - clampResult673 ) , ( i.vertexColor / float4( 1,1,1,1 ) ).g , ( i.vertexColor / float4( 1,1,1,1 ) ).b , ( i.vertexColor / float4( 1,1,1,1 ) ).a));
			float4 clampResult672 = clamp( appendResult665 , float4( 0,0,0,0 ) , float4( 1,1,1,1 ) );
			float3 lerpResult593 = lerp( lerpResult479 , lerpResult617 , ( 1.0 - clampResult672 ).x);
			o.Normal = lerpResult593;
			float2 uv_MainTex = i.uv_texcoord * _MainTex_ST.xy + _MainTex_ST.zw;
			fixed4 tex2DNode1 = tex2D( _MainTex, uv_MainTex );
			float4 temp_output_77_0 = ( ( _MainRoadBrightness * tex2DNode1 ) * _MainRoadColor );
			fixed4 tex2DNode486 = tex2D( _DetailAlbedoMap, uv_DetailAlbedoMap );
			fixed4 blendOpSrc474 = temp_output_77_0;
			fixed4 blendOpDest474 = ( _DetailAlbedoPower * tex2DNode486 );
			float4 lerpResult480 = lerp( temp_output_77_0 , (( blendOpDest474 > 0.5 ) ? ( 1.0 - ( 1.0 - 2.0 * ( blendOpDest474 - 0.5 ) ) * ( 1.0 - blendOpSrc474 ) ) : ( 2.0 * blendOpDest474 * blendOpSrc474 ) ) , ( _DetailAlbedoPower * tex2DNode481.a ));
			float2 uv_TextureSample3 = i.uv_texcoord * _TextureSample3_ST.xy + _TextureSample3_ST.zw;
			fixed4 tex2DNode537 = tex2D( _TextureSample3, uv_TextureSample3 );
			float4 temp_output_540_0 = ( ( _SecondRoadBrightness * tex2DNode537 ) * _SecondRoadColor );
			fixed4 blendOpSrc619 = temp_output_540_0;
			fixed4 blendOpDest619 = ( tex2DNode486 * _Float2 );
			float4 lerpResult618 = lerp( temp_output_540_0 , (( blendOpDest619 > 0.5 ) ? ( 1.0 - ( 1.0 - 2.0 * ( blendOpDest619 - 0.5 ) ) * ( 1.0 - blendOpSrc619 ) ) : ( 2.0 * blendOpDest619 * blendOpSrc619 ) ) , ( _Float2 * tex2DNode481.a ));
			float4 lerpResult592 = lerp( lerpResult480 , lerpResult618 , ( 1.0 - clampResult672 ).x);
			o.Albedo = lerpResult592.rgb;
			float2 uv_MetalicRAmbientOcclusionGHeightBEmissionA = i.uv_texcoord * _MetalicRAmbientOcclusionGHeightBEmissionA_ST.xy + _MetalicRAmbientOcclusionGHeightBEmissionA_ST.zw;
			fixed4 tex2DNode2 = tex2D( _MetalicRAmbientOcclusionGHeightBEmissionA, uv_MetalicRAmbientOcclusionGHeightBEmissionA );
			float2 uv_SecondRoadMetallicRAmbientocclusionGHeightBSmoothnessA = i.uv_texcoord * _SecondRoadMetallicRAmbientocclusionGHeightBSmoothnessA_ST.xy + _SecondRoadMetallicRAmbientocclusionGHeightBSmoothnessA_ST.zw;
			fixed4 tex2DNode536 = tex2D( _SecondRoadMetallicRAmbientocclusionGHeightBSmoothnessA, uv_SecondRoadMetallicRAmbientocclusionGHeightBSmoothnessA );
			float lerpResult601 = lerp( ( tex2DNode2.r * _MainRoadMetalicPower ) , ( tex2DNode536.r * _SecondRoadMetalicPower ) , ( 1.0 - clampResult672 ).x);
			o.Metallic = lerpResult601;
			float lerpResult594 = lerp( ( tex2DNode2.a * _MainRoadSmoothnessPower ) , ( _SecondRoadSmoothnessPower * tex2DNode536.a ) , ( 1.0 - clampResult672 ).x);
			o.Smoothness = lerpResult594;
			float clampResult96 = clamp( tex2DNode2.g , ( 1 - _MainRoadAmbientOcclusionPower ) , 1 );
			float clampResult546 = clamp( tex2DNode536.g , ( 1 - _SecondRoadAmbientOcclusionPower ) , 1 );
			float lerpResult602 = lerp( clampResult96 , clampResult546 , ( 1.0 - clampResult672 ).x);
			o.Occlusion = lerpResult602;
			o.Alpha = 1;
			float temp_output_629_0 = ( tex2DNode1.a * _MainRoadAlphaCutOut );
			#ifdef _IGNORESECONDROADALPHA_ON
				float staticSwitch685 = temp_output_629_0;
			#else
				float staticSwitch685 = ( tex2DNode537.a * _SecondRoadAlphaCutOut );
			#endif
			float lerpResult628 = lerp( temp_output_629_0 , staticSwitch685 , ( 1.0 - clampResult672 ).x);
			clip( lerpResult628 - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
}