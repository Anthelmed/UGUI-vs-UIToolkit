using UnityEngine;
using UnityEngine.UIElements;

//Advanced version: https://gist.githubusercontent.com/Anthelmed/5fcc49a402ad5c81136c73ce96ba004f/raw/b4b2c1527974ccf4aeb8e98e62feeef83a1a7bf0/WorldSpaceUIDocument.cs

namespace Samples.Scripts
{
    [RequireComponent(typeof(UIDocument))]
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshCollider))]
    public class WorldSpaceUIDocument : MonoBehaviour
    {
        [SerializeField] private UIDocument uiDocument;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private MeshCollider meshCollider;
        
        private void Start()
        {
            uiDocument.panelSettings.SetScreenToPanelSpaceFunction(ConvertScreenSpacePositionToPanelSpacePosition);
        }
        
        private Vector2 ConvertScreenSpacePositionToPanelSpacePosition(Vector2 screenPosition)
        {
            screenPosition.y = Screen.height - screenPosition.y;
            var ray = mainCamera.ScreenPointToRay(screenPosition);

            if (!meshCollider.Raycast(ray, out var hit, Mathf.Infinity)) return new Vector2(float.NaN, float.NaN);

            var targetTexture = uiDocument.panelSettings.targetTexture;
            var textureCoord = hit.textureCoord;
        
            textureCoord.y = 1 - textureCoord.y;
            textureCoord *= new Vector2(targetTexture.width, targetTexture.height);

            return textureCoord;
        }
    }
}