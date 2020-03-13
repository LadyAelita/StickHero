using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundScript : MonoBehaviour
{

    private Music music;
    public Button musicToggleButton;
   // public Sprite musicOnSprite;
   // public Sprite musicOffSprite;


    // Start is called before the first frame update
    void Start()
    {
        music = GameObject.FindObjectOfType<Music>();
        UpdateIconAndVolume();

    }

  public void PauseMusic()
    {
        music.ToogleSound();
        UpdateIconAndVolume();
    }

    void UpdateIconAndVolume()
    {
        if(PlayerPrefs.GetInt("Muted", 0) == 0)
        {
            AudioListener.volume = 1;
            Debug.Log("Audio On");

        }
        else
        {
            AudioListener.volume = 0;
            Debug.Log("Audio Off");
        }

    }
}
