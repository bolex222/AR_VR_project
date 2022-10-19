using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RegisterOriginLocation : MonoBehaviour
{
   [SerializeField] private Camera Head;
   [SerializeField] bool rotation;
   [SerializeField] private Camera MiniMapCamera;
   [SerializeField] bool miniRotation;

    private void Update()
   {
      transform.position = Head.gameObject.transform.position;

      if (rotation)
      {
         transform.rotation = Quaternion.Euler(new Vector3(0, Head.gameObject.transform.rotation.y, 0));
      }

      transform.position = MiniMapCamera.gameObject.transform.position;

      if (miniRotation)
      {
         transform.rotation = Quaternion.Euler(new Vector3(0, MiniMapCamera.gameObject.transform.rotation.y, 0));
      }
    }
}
