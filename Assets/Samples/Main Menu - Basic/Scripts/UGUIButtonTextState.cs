using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Samples.Scripts
{
	public class UGUIButtonTextState : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, 
		IPointerExitHandler, ISelectHandler, IDeselectHandler
	{
		[SerializeField] private TextMeshProUGUI targetText;
		[SerializeField] private Color normalColor = ColorBlock.defaultColorBlock.normalColor;
		[SerializeField] private Color highlightedColor = ColorBlock.defaultColorBlock.highlightedColor;
		[SerializeField] private Color pressedColor = ColorBlock.defaultColorBlock.pressedColor;
		[SerializeField] private Color selectedColor = ColorBlock.defaultColorBlock.selectedColor;

		private bool IsSelected => EventSystem.current.currentSelectedGameObject == gameObject;
		
		private void Reset()
		{
			targetText = GetComponentInChildren<TextMeshProUGUI>();
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			StartColorTween(pressedColor);
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			StartColorTween(highlightedColor);
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			StartColorTween(highlightedColor);
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			var color = IsSelected ? selectedColor : normalColor;
			
			StartColorTween(color);
		}

		public void OnSelect(BaseEventData eventData)
		{
			StartColorTween(selectedColor);
		}

		public void OnDeselect(BaseEventData eventData)
		{
			StartColorTween(normalColor);
		}
		
		private void StartColorTween(Color targetColor)
		{
			if (targetText == null) return;

			targetText.CrossFadeColor(targetColor, ColorBlock.defaultColorBlock.fadeDuration, true, true);
		}
	}
}
