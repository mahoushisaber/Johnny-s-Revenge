using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Draggable.Slot typeOfItem = Draggable.Slot.UNIT;
    public GameObject Hand; 
    CardController Card;
    EnemyController Enemy;
    PlayerController Player;
    PlayerDeck PlayerDeck;

    public void Start()
    {
        Hand = GameObject.Find("Hand");
    }
    
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

            Debug.Log("You used " + eventData.pointerDrag.gameObject.GetComponent<CardController>().cardName);

            Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
            if (d != null)
                if (typeOfItem == d.typeOfItem)
                {
                    d.parentToReturnTo = this.transform;
                }

            string cardToUse = eventData.pointerDrag.gameObject.GetComponent<CardController>().cardName;
            int cardPower = eventData.pointerDrag.gameObject.GetComponent<CardController>().power;
            Card.PlayCard(cardToUse, cardPower); 

            foreach (Transform t in Hand.transform)
            {
                Debug.Log(t.name); 
                if (t.name == "New Game Object")
                {
                    Destroy(t.gameObject);
                    Debug.Log("REMOVED EMPTY GAME OBJECT");
                }
            }
            Enemy.EnemyTurn = true;
            Destroy(eventData.pointerDrag.gameObject);
        } 
    }
}
