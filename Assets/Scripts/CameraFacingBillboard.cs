using UnityEngine;
using System.Collections;

public class CameraFacingBillboard : MonoBehaviour
{
    public GameObject m_player;

    void LateUpdate()
    {
        transform.LookAt(transform.position + m_player.transform.rotation * Vector3.forward,
            m_player.transform.rotation * Vector3.up);
    }
}
