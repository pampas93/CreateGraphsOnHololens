using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using System;

public class MenuScript : MonoBehaviour, IInputClickHandler
{
    bool showMenu = false;

    public void OnInputClicked(InputClickedEventData eventData)
    {
        if (!showMenu)
        {
            //Enable children
            foreach (Transform t_child in transform)
            {
                GameObject child = t_child.gameObject;
                child.SetActive(true);
            }
            showMenu = true;
        }
        else
        {
            //disable children
            foreach (Transform t_child in transform)
            {
                GameObject child = t_child.gameObject;
                child.SetActive(false);
            }
            showMenu = false;
        }
    }
}
