Shader "Unlit/InfiniteGradient"
{
    Properties
    {
        _ColorTop ("Top Color", Color) = (0.1, 0.5, 1, 1)
        _ColorBottom ("Bottom Color", Color) = (0, 0, 0.5, 1)
    }
    SubShader
    {
        Tags { "Queue" = "Background" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            fixed4 _ColorTop;
            fixed4 _ColorBottom;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Используем координаты экрана для создания градиента
                float gradient = i.pos.y / _ScreenParams.y;
                return lerp(_ColorBottom, _ColorTop, gradient);
            }
            ENDCG
        }
    }
}
