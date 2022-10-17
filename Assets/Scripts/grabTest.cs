using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit;

public class grabTest : XRBaseInteractable
{
    // Start is called before the first frame update

    public Rigidbody RB;

    private bool isValveGrabbed = false;
    private Vector3 lastHandPosition;
    private IXRSelectInteractable m_Valve;
    private IXRSelectInteractor m_Hand;
    private Vector3 m_previousHandPosition;


    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntering(args);
        m_Hand = args.interactorObject;
        m_Valve = args.interactableObject;
        isValveGrabbed = true;
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        isValveGrabbed = false;
        m_Valve = null;
        m_Hand = null;
    }


    // Update is called once per frame


    public float radAngle;
    public float degAngle;
    public Vector3 HandPosition;
    public float ValveRot;

    void Update()
    {
        if (isValveGrabbed)
        {
            Vector3 handPosition = HandPosition = m_Hand.transform.position;
            Vector3 valvePosition = m_Valve.transform.position;
            float valveRotation = ValveRot = m_Valve.transform.rotation.x;
            float radius = Vector2.Distance(new Vector2(handPosition.y, handPosition.z),
                new Vector2(valvePosition.y, valvePosition.z));
            
            


            if (m_previousHandPosition != null)
            {
                float radAngle = this.radAngle =
                    Mathf.Atan2(handPosition.y - valvePosition.y, handPosition.z - valvePosition.z);
                degAngle = radAngle * (180 / Mathf.PI);
                RB.angularVelocity = Vector3.zero;
                RB.rotation = Quaternion.Euler(new Vector3(0f, -90f , degAngle));
                m_Hand.transform.position = new Vector3(Mathf.Cos(degAngle) * radius, Mathf.Sin(degAngle) * radius,
                    transform.position.z);
            }

            m_previousHandPosition = m_Valve.transform.position;
        }
    }
}