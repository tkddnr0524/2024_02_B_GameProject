using System.Collections;
using System.Collections.Generic;   
using UnityEngine;
using System.Linq;
//모든 아이템의 기본 인터페이스 interface 클래스
//메소드, 이벤트, 인덱서, 프로퍼티
//모든이 무조건 public으로 선언된다.
//구현부가 없다.



public interface IItem //아이템은 기본적으로 이름, ID, Use메소드를 가지고있음
{
    string Name { get; }
    int ID { get; }
    void Use();
}

//CraftingMaterial 클래스 추가
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

//구체적인 아이템 클래스 (Weapon)
public class Weapon : IItem 
{
    // 웨폰이 아이템 인터페이스를 부모로 가지고 이름, ID, Use메소드를 상속받아 사용가능
    // 웨폰에는 대미지라는 새로운 기능을 넣어줌

    public string Name { get; private set; }
    public int ID { get; private set; }
    public int Damage { get; private set; }

    public Weapon(string name, int id, int damage)  //생성자
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


//구체적인 아이템 클래스 (HealthPotion)
public class HealthPotion : IItem
{
    // HealthPotion 클래스가 아이템 인터페이스를 부모로 가지고 이름, ID, Use메소드를 상속받아 사용가능
    // HealthPotion 클래스에는 힐양 이라는 새로운 기능을 넣어줌

    public string Name { get; private set; }
    public int ID { get; private set; }
    public int HealAmount { get; private set; }

    public HealthPotion(string name, int id, int healAmount)  //생성자
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

//제네릭 인벤토리 클래스
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
    
    public bool HasEnough(int itemId, int amount)           //아이템이 충분한지 검사
    {
        return GetItemCount(itemId) >= amount;
    }

    public int GetItemCount(int itemId)                     //아이템 카운트 함수
    {
        return items.Count(item => item.ID == itemId);
    }
}


//인벤토리 Manager
public class InventoryManager : MonoBehaviour
{
    private Inventory<IItem> playerInventory;
    public int UseBagIndex;

    void Start()
    {
        playerInventory = new Inventory<IItem>();

        //아이템 추가
        playerInventory.AddItem(new Weapon("Sword", 1, 10));
        playerInventory.AddItem(new Weapon("Small Potion", 2, 20));

        playerInventory.AddItem(new CraftingMaterial("Iron Ingot", 101));           //ID 101 : 철 주괴
        playerInventory.AddItem(new CraftingMaterial("Iron Ingot", 101));           //ID 101 : 철 주괴
        playerInventory.AddItem(new CraftingMaterial("Wood", 102));                 //ID 102 : 나무

        playerInventory.AddItem(new CraftingMaterial("Herb", 201));                 //ID 201 : 약초
        playerInventory.AddItem(new CraftingMaterial("Herb", 201));                 //ID 201 : 약초
        playerInventory.AddItem(new CraftingMaterial("Water", 202));                //ID 202 : 물
    }

    //인벤토리 접근자 메서드 추가
    public Inventory<IItem> GetInventory()
    {

        return playerInventory; 

    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            playerInventory.ListItems();        //인벤토리 내용 출력
        }

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            playerInventory.UseItem(UseBagIndex);         //첫번째 아이템 사용
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            playerInventory.AddItem(new Weapon("Sword", 1, 10));            //아이템 생성
        }
    }
}