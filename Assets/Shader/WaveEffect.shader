Shader "Unlit/WaveEffect"
{
    Properties
    {
        _Color ("Color", Color) = (1, 1, 1, 1)
        _Frequency ("Frequency", Float) = 1
        _Speed ("Speed", Float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" "IgnoreProjector"="True" }
        LOD 100

        Pass
        {
            Blend One One
            ZWrite Off
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            fixed4 _Color;
            float _Frequency;
            float _Speed;

            v2f vert (appdata_img v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float d = distance(i.uv, float2(0.5, 0.5));
                float wave = 0.5 + 0.5 * sin(d * _Frequency - _Speed * _Time.y);
                fixed4 color = _Color;
                color.rgb *= wave;
                color.a *= wave;
                return color;//fixed4(wave, wave, wave, wave);
            }
            ENDCG
        }
    }
}
