using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUIManager : MonoBehaviour
{
    [SerializeField] Button play, soundOn, soundOff;
    [SerializeField] AudioSource gameMusic;
    [SerializeField] Image bg;
    [SerializeField] Sprite first, second, third;
    void Start()
    {
        if (PlayerPrefs.HasKey("Sound"))
        {
            if (PlayerPrefs.GetString("Sound") == "true")
            {
                gameMusic.Play();
            }
        }
        else
        {
            PlayerPrefs.SetString("Sound", "true");
            gameMusic.Play();
        }
        play.onClick.AddListener(Play);
        soundOn.onClick.AddListener(delegate { Sound("true"); });
        soundOff.onClick.AddListener(delegate { Sound("false"); });
        InvokeRepeating("BgChange", 0, 5);
    }
    void BgChange()
    {
        if (bg.sprite == first)
        {
            bg.sprite = second;
        }
        else if (bg.sprite == second)
        {
            bg.sprite = third;
        }
        else
        {
            bg.sprite = first;
        }
    }
    void Play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    void Sound(string soundState)
    {
        PlayerPrefs.SetString("Sound", soundState);
        if (soundState == "true" && !gameMusic.isPlaying)
        {
            gameMusic.Play();
        }
        else if (soundState == "false")
        {
            gameMusic.Stop();
        }
    }
}
