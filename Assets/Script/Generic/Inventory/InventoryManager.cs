using System.Collections;
using System.Collections.Generic;   
using UnityEngine;
using System.Linq;
//��� �������� �⺻ �������̽� interface Ŭ����
//�޼ҵ�, �̺�Ʈ, �ε���, ������Ƽ
//����� ������ public���� ����ȴ�.
//�����ΰ� ����.



public interface IItem //�������� �⺻������ �̸�, ID, Use�޼ҵ带 ����������
{
    string Name { get; }
    int ID { get; }
    void Use();
}

//CraftingMaterial Ŭ���� �߰�
public class CraftingMaterial : IItem
{
    public string Name { get; private set; }
    public int ID { get; private set; }

    public CraftingMaterial(string name, int id)
    {
        Name = name;
        ID = id;
    }

    public void Use()
    {
        Debug.Log($"This is a crafting material : {Name}");
    }
}

//��ü���� ������ Ŭ���� (Weapon)
public class Weapon : IItem 
{
    // ������ ������ �������̽��� �θ�� ������ �̸�, ID, Use�޼ҵ带 ��ӹ޾� ��밡��
    // �������� �������� ���ο� ����� �־���

    public string Name { get; private set; }
    public int ID { get; private set; }
    public int Damage { get; private set; }

    public Weapon(string name, int id, int damage)  //������
    {
        Name = name;
        ID = id;
        Damage = damage;
    }

    public void Use()
    {
        Debug.Log($"Using weapon {Name} with damage {Damage}");

    }
}


//��ü���� ������ Ŭ���� (HealthPotion)
public class HealthPotion : IItem
{
    // HealthPotion Ŭ������ ������ �������̽��� �θ�� ������ �̸�, ID, Use�޼ҵ带 ��ӹ޾� ��밡��
    // HealthPotion Ŭ�������� ���� �̶�� ���ο� ����� �־���

    public string Name { get; private set; }
    public int ID { get; private set; }
    public int HealAmount { get; private set; }

    public HealthPotion(string name, int id, int healAmount)  //������
    {
        Name = name;
        ID = id;
        HealAmount = healAmount;
    }

    public void Use()   
    {
        Debug.Log($"Using weapon {Name} with damage {HealAmount}");

    }



}

//���׸� �κ��丮 Ŭ����
public class Inventory<T> where T : IItem
{
    private List<T> items = new List<T>();

    public void AddItem(T item)
    {
        items.Add(item);
        Debug.Log($"Add {item.Name} to inventory");
    }

    public void UseItem(int index)
    {
        if (index >= 0 && index < items.Count)
        {
            items[index].Use();
        }
        else
        {
            Debug.Log("Invalid item index");
        }
    }


    public void ListItems()
    {
        foreach (var item in items)
        {
            Debug.Log($"Item: {item.Name} , ID : {item.ID}");
        }
    }

    public void RemoveItems(int itemId, int amount)
    {
        int removed = 0;
        for(int i = items.Count - 1; i >= 0; i--)
        {
            if (items[i].ID == itemId)
            {
                items.RemoveAt(i);
                removed++;
                if (removed >= amount)
                    break;
            }
        }
    }
    
    public bool HasEnough(int itemId, int amount)           //�������� ������� �˻�
    {
        return GetItemCount(itemId) >= amount;
    }

    public int GetItemCount(int itemId)                     //������ ī��Ʈ �Լ�
    {
        return items.Count(item => item.ID == itemId);
    }
}


//�κ��丮 Manager
public class InventoryManager : MonoBehaviour
{
    private Inventory<IItem> playerInventory;
    public int UseBagIndex;

    void Start()
    {
        playerInventory = new Inventory<IItem>();

        //������ �߰�
        playerInventory.AddItem(new Weapon("Sword", 1, 10));
        playerInventory.AddItem(new Weapon("Small Potion", 2, 20));

        playerInventory.AddItem(new CraftingMaterial("Iron Ingot", 101));           //ID 101 : ö �ֱ�
        playerInventory.AddItem(new CraftingMaterial("Iron Ingot", 101));           //ID 101 : ö �ֱ�
        playerInventory.AddItem(new CraftingMaterial("Wood", 102));                 //ID 102 : ����

        playerInventory.AddItem(new CraftingMaterial("Herb", 201));                 //ID 201 : ����
        playerInventory.AddItem(new CraftingMaterial("Herb", 201));                 //ID 201 : ����
        playerInventory.AddItem(new CraftingMaterial("Water", 202));                //ID 202 : ��
    }

    //�κ��丮 ������ �޼��� �߰�
    public Inventory<IItem> GetInventory()
    {

        return playerInventory; 

    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            playerInventory.ListItems();        //�κ��丮 ���� ���
        }

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            playerInventory.UseItem(UseBagIndex);         //ù��° ������ ���
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            playerInventory.AddItem(new Weapon("Sword", 1, 10));            //������ ����
        }
    }
}