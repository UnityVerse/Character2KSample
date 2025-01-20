namespace Controller2k
{
    // sliding starts after a delay, and stops after a delay. we need an enum.
    public enum SlidingState : byte
    {
        NONE,
        STARTING,
        SLIDING,
        STOPPING
    };
}