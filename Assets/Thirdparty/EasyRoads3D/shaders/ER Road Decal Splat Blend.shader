// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// EasyRoads3D v3 Shader
// Blends detail texture with main texture on splat color UV4, the UV4 dat is set in the Inspector:
// Additional UV Data > Detail
// The tile distance defines over which distance the control map will tile. 
// This can be expanded to 4 detail textures by also using the blue and alpha channel of the control map

Shader "EasyRoads3D/ER Road Decal Splat Blend" {
    Properties {
        _Control ("Control (RGB)", 2D) = "red" {}
        _Detail1 ("Detail", 2D) = "white" {}
        _Normal1 ("Detail Normal", 2D) = "bump" {}
        _Metallic1 ("Metallic 1", Range(0.0, 1.0)) = 0.0  
        _Smoothness1 ("Smoothness 1", Range(0.0, 1.0)) = 0.0  

        _MainTex ("Road", 2D) = "white" {}
        _NormalRoad ("Road Normal", 2D) = "bump" {}
        _MetallicRoad ("Road Metallic", Range(0.0, 1.0)) = 0.0 
        _SmoothnessRoad ("Road Smoothness", Range(0.0, 1.0)) = 0.0  

        // used in fallback on old cards & base map
        _Color ("Main Color", Color) = (1,1,1,1)
        _Cutoff ("Alpha Cutoff", Range(0,1)) = 0.5

        [Header(Terrain Z Fighting Offset)]
		_OffsetFactor ("Offset Factor", Range(0.0,-10.0)) = -1
        _OffsetUnit ("Offset Unit", Range(0.0,-10.0)) = -1
    }

    SubShader {
        Tags {
            "SplatCount" = "3"
            "Queue" = "AlphaTest"
            "RenderType" = "TransparentCutout"
            "ForceNoShadowCasting"="True"
        }

        Offset [_OffsetFactor],[_OffsetUnit]

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
   //     #pragma surface surf Standard  fullforwardshadows alphatest:_Cutoff
        #pragma surface surf Standard  fullforwardshadows decal:blend
        #pragma target 3.0
        #pragma multi_compile_fog
        #pragma exclude_renderers gles
        #include "UnityPBSLighting.cginc"

        // Access the Shaderlab properties
        half _depthThresh;

        uniform sampler2D _Control;

        sampler2D _Detail1, _MainTex;
        //float4 _Splat0_ST;
        //float4 _Detail1_ST;
        //float4 _Splat2_ST;
        sampler2D _Normal1, _NormalRoad;

        half _Metallic1;

        half _Smoothness1;

		half _MetallicRoad;
		half _SmoothnessRoad;

        half _Height0;
        half _Height1;
        half _Height2;

        float _TextureScale;
        half _NormalStrengh;

        struct Input {
            float3 worldPos;
            float3 worldNormal;
            float2 uv_MainTex : TEXCOORD0;
            float2 uv_Control : TEXCOORD1;

            // if main uv would be used
      //      float2 uv_Splat0 : TEXCOORD2;

			// for splats we use mesh.uv4 > shader uv3
            float2 uv_Splat0 : TEXCOORD2;
            float2 uv4_Detail1 : TEXCOORD3;
            float2 uv_Splat2 : TEXCOORD4;
            float2 uv_Splat3 : TEXCOORD5;
            float2 uv_Road : TEXCOORD5;

            //float2 uv_Normal0 : TEXCOORD6;
            //float2 uv_Normal1 : TEXCOORD7;
            //float2 uv_Normal2 : TEXCOORD8;
            //float2 uv_Normal3 : TEXCOORD9;
            INTERNAL_DATA
        };

        void SplatmapVert(inout appdata_full v, out Input data)
        {
            UNITY_INITIALIZE_OUTPUT(Input, data);
            float4 pos = UnityObjectToClipPos (v.vertex);
            //UNITY_TRANSFER_FOG(data, pos);

            v.tangent.xyz = cross(v.normal, float3(0,0,1));
            v.tangent.w = -1;
        }

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        void surf (Input IN, inout SurfaceOutputStandard o) {
            fixed4 splatControl = tex2D(_Control, IN.uv4_Detail1);

            fixed4 alb = 0.0f;

			float4 _main = tex2D (_MainTex, IN.uv_MainTex);
			float4 _detail = tex2D (_Detail1, IN.uv_MainTex);
			
            fixed4 nrm = 0.0f;
            nrm += splatControl.r * tex2D(_NormalRoad, IN.uv_MainTex);
            nrm += splatControl.g * tex2D(_Normal1, IN.uv_MainTex);

            fixed3 finalNormal = UnpackNormal(nrm);

            //Final output
            o.Normal = finalNormal;
            o.Albedo = lerp(_main.rgb, _detail.rgb, splatControl.g);
            o.Alpha = _main.a;//1;
            o.Smoothness = _SmoothnessRoad * _main.a;//dot(tex2D(_Control, IN.uv_Control), half3(_Smoothness0, _Smoothness1, _Smoothness2));
            o.Metallic = _MetallicRoad * _main.a;//dot(tex2D(_Control, IN.uv_Control), half3(_Metallic0, _Metallic1, _Metallic2));
        }
        ENDCG
    }

    Fallback "Standard"
}
