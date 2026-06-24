Shader "Custom/SpritePulse"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}

        _OutlineColor ("Outline Color", Color) = (1, 1, 1, 1)
        _OutlineWidth ("Outline Width (pixels)", Range(1, 8)) = 1

        _PulseSpeed ("Pulse Speed", Float) = 5
        _MinAlpha ("Minimum Alpha", Range(0, 1)) = 0
        _MaxAlpha ("Maximum Alpha", Range(0, 1)) = 1
        _AlphaCutoff ("Sprite Alpha Cutoff", Range(0, 1)) = 0.1
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

        Cull Off
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;

            fixed4 _OutlineColor;
            float _OutlineWidth;
            float _PulseSpeed;
            float _MinAlpha;
            float _MaxAlpha;
            float _AlphaCutoff;

            struct appdata_t
            {
                float4 vertex : POSITION;
                fixed4 color : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.texcoord;
                o.color = v.color;
                return o;
            }

            float SampleAlpha(float2 uv)
            {
                return tex2D(_MainTex, uv).a;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float centerAlpha = SampleAlpha(i.texcoord);

                // Do not draw over the actual sprite.
                if (centerAlpha > _AlphaCutoff)
                    discard;

                float2 stepUV = _MainTex_TexelSize.xy * _OutlineWidth;

                float nearbyAlpha = 0;

                nearbyAlpha = max(nearbyAlpha, SampleAlpha(i.texcoord + float2( stepUV.x, 0)));
                nearbyAlpha = max(nearbyAlpha, SampleAlpha(i.texcoord + float2(-stepUV.x, 0)));
                nearbyAlpha = max(nearbyAlpha, SampleAlpha(i.texcoord + float2(0,  stepUV.y)));
                nearbyAlpha = max(nearbyAlpha, SampleAlpha(i.texcoord + float2(0, -stepUV.y)));

                nearbyAlpha = max(nearbyAlpha, SampleAlpha(i.texcoord + float2( stepUV.x,  stepUV.y)));
                nearbyAlpha = max(nearbyAlpha, SampleAlpha(i.texcoord + float2(-stepUV.x,  stepUV.y)));
                nearbyAlpha = max(nearbyAlpha, SampleAlpha(i.texcoord + float2( stepUV.x, -stepUV.y)));
                nearbyAlpha = max(nearbyAlpha, SampleAlpha(i.texcoord + float2(-stepUV.x, -stepUV.y)));

                if (nearbyAlpha <= _AlphaCutoff)
                    discard;

                // 0 → 1 → 0, continuously.
                float heartbeat = (sin(_Time.y * _PulseSpeed) + 1.0) * 0.5;
                float pulseAlpha = lerp(_MinAlpha, _MaxAlpha, heartbeat);

                fixed4 result = _OutlineColor;
                result.a *= pulseAlpha * i.color.a;

                return result;
            }
            ENDCG
        }
    }
}