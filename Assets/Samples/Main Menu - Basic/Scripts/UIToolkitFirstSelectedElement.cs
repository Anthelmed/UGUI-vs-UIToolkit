using UnityEngine;
using UnityEngine.UIElements;

namespace Samples.Scripts
{
    public class UIToolkitDefaultSelectedElement : MonoBehaviour
    {
        [SerializeField] private UIDocument uiDocument;
        [SerializeField] private string elementID;

        private void Reset()
        {
            uiDocument = GetComponent<UIDocument>();
        }

        private void Start()
        {
            if (!uiDocument.isActiveAndEnabled) return;
            
            SelectElement();
        }

        private void SelectElement()
        {
            var element = uiDocument.rootVisualElement.Q<VisualElement>(elementID);

            element?.Focus();
        }
    }
}
