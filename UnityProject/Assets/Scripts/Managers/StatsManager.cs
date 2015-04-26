﻿using System.Collections;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    // Time
    public float timePlayed { get; set; }

    // Attack
    public float damageDealt { get; set; }

    public float damageTaken { get; set; }

    // Enemies
    public int enemiesKilledTotal { get; set; }

    // Arrows

    public int shotsFired { get; set; }

    public int shotsHit { get; set; }

    public int accuracy
    {
        get { return (shotsHit * 100) / shotsFired; }
    }

    public int arrowsRedirected { get; set; }

    public int arrowsBoosted { get; set; }

    // Use this for initialization
    private void Start()
    {
        timePlayed = damageDealt = damageTaken = 0.0f;
        enemiesKilledTotal = shotsFired = arrowsRedirected = arrowsBoosted = shotsHit = 0;
    }
}