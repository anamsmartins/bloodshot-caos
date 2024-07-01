using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayScore : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;

    void Start() {
        int playerScore = PlayerStats.Score;
        scoreText.text = "Score: " + playerScore.ToString();
    }
}
