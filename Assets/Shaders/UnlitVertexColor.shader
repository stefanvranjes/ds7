Shader "Custom/UnlitVertexColor"
{
    Properties
    {
        _Color ("Main Color", Color) = (1,1,1,1)
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
            #pragma target 2.0
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            fixed4 _Color;

            struct appdata_t {
                float4 vertex : POSITION;
                float4 color : COLOR; // Vertex color data
            };

            struct v2f {
                float4 vertex : SV_POSITION;
                float4 color : COLOR; // Pass color to fragment shader
                UNITY_FOG_COORDS(0)
            };

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.color = v.color; // Pass through vertex color
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Return vertex color directly
                fixed4 col = i.color * _Color;
                
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
