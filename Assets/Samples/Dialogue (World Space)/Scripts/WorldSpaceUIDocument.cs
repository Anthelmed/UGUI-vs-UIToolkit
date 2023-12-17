#if ENABLE_INPUT_SYSTEM
    using UnityEngine.InputSystem.UI;
#endif
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

//From https://gist.githubusercontent.com/Anthelmed/5fcc49a402ad5c81136c73ce96ba004f/raw/b4b2c1527974ccf4aeb8e98e62feeef83a1a7bf0/WorldSpaceUIDocument.cs

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

        private const string AssetsFolderName = "WorldSpaceUI";
        private static readonly Vector2Int DefaultResolution = new (1920, 1080);

        private MeshFilter _meshFilter;
        private MeshRenderer _meshRenderer;
        private RenderTexture _targetRenderTexture;
    
    
        private static readonly int MainTex = Shader.PropertyToID("_MainTex");
    
        private void Start()
        {
            uiDocument.panelSettings.SetScreenToPanelSpaceFunction(ConvertScreenSpacePositionToPanelSpacePosition);
        }

#if UNITY_EDITOR
        private void Reset()
        {
            if (!AssetDatabase.IsValidFolder($"Assets/{AssetsFolderName}"))
                AssetDatabase.CreateFolder("Assets", AssetsFolderName);
        
            InitializeCamera();
            InitializeUIDocument();
            InitializeMeshComponents();
        }

        private void InitializeCamera()
        {
            mainCamera = Camera.main;
        
            if (FindFirstObjectByType<EventSystem>() == null)
            {
#if ENABLE_INPUT_SYSTEM
            new GameObject("EventSystem", typeof(EventSystem), typeof(InputSystemUIInputModule));
#else
                new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
#endif
            }
        }

        private void InitializeMeshComponents()
        {
            meshCollider = GetComponent<MeshCollider>();
        
            var meshFilter = GetComponent<MeshFilter>();
            var meshRenderer = GetComponent<MeshRenderer>();
        
            var mesh = Resources.GetBuiltinResource<Mesh>("Quad.fbx");
            var material = new Material(Shader.Find("Unlit/Transparent"));
        
            meshCollider.sharedMesh = mesh;
            meshFilter.mesh = mesh;

            material.SetTexture(MainTex, _targetRenderTexture);
            meshRenderer.sharedMaterial = material;
            meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
        }
    
        private void InitializeUIDocument()
        {
            uiDocument = GetComponent<UIDocument>();

            _targetRenderTexture = new RenderTexture(DefaultResolution.x, DefaultResolution.y, 16);
            _targetRenderTexture.name = "WorldSpaceUIRenderTexture";
        
            var panelSettings = ScriptableObject.CreateInstance<PanelSettings>();
            panelSettings.name = "WorldSpaceUIPanelSettings";
            panelSettings.targetTexture = _targetRenderTexture;
            panelSettings.scaleMode = PanelScaleMode.ScaleWithScreenSize;
            panelSettings.screenMatchMode = PanelScreenMatchMode.MatchWidthOrHeight;
            panelSettings.referenceResolution = DefaultResolution;
            panelSettings.match = 0.5f;
            panelSettings.clearColor = true;
        
            uiDocument.panelSettings = panelSettings;
        
            var searchInFolders = new[] { $"Assets/{AssetsFolderName}" };
            var texturesGUIDs = AssetDatabase.FindAssets("t:RenderTexture", searchInFolders);
            var panelSettingsGUIDs = AssetDatabase.FindAssets("t:PanelSettings", searchInFolders);
        
            AssetDatabase.CreateAsset(panelSettings, $"Assets/{AssetsFolderName}/{panelSettings.name}{panelSettingsGUIDs.Length + 1}.asset");
            AssetDatabase.CreateAsset(_targetRenderTexture, $"Assets/{AssetsFolderName}/{_targetRenderTexture.name}{texturesGUIDs.Length + 1}.asset");
        
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    
#endif
    
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