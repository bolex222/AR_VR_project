using UnityEngine;
public class RegisterOriginLocation : MonoBehaviour
{
   [SerializeField] private Camera Head;
   [SerializeField] bool rotation;

   private void Update()
   {
      transform.position = Head.gameObject.transform.position;

      if (rotation)
      {
         transform.eulerAngles = new Vector3(0, Head.gameObject.transform.eulerAngles.y - 180f, 0);
      }
   }
}
