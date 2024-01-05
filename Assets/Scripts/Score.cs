using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    private int score = 0;

    void Update()
    {
        text.text = score.ToString();
    }

    public void AddToScore(int amount) {
        score += amount;
    }
}