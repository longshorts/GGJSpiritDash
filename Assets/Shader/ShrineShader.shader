Shader "Domination/Shrine"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _IconTex ("Alpha Blended (RGBA) ", 2D) = "white" {}
    }
    SubShader
    {
        Pass
        {
            // Apply base texture
            SetTexture [_MainTex]
            {
                combine texture
            }
            
            // Blend in the alpha texture using the lerp operator
            SetTexture [_IconTex]
            {
                combine texture lerp (texture) previous
            }
        }
    }
}