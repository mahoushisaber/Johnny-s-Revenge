using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Draggable.Slot typeOfItem = Draggable.Slot.UNIT;
    CardController Card;
    EnemyController Enemy;
    PlayerController Player;
    PlayerDeck PlayerDeck;
    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("OnPointerEnter");
        Card = FindObjectOfType<CardController>(); 
        if (eventData.pointerDrag == null)
            return; 
       
        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
        if (d != null)
        {
            d.placeholderParent = this.transform; 
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("OnPointerExit");
        if (eventData.pointerDrag == null)
            return;
        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
        if (d != null && d.placeholderParent == this.transform)
        {
            d.placeholderParent = d.parentToReturnTo;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if(eventData.pointerCurrentRaycast.gameObject.name == "Drop Zone")
        {
            Enemy = FindObjectOfType<EnemyController>();
            Player = FindObjectOfType<PlayerController>();
            PlayerDeck = FindObjectOfType<PlayerDeck>();

            if (Enemy.EnemyTurn)
            {
                Debug.Log("Cannot play cards during the enemy's turn");
                return;
            }

            string usedCard = eventData.pointerDrag.gameObject.GetComponent<CardController>().cardName;
            int cardPower = eventData.pointerDrag.gameObject.GetComponent<CardController>().power;

            Debug.Log("You used " + eventData.pointerDrag.gameObject.GetComponent<CardController>().cardName);

            Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
            if (d != null)
                if (typeOfItem == d.typeOfItem)
                {
                    d.parentToReturnTo = this.transform;
                }

            if (usedCard == "Slash")
            {
                Enemy.Health -= cardPower;
                Debug.Log("You dealt " + cardPower + " damage");
                Debug.Log("The enemy now has " + Enemy.Health + " health");
            } else if (usedCard == "Block")
            {
                Player.Shield += cardPower;
                Debug.Log("You shielded for " + cardPower);
            } else if (usedCard == "Siphon")
            {
                Enemy.Health -= cardPower;
                Player.Health += cardPower;
                Debug.Log("You dealt " + cardPower + " damage");
                Debug.Log("The enemy now has " + Enemy.Health + " health");
                Debug.Log("You healed for " + cardPower + " health");
            }
            Enemy.EnemyTurn = true;
            Destroy(eventData.pointerDrag.gameObject);
        } 
    }
}
