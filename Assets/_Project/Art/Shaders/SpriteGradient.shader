Shader "Unlit/SpriteGradient"
{
    Properties
    {
        [HideInInspector] [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        [HideInInspector] _Color ("Tint", Color) = (1,1,1,1)
        
        _Color1 ("Color 1", Color) = (1,0,0,1)
        _Color2 ("Color 2", Color) = (0,0,1,1)
        _GradientRotation ("Gradient Rotation", Range(0, 360)) = 0
        
        _RectMin ("Rect Min (internal)", Vector) = (-50, -50, 0, 0)
        _RectMax ("Rect Max (internal)", Vector) = (50, 50, 0, 0)
        
        [HideInInspector] _StencilComp ("Stencil Comparison", Float) = 8
        [HideInInspector] _Stencil ("Stencil ID", Float) = 0
        [HideInInspector] _StencilOp ("Stencil Operation", Float) = 0
        [HideInInspector] _StencilWriteMask ("Stencil Write Mask", Float) = 255
        [HideInInspector] _StencilReadMask ("Stencil Read Mask", Float) = 255
        [HideInInspector] _ColorMask ("Color Mask", Float) = 15
        [HideInInspector] [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            #pragma multi_compile_local _ UNITY_UI_CLIP_RECT
            #pragma multi_compile_local _ UNITY_UI_ALPHACLIP

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                float4 worldPosition : TEXCOORD1;
                float2 objectPos : TEXCOORD2;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex;
            fixed4 _Color;
            fixed4 _Color1;
            fixed4 _Color2;
            float _GradientRotation;
            float2 _RectMin;
            float2 _RectMax;
            
            float4 _MainTex_ST;
            float4 _ClipRect;

            v2f vert(appdata_t v)
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                
                OUT.worldPosition = v.vertex;
                OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);
                OUT.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                OUT.color = v.color * _Color;
                
                OUT.objectPos = v.vertex.xy;
                
                return OUT;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                float2 normalizedPos;
                normalizedPos.x = (IN.objectPos.x - _RectMin.x) / (_RectMax.x - _RectMin.x);
                normalizedPos.y = (IN.objectPos.y - _RectMin.y) / (_RectMax.y - _RectMin.y);
                
                float angleRad = radians(_GradientRotation);
                float cosAngle = cos(angleRad);
                float sinAngle = sin(angleRad);
                
                float2 centeredPos = normalizedPos - 0.5;
                float2 rotatedPos;
                rotatedPos.x = centeredPos.x * cosAngle - centeredPos.y * sinAngle;
                rotatedPos.y = centeredPos.x * sinAngle + centeredPos.y * cosAngle;
                rotatedPos += 0.5;
                
                float gradientValue = saturate(rotatedPos.x);
                half4 gradientColor = lerp(_Color1, _Color2, gradientValue);
                
                half4 texColor = tex2D(_MainTex, IN.texcoord);
                half4 color = gradientColor;
                color.a *= texColor.a; 
                color *= IN.color;
                
                #ifdef UNITY_UI_CLIP_RECT
                color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
                #endif

                #ifdef UNITY_UI_ALPHACLIP
                clip(color.a - 0.001);
                #endif

                return color;
            }
            ENDCG
        }
    }
}