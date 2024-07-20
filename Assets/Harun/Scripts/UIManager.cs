using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI hintText, timeText;
    [SerializeField] List<Hints> hints;
    [SerializeField] Button hintBuy;
    public float time;
    [SerializeField] GameObject interact;
    int hintLevelIndex;
    void Start()
    {
        HintUpdate(false, false);
        timeText.text = ((int)(time / 60)).ToString() + " : " + ((int)(time % 60)).ToString();
        hintBuy.onClick.AddListener(HintBuy);
    }
    void Update()
    {
        time-= Time.deltaTime;
        timeText.text = ((int)(time / 60)).ToString() + " : " + ((int)(time % 60)).ToString();
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
}
[Serializable]
public class Hints
{
    public List<string> hints;
    public int hintIndex;
}
