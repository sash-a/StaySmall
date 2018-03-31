﻿using System;
using System.Collections.Generic;

/*
 * This class hold statistics for each type of gun
 * Bullet speed
 * Number of bullets per mouse click
 * Time till bullets are destroyed
 * Time between each shot
 */
public class Gun
{
    public const string SPEED = "speed";
    public const string NUM_BULLETS = "num";
    public const string BULLET_LIVE_TIME = "time";
    public const string FIRE_RATE = "rate";
    public const string RANGE = "range"; //TODO: work out how far 1 unit is

    public static Dictionary<string, float> machinegun = new Dictionary<string, float>
    {
        {SPEED, 5000},
        {NUM_BULLETS, 1},
        {BULLET_LIVE_TIME, 3},
        {FIRE_RATE, 0.2f},
        {RANGE, 50f}
    };

    public static Dictionary<string, float> shotgun = new Dictionary<string, float>
    {
        {SPEED, 3000},
        {NUM_BULLETS, 3},
        {BULLET_LIVE_TIME, 4},
        {FIRE_RATE, 1f},
        {RANGE, 8f}
    };

    public static Dictionary<string, float> pistol = new Dictionary<string, float>
    {
        {NUM_BULLETS, 1},
        {FIRE_RATE, 0.4f},
        {RANGE,50f}

    };
}