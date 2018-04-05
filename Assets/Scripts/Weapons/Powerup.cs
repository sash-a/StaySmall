using System.Collections.Generic;

public class Powerup
{
    public const string DOUBLE_DAMAGE = "doubleDmg";
    public const string DOUBLE_FIRE_RATE = "doubleFireRate";
    public const string INFINITE_AMMO = "InfiniteAmmo";
    public const string WALLFAZE = "WallFaze";
    public const string ZOOM_OUT = "ZoomOut";

    // Maps power up names to timer
    public static Dictionary<string, float> powerUps = new Dictionary<string, float>
    {
        {DOUBLE_DAMAGE, 10},
        {DOUBLE_FIRE_RATE, 10},
        {INFINITE_AMMO, 10}, // 1 = true
        {WALLFAZE, 2},
        {ZOOM_OUT, 1.5f}
    };
}