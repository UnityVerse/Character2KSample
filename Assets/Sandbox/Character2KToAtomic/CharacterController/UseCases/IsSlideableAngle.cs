namespace Sandbox
{
    public static partial class Character2KUseCases
    {
        public static bool IsSlideableAngle(float slopeAngle, float slopeLimit)
        {
            // needs to be between slopeLimit (to start sliding) and maxSlide
            // (to stop sliding)
            return slopeLimit <= slopeAngle && slopeAngle < Character2KData.k_MaxSlideAngle;
        }
    }
}