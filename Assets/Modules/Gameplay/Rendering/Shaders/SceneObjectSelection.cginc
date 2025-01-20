#include "SceneObjectConsts.cginc"
#include "Common.cginc"

fixed4 draw_object_selection(
    const float3 position,
    const float4 center,
    const float radius,
    const float thickness,
    const fixed4 base_color
)
{
    const float distance = length(position.xz - center.xz);
    const float half_thickness = thickness / 2;
    const float max_radius = radius + half_thickness;

    //Если попали за окружность, то прозрачный цвет
    if (distance > max_radius)
    {
        return fixed4(0, 0, 0, 0);
    }

    //Если попали на окружность
    if (distance >= radius && distance <= max_radius)
    {
        const float offset = distance - radius;
        const float t = 1 - offset / half_thickness;
        return base_color * t;
    }

    //Если попали внутрь окружности в вырезку 
    const float half_radius = radius / 2;
    if (distance < half_radius)
    {
        return fixed4(0, 0, 0, 0);
    }

    //Иначе попали во внутр. область окружности (не в вырезку) 
    const float t = (distance - half_radius) / half_radius;
    return base_color * t;
}

fixed4 draw_objects_selection(
    const int object_count,
    const float3 position,
    const float4 centers[SCENE_OBJECT_BUFFER_SIZE],
    const float radiuses[SCENE_OBJECT_BUFFER_SIZE],
    const float thickness,
    const fixed4 base_color
)
{
    fixed4 result_color = fixed4(0, 0, 0, 0);

    for (int i = 0; i < object_count; i++)
    {
        const float4 center = centers[i];
        const float radius = radiuses[i];

        result_color += draw_object_selection(position, center, radius, thickness, base_color);
        result_color = clamp_color(result_color, base_color);
    }

    return result_color;
}
