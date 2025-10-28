
using UnityEngine;
using UnityEngine.UI;

public class AudioSliderController : MonoBehaviour
{
    public enum MixerName { MasterVolume, MusicVolume, SFXVolume }
    public MixerName mixerName;
    private Slider audioSlider;

    // Start is called before the first frame update
    void Start()
    {
        audioSlider.onValueChanged.AddListener(ChangeVolume);
    }

    public void ChangeVolume(float value)
    {
        AudioManager.instance.ChangeVolume(mixerName.ToString(), value);
    }

    void OnEnable()
    {
        audioSlider = GetComponent<Slider>();
        audioSlider.value = DeconvertDBValue(PlayerPrefs.GetFloat(mixerName.ToString(), 0));
    }

    private float DeconvertDBValue(float value)
    {
        return Mathf.Pow(10f, value / 20f);
    }
}
