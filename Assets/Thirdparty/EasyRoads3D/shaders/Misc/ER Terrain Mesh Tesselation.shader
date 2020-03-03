Shader "EasyRoads3D/Terrain/ER Terrain Mesh Tesselation" {
        Properties {

    	[Header(Terrain Splatmap)]
    	[Space(10)]_terrainSize("Terrain Size", Float) = 500
        _Control ("Control (RGB)", 2D) = "red" {}
        [Space(25)]_Splat0 ("Terrain Splat 1 (R)", 2D) = "white" {}
        _Normal0 ("Normal 1 (R)", 2D) = "bump" {}
        _splatTyle0("Tiling", Float) = 1
        
        [Space(25)]_Splat1 ("Terrain Splat 2 (G)", 2D) = "white" {}
        _Normal1 ("Normal 2 (G)", 2D) = "bump" {}
        _splatTyle1("Tiling", Float) = 1
        
        [Space(25)]_Splat2 ("Terrain Splat 3 (B)", 2D) = "white" {}
        _Normal2 ("Normal 3 (B)", 2D) = "bump" {}
        _splatTyle2("Tiling", Float) = 1
        
        [Space(25)]_Splat3 ("Terrain Splat 4 (A)", 2D) = "white" {}
        _Normal3 ("Normal 4 (A)", 2D) = "bump" {}
        _splatTyle3("Tiling", Float) = 1

        [Space(25)]_Metallic0 ("Metallic 0", Range(0.0, 1.0)) = 0.0  
        _Metallic1 ("Metallic 1", Range(0.0, 1.0)) = 0.0  
        _Metallic2 ("Metallic 2", Range(0.0, 1.0)) = 0.0  
        _Metallic3 ("Metallic 3", Range(0.0, 1.0)) = 0.0
        _Smoothness0 ("Smoothness 0", Range(0.0, 1.0)) = 0.0  
        _Smoothness1 ("Smoothness 1", Range(0.0, 1.0)) = 0.0  
        _Smoothness2 ("Smoothness 2", Range(0.0, 1.0)) = 0.0  
        _Smoothness3 ("Smoothness 3", Range(0.0, 1.0)) = 1.0

        [Space(25)][Header(Surface Textures)]
        [Space(10)]_MainTex ("Red Color Texture", 2D) = "white" {}
		_NormalMap ("Normalmap", 2D) = "bump" {}

        _greenTex ("Green Color Texture", 2D) = "white" {}
        _greenNormalMap ("Normalmap", 2D) = "bump" {}

        [Space(25)][Header(Tesselation)]

        [Space(10)] _Tess ("Level", Range(1,32)) = 4
		 _DispTex ("Disp Texture", 2D) = "gray" {}
		 _Displacement ("Displacement", Range(0, 5.0)) = 0.3


		 [Space(25)][Header(Additional Settings)]

         [Space(10)]_Color ("Color", color) = (1,1,1,0)
         _SpecColor ("Spec color", color) = (0.5,0.5,0.5,0.5)
        }
        SubShader {
            Tags { "RenderType"="Opaque" }
            LOD 300
            
            CGPROGRAM
      //      #pragma surface surf BlinnPhong addshadow fullforwardshadows vertex:disp tessellate:tessFixed nolightmap
            #pragma surface surf Standard fullforwardshadows vertex:disp tessellate:tessFixed
            #pragma target 5.0

            struct appdata {
                float4 vertex : POSITION;
                float4 tangent : TANGENT;
                float3 normal : NORMAL;
                float2 texcoord : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
                float4 color : Color;
            };

            float _Tess;

            float4 tessFixed()
            {
                return _Tess;
            }

            sampler2D _DispTex;
            float _Displacement, _splatTyle0, _splatTyle1, _splatTyle2, _splatTyle3;
            float _terrainSize;

            void disp (inout appdata v)
            {
            	float adj;
            	if(v.color.r > 0){
            	//	adj = 1 - v.color.r;
            		float al = v.color.r;
            		if(al < 0.9){
            			al = lerp(0, 1, al / 0.9);
            		}else{
            			al = 1;
            		}

 	           	 	adj = 1-al;
            	}else{
            		float al = v.color.a;
            		if(al < 0.3){
            			al = lerp(0, 1, al / 0.3);
            		}else{
            			al = 1;
            		}

 	           	 	adj = 1-al;
            	 }

                float d = tex2Dlod(_DispTex, float4(v.texcoord.xy * 1000,0,0)).r * _Displacement * adj;
                v.vertex.xyz += v.normal * d;
            }


        // Access the Shaderlab properties
        half _depthThresh;

        uniform sampler2D _Control;

        sampler2D _Splat0,_Splat1,_Splat2,_Splat3;
        sampler2D _Normal0,_Normal1,_Normal2,_Normal3;

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
            float2 uv_greenTex : TEXCOORD1;
       //     float2 uv_Splat1 : TEXCOORD1;
       //     float2 uv_Splat2 : TEXCOORD2; 

        float4 color : COLOR;

            INTERNAL_DATA
        };

            sampler2D _MainTex, _greenTex;
            sampler2D _NormalMap, _greenNormalMap;
            fixed4 _Color;

            half _Glossiness;
       		half _Metallic;

 			void surf (Input IN, inout SurfaceOutputStandard o) {
                half4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;

                // get terrain uv
      			half2 uv = {IN.worldPos.x / _terrainSize, IN.worldPos.z / _terrainSize};
      			half2 uv1 = {1-IN.worldPos.x / _terrainSize, 1-IN.worldPos.z / _terrainSize};

          //      half4 splatControl = tex2D(_Control, IN.uv_MainTex);
                half4 splatControl = tex2D(_Control, uv);

      			// multiply with tiling!
      			c = splatControl.r * tex2D(_Splat0, uv * _splatTyle0);
                c += splatControl.g * tex2D(_Splat1, uv * _splatTyle1);
      			c += splatControl.b * tex2D(_Splat2, uv * _splatTyle2);
      			c += splatControl.a * tex2D(_Splat3, uv * _splatTyle3);

      			c *= _Color;

      			// rock side
            float4 bc = IN.color;
            if(bc.a < 0.05){
            	bc.a = lerp(0, 1, bc.a / 0.05);
            }else{
            	bc.a = 1;
            }

            // one surface texture
       //     half4 albSurface = tex2D(_MainTex, IN.uv_MainTex);
       // 	half4 nSurface = tex2D(_NormalMap, IN.uv_MainTex);


            // two surface texture red and green channels
             if(bc.r > 0){
            	if(bc.r < 0.1){
            		bc.r = lerp(0, 1, bc.r / 0.1);
            	}else{
            		bc.r = 1;
            	}
            	bc.g = 1 - bc.r;
            }
            half4 albSurface = bc.r * tex2D(_MainTex, IN.uv_MainTex);
            albSurface += bc.g * tex2D(_greenTex, IN.uv_greenTex);

            half4 nSurface = bc.r * tex2D(_NormalMap, IN.uv_MainTex);
            nSurface += bc.g * tex2D(_greenNormalMap, IN.uv_greenTex);

            // alpha fade road texture
          //  half4 albRoad1 = lerp(alb, albRoad, albRoad.a);
            
			c = lerp(albSurface, c, bc.a);

      half4 nrm = 0.0f;
            nrm += splatControl.r * tex2D(_Normal0, uv * _splatTyle0);
            nrm += splatControl.g * tex2D(_Normal1, uv * _splatTyle1);
            nrm += splatControl.b * tex2D(_Normal2, uv * _splatTyle2);
            nrm += splatControl.a * tex2D(_Normal3, uv * _splatTyle3);

            nrm = lerp(nSurface, nrm, bc.a);

     //       nrm.g = nrm.r;

            fixed3 finalNormal = UnpackNormal(nrm);

                o.Albedo = c.rgb;
                o.Metallic = 0;
                o.Smoothness = 0;
                o.Normal = finalNormal;
            }
            ENDCG
        }
        FallBack "Diffuse"
    }