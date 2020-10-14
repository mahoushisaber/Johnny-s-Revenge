using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using UnityEngine;
using UnityEngine.UI;

public class TextEffects : MonoBehaviour
{
    public bool visible;
    public float flashInSecs;   // Zero = no flash
    public float zoomInSecs;    // Zero = no zoom
    public float zoomScale;     // Same as editor

    private Text textObj;
    private RectTransform recTrans;

    private float flashTmr = 0.0f;
    private bool flashOn = false;

    private float zoomTmr = 0.0f;
    private Vector3 scaleChange;
    private Vector3 origVal;
    private bool oldVisible;

    // Start is called before the first frame update
    void Start()
    {
        textObj = gameObject.GetComponent<Text>() as Text;
        recTrans = gameObject.GetComponent<RectTransform>() as RectTransform;
        scaleChange = new Vector3(0.005f, 0.005f, 0f);
        origVal = recTrans.localScale;
        oldVisible = !visible;
    }

    // Update is called once per frame
    void Update()
    {
        if (visible != oldVisible)
            OnVisible(visible);

        if (visible)
        {
            // Flash enabled if flash in seconds is not zero
            if (flashInSecs > 0.0f)
            {
                DoFlash();
            }

            // Zoom enabled if zoom in seconds is not zero
            if (zoomInSecs > 0.0f)
            {
                DoZoom();
            }
        }
    }

    private void OnVisible(bool newVis)
    {
        oldVisible = newVis;

        // Always get current color as it mught change during updates
        Color textColor = textObj.color;

        if (newVis)
        {
            textColor.a = 255;
        }
        else 
        {
            textColor.a = 0;
        }

        textObj.color = textColor;
    }

    void DoFlash()
    {
        flashTmr += Time.deltaTime;

        if (flashTmr > flashInSecs)
        {
            // Always get current color as it mught change during updates
            Color textColor = textObj.color;

            if (flashOn)
            {
                textColor.a = 0;
            }
            else
            {
                textColor.a = 255;
            }
            textObj.color = textColor;
            flashOn = !flashOn;

            flashTmr -= flashInSecs;
        }
    }

    void DoZoom()
    {
        zoomTmr += Time.deltaTime;

        if (zoomTmr > zoomInSecs)
        {
            recTrans.localScale += scaleChange;

            if (recTrans.localScale.y < origVal.y || recTrans.localScale.y > (origVal.y + zoomScale))
            {
                scaleChange = -scaleChange;
            }

            zoomTmr -= zoomInSecs;
        }
    }
}
