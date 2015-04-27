using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUpdate : MonoBehaviour
{
    // Use this for initialization
    private void Start()
    {
        Slider musicSlider = GetComponentsInChildren<Slider>()[0], SfxSlider = GetComponentsInChildren<Slider>()[1];

        musicSlider.value = GameManager.instance.music.volume * 100;
        SfxSlider.value = AudioListener.volume * 100;
    }
}