
// MoveState as byte for minimal bandwidth (otherwise it's int by default)
// note: distinction between WALKING and RUNNING in case we need to know the
//       difference somewhere (e.g. for endurance recovery)
public enum MoveState : byte
{
    IDLE,
    WALKING,
    RUNNING,
    CROUCHING,
    CRAWLING,
    AIRBORNE,
    CLIMBING,
    SWIMMING
}