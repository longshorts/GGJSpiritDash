Shader "Domination/Shrine"
{
    Properties
    {
        _MainTex ("Base Texture", 2D) = "white" {}
        _IconTex ("Icon Texture", 2D) = "white" {}
        _IconColour("Icon Progress", Color) = (1,0,0,0)
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
            	constantColor[_IconColour]
                combine constant lerp(texture) previous
            }
            
        }
    }
}