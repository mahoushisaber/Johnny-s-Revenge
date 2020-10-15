using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthScript : MonoBehaviour
{
    public Text PlayerHealthText;
    public GameObject SheildControl;
    public float CurrentHealth;
    public float CurrentShield;

    private PlayerController Player;
    private ShieldScript SheildCtrl;
    private Image HealthBar;

    private void Start()
    {
        HealthBar = GetComponent<Image>();
        Player = FindObjectOfType<PlayerController>();
        SheildCtrl = FindObjectOfType<ShieldScript>();
    }

    private void Update()
    {
        CurrentHealth = Player.Health;
        CurrentShield = Player.Shield;
        HealthBar.fillAmount = CurrentHealth / Player.MaxHealth;
        PlayerHealthText.text = string.Format("{0}/{1}", CurrentHealth, Player.MaxHealth);
        SheildControl.GetComponent<ShieldScript>().CurrentShield = CurrentShield;
    }
}
