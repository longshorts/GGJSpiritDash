﻿Shader "Custom/OreShader" {
      Properties {
          _Color ("Color", Color) = (1,1,1,1)
          _MainTex ("Main Texture Color (RGB)", 2D) = "white" {}
          _IconTex ("Overlay Texture Color (RGB) Alpha (A)", 2D) = "white" {}
          _Glossiness ("Smoothness", Range(0,1)) = 0.5
          _Metallic ("Metallic", Range(0,1)) = 0.0
      }
      SubShader {
          Tags { "RenderType"="Transparent"}
 
          LOD 200
          CGPROGRAM
          // Physically based Standard lighting model, and enable shadows on all light types
          #pragma surface surf Standard alpha fullforwardshadows
  
          // Use shader model 3.0 target, to get nicer looking lighting
          #pragma target 3.0
  
          sampler2D _MainTex;
          sampler2D _SecondaryTex;
  
          struct Input {
              float2 uv_MainTex;
              //float2 uv_SecondTex;
          };
  
          half _Glossiness;
          half _Metallic;
          fixed4 _Color;
  
          void surf (Input IN, inout SurfaceOutputStandard o) {
              // Albedo comes from a texture tinted by colora
              float4 mainTex = tex2D (_MainTex, IN.uv_MainTex);
              float4 overlayTex = tex2D (_SecondaryTex, IN.uv_MainTex);
         half3 mainTexVisible = mainTex.rgb * (1-overlayTex.a);
         half3 overlayTexVisible = overlayTex.rgb * (overlayTex.a);
             
         float3 finalColor = (mainTexVisible + overlayTexVisible);
              
              o.Albedo = finalColor.rgb;
              // Metallic and smoothness come from slider variables
              o.Metallic = _Metallic;
              o.Smoothness = _Glossiness;
              o.Alpha = 0.5 * ( mainTex.a + overlayTex.a );
          }
          ENDCG
      } 
      FallBack "Diffuse"
  }