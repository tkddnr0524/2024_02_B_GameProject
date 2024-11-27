using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MyGame.CraftingSystem
{
    [System.Serializable]
    public class Recipe
    {
        public string recipeId;                             //������ ���� ID
        public IItem resultItem;                            //��� ������
        public int resultAmount;                            //���� ����
        public Dictionary<int, int> requiredMaterials;      //�ʿ� ��� <������ ID, ����>
        public int requiredLevel;                           //�䱸 ���� ����
        public float baseSuccessRate;                       //�⺻ ���� Ȯ��
        public float craftTime;                             //���� �ð�

        public Recipe(string id, IItem result, int amount)
        {
            recipeId = id;
            resultItem = result;
            resultAmount = amount;
            requiredMaterials = new Dictionary<int, int>();
            requiredLevel = 1;
            baseSuccessRate = 1;
            craftTime = 0;
        }

        public void AddRequiredMaterial(int itemId, int amount)
        {
            
            if (requiredMaterials.ContainsKey(itemId))
            {
                requiredMaterials[itemId] += amount;
            }
            else
            {
                requiredMaterials[itemId] = amount;
            }
        }
    }
}
