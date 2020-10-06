using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float Health = 100f;
    public float Shield = 0;
    public float Mana = 100f;
    public float MaxHealth = 100f;
    public float MaxMana = 100f;
    public int cardsInDeck;
    public int cardsInHand;
    public int CurrentStage = 1;
    public Text StageText;
    public GameObject Hand;

    PlayerDeck PlayerDeck;
    void Start()
    {
        Hand = GameObject.Find("Hand");
        PlayerDeck = FindObjectOfType<PlayerDeck>();
        Health = MaxHealth;
        Mana = MaxMana;
        cardsInDeck = PlayerDeck.deck.Count;
        CurrentStage = 1;
    }

    void Update()
    {
        cardsInHand = Hand.transform.childCount;
        cardsInDeck = PlayerDeck.deck.Count;
        StageText.text = "Stage: " + CurrentStage.ToString();
    }
}
