using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace MyGame.QuestSystem
{
    public class Quest
    {
        public string Id {  get; set; }                 //����Ʈ ���� �ĺ���
        public string Title {  get; set; }              //����Ʈ ����
        public string Description { get; set; }         //����Ʈ�� �� ����
        public QuestType Type { get; set; }             //����Ʈ ����
        public QuestStatus Status { get; set; }         //����Ʈ ���� ����
        public int Level { get; set; }                  //����Ʈ �䱸 ����

        private List<IQuestCondition> conditions = new List<IQuestCondition>();             //����Ʈ �Ϸ� ���� ���
        private List<IQuestReward> rewards = new List<IQuestReward>();                      //����Ʈ ���� ���
        private List<string> prerequisiteQuestIds;                                          //���� ����Ʈ ID ���

        //����Ʈ �ʱ�ȭ ������
        public Quest(string id, string title, string description, QuestType type, int level)
        {
            Id = id;
            Title = title;
            Description = description;
            Type = type;
            Status = QuestStatus.NotStarted;
            Level = level;
            this.conditions = new List<IQuestCondition>(); 
            this.rewards = new List<IQuestReward>(); 
            this.prerequisiteQuestIds = new List<string>();
        }

        public List<IQuestCondition> GetConditions()
        {
            return conditions;
        }
        public void AddCondition(IQuestCondition condition)     //����Ʈ�� �Ϸ� ������ �߰��ϴ� �޼���
        {
            conditions.Add(condition);
        }

        public void AddReward(IQuestReward reward)              //����Ʈ�� ������ �߰��ϴ� �޼���
        {
            rewards.Add(reward);
        }
        public void Start()                                     //����Ʈ�� �����ϴ� �޼���
        {
            if (Status == QuestStatus.NotStarted)
            {
                Status = QuestStatus.InProgress;
                foreach (var condition in conditions)
                {
                    condition.Initialize();
                }
            }
        }

        public bool CheckCompletion()                           //����Ʈ �Ϸ� ������ �˻��ϴ� �޼���
        {
            if (Status != QuestStatus.InProgress) return false;
            return conditions.All(c => c.IsMet());
        }

        public void Complete(GameObject player)                 //����Ʈ�� �Ϸ��ϰ� ������ �����ϴ� �޼���
        {
            if (Status != QuestStatus.InProgress) return;
            if (!CheckCompletion()) return;

            foreach (var reward in rewards)
            {
                reward.Grant(player);
            }

            Status = QuestStatus.Completed;
        }

        public float GetProgress()                              //����Ʈ�� ��ü ���൵�� �Ի��ϴ� �޼���
        {
            if (conditions.Count == 0) return 0;
            return conditions.Average(c => c.GetProgress());
        }

        public List<string> GetConditionDescriptions()          //��� ����Ʈ ������ ������ �������� �޼���
        {
            return conditions.Select(c => c.GetDescription()).ToList();
        }

        public List<string> GetRewardDescriptions()             //��� ����Ʈ ������ ������ �������� �޼���
        {
            return rewards.Select(r => r.GetDescription()).ToList();
        }
    }
}
