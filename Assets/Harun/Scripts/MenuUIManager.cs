using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUIManager : MonoBehaviour
{
    [SerializeField] Button play, soundOn, soundOff;
    void Start()
    {
        play.onClick.AddListener(Play);
        soundOn.onClick.AddListener(delegate { Sound("true"); });
        soundOff.onClick.AddListener(delegate { Sound("false"); });
    }
    void Update()
    {
        
    }
    void Play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    void Sound(string soundState)
    {
        PlayerPrefs.SetString("Sound", soundState);
    }
}
