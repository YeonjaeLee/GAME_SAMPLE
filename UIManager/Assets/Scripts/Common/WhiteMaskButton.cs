using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
#if UNITY_EDITOR
using UnityEditor;
public class WhiteMaskButtonEditor : Editor
{
	[MenuItem("GameObject/Add White Mask Image", false, 0)]
	static void Init()
	{
		GameObject s = Selection.activeGameObject;

		if (s != null && s.GetComponent<WhiteMaskButton>()==null) {
			Button btn = s.GetComponent<Button> ();
			if (btn != null) {
				Graphic g = btn.targetGraphic;
				if (btn.transition == Selectable.Transition.ColorTint)
					btn.transition = Selectable.Transition.None;
				g.material = new Material (Shader.Find ("Sprites/DefaultColorFlash"));
				s.AddComponent<WhiteMaskButton> ();
			}

		}
	}

}

#endif

[RequireComponent(typeof(Button))]
public class WhiteMaskButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
	Graphic m_image;

	public float TargetAmount=2f;

	void Awake ()
	{
		m_image = GetComponent<Button> ().targetGraphic as Graphic;
		m_image.material = new Material(Shader.Find ("Sprites/DefaultColorFlash"));
	}

	void OnEnable ()
	{
		m_image.material.SetFloat ("_FlashAmount", 1f);
	}

	public void OnPointerDown (PointerEventData eventData)
	{
		
		m_image.material.SetFloat ("_FlashAmount", TargetAmount);
	}

	public void OnPointerUp (PointerEventData eventData)
	{
		
		m_image.material.SetFloat ("_FlashAmount", 1f);
	}

	public void OnPointerExit (PointerEventData eventData)
	{
		OnPointerUp (eventData);
	}

}