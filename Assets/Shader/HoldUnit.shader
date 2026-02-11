Shader "Unlit/HoldUnit"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _HoldProgress("Hold Progress",Range(0,1)) = 1
        _StartColor("Start Color",Color) = (1,1,1,1)
        _EndColor("End Color",Color) = (1,1,1,1)
        _Alpha("Alpha",Range(0,1)) = 1
        _EdgeStartColor("Edge Start Color",Color) = (1,1,1,1)
        _EdgeEndColor("Edge End Color",Color) = (1,1,1,1)
        _EdgeThreshold("Edge Threshold",Range(0,1)) = 0
        _EdgePower("Edge Power",Range(0,1)) = 0
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
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _HoldProgress;
            fixed4 _StartColor;
            fixed4 _EndColor;
            float _Alpha;
            fixed4 _EdgeStartColor;
            fixed4 _EdgeEndColor;
            float _EdgeThreshold;
            float _EdgePower;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                i.uv.y += 1 - _HoldProgress;
                i.uv.y = frac(i.uv.y);
                float clipThreshold = _HoldProgress - uv.y;
                clip(clipThreshold);
                //±ßÔµÌØÐ§
                float edgeWeight = step(0,_EdgeThreshold - uv.y );
                edgeWeight *= _EdgePower;
                edgeWeight = 1 - edgeWeight;
                float edgeColorBlend = smoothstep(0,_EdgeThreshold,uv.y);
                fixed3 edgeColor = lerp( _EdgeStartColor,_EdgeEndColor,edgeColorBlend);
                // sample the texture
                fixed4 color = lerp(_StartColor,_EndColor,i.uv.y); //tex2D(_MainTex, i.uv);
                color.rgb = lerp(edgeColor,color.rgb,edgeWeight);
                return fixed4(color.rgb,_Alpha);
            }
            ENDCG
        }
    }
}
