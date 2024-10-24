using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.QuestSystem
{


    public interface IQuestReward               //����Ʈ ������ �����ϴ� �⺻ �������̽� 
    {
        void Grant(GameObject player);          //�÷��̾�� ������ �����ϴ� �Լ�

        string GetDescription();                //���� ���� ������ ��ȯ�ϴ� �Լ�
    }
}