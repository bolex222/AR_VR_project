using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapCapturingUI : MonoBehaviour
{
    public Image progressImage;

    [SerializeField] private List<ZoneCapture> captureAreas;

    private void Awake()
    {
        //progressImage = transform.Find("ProgressImage").GetComponent<Image>();
    }
}
