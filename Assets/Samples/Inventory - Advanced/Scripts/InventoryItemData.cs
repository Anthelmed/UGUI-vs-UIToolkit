using UnityEngine;

namespace Samples.Scripts
{
    [CreateAssetMenu(fileName = "Inventory Item Data", menuName = "Samples/Inventory/Item Data")]
    public class InventoryItemData : ScriptableObject
    {
        public string name = "Item";
        public Sprite icon;
        public Color color = Color.white;
    }
}
