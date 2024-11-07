using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyGame.QuestSystem;
using System.Linq;
using System;
using System.Reflection;

public class QuestManager : Singleton<QuestManager>
{
    private Dictionary<string, Quest> allQuests = new Dictionary<string, Quest>();           //������ ��� ����Ʈ�� �����ϴ� ��ųʸ�
    private Dictionary<string, Quest> activeQuests = new Dictionary<string, Quest>();       //���� ���� ���� ����Ʈ�� �����ϴ� ��ųʸ�
    private Dictionary<string, Quest> completedQuests = new Dictionary<string, Quest>();    //�Ϸ�� ����Ʈ�� �����ϴ� ��ųʸ�

    public event Action<Quest> OnQuestStarted;      //����Ʈ ���� �� �߻��ϴ� �̺�Ʈ
    public event Action<Quest> OnQuestCompleted;    //����Ʈ �Ϸ� �� �߻��ϴ� �̺�Ʈ
    public event Action<Quest> OnQuestFailed;       //����Ʈ ���� �� �߻��ϴ� �̺�Ʈ

    public void Start()
    {
        InitiallizeQuests();
    }

    //�⺻ ����Ʈ���� �����ϰ� ����ϴ� �޼���
    private void InitiallizeQuests()
    {
        //�� ��� ����Ʈ ���� ����
        var ratHuntQuest = new Quest("Q001", "Rat Problem", "Clear the basement of rate", QuestType.Kill, 1);
        ratHuntQuest.AddCondition(new KillQuestCondition("Rat", 5));
        ratHuntQuest.AddReward(new ExperienceReward(100));
        ratHuntQuest.AddReward(new ItemReward("Gold", 50));

        //���� ���� ����Ʈ
        var herbQuest = new Quest("Q002", "Herb Collection", "Collect herbs for the healer", QuestType.Collection, 1);
        herbQuest.AddCondition(new CollectionQuestCondition("Herb", 3));
        herbQuest.AddReward(new ExperienceReward(50));

        //����Ʈ �Ŵ����� ����Ʈ �߰�
        allQuests.Add(ratHuntQuest.Id, ratHuntQuest);
        allQuests.Add(herbQuest.Id, herbQuest);

        //�׽�Ʈ ���ؼ� �ٷ� ���� (StartQuest �Լ�)
        StartQuest("Q001");
        StartQuest("Q002");
    }    

    public bool CanStartQuest(string questId)       //Ư�� ����Ʈ�� ������ �� �ִ��� �˻��ϴ� �޼���
    {
        if(!allQuests.TryGetValue(questId, out Quest quest)) return false;
        if(activeQuests.ContainsKey(questId)) return false;
        if(completedQuests.ContainsKey(questId)) return false;

        //���� ����Ʈ �Ϸ� ���� Ȯ��
        foreach (var perrequisiteid in quest.GetType().GetField("prerequisitequestids")?.GetValue(quest) as List<string> ?? new List<string>())
        {
            if (!completedQuests.ContainsKey(perrequisiteid)) return false;
        }

        //Type questType = quest.GetType();                                                   //Quest ��ü�� Ÿ���� �����´�.
        //FieldInfo perrequisiteIdsField = questType.GetField("prerequisiteQuestIds");        //Quest Type���� �ʵ带 �˻�
        //object perrequisiteIdsValue = perrequisiteIdsField?.GetValue(quest);                //�ʵ� ���� �����´�.
        //List<string> perrequisiteQuestIds = perrequisiteIdsValue as List<string>;           //List �� ��ȯ�ϰ�
        //perrequisiteQuestIds = perrequisiteQuestIds ?? new List<string>();                  //null ���� ��쿡�� �� List�� ����Ѵ�.
        //?? null ���� ������ -> ���� ���� null ���� Ȯ���Ͽ� null�� ��� ���������� ��ȯ


        return true;
    }

    //����Ʈ�� �����ϴ� �޼���

    public void StartQuest(string questId)
    {
        if(!CanStartQuest(questId)) return;

        var quest = allQuests[questId];
        quest.Start();
        activeQuests.Add(questId, quest);
        OnQuestStarted?.Invoke(quest);
    }

    //����Ʈ ���� ��Ȳ�� ������Ʈ �ϴ� �޼���

    public void UpdateQuestProgress(string questId)
    {
        if(!activeQuests.TryGetValue(questId,out Quest quest)) return;

        if(quest.CheckCompletion())
        {
            CompleteQuest(questId);
        }
    }

    //����Ʈ�� �Ϸ� ó�� �ϴ� �޼���
    private void CompleteQuest(string questId)
    {
        if (!activeQuests.TryGetValue(questId, out Quest quest)) return;

        //�÷��̾� ã�� �����ص� ����Ʈ�� �Ϸ�
        var player = GameObject.FindGameObjectWithTag("Player");
        quest.Complete(player);             //player�� null �̿��� ����ǵ��� ��

        activeQuests.Remove(questId);
        completedQuests.Add(questId, quest);
        OnQuestCompleted?.Invoke(quest);

        Debug.Log($"Quest completed : {quest.Title}");
    }

    //���� ������ ����Ʈ ����� ��ȯ�ϴ� �޼���
    public List<Quest> GetAvailableQuests()
    {
        return allQuests.Values.Where(q => CanStartQuest(q.Id)).ToList();
    }

    //���� ���� ���� ����Ʈ ����� ��ȯ�ϴ� �޼���
    public List<Quest> GetActiveQuest()
    {
        return activeQuests.Values.ToList();
    }

    //�Ϸ�� ����Ʈ ����� ��ȯ�ϴ� �޼���
    public List<Quest> GetCompletedQuest()
    {
        return completedQuests.Values.ToList();
    }

    //�� óġ �� ȣ��Ǵ� �̺�Ʈ �ڵ鷯
    public void OnEnemykilled(string enemyType)
    {
        //Ȱ�� ����Ʈ�� ���纻�� ���� ���
        var activeQuestsList = activeQuests.Values.ToList();

        foreach(var quest in activeQuestsList)
        {
            foreach(var condition in quest.GetConditions())
            {
                if(condition is KillQuestCondition killCondition)
                {
                    killCondition.EnemyKilled(enemyType);
                    UpdateQuestProgress(quest.Id);
                }
            }
        }
    }

    //���� �� ȣ�� �Ǵ� �̺�Ʈ �ڵ鷯
    public void OnItemCollected(string itemid)
    {
        //Ȱ�� ����Ʈ�� ���纻�� ���� ���
        var activeQuestsList = activeQuests.Values.ToList();

        foreach (var quest in activeQuestsList)
        {
            foreach (var condition in quest.GetConditions())
            {
                if (condition is CollectionQuestCondition collectCondition)
                {
                    collectCondition.ItemCollected(itemid);
                    UpdateQuestProgress(quest.Id);
                }
            }
        }
    }

}
