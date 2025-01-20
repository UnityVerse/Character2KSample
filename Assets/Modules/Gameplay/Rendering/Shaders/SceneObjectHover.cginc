#include "SceneObjectConsts.cginc"
#include "Common.cginc"

fixed4 draw_object_hover(
    const float3 position,
    const float4 center,
    float radius,
    const float thickness,
    const fixed4 color,
    const int dash
)
{
    const float2 delta = position.xz - center.xz;
    const float distance = length(delta);
    radius *= 1.1f;

    //Если попали не на окружность, то прозрачный цвет
    if (distance > radius + thickness || distance < radius)
    {
        return fixed4(0, 0, 0, 0);
    }

    //Считаем угол относительно окружности
    const float angle_radians = atan2(delta.y, delta.x) + _Time.y;
    const int angle_degrees = angle_radians * 57.3;

    //Проверяем, рисовать ли точку или нет
    const bool draw_required = angle_degrees % dash == angle_degrees % (dash * 2);
    return draw_required ? color : fixed4(0, 0, 0, 0);
}


fixed4 draw_objects_hover(
    const int object_count,
    const float3 position,
    const float4 centers[SCENE_OBJECT_BUFFER_SIZE],
    const float radiuses[SCENE_OBJECT_BUFFER_SIZE],
    const float thickness,
    const fixed4 color,
    const float dash
)
{
    fixed4 result_color = fixed4(0, 0, 0, 0);

    for (int i = 0; i < object_count; i++)
    {
        const float4 center = centers[i];
        const float radius = radiuses[i];

        result_color += draw_object_hover(position, center, radius, thickness, color, dash);
        result_color = clamp_color(result_color, color);
    }

    return result_color;
}
