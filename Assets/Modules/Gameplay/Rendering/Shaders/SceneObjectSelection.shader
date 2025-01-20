Shader "StarKRE/SceneObjectSelection"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _Thickness ("Thickness", Range(0, 0.5)) = 0.2
    }
    SubShader
    {
        Tags
        {
            "RenderType" = "Opaque"
            "Queue" = "Transparent"
            "Queue" = "Overlay"
        }

        Blend SrcAlpha OneMinusSrcAlpha
        LOD 100

        Pass
        {
            CGPROGRAM
// Upgrade NOTE: excluded shader from DX11, OpenGL ES 2.0 because it uses unsized arrays
#pragma exclude_renderers d3d11 gles
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "SceneObjectSelection.cginc"
            #include "SceneObjectConsts.cginc"

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
            float4 _Centers[SCENE_OBJECT_BUFFER_SIZE]; //Соединить позиции и радиус
            float _Radiuses[SCENE_OBJECT_BUFFER_SIZE];
            
            float _Thickness;
            fixed4 _Color;

            v2f vert(appdata input)
            {
                v2f output;
                output.vertex = UnityObjectToClipPos(input.vertex);
                output.worldPos = mul(unity_ObjectToWorld, input.vertex);
                return output;
            }

            fixed4 frag(v2f data) : SV_Target
            {
                const float3 position = data.worldPos.xyz;
                return draw_objects_selection(_Number, position, _Centers, _Radiuses, _Thickness, _Color);
            }
            
            ENDCG
        }
    }
}