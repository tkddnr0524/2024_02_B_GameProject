using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    private void OnEnable()                     //Ȱ��ȭ �ɶ� �̺�Ʈ ���
    {
        EventSystem.OnScoreChanged += UpdateScore;
        EventSystem.OnGameOver += ShowGameOver;
    }

    private void OnDisable()                    //��Ȱ��ȭ �ɶ� �̺�Ʈ ����
    {
        EventSystem.OnScoreChanged -= UpdateScore;
        EventSystem.OnGameOver -= ShowGameOver;
    }

    void UpdateScore(int newScore)
    {
        Debug.Log($"Score update: {newScore}");
    }

    void ShowGameOver()
    {
        Debug.Log("Game Over!");
    }
}
