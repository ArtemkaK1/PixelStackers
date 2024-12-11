Shader "Unlit/BackgroundGradient"
{
    Properties
    {
        _TopColor ("Top Color", Color) = (0, 0.85, 1, 1)
        _BottomColor ("Bottom Color", Color) = (0, 0.45, 1, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

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
                float4 vertex : SV_POSITION;
                float4 worldPos : TEXCOORD0;
            };

            fixed4 _TopColor;
            fixed4 _BottomColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float t = saturate(i.worldPos.y / 10.0);
                return lerp(_BottomColor, _TopColor, t);
            }
            ENDCG
        }
    }
}
