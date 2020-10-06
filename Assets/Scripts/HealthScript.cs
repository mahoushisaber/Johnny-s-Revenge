using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour
{
    private Image HealthBar;
    public float CurrentHealth;
    PlayerController Player;

    private void Start()
    {
        HealthBar = GetComponent<Image>();
        Player = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        CurrentHealth = Player.Health;
        HealthBar.fillAmount = CurrentHealth / Player.MaxHealth;
    }
}
