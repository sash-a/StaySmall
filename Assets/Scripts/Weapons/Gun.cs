﻿using System.Collections.Generic;

/*
 * This class hold statistics for each type of gun
 * Bullet speed
 * Number of bullets per mouse click
 * Time till bullets are destroyed
 * Time between each shot
 */
public class Gun
{
    public const string NUM_BULLETS = "num";
    public const string FIRE_RATE = "rate";
    public const string RANGE = "range";
    public const string DAMAGE = "damage";
    public const string BULLET_SPEED = "b_speed";

    public static float damageMod = 1;
    public static float fireRateMod = 1;

    public static Dictionary<string, float> machinegun = new Dictionary<string, float>
    {
        {NUM_BULLETS, 1},
        {FIRE_RATE, 0.2f},
        {RANGE, 50f},
        {BULLET_SPEED, 3000},
        {DAMAGE, 40}
    };

    public static Dictionary<string, float> shotgun = new Dictionary<string, float>
    {
        {NUM_BULLETS, 3},
        {FIRE_RATE, 1f},
        {RANGE, 8f},
        {BULLET_SPEED, 2000},
        {DAMAGE, 60}
    };

    public static Dictionary<string, float> pistol = new Dictionary<string, float>
    {
        {NUM_BULLETS, 1},
        {FIRE_RATE, 0.4f},
        {RANGE, 50f},
        {BULLET_SPEED, 1500},
        {DAMAGE, 20}
    };
}