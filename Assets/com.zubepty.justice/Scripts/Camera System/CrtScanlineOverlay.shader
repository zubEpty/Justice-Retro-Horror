Shader "Justice/UI/CRT Scanline Overlay"
{
    Properties
    {
        _Color ("Color", Color) = (0, 0, 0, 1)
        _ScanlineIntensity ("Scanline Intensity", Range(0, 1)) = 0.28
        _ScanlineCount ("Scanline Count", Range(120, 720)) = 360
        _ScanlineThickness ("Scanline Thickness", Range(0.05, 0.95)) = 0.42
        _VignetteIntensity ("Vignette Intensity", Range(0, 1)) = 0.22
        _NoiseIntensity ("Noise Intensity", Range(0, 0.2)) = 0.025
        _ScrollSpeed ("Scroll Speed", Range(-2, 2)) = 0.18
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest Always
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            fixed4 _Color;
            float _ScanlineIntensity;
            float _ScanlineCount;
            float _ScanlineThickness;
            float _VignetteIntensity;
            float _NoiseIntensity;
            float _ScrollSpeed;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color;
                return o;
            }

            float Hash(float2 p)
            {
                return frac(sin(dot(p, float2(12.9898, 78.233))) * 43758.5453);
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float movingLine = frac(i.uv.y * _ScanlineCount + _Time.y * _ScrollSpeed);
                float darkLine = 1.0 - smoothstep(_ScanlineThickness, _ScanlineThickness + 0.08, movingLine);

                float2 centeredUv = i.uv * 2.0 - 1.0;
                float vignette = saturate(dot(centeredUv, centeredUv) * _VignetteIntensity);

                float noise = Hash(i.uv * _ScreenParams.xy + _Time.y) * _NoiseIntensity;
                float alpha = saturate(darkLine * _ScanlineIntensity + vignette + noise);

                fixed4 col = _Color * i.color;
                col.a *= alpha;
                return col;
            }
            ENDCG
        }
    }
}
