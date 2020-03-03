// EasyRoads3D v3 Shader
// Blends detail texture with main texture on vertex color alpha


Shader "EasyRoads3D/Misc/ER Race Track" {
    Properties {
		
		[Header(Road 1)]
        _MainTex ("Albedo", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _BumpMap ("Normal", 2D) = "bump" {}
        _Metallic ("Metallic", Range(0.0, 1.0)) = 0.0 
        _Glossiness ("Smoothness", Range(0.0, 1.0)) = 0.5  
		_detail ("Racing Line", 2D) = "gray" {}
		_GlossinessTex ("Smoothness Tex", 2D) = "gray" {}

		
        _Cutoff ("Alpha Cutoff", Range(0,1)) = 0.5
        _Threshold ("Blend Threshold", Range(0.001,1)) = 1
    }

    SubShader {
        Tags {
            "SplatCount" = "3"
            "Queue" = "AlphaTest"
            "RenderType" = "TransparentCutout"
        }

        CGPROGRAM
        #pragma surface surf Standard  fullforwardshadows alphatest:_Cutoff
        #pragma target 3.0
        #pragma multi_compile_fog
        #pragma exclude_renderers gles
        #include "UnityPBSLighting.cginc"

        half _depthThresh;

        uniform sampler2D _Control;

        sampler2D _MainTex, _maskTex, _detail, _GlossinessTex;
        sampler2D _BumpMap;

        half _Metallic1;

        half _Glossiness1;

		half _Metallic;
		half _Glossiness;

        half _NormalStrengh;

        struct Input {
            float3 worldPos;
            float3 worldNormal;
            float2 uv_MainTex : TEXCOORD0;
            float2 uv4_detail : TEXCOORD1;

            float4 color : COLOR;

            INTERNAL_DATA
        };

        fixed4 _Color, _Color2;
        half _Threshold;

        void surf (Input IN, inout SurfaceOutputStandard o) {

            fixed4 alb = 0.0f;
			float4 c = IN.color;
			if(c.a < _Threshold){
				if(c.a > 0)c.a = (c.a / _Threshold);
			}else{
				c.a = 1;
			}
			
			float4 _main = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			float4 detail = tex2D (_detail, IN.uv4_detail) * _Color;
			float4 GlossinessTex = tex2D (_GlossinessTex, IN.uv4_detail) * _Color;

		//	float4 _mask = tex2D (_maskTex, IN.uv_MainTex);
			
            fixed4 nrm = tex2D(_BumpMap, IN.uv_MainTex); 

	//		half alpha = 0;
	//		if(_main.a > _main1.a)alpha = _main.a;
	//		else alpha = _main1.a;


            o.Normal = UnpackNormal(nrm);
         //   o.Albedo = _main.rgb;
            o.Albedo = _main.rgb * detail.rgb  * fixed4(2.0, 2.0, 2.0, 2.0).rgb;
            o.Alpha = _main.a;
            o.Smoothness = GlossinessTex;//detail.rgb * _Glossiness * 1;
            o.Metallic = _Metallic;
        }
        ENDCG
    }

    Fallback "Standard"
}
