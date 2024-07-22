using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using DG.Tweening;
using static System.TimeZoneInfo;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI hintText, timeText;
    [SerializeField] List<Hints> hints;
    [SerializeField] Button hintBuy;
    [SerializeField] GameObject interact;
    [SerializeField] CanvasGroup loading;
    [SerializeField] PlayerControl playerControl;
    public float time;
    int hintLevelIndex;
    bool timer = true;
    void Start()
    {
        HintUpdate(false, false);
        timeText.text = ((int)(time / 60)).ToString() + " : " + ((int)(time % 60)).ToString();
        hintBuy.onClick.AddListener(HintBuy);
    }
    void Update()
    {
        if (timer)
        {
            time -= Time.deltaTime;
            timeText.text = ((int)(time / 60)).ToString() + " : " + ((int)(time % 60)).ToString();
        }
        if (time <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
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
        timer = false;
        playerControl.enabled = false;
        DOTween.To(() => loading.alpha, x => loading.alpha = x, 1, 1).SetEase(Ease.Linear).OnComplete(() =>
        {
            HintUpdate(true, false);
            playerControl.transform.position = hints[hintLevelIndex].startPoint.position;
            DOTween.To(() => loading.alpha, x => loading.alpha = x, 0, 1).SetEase(Ease.Linear).OnComplete(() =>
            {
                time = 180;
                timer = true;
                playerControl.enabled = true;
            });
        });
        time = 180;
    }
}
[Serializable]
public class Hints
{
    public List<string> hints;
    public int hintIndex;
    public Transform startPoint;
}
