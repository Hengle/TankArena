﻿using System;
using System.Globalization;
using SOReferences.FloatReference;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeChronoUI : MonoBehaviour {

    [Header("Internal References")]
    public Image TimeCircle;
    public TextMeshProUGUI TimeText;

    [Header("SO References")] 
    public FloatReference MaxTimeReference;
    public FloatReference CurrentTimeReference;

    private int _timeLeft;
    
    private void Update() {
        _timeLeft = Mathf.FloorToInt(MaxTimeReference.Value - CurrentTimeReference.Value);
        TimeText.text = _timeLeft.ToString(CultureInfo.CurrentCulture);
        TimeCircle.fillAmount = 1 - CurrentTimeReference.Value / MaxTimeReference.Value;
    }
}
