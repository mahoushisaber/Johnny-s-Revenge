using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthScript : MonoBehaviour
{
    private Image HealthBar;
    public float CurrentHealth;
    EnemyController Enemy;

    private void Start()
    {
        HealthBar = GetComponent<Image>();
        Enemy = FindObjectOfType<EnemyController>();
    }

    private void Update()
    {
        CurrentHealth = Enemy.Health;
        HealthBar.fillAmount = CurrentHealth / Enemy.MaxHealth;
    }
}