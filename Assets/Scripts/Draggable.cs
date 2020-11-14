﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    public Transform parentToReturnTo = null;
    public Transform placeholderParent = null; 

    GameObject placeholder = null; 

    public enum Slot { ANY, BATTLE, MANA };
    public Slot typeOfItem = Slot.ANY;

    private Vector3 p;
    private Vector3 pos;
    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
        p = Input.mousePosition;
        p.z = 71.6f;
    }

    private void Update()
    {
        p = Input.mousePosition;
        p.z = 71.6f;
        pos = Camera.main.ScreenToWorldPoint(p);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (   this.transform.parent.name == "Battle Drop Zone"
            || this.transform.parent.name == "Battle Enemy Drop Zone")
        {
            eventData.pointerDrag = null;
            Debug.Log("OnBeginDrag - Stopping drag of anything from battle zone");
            return;
        }

        //Debug.Log("OnBeginDrag");
        FindObjectOfType<AudioManager>().Play("PaperFlip");

        placeholder = new GameObject();
        placeholder.transform.SetParent(this.transform.parent);
        LayoutElement le = placeholder.AddComponent<LayoutElement>();
        le.preferredWidth = this.GetComponent<LayoutElement>().preferredWidth;
        le.preferredHeight = this.GetComponent<LayoutElement>().preferredHeight;
        le.flexibleWidth = 0;
        le.flexibleHeight = 0;

        placeholder.transform.SetSiblingIndex(this.transform.GetSiblingIndex());

        parentToReturnTo = this.transform.parent;
        placeholderParent = parentToReturnTo;
        this.transform.SetParent(this.transform.parent.parent);

        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("OnDrag");

        //use below for gameplay scene
        //this.transform.position = eventData.position; 
        //use below for test scene
        this.transform.position = pos;

        if (placeholder.transform.parent != placeholderParent)
        {
            placeholder.transform.SetParent(placeholderParent); 
        }

        int newSiblingIndex = placeholderParent.childCount; 

        for (int i = 0; i < placeholderParent.childCount; i++)
        {
            if(this.transform.position.x < placeholderParent.GetChild(i).position.x)
            {
                newSiblingIndex = i;

                if (placeholder.transform.GetSiblingIndex() < newSiblingIndex)
                {
                    newSiblingIndex--; 
                }
                break; 
            }
        }

        placeholder.transform.SetSiblingIndex(newSiblingIndex); 
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("OnEndDrag");
        FindObjectOfType<AudioManager>().Play("CardDrop");

        this.transform.SetParent(parentToReturnTo);
        this.transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        Destroy(placeholder);
    }
}
