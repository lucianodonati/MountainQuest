﻿using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float currentHP = 100.0f, maxHP = 100.0f;
    public bool showHealthBar = true;
    public HealthBar hBar;

    // Use this for initialization
    private void Start()
    {
        hBar = new HealthBar();
    }

    public void TakeDamage(float _damage)
    {
        if (_damage >= 0)
        {
            currentHP -= _damage;
            if (currentHP < 0)
                currentHP = 0;
        }
        else
            Debug.LogWarning(gameObject.name + ": Trying to deal negative damage (Use heal function).");
    }

    public void Heal(float _hp)
    {
        if (_hp >= 0)
        {
            currentHP += _hp;
            if (currentHP > maxHP)
                currentHP = maxHP;
        }
        else
            Debug.LogWarning(gameObject.name + ": Trying to heal negative hp (Use TakeDamage function).");
    }

    private void LateUpdate()
    {
        hBar.gameObject.SetActive(showHealthBar);
    }
}