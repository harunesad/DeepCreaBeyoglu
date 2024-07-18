using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI hintText;
    [SerializeField] List<Hints> hints;
    int hintLevelIndex;
    void Start()
    {
        
    }
    void Update()
    {
        
    }
    public void HintUpdate(bool nextLevel, bool hintTake)
    {
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
}
[Serializable]
public class Hints
{
    public List<string> hints;
    public int hintIndex;
}
