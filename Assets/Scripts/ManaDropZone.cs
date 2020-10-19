using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ManaDropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Draggable.Slot typeOfSlot;
    public GameObject BattleZone;

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
        if (eventData.pointerCurrentRaycast.gameObject.name == "Player Mana Drop Zone")
        {
            Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
            if (d != null)
                if (d.typeOfItem == Draggable.Slot.ANY || typeOfSlot == d.typeOfItem)
                {
                    GameController GCtrl = FindObjectOfType<GameController>();

                    if (GCtrl == null || !GCtrl.CanDropPlayerCard(typeOfSlot))
                    {
                        Debug.Log("Cannot drop on Mana right now");
                        return;
                    }

                    eventData.pointerDrag.gameObject.GetComponent<CardController>().Enhanced = true;

                    // Transfer to the battle zone instead of mana dock
                    if (BattleZone != null)
                    {
                        d.parentToReturnTo = BattleZone.transform;

                        Debug.Log("You used " + eventData.pointerDrag.gameObject.GetComponent<CardController>().Name);
                    }
                    else
                    {
                        Debug.Log("ManaDropZone: BattleZone is not set! Cannot move card.");
                    }
                }
        }
    }
}
