using UnityEngine;
using System.Collections;
//using DG.Tweening;
using System;


public class UIPopup : MonoBehaviour
{

	#region VARIABLES
    public Action OnBackEvent = null;

    [SerializeField]
    private bool _isOpenPopupAction = true;
  
    public bool IsEnableBackBtn = true;
    #endregion


    #region PROPERTIES

    public bool IsActive
    {
        get
        {
            if (null == gameObject)
                return false;

            return gameObject.activeSelf;
        }
    }
    #endregion


    #region METHODS - reserved
    protected virtual void Awake() { }

	public virtual void OnBackKeyEvent() {
		Close ();
	}

    #endregion


    #region METHODS - base
    public virtual void Init(params object[] args_)
    {
        //if (UIManager.a.IsSound)
        //    Sound.Play("Meta_se_button_1");

		if (_isOpenPopupAction)
		{
			Animator ani = gameObject.AddComponent<Animator>();
			if (null != ani)
			{
				//ani.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("meta/ani/UIPopupAction");
			}
		}
    }

    public virtual void Destroy()
    {
        if (null == gameObject || !gameObject.activeSelf)
            return;

        //if (UIManager.a.IsSound)
        //    Sound.Play("Meta_se_button_5");
		if (OnBackEvent != null)
			OnBackEvent ();
		Destroy(gameObject);
    }
    #endregion

    #region METHODS - public

    public void Close()
    {
		Destroy ();
    }

    #endregion


}
