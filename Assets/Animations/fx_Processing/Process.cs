using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Process : MonoBehaviour
{
    public Animator volumeAnim;

    public void ProcessScreen(string animation)
    {
        if (animation == "vision")
        {
            volumeAnim.SetTrigger("vision");
        }
    }
}
