﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Draggable.Slot typeOfItem = Draggable.Slot.BATTLE;

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("OnPointerEnter");
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
        if(eventData.pointerCurrentRaycast.gameObject.name == "Battle Drop Zone")
        {
            Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
            if (d != null)
                if (d.typeOfItem == Draggable.Slot.ANY || typeOfItem == d.typeOfItem)
                {
                    GameController GCtrl = FindObjectOfType<GameController>();

                    if (GCtrl != null && GCtrl.CanDropPlayerCard(typeOfItem))
                    {
                        d.parentToReturnTo = this.transform;

                        Debug.Log("You used " + eventData.pointerDrag.gameObject.GetComponent<CardController>().CardName);
                    }
                }
        }
    }
}
