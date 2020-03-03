// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'


Shader "EasyRoads3D/Terrain/Road Splat 4 UV4" {
    Properties {
    	[Header(Terrain Splatmap)]
        _Control ("Control (RGB)", 2D) = "red" {}
        _Splat0 ("Terrain Splat 1 (R)", 2D) = "white" {}
        _Normal0 ("Normal 1 (R)", 2D) = "bump" {}
        
        
        _Splat1 ("Terrain Splat 2 (G)", 2D) = "white" {}
        _Normal1 ("Normal 2 (G)", 2D) = "bump" {}
        
        
        _Splat2 ("Terrain Splat 3 (B)", 2D) = "white" {}
        _Normal2 ("Normal 3 (B)", 2D) = "bump" {}
        
        
        _Splat3 ("Terrain Splat 4 (A)", 2D) = "white" {}
        _Normal3 ("Normal 4 (A)", 2D) = "bump" {}


		_Metallic0 ("Metallic 1", Range(0.0, 1.0)) = 0.0
        _Smoothness0 ("Smoothness 1", Range(0.0, 1.0)) = 0.0
        
        _Metallic1 ("Metallic 2", Range(0.0, 1.0)) = 0.0
        _Smoothness1 ("Smoothness 2", Range(0.0, 1.0)) = 0.0
        
        _Metallic2 ("Metallic 3", Range(0.0, 1.0)) = 0.0
        _Smoothness2 ("Smoothness 3", Range(0.0, 1.0)) = 0.0
        
        _Metallic3 ("Metallic 4", Range(0.0, 1.0)) = 0.0
        _Smoothness3 ("Smoothness 4", Range(0.0, 1.0)) = 1.0
        
        [Header(Road Texture)]
        // used in fallback on old cards & base map
        _MainTex ("Diffuse", 2D) = "white" {}
        _MainTexNormal ("Normal", 2D) = "bump" {}
        
        _BaseMap ("BaseMap (RGB)", 2D) = "white" {}
        _Color ("Main Color", Color) = (1,1,1,1)
    }

    SubShader {
        Tags {
            "SplatCount" = "3"
            "Queue" = "Geometry-100"
            "RenderType" = "Opaque"
        }

        CGPROGRAM
        #pragma surface surf Standard  fullforwardshadows 
        #pragma target 5.0
        #pragma multi_compile_fog
        #pragma exclude_renderers gles
        #include "UnityPBSLighting.cginc"

        half _depthThresh;

        uniform sampler2D _Control;

        sampler2D _Splat0,_Splat1,_Splat2,_Splat3, _MainTex;
        sampler2D _Normal0,_Normal1,_Normal2,_Normal3, _MainTexNormal;

        half _Metallic0;
        half _Metallic1;
        half _Metallic2;
        half _Metallic3;

        half _Smoothness0;
        half _Smoothness1;
        half _Smoothness2;
        half _Smoothness3;

        half _Height0;
        half _Height1;
        half _Height2;

        float _TextureScale;
        half _NormalStrengh;

        struct Input {
            float3 worldPos;
            float3 worldNormal;
            float2 uv_MainTex : TEXCOORD0;
            float2 uv4_Control : TEXCOORD1;
            float2 uv4_Splat0 : TEXCOORD2;
            float2 uv4_Splat1 : TEXCOORD3;
            float2 uv4_Splat2 : TEXCOORD4;
            float2 uv4_Splat3 : TEXCOORD5;
            float4 color : COLOR;

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

            v.tangent.xyz = cross(v.normal, float3(0,0,1));
            v.tangent.w = -1;
        }

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        void surf (Input IN, inout SurfaceOutputStandard o) {
            fixed4 splatControl = tex2D(_Control, IN.uv4_Control);
			
			// Terrain
             fixed4 alb = 0.0f;
            //nrm += splatControl.r * normal0;
            //nrm += splatControl.g * normal1;
            //nrm += splatControl.b * normal2;
            alb += splatControl.r * tex2D(_Splat0, IN.uv4_Splat0);
            alb += splatControl.g * tex2D(_Splat1, IN.uv4_Splat1);
            alb += splatControl.b * tex2D(_Splat2, IN.uv4_Splat2);
            alb += splatControl.a * tex2D(_Splat3, IN.uv4_Splat3);
       //     alb *= _Color;
            
            // Track
            float4 c = IN.color;
            fixed4 albRoad = tex2D(_MainTex, IN.uv_MainTex);
			alb = lerp(alb, albRoad, c.a);
            
   //         half weight;
    //        fixed4 mixedDiffuse;
	//		half4 defaultSmoothness = half4(tex2D(_Splat0, IN.uv_Splat0), tex2D(_Splat1, IN.uv_Splat1), tex2D(_Splat2, IN.uv_Splat2), tex2D(_Splat3, IN.uv_Splat3));
	//		SplatmapMix(IN, defaultSmoothness, splatControl, weight, mixedDiffuse, o.Normal);
	//		o.Albedo = mixedDiffuse.rgb;

            fixed4 nrm = 0.0f;
            //nrm += splatControl.r * normal0;
            //nrm += splatControl.g * normal1;
            //nrm += splatControl.b * normal2;
            nrm += splatControl.r * tex2D(_Normal0, IN.uv4_Splat0);
            nrm += splatControl.g * tex2D(_Normal1, IN.uv4_Splat1);
            nrm += splatControl.b * tex2D(_Normal2, IN.uv4_Splat2);
            nrm += splatControl.a * tex2D(_Normal3, IN.uv4_Splat3);
            //nrm /= b1 + b2 + b3;

            // Track
            fixed4 nRoad = tex2D(_MainTexNormal, IN.uv_MainTex);
			nrm = lerp(nrm, nRoad, c.a);

     //       nrm.g = nrm.r;

            fixed3 finalNormal = UnpackNormal(nrm);

            //Final output
            o.Normal = finalNormal;
            o.Albedo = alb;
            o.Alpha = 1;
            o.Smoothness = dot(tex2D(_Control, IN.uv4_Control), half4(_Smoothness0, _Smoothness1, _Smoothness2, _Smoothness3));
            o.Metallic = dot(tex2D(_Control, IN.uv4_Control), half4(_Metallic0, _Metallic1, _Smoothness2, _Metallic3));
        }
        ENDCG
    }
    Dependency "AddPassShader" = "Hidden/TerrainEngine/Splatmap/Standard-AddPass"
    Dependency "BaseMapShader" = "Hidden/TerrainEngine/Splatmap/Standard-Base"

    Fallback "Nature/Terrain/Diffuse"
}
