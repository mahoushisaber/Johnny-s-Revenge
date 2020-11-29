﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaScript : MonoBehaviour
{
    private Image ManaBar;
    public Text ManaText;
    public float CurrentMana;
    PlayerController Player;

    private void Start()
    {
        ManaBar = GetComponent<Image>();
        Player = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        CurrentMana = Mathf.Round(Player.Mana);
        ManaBar.fillAmount = Player.Mana / Player.MaxMana;
        ManaText.text = "" + CurrentMana + "/" + Player.MaxMana;
    }
}
