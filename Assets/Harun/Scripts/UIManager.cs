using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using static Unity.VisualScripting.Member;

public class UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI hintText, timeText;
    public List<Hints> hints;
    [SerializeField] Button hintBuy, home, restart, join, message;
    [SerializeField] GameObject interact;
    [SerializeField] Transform point;
    [SerializeField] CanvasGroup loading;
    [SerializeField] PlayerControl playerControl;
    [SerializeField] AudioSource gameMusic, clips;
    [SerializeField] AudioClip homeClip, outClip, firstClip, secondClip;
    public float time;
    public int hintLevelIndex;
    bool timer = false;
    void Start()
    {
        MusicPlayer(gameMusic);
        HintUpdate(false, false);
        timeText.text = ((int)(time / 60)).ToString() + " . " + ((int)(time % 60)).ToString();
        hintBuy.onClick.AddListener(HintBuy);
        home.onClick.AddListener(Home);
        join.onClick.AddListener(Join);
        message.onClick.AddListener(Message);
        restart.onClick.AddListener(GameRestart);
        clips.clip = firstClip;
        MusicPlayer(clips);
        StartCoroutine(WaitFirstClip());
    }
    IEnumerator WaitFirstClip()
    {
        yield return new WaitForSecondsRealtime(firstClip.length);
        clips.clip = secondClip;
        MusicPlayer(clips);
        StartCoroutine(WaitSecondClip());
    }
    IEnumerator WaitSecondClip()
    {
        yield return new WaitForSecondsRealtime(secondClip.length);
        playerControl.enabled = true;
        timer = true;
    }
    void MusicPlayer(AudioSource source)
    {
        if (PlayerPrefs.GetString("Sound") == "true")
        {
            source.Play();
        }
    }
    void Update()
    {
        if (timer)
        {
            time -= Time.deltaTime;
            timeText.text = ((int)(time / 60)).ToString() + " . " + ((int)(time % 60)).ToString();
        }
        if (time <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
    public void CollisionEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Home") && gameMusic.clip != homeClip)
        {
            gameMusic.clip = homeClip;
            MusicPlayer(gameMusic);
        }
        else if (other.gameObject.CompareTag("Out") && gameMusic.clip != outClip)
        {
            gameMusic.clip = outClip;
            MusicPlayer(gameMusic);
        }
    }
    void GameRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    void Join()
    {
        Application.OpenURL("https://www.youtube.com/");
    }
    void Message()
    {
        Application.OpenURL("https://www.youtube.com/");
    }
    public void HintUpdate(bool nextLevel, bool hintTake)
    {
        hintText.text = "";
        if (nextLevel)
        {
            hintLevelIndex++;
        }
        if (hintTake)
        {
            hints[hintLevelIndex].hintIndex++;
        }
        if (hintLevelIndex == 6)
        {
            return;
        }
        for (int i = 0; i < hints[hintLevelIndex].hintIndex; i++)
        {
            if (i != 0)
            {
                hintText.text = hintText.text + "\n";
            }
            hintText.text = hintText.text + hints[hintLevelIndex].hints[i];
        }
    }
    public void InteractUpdate(bool interactState, string message)
    {
        interact.GetComponentInChildren<TextMeshProUGUI>().text = message;
        interact.SetActive(interactState);
    }
    void HintBuy()
    {
        if (hints[hintLevelIndex].hintIndex < hints[hintLevelIndex].hints.Count)
        {
            time -= 5;
            HintUpdate(false, true);
        }
    }
    public void NextLevel()
    {
        hintText.text = "";
        loading.GetComponentInChildren<TextMeshProUGUI>().text = "";
        timer = false;
        playerControl.enabled = false;
        DOTween.To(() => loading.alpha, x => loading.alpha = x, 1, 1).SetEase(Ease.Linear).OnComplete(() =>
        {
            HintUpdate(true, false);
            if (hintLevelIndex == 6)
            {
                playerControl.transform.position = point.position;
            }
            loading.GetComponentInChildren<TextMeshProUGUI>().text = hints[hintLevelIndex - 1].info;
            StartCoroutine(WaitStart());
        });
    }
    IEnumerator WaitStart()
    {
        yield return new WaitForSecondsRealtime(3);
        DOTween.To(() => loading.alpha, x => loading.alpha = x, 0, 1).SetEase(Ease.Linear).OnComplete(() =>
        {
            Debug.Log(hintLevelIndex);
            hints[hintLevelIndex - 1].coin.SetActive(true);
            if (hintLevelIndex < 6)
            {
                hints[hintLevelIndex].target.layer = 6;
            }
            hints[hintLevelIndex - 1].target.layer = 7;
            clips.clip = hints[hintLevelIndex - 1].infoSound;
            //clips.Play();
            MusicPlayer(clips);
            StartCoroutine(WaitWalk());
        });
    }
    IEnumerator WaitWalk()
    {
        yield return new WaitForSecondsRealtime(hints[hintLevelIndex - 1].infoSound.length);
        time = 180 - hintLevelIndex * 10;
        if (hintLevelIndex < 6)
        {
            timer = true;
            playerControl.enabled = true;
        }
        else
        {
            timeText.text = "";
            loading.blocksRaycasts = true;
            loading.interactable = true;
            restart.transform.parent.gameObject.SetActive(true);
            DOTween.To(() => loading.alpha, x => loading.alpha = x, 1, 1).SetEase(Ease.Linear);
        }
    }
    void Home()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
[Serializable]
public class Hints
{
    public List<string> hints;
    public int hintIndex;
    public string info;
    public GameObject target;
    public GameObject coin;
    public AudioClip infoSound;
}
