Shader "Custom/GridLines" {
    Properties {
        _GridColor ("Grid Color", Color) = (1,1,1,1)
        _GridSize ("Grid Size", Range(0.001,1)) = 0.1
        _GridLineThickness ("Grid Line Thickness", Range(0,1)) = 0.01
        _FadeRadius ("Fade Radius", Range(0,1)) = 0.1
    }
    SubShader {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

        Pass {
            Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float4 _GridColor;
            float _GridSize;
            float _GridLineThickness;
            float4 _MousePosition;
            float _FadeRadius;

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float GridTest(float2 r) {
                float result;

                for (float i = 0.0; i <= 1; i += _GridSize) {
                    for (int j = 0; j < 2; j++) {
                        result += 1.0 - smoothstep(0, _GridLineThickness, abs(r[j] - i));
                    }
                }

                return result;
            }

            fixed4 frag(v2f i) : SV_Target {
                // return gridColor;
                float gridTestResult = GridTest(i.uv);
                fixed4 gridColor = (_GridColor * step(0.5, gridTestResult));

                // Calculate the distance from the current pixel to the mouse position
                float dist = distance(i.uv, _MousePosition.xz);

                // Adjust the smoothstep function to create a circular fading effect
                gridColor.a *= 1.0 - smoothstep(0.0, _FadeRadius, dist);

                return gridColor;
            }
            ENDCG
        }
    }
}