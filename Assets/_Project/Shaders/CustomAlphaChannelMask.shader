Shader "Custom/AlphaChannelMask" 
{
    Properties 
    {
        _MainTex ("Texture (RGB) + Mask (A)", 2D) = "white" {}
        _BackgroundColor ("Background Color", Color) = (0,0,1,1)
    }

    SubShader 
    {
        Tags { "RenderType"="Opaque" }
        
        CGPROGRAM
        #pragma surface surf Standard
        
        sampler2D _MainTex;
        fixed4 _BackgroundColor;
        
        struct Input { float2 uv_MainTex; };
        
        void surf (Input IN, inout SurfaceOutputStandard o) 
        {
            fixed4 texData = tex2D(_MainTex, IN.uv_MainTex);
            fixed3 color = texData.rgb;
            fixed mask = texData.a;
            
            o.Albedo = lerp(_BackgroundColor.rgb, color, mask);
            o.Alpha = 1.0;
        }
        ENDCG
    }
}