using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Samples.Scripts
{
    public class UIToolkitInventorySystem : MonoBehaviour
    {
        [SerializeField] private UIDocument uiDocument;
        [SerializeField] private string itemsGridID;
        [SerializeField] private VisualTreeAsset itemTemplate;
        [SerializeField] private List<InventoryItemData> itemsData;
        
        private void Start()
        {
            if (!uiDocument.isActiveAndEnabled) return;
            
            InstantiateItems();
        }

        private void InstantiateItems()
        {
            var itemsGrid = uiDocument.rootVisualElement.Q(itemsGridID);
            
            foreach (var itemData in itemsData)
            {
                var itemInstance = itemTemplate.Instantiate().contentContainer;
                
                var button = itemInstance.Q<Button>();
                var icon = itemInstance.Q("Icon");

                button.clicked += () => Debug.Log($"Item {itemData.name} clicked");
                
                icon.style.backgroundImage = new StyleBackground(itemData.icon);
                icon.style.unityBackgroundImageTintColor = itemData.color;
                
                itemsGrid.Add(itemInstance);
            }
        }
    }
}
