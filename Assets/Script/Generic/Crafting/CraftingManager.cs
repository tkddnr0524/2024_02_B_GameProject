using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.CraftingSystem
{
    public class CraftingManager : MonoBehaviour
    {
        private static CraftingManager instance;
        public static CraftingManager Instance => instance;

        private Dictionary<string, Recipe> recipes = new Dictionary<string, Recipe>();
        private Inventory<IItem> playerInventory;
        public InventoryManager inventoryManager;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            if(inventoryManager != null)
            {
                playerInventory = inventoryManager.GetInventory();
                Debug.Log("�κ��丮 �������� ����");
            }
            else
            {
                Debug.LogError("InventoryManager no found !");
            }

            CreateSwordRecipe();
            CreatePotionRecipe();
        }

        private void CreateSwordRecipe()
        {
            var ironSword = new Weapon("Iron Sword", 1001, 10);
            var recipe = new Recipe("RECIPE_IRON_SWORD" , ironSword , 1);
            recipe.AddRequiredMaterial(101, 2);          //Iron Ingot x 2
            recipe.AddRequiredMaterial(102, 1);          //Wood x 1
            recipes.Add(recipe.recipeId, recipe);
        }

        private void CreatePotionRecipe()
        {
            var healthPotion = new HealthPotion("Health Potion", 2001, 50);
            var recipe = new Recipe("RECIPE_HEALTH_POTION", healthPotion, 1);
            recipe.AddRequiredMaterial(201, 2);      //Herb  x2
            recipe.AddRequiredMaterial(202, 1);      //Water x1
            recipes.Add(recipe.recipeId, recipe);
        }

        public bool TryCraft(string recipeId)                               //���� �õ� 
        {
            if(!recipes.TryGetValue(recipeId , out Recipe recipe))
                return false;

            if(!CheckMaterials(recipe))
                return false;

            ConsumeMaterials(recipe);
            CreateResult(recipe);

            return true;
        }

        private bool CheckMaterials(Recipe recipe)                           //��� Ȯ�� �Լ� 
        {
            playerInventory = inventoryManager.GetInventory();

            foreach (var material in recipe.requiredMaterials)
            {
                if (!playerInventory.HasEnough(material.Key, material.Value))
                    return false;

            }
            return true;
        }

        private void ConsumeMaterials(Recipe recipe)                            //���ս� �����ǿ� �ִ� �ʿ� �������� ���� 
        {
            foreach (var material in recipe.requiredMaterials)
            {
                playerInventory.RemoveItems(material.Key, material.Value);  
            }
        }

        private void CreateResult(Recipe recipe)                                //���� �Ϸ�� �κ��丮�� ������ �߰� 
        {
            playerInventory.AddItem(recipe.resultItem);
        }

        public List<Recipe> GetAvailableRecipes()                       //������ ������ �����ϴ� �Լ� 
        {
            return new List<Recipe>(recipes.Values);
        }
    }


}
