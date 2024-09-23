using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSystem : MonoBehaviour
{

    public static event Action<int> OnScoreChanged;                 //스코어 반환 Action 등록
    public static event Action OnGameOver;                          //게임 상태 Aciton 등록

    private int score = 0;
    // Start is called before the first frame update
    void Start()
    {
        if( Input.GetKeyDown(KeyCode.Escape) )
        {
            score += 10;
            OnScoreChanged?.Invoke(score); ;                        //스코어 변동 시 호출
        }
        if (score >= 100)
        {
            OnGameOver?.Invoke();                                   //게임 오버 시 호출
        }

    }

    
}
