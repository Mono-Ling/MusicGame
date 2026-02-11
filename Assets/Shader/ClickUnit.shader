Shader "Unlit/ClickUnit"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _StartColor("Start Color",Color) = (1,1,1,1)
        _EndColor("End Color",Color) = (1,1,1,1)
        _Alpha("Alpha",Range(0,1)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "IgnoreProjector" = "True" "Queue" = "Transparent" }
        LOD 100

        Pass
        {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _StartColor;
            fixed4 _EndColor;
            float _Alpha;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 color = lerp(_StartColor,_EndColor,i.uv.y); //tex2D(_MainTex, i.uv);
                return fixed4(color.rgb,_Alpha);
            }
            ENDCG
        }
    }
}
