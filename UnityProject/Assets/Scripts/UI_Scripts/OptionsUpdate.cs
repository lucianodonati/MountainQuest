using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUpdate : MonoBehaviour
{
    // Use this for initialization
    private void Awake()
    {
        Slider musicSlider = GetComponentsInChildren<Slider>()[0], SfxSlider = GetComponentsInChildren<Slider>()[1];

        musicSlider.value = (float)GameManager.instance.musicVol;
        SfxSlider.value = (float)GameManager.instance.sfxVol;
    }
}