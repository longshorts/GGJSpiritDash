Shader "Domination/Shrine"
{
    Properties
    {
        _IconTex ("Icon Texture", 2D) = "white" {}
        _IconColour("Icon Progress", Color) = (1,0,0,0)
    }
    SubShader
    {
    
        Pass
        {       
            AlphaTest Greater 0.5
            
            // Blend in the alpha texture using the lerp operator
            SetTexture [_IconTex]
            {
                constantColor [_IconColour]
                combine constant lerp(texture) previous
            }
            
            // Multiply in texture
            SetTexture [_IconTex] 
            {
                combine previous * texture
            }
        }
    }
}