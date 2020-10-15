using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldScript : MonoBehaviour
{
    public Text ShieldText;
    public float CurrentShield;

    private void Start()
    {
        CurrentShield = 0f;
    }

    private void Update()
    {
        ShieldText.text = CurrentShield.ToString("##0");
    }
}