using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SocketInteractor : MonoBehaviour
{
    public XRSocketInteractor xrSocketInteractor;
    [SerializeField] private XRGrabInteractable _xrGrabInteractable;

    public void GunTp()
    {
        Debug.Log("TP");
        xrSocketInteractor.StartManualInteraction(_xrGrabInteractable);
    }
    
    public void Debug1() {Debug.Log(1);}
    public void Debug2() {Debug.Log(2);}
    public void Debug3() {Debug.Log(3);}
    public void Debug4() {Debug.Log(4);}
}
