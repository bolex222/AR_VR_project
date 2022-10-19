using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RegisterOriginLocation : MonoBehaviour
{
   [SerializeField] private Camera Head;
   [SerializeField] bool rotation;

    private void Update()
   {
      transform.position = Head.gameObject.transform.position;

      if (rotation)
      {
         transform.rotation = Quaternion.Euler(new Vector3(0, Head.gameObject.transform.rotation.y, 0));
      }
    }
}
