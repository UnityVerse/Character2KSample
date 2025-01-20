#include <HLSLSupport.cginc>

fixed4 clamp_color(fixed4 source, fixed4 target)
{
    return fixed4(
        clamp(source.x, 0, target.x),
        clamp(source.y, 0, target.y),
        clamp(source.z, 0, target.z),
        clamp(source.w, 0, target.w)
    );
}
