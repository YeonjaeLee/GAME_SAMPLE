using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIPopup_TwoBtn : UIPopup
{
	#region VARIABLES
	private Text _txtMsg;
	private Text _txtTitle;
	private Text _txtBtnLeft;
	private Text _txtBtnRight;
	private Action _actOnClickLeft;
	private Action _actOnClickRight;
	#endregion


	#region METHODS - reserved
	protected override void Awake()
	{
		base.Awake();

		_txtMsg = transform.Find<Text>("Dialog/Text_Msg");
		_txtTitle = transform.Find<Text>("Dialog/Text_Title");
		_txtBtnLeft = transform.Find<Text>("Dialog/Button_Left/Text");
		_txtBtnRight = transform.Find<Text>("Dialog/Button_Right/Text");
		transform.Find<Button>("Dialog/Button_Left").AddEvent(OnClick_BtnLeft);
		transform.Find<Button>("Dialog/Button_Right").AddEvent(OnClick_BtnRight);
		transform.Find<Button>("Dialog/close_button").AddEvent(OnClick_BtnRight);
    }
	#endregion


	#region METHODS - base
	#endregion


	#region METHODS - public

    public static UIPopup_TwoBtn Create(string msg_, string title_ = null, string btnMsgLeft_ = null, string btnMsgRight_ = null, Action actOnClickLeft_ = null, Action actOnClickRight_ = null)
    {
        UIPopup_TwoBtn popup = UIManager.a.OpenPopup<UIPopup_TwoBtn>();
        popup._txtMsg.text = false == msg_.IsNullOrEmpty() ? msg_ : string.Empty;
        popup._txtTitle.text = false == title_.IsNullOrEmpty() ? title_ : "ERROR"; //I2.Loc.ScriptLocalization.Get("ERROR");
        popup._txtBtnLeft.text = false == btnMsgLeft_.IsNullOrEmpty() ? btnMsgLeft_ : "NO"; //I2.Loc.ScriptLocalization.Get("NO");
        popup._txtBtnRight.text = false == btnMsgRight_.IsNullOrEmpty() ? btnMsgRight_ : "OK"; //I2.Loc.ScriptLocalization.Get("OK");
        popup._actOnClickLeft += actOnClickLeft_;
        popup._actOnClickRight += actOnClickRight_;
       
        return popup;
    }


	#endregion


	#region METHODS - event
	private void OnClick_BtnLeft()
	{
		Close();
		if (null != _actOnClickLeft)
		{
			_actOnClickLeft();
		}
		
	}

	private void OnClick_BtnRight()
	{
		Close();

		if (null != _actOnClickRight)
		{
			_actOnClickRight();
		}
		
	}

    #endregion
}
