using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Samples.Scripts
{
    public class UGUIInventorySystem : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] private Transform itemsGrid;
        [SerializeField] private Transform itemPrefab;
        [SerializeField] private List<InventoryItemData> itemsData;

        private void Start()
        {
            if (!canvas.isActiveAndEnabled) return;
            
            InstantiateItems();
        }

        private void InstantiateItems()
        {
            foreach (var itemData in itemsData)
            {
                var itemInstance = Instantiate(itemPrefab, itemsGrid);
                
                var button = itemInstance.GetComponent<Button>();
                var icon = itemInstance.Find("Icon").GetComponent<Image>();
                
                button.onClick.AddListener(() => Debug.Log($"Item {itemData.name} clicked"));
                
                icon.sprite = itemData.icon;
                icon.color = itemData.color;
            }
        }
    }
}
