using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthScript : MonoBehaviour
{
    public Text EnemyHealthText;
    public GameObject SheildControl;
    public float CurrentHealth;
    public float CurrentShield;

    private EnemyController Enemy;
    private Image HealthBar;

    private void Start()
    {
        HealthBar = GetComponent<Image>();
        Enemy = FindObjectOfType<EnemyController>();
    }

    private void Update()
    {
        CurrentHealth = Enemy.Health;
        CurrentShield = Enemy.Shield;
        HealthBar.fillAmount = CurrentHealth / Enemy.MaxHealth;
        EnemyHealthText.text = string.Format("{0}/{1}", CurrentHealth, Enemy.MaxHealth);
        SheildControl.GetComponent<ShieldScript>().CurrentShield = CurrentShield;
    }
}