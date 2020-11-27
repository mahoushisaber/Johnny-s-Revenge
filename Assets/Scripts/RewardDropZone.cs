using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RewardDropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Draggable.Slot typeOfSlot;

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
        if (eventData.pointerCurrentRaycast.gameObject.name == "Reward Drop Zone")
        {
            Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
            if (d != null)
                if (d.typeOfItem == Draggable.Slot.ANY || typeOfSlot == d.typeOfItem)
                {
                    d.parentToReturnTo = this.transform;
                    Debug.Log("Reward used " + eventData.pointerDrag.gameObject.GetComponent<CardController>().Name);
                }
        }
    }
}
