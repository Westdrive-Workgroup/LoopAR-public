 Shader "NatureManufacture Shaders/Standard Metalic Cross Road Material Parallax"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_TextureSample1("Second Road Noise Mask", 2D) = "white" {}
		_SecondRoadNoiseMaskPower("Second Road Noise Mask Power", Range( 0 , 10)) = 0.1
		_SecondRoadNoiseMaskTreshold("Second Road Noise Mask Treshold", Range( 0 , 10)) = 1
		_MainRoadColor("Main Road Color", Color) = (1,1,1,1)
		_MainRoadBrightness("Main Road Brightness", Float) = 1
		_MainTex("Main Road Albedo_T", 2D) = "white" {}
		[Toggle]_MainRoadUV3("Main Road UV3", Float) = 0
		_MainRoadAlphaCutOut("Main Road Alpha CutOut", Range( 0 , 2)) = 1
		_BumpMap("Main Road Normal", 2D) = "bump" {}
		_BumpScale("Main Road BumpScale", Range( 0 , 5)) = 0
		_MetalicRAmbientOcclusionGHeightBEmissionA("Main Road Metallic (R) Ambient Occlusion (G) Height (B) Smoothness (A)", 2D) = "white" {}
		_MainRoadMetalicPower("Main Road Metalic Power", Range( 0 , 2)) = 0
		_MainRoadAmbientOcclusionPower("Main Road Ambient Occlusion Power", Range( 0 , 1)) = 1
		_MainRoadParallaxPower("Main Road Parallax Power", Range( 0 , 0.1)) = 0
		_MainRoadSmoothnessPower("Main Road Smoothness Power", Range( 0 , 2)) = 1
		_SecondRoadColor("Second Road Color", Color) = (1,1,1,1)
		_SecondRoadBrightness("Second Road Brightness", Float) = 1
		_TextureSample3("Second Road Albedo_T", 2D) = "white" {}
		[Toggle]_SecondRoadUV3("Second Road UV3", Float) = 0
		[Toggle(_IGNORESECONDROADALPHA_ON)] _IgnoreSecondRoadAlpha("Ignore Second Road Alpha", Float) = 0
		_SecondRoadAlphaCutOut("Second Road Alpha CutOut", Range( 0 , 2)) = 1
		_SecondRoadNormal("Second Road Normal", 2D) = "bump" {}
		_SecondRoadNormalScale("Second Road Normal Scale", Range( 0 , 5)) = 0
		_SecondRoadNormalBlend("Second Road Normal Blend", Range( 0 , 1)) = 0.8
		_SecondRoadMetallicRAmbientocclusionGHeightBSmoothnessA("Second Road Metallic (R) Ambient occlusion (G) Height (B) Smoothness (A)", 2D) = "white" {}
		_SecondRoadMetalicPower("Second Road Metalic Power", Range( 0 , 2)) = 1
		_SecondRoadAmbientOcclusionPower("Second Road Ambient Occlusion Power", Range( 0 , 1)) = 1
		_SecondRoadParallaxPower("Second Road Parallax Power", Range( -0.1 , 0.1)) = 0
		_SecondRoadSmoothnessPower("Second Road Smoothness Power", Range( 0 , 2)) = 1
		_CrossRoadColor("Cross Road Color", Color) = (1,1,1,1)
		_CrossRoadBrightness("Cross Road Brightness", Float) = 1
		_TextureSample4("Cross Road Albedo_T", 2D) = "white" {}
		[Toggle]_CrossRoadUV3("Cross Road UV3", Float) = 0
		[Toggle(_IGNORECROSSROADALPHA_ON)] _IgnoreCrossRoadAlpha("Ignore Cross Road Alpha", Float) = 0
		_CrossRoadAlphaCutOut("Cross Road Alpha CutOut", Range( 0 , 2)) = 1
		_CrossRoadNormal("Cross Road Normal", 2D) = "bump" {}
		_CrossRoadNormalScale("Cross Road Normal Scale", Range( 0 , 5)) = 0
		_CrossRoadMetallicRAmbientocclusionGHeightBSmoothnessA("Cross Road Metallic (R) Ambient occlusion (G) Height (B) Smoothness (A)", 2D) = "white" {}
		_CrossRoadMetallicPower("Cross Road Metallic Power", Range( 0 , 2)) = 1
		_CrossRoadAmbientOcclusionPower("Cross Road Ambient Occlusion Power", Range( 0 , 1)) = 1
		_CrossRoadParallaxPower("Cross Road Parallax Power", Range( -0.1 , 0.1)) = 0
		_CrossRoadSmoothnessPower("Cross Road Smoothness Power", Range( 0 , 2)) = 1
		_DetailAlbedoMap("DetailAlbedoMap", 2D) = "black" {}
		_DetailAlbedoPower("Main Road Detail Albedo Power", Range( 0 , 2)) = 0
		_Float3("Cross Road Detail Albedo Power", Range( 0 , 2)) = 2
		_Float2("Second Road Detail Albedo Power", Range( 0 , 2)) = 0
		_DetailNormalMap("DetailNormalMap", 2D) = "bump" {}
		_DetailNormalMapScale("Main Road DetailNormalMapScale", Range( 0 , 5)) = 0
		_Float0("Cross Road Detail NormalMap Scale", Range( 0 , 5)) = 0
		_Float1("Second Road DetailNormalMapScale", Range( 0 , 5)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] _texcoord3( "", 2D ) = "white" {}
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
		Offset  0 , 0
		CGINCLUDE
		#include "UnityStandardUtils.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#pragma shader_feature _SPECULARHIGHLIGHTS_OFF
		#pragma shader_feature _GLOSSYREFLECTIONS_OFF
		#pragma shader_feature _IGNORESECONDROADALPHA_ON
		#pragma shader_feature _IGNORECROSSROADALPHA_ON
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) fixed3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			fixed2 uv_texcoord;
			fixed2 uv3_texcoord3;
			float3 viewDir;
			INTERNAL_DATA
			float4 vertexColor : COLOR;
		};

		uniform fixed _BumpScale;
		uniform sampler2D _BumpMap;
		uniform fixed _MainRoadUV3;
		uniform sampler2D _MainTex;
		uniform float4 _MainTex_ST;
		uniform float4 _BumpMap_ST;
		uniform sampler2D _MetalicRAmbientOcclusionGHeightBEmissionA;
		uniform float4 _MetalicRAmbientOcclusionGHeightBEmissionA_ST;
		uniform fixed _MainRoadParallaxPower;
		uniform fixed _DetailNormalMapScale;
		uniform sampler2D _DetailNormalMap;
		uniform sampler2D _DetailAlbedoMap;
		uniform float4 _DetailAlbedoMap_ST;
		uniform fixed _SecondRoadNormalScale;
		uniform sampler2D _SecondRoadNormal;
		uniform fixed _SecondRoadUV3;
		uniform sampler2D _TextureSample3;
		uniform float4 _TextureSample3_ST;
		uniform float4 _SecondRoadNormal_ST;
		uniform sampler2D _SecondRoadMetallicRAmbientocclusionGHeightBSmoothnessA;
		uniform float4 _SecondRoadMetallicRAmbientocclusionGHeightBSmoothnessA_ST;
		uniform fixed _SecondRoadParallaxPower;
		uniform fixed _SecondRoadNormalBlend;
		uniform fixed _Float1;
		uniform sampler2D _TextureSample1;
		uniform float4 _TextureSample1_ST;
		uniform fixed _SecondRoadNoiseMaskPower;
		uniform fixed _SecondRoadNoiseMaskTreshold;
		uniform fixed _CrossRoadNormalScale;
		uniform sampler2D _CrossRoadNormal;
		uniform fixed _CrossRoadUV3;
		uniform sampler2D _TextureSample4;
		uniform float4 _TextureSample4_ST;
		uniform float4 _CrossRoadNormal_ST;
		uniform sampler2D _CrossRoadMetallicRAmbientocclusionGHeightBSmoothnessA;
		uniform float4 _CrossRoadMetallicRAmbientocclusionGHeightBSmoothnessA_ST;
		uniform fixed _CrossRoadParallaxPower;
		uniform fixed _Float0;
		uniform fixed _MainRoadBrightness;
		uniform fixed4 _MainRoadColor;
		uniform fixed _DetailAlbedoPower;
		uniform fixed _SecondRoadBrightness;
		uniform fixed4 _SecondRoadColor;
		uniform fixed _Float2;
		uniform fixed _CrossRoadBrightness;
		uniform fixed4 _CrossRoadColor;
		uniform fixed _Float3;
		uniform fixed _MainRoadMetalicPower;
		uniform fixed _SecondRoadMetalicPower;
		uniform fixed _CrossRoadMetallicPower;
		uniform fixed _MainRoadSmoothnessPower;
		uniform fixed _SecondRoadSmoothnessPower;
		uniform fixed _CrossRoadSmoothnessPower;
		uniform fixed _MainRoadAmbientOcclusionPower;
		uniform fixed _SecondRoadAmbientOcclusionPower;
		uniform fixed _CrossRoadAmbientOcclusionPower;
		uniform fixed _MainRoadAlphaCutOut;
		uniform fixed _SecondRoadAlphaCutOut;
		uniform fixed _CrossRoadAlphaCutOut;
		uniform float _Cutoff = 0.5;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_MainTex = i.uv_texcoord * _MainTex_ST.xy + _MainTex_ST.zw;
			float2 uv_BumpMap = i.uv3_texcoord3 * _BumpMap_ST.xy + _BumpMap_ST.zw;
			float2 uv_MetalicRAmbientOcclusionGHeightBEmissionA = i.uv_texcoord * _MetalicRAmbientOcclusionGHeightBEmissionA_ST.xy + _MetalicRAmbientOcclusionGHeightBEmissionA_ST.zw;
			float2 Offset724 = ( ( tex2D( _MetalicRAmbientOcclusionGHeightBEmissionA, uv_MetalicRAmbientOcclusionGHeightBEmissionA ).b - 1 ) * i.viewDir.xy * _MainRoadParallaxPower ) + lerp(uv_MainTex,uv_BumpMap,_MainRoadUV3);
			float2 Offset744 = ( ( tex2D( _MetalicRAmbientOcclusionGHeightBEmissionA, Offset724 ).b - 1 ) * i.viewDir.xy * _MainRoadParallaxPower ) + Offset724;
			float2 Offset766 = ( ( tex2D( _MetalicRAmbientOcclusionGHeightBEmissionA, Offset744 ).b - 1 ) * i.viewDir.xy * _MainRoadParallaxPower ) + Offset744;
			float2 Offset795 = ( ( tex2D( _MetalicRAmbientOcclusionGHeightBEmissionA, Offset766 ).b - 1 ) * i.viewDir.xy * _MainRoadParallaxPower ) + Offset766;
			fixed3 tex2DNode4 = UnpackScaleNormal( tex2D( _BumpMap, Offset795 ) ,_BumpScale );
			float2 uv_DetailAlbedoMap = i.uv_texcoord * _DetailAlbedoMap_ST.xy + _DetailAlbedoMap_ST.zw;
			fixed4 tex2DNode486 = tex2D( _DetailAlbedoMap, uv_DetailAlbedoMap );
			float3 lerpResult479 = lerp( tex2DNode4 , BlendNormals( tex2DNode4 , UnpackScaleNormal( tex2D( _DetailNormalMap, uv_DetailAlbedoMap ) ,_DetailNormalMapScale ) ) , tex2DNode486.a);
			float2 uv_TextureSample3 = i.uv_texcoord * _TextureSample3_ST.xy + _TextureSample3_ST.zw;
			float2 uv_SecondRoadNormal = i.uv3_texcoord3 * _SecondRoadNormal_ST.xy + _SecondRoadNormal_ST.zw;
			float2 uv_SecondRoadMetallicRAmbientocclusionGHeightBSmoothnessA = i.uv_texcoord * _SecondRoadMetallicRAmbientocclusionGHeightBSmoothnessA_ST.xy + _SecondRoadMetallicRAmbientocclusionGHeightBSmoothnessA_ST.zw;
			float2 Offset728 = ( ( tex2D( _SecondRoadMetallicRAmbientocclusionGHeightBSmoothnessA, uv_SecondRoadMetallicRAmbientocclusionGHeightBSmoothnessA ).b - 1 ) * i.viewDir.xy * _SecondRoadParallaxPower ) + lerp(uv_TextureSample3,uv_SecondRoadNormal,_SecondRoadUV3);
			float2 Offset747 = ( ( tex2D( _SecondRoadMetallicRAmbientocclusionGHeightBSmoothnessA, Offset728 ).b - 1 ) * i.viewDir.xy * _SecondRoadParallaxPower ) + Offset728;
			float2 Offset774 = ( ( tex2D( _SecondRoadMetallicRAmbientocclusionGHeightBSmoothnessA, Offset747 ).b - 1 ) * i.viewDir.xy * _SecondRoadParallaxPower ) + Offset747;
			float2 Offset794 = ( ( tex2D( _SecondRoadMetallicRAmbientocclusionGHeightBSmoothnessA, Offset774 ).b - 1 ) * i.viewDir.xy * _SecondRoadParallaxPower ) + Offset774;
			fixed3 tex2DNode535 = UnpackScaleNormal( tex2D( _SecondRoadNormal, Offset794 ) ,_SecondRoadNormalScale );
			float3 lerpResult570 = lerp( lerpResult479 , tex2DNode535 , _SecondRoadNormalBlend);
			float3 lerpResult617 = lerp( tex2DNode535 , BlendNormals( lerpResult570 , UnpackScaleNormal( tex2D( _DetailNormalMap, uv_DetailAlbedoMap ) ,_Float1 ) ) , tex2DNode486.a);
			float2 uv_TextureSample1 = i.uv_texcoord * _TextureSample1_ST.xy + _TextureSample1_ST.zw;
			float temp_output_682_0 = min( min( tex2D( _TextureSample1, uv_TextureSample1 ).r , tex2D( _TextureSample1, ( uv_TextureSample1 * float2( 0.4,0.4 ) ) ).r ) , tex2D( _TextureSample1, ( uv_TextureSample1 * float2( 0.2,0.2 ) ) ).r );
			float clampResult673 = clamp( pow( ( temp_output_682_0 * _SecondRoadNoiseMaskPower ) , _SecondRoadNoiseMaskTreshold ) , 0 , 1 );
			float4 appendResult665 = (fixed4(( ( 1.0 - ( i.vertexColor / float4( 1,1,1,1 ) ).r ) - clampResult673 ) , ( 1.0 - ( i.vertexColor / float4( 1,1,1,1 ) ).g ) , ( i.vertexColor / float4( 1,1,1,1 ) ).b , ( i.vertexColor / float4( 1,1,1,1 ) ).a));
			float4 clampResult672 = clamp( appendResult665 , float4( 0,0,0,0 ) , float4( 1,1,1,1 ) );
			float3 lerpResult593 = lerp( lerpResult479 , lerpResult617 , ( 1.0 - clampResult672 ).x);
			float2 uv_TextureSample4 = i.uv_texcoord * _TextureSample4_ST.xy + _TextureSample4_ST.zw;
			float2 uv_CrossRoadNormal = i.uv3_texcoord3 * _CrossRoadNormal_ST.xy + _CrossRoadNormal_ST.zw;
			float2 uv_CrossRoadMetallicRAmbientocclusionGHeightBSmoothnessA = i.uv_texcoord * _CrossRoadMetallicRAmbientocclusionGHeightBSmoothnessA_ST.xy + _CrossRoadMetallicRAmbientocclusionGHeightBSmoothnessA_ST.zw;
			float2 Offset805 = ( ( tex2D( _CrossRoadMetallicRAmbientocclusionGHeightBSmoothnessA, uv_CrossRoadMetallicRAmbientocclusionGHeightBSmoothnessA ).b - 1 ) * i.viewDir.xy * _CrossRoadParallaxPower ) + lerp(uv_TextureSample4,uv_CrossRoadNormal,_CrossRoadUV3);
			float2 Offset815 = ( ( tex2D( _CrossRoadMetallicRAmbientocclusionGHeightBSmoothnessA, Offset805 ).b - 1 ) * i.viewDir.xy * _CrossRoadParallaxPower ) + Offset805;
			float2 Offset839 = ( ( tex2D( _CrossRoadMetallicRAmbientocclusionGHeightBSmoothnessA, Offset815 ).b - 1 ) * i.viewDir.xy * _CrossRoadParallaxPower ) + Offset815;
			float2 Offset838 = ( ( tex2D( _CrossRoadMetallicRAmbientocclusionGHeightBSmoothnessA, Offset839 ).b - 1 ) * i.viewDir.xy * _CrossRoadParallaxPower ) + Offset839;
			fixed3 tex2DNode637 = UnpackScaleNormal( tex2D( _CrossRoadNormal, Offset838 ) ,_CrossRoadNormalScale );
			float3 lerpResult647 = lerp( tex2DNode637 , BlendNormals( tex2DNode637 , UnpackScaleNormal( tex2D( _DetailNormalMap, uv_DetailAlbedoMap ) ,_Float0 ) ) , tex2DNode486.a);
			float3 lerpResult848 = lerp( lerpResult647 , lerpResult617 , ( 1.0 - clampResult672 ).x);
			float3 lerpResult640 = lerp( lerpResult593 , lerpResult848 , ( 1.0 - clampResult672 ).y);
			o.Normal = lerpResult640;
			fixed4 tex2DNode1 = tex2D( _MainTex, Offset795 );
			float4 temp_output_77_0 = ( ( _MainRoadBrightness * tex2DNode1 ) * _MainRoadColor );
			fixed4 blendOpSrc474 = temp_output_77_0;
			fixed4 blendOpDest474 = ( _DetailAlbedoPower * tex2DNode486 );
			float4 lerpResult480 = lerp( temp_output_77_0 , (( blendOpDest474 > 0.5 ) ? ( 1.0 - ( 1.0 - 2.0 * ( blendOpDest474 - 0.5 ) ) * ( 1.0 - blendOpSrc474 ) ) : ( 2.0 * blendOpDest474 * blendOpSrc474 ) ) , ( _DetailAlbedoPower * tex2DNode486.a ));
			fixed4 tex2DNode537 = tex2D( _TextureSample3, Offset794 );
			float4 temp_output_540_0 = ( ( _SecondRoadBrightness * tex2DNode537 ) * _SecondRoadColor );
			fixed4 blendOpSrc619 = temp_output_540_0;
			fixed4 blendOpDest619 = ( tex2DNode486 * _Float2 );
			float4 lerpResult618 = lerp( temp_output_540_0 , (( blendOpDest619 > 0.5 ) ? ( 1.0 - ( 1.0 - 2.0 * ( blendOpDest619 - 0.5 ) ) * ( 1.0 - blendOpSrc619 ) ) : ( 2.0 * blendOpDest619 * blendOpSrc619 ) ) , ( _Float2 * tex2DNode486.a ));
			float4 lerpResult592 = lerp( lerpResult480 , lerpResult618 , ( 1.0 - clampResult672 ).x);
			fixed4 tex2DNode638 = tex2D( _TextureSample4, Offset838 );
			float4 temp_output_654_0 = ( ( _CrossRoadBrightness * tex2DNode638 ) * _CrossRoadColor );
			fixed4 blendOpSrc652 = temp_output_654_0;
			fixed4 blendOpDest652 = tex2DNode486;
			float4 lerpResult653 = lerp( temp_output_654_0 , (( blendOpDest652 > 0.5 ) ? ( 1.0 - ( 1.0 - 2.0 * ( blendOpDest652 - 0.5 ) ) * ( 1.0 - blendOpSrc652 ) ) : ( 2.0 * blendOpDest652 * blendOpSrc652 ) ) , ( tex2DNode486.a * _Float3 ));
			float4 lerpResult852 = lerp( lerpResult653 , lerpResult618 , ( 1.0 - clampResult672 ).x);
			float4 lerpResult644 = lerp( lerpResult592 , lerpResult852 , ( 1.0 - clampResult672 ).y);
			o.Albedo = lerpResult644.rgb;
			fixed4 tex2DNode2 = tex2D( _MetalicRAmbientOcclusionGHeightBEmissionA, Offset795 );
			fixed4 tex2DNode536 = tex2D( _SecondRoadMetallicRAmbientocclusionGHeightBSmoothnessA, Offset794 );
			float temp_output_547_0 = ( tex2DNode536.r * _SecondRoadMetalicPower );
			float lerpResult601 = lerp( ( tex2DNode2.r * _MainRoadMetalicPower ) , temp_output_547_0 , ( 1.0 - clampResult672 ).x);
			fixed4 tex2DNode639 = tex2D( _CrossRoadMetallicRAmbientocclusionGHeightBSmoothnessA, Offset838 );
			float lerpResult850 = lerp( ( _CrossRoadMetallicPower * tex2DNode639.r ) , temp_output_547_0 , 0);
			float lerpResult643 = lerp( lerpResult601 , lerpResult850 , ( 1.0 - clampResult672 ).y);
			o.Metallic = lerpResult643;
			float temp_output_548_0 = ( _SecondRoadSmoothnessPower * tex2DNode536.a );
			float lerpResult594 = lerp( ( tex2DNode2.a * _MainRoadSmoothnessPower ) , temp_output_548_0 , ( 1.0 - clampResult672 ).x);
			float lerpResult847 = lerp( ( tex2DNode639.a * _CrossRoadSmoothnessPower ) , temp_output_548_0 , ( 1.0 - clampResult672 ).x);
			float lerpResult645 = lerp( lerpResult594 , lerpResult847 , ( 1.0 - clampResult672 ).y);
			o.Smoothness = lerpResult645;
			float clampResult96 = clamp( tex2DNode2.g , ( 1 - _MainRoadAmbientOcclusionPower ) , 1 );
			float clampResult546 = clamp( tex2DNode536.g , ( 1 - _SecondRoadAmbientOcclusionPower ) , 1 );
			float lerpResult602 = lerp( clampResult96 , clampResult546 , ( 1.0 - clampResult672 ).x);
			float clampResult662 = clamp( tex2DNode639.g , ( 1 - _CrossRoadAmbientOcclusionPower ) , 1 );
			float lerpResult851 = lerp( clampResult662 , clampResult546 , ( 1.0 - clampResult672 ).x);
			float lerpResult642 = lerp( lerpResult602 , lerpResult851 , ( 1.0 - clampResult672 ).y);
			o.Occlusion = lerpResult642;
			o.Alpha = 1;
			float temp_output_629_0 = ( tex2DNode1.a * _MainRoadAlphaCutOut );
			#ifdef _IGNORESECONDROADALPHA_ON
				float staticSwitch693 = temp_output_629_0;
			#else
				float staticSwitch693 = ( tex2DNode537.a * _SecondRoadAlphaCutOut );
			#endif
			float lerpResult628 = lerp( temp_output_629_0 , staticSwitch693 , ( 1.0 - clampResult672 ).x);
			#ifdef _IGNORECROSSROADALPHA_ON
				float staticSwitch696 = lerpResult628;
			#else
				float staticSwitch696 = ( tex2DNode638.a * _CrossRoadAlphaCutOut );
			#endif
			float lerpResult849 = lerp( staticSwitch696 , staticSwitch693 , ( 1.0 - clampResult672 ).x);
			float lerpResult641 = lerp( lerpResult628 , lerpResult849 , ( 1.0 - clampResult672 ).y);
			clip( lerpResult641 - _Cutoff );
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows 

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
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float4 customPack1 : TEXCOORD1;
				float4 tSpace0 : TEXCOORD2;
				float4 tSpace1 : TEXCOORD3;
				float4 tSpace2 : TEXCOORD4;
				fixed4 color : COLOR0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				fixed3 worldNormal = UnityObjectToWorldNormal( v.normal );
				fixed3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				fixed tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				fixed3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.customPack1.zw = customInputData.uv3_texcoord3;
				o.customPack1.zw = v.texcoord2;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				o.color = v.color;
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
				surfIN.uv_texcoord = IN.customPack1.xy;
				surfIN.uv3_texcoord3 = IN.customPack1.zw;
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				fixed3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.viewDir = IN.tSpace0.xyz * worldViewDir.x + IN.tSpace1.xyz * worldViewDir.y + IN.tSpace2.xyz * worldViewDir.z;
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				surfIN.vertexColor = IN.color;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
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
}