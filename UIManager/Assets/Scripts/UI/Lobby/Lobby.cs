using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lobby : MonoBehaviour {

    public static Lobby instance;
    public Transform PopupOpenBtn;
    public Transform CheckBtn;
    private Text Touch;

	private void Awake()
	{
        instance = this;
        Touch = CheckBtn.Find<Text>("Text");
	}

	public void OnClick_PopupOpen ()
    {
        UIManager.a.OpenPopup<UIPopuo_Dummy>();
        PopupOpenBtn.SetActive(false);
	}
    public void OnClick_CheckTouchDown()
    {
        Touch.text = "IS TOUCH";
    }
    public void OnClick_CheckTouchUp()
    {
        Touch.text = "CHECK";
    }
}
