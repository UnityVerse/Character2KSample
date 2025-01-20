Shader "StarKRE/SceneObjectHover"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _Thickness ("Thickness", Range(0, 0.5)) = 0.25
        _Dash ("Dash", Range(0, 360)) = 14
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
            "Queue" = "Transparent"
            "Queue" = "Overlay"
        }
        
        Blend SrcAlpha OneMinusSrcAlpha
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "SceneObjectHover.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 worldPos : TEXCOORD1;
            };

            int _Number;
            float4 _Centers[SCENE_OBJECT_BUFFER_SIZE];
            float _Radiuses[SCENE_OBJECT_BUFFER_SIZE];
            
            int _Dash;
            float _Thickness;
            fixed4 _Color;
            
            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                return o;
            }

            fixed4 frag(v2f data) : SV_Target
            {
                const float3 position = data.worldPos.xyz;
                return draw_objects_hover(_Number, position, _Centers, _Radiuses, _Thickness, _Color, _Dash);
            }
            
            ENDCG
        }
    }
}