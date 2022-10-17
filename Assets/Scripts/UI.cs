using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{

    public static UI Instance;

    public Image flagZoneCaptureProgressCanvas;
    public TextMeshProUGUI battleText;

    private void Awake()
    {
        Instance = this;
    }
}
