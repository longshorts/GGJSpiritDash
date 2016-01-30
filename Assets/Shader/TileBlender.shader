Shader "Domination/TileBlender"
 {
     Properties 
     {
        _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {} 
        _BlendTex ("Blend (RGB)", 2D) = "white"
        _BlendAmount ("Blend Alpha", float) = 0
     }
     
     SubShader 
     {
        Tags { "Queue"="Geometry-9" "IgnoreProjector"="True" "RenderType"="Transparent" }
        Lighting Off
        LOD 200
        Blend SrcAlpha OneMinusSrcAlpha
  
        CGPROGRAM
        #pragma surface surf Lambert
  
        sampler2D _MainTex;
        sampler2D _BlendTex;
        float _BlendAmount;
  
        struct Input
       	{
          float2 uv_MainTex;
        };
  
        void surf (Input IN, inout SurfaceOutput output)
        {
          fixed4 finalColour = ( ( 1 - _BlendAmount ) * tex2D( _MainTex, IN.uv_MainTex ) + _BlendAmount * tex2D( _BlendTex, IN.uv_MainTex ) );
          output.Albedo = finalColour.rgb;
          output.Alpha = finalColour.a;
        }
        ENDCG
     }
  
     Fallback "Transparent/VertexLit"
 }