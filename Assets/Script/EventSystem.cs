using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSystem : MonoBehaviour
{

    public static event Action<int> OnScoreChanged;                 //���ھ� ��ȯ Action ���
    public static event Action OnGameOver;                          //���� ���� Aciton ���

    private int score = 0;
    // Start is called before the first frame update
    void Start()
    {
        if( Input.GetKeyDown(KeyCode.Escape) )
        {
            score += 10;
            OnScoreChanged?.Invoke(score); ;                        //���ھ� ���� �� ȣ��
        }
        if (score >= 100)
        {
            OnGameOver?.Invoke();                                   //���� ���� �� ȣ��
        }

    }

    
}
