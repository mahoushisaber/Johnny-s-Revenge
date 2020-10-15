using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyShieldScript : MonoBehaviour
{
    public Text EnemyShieldText;
    public float CurrentShield;
    EnemyController Enemy;

    private void Start()
    {
        Enemy = FindObjectOfType<EnemyController>();
    }

    private void Update()
    {
        CurrentShield = Enemy.Shield;
        EnemyShieldText.text = "" + CurrentShield;
    }
}