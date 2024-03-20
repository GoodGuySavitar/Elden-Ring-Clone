using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
public class UI_StatBar : MonoBehaviour
{
    private Slider slider;
    //  VARIABLE TO SCALE BAR SIZE DEPENDING ON STAT( HIGHER STAT = LONGER BAR ACROSS SCREEN)
    // SECONDARY BAR BEHIND THE MAIN BAR FOR POLISH EFFECT ( THE YELLOW BAR THAT SHOWS HOW MUCH AN ACTION/DAMAGE TAKES AWAY FROM THE CURRENT STAT)

    protected virtual void Awake()
    {
        slider = GetComponent<Slider>();
    }

    public virtual void SetStat(int newValue)
    {
        slider.value = newValue;
    }

    public virtual void SetMaxStat(int maxValue)
    {
        slider.maxValue = maxValue;
        slider.value = maxValue;
    }
}
