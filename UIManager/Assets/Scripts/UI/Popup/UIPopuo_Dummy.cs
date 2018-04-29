using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPopuo_Dummy : UIPopup
{
    #region VARIABLES
    private Text txt;
    #endregion

    #region METHODS - reserved
    protected override void Awake()
    {
        base.Awake();
        txt = transform.Find<Text>("Body/Text");
        transform.Find<Button>("Body/OkButton").AddEvent(OnclickBtn_Ok);
        transform.Find<Button>("Body/CloseButton").AddEvent(OnclickBtn_Close);
    }
    #endregion

    #region METHODS - base
    public virtual void Init(params object[] args_)
    {
        base.Init(args_);

        SetUI();
    }
    #endregion

    #region METHODS - private
    private void SetUI()
    {
        txt.text = "";
    }
    private void OnclickBtn_Ok()
    {
        txt.text = "OK";
    }
    private void OnclickBtn_Close()
    {
        Lobby.instance.PopupOpenBtn.SetActive(true);
        Close();
    }
    #endregion
}
