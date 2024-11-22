using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField] Slider volumeSlider;

    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 0.1f); // Set default volume to 10%
        }

        Load(); // Load the saved volume setting
        AudioListener.volume = volumeSlider.value; // Apply the volume setting globally
    }

    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value; // Update global volume
        Save(); // Save the new volume setting
    }

    private void Load()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
    }

    private void Save()
    {
        PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
    }
}
