using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;


public static class Extension
{
	#region Unity
	public static T Find<T>(this Transform this_, string name_) where T : Component
	{
		if (null == this_)
			return null;

		Transform tm = this_.Find(name_);
		if (null == tm)
			return null;

		return tm.GetComponent(typeof(T)) as T;
	}

	public static void AddEvent(this Button this_, System.Action act_)
	{
		if (null == this_ || null == act_)
			return;

		this_.onClick.AddListener(() => act_());
	}

    public static void Show(this Component this_)
    {
        if (null == this_)
            return;

        if (false == this_.gameObject.activeSelf)
            this_.gameObject.SetActive(true);
    }

    public static void Hide(this Component this_)
    {
        if (null == this_)
            return;

        if (true == this_.gameObject.activeSelf)
            this_.gameObject.SetActive(false);
    }

    public static void Show(this Transform this_)
	{
		if (null == this_)
			return;

		if (false == this_.gameObject.activeSelf)
			this_.gameObject.SetActive(true);
	}

	public static void Hide(this Transform this_)
	{
		if (null == this_)
			return;

		if (true == this_.gameObject.activeSelf)
			this_.gameObject.SetActive(false);
	}

	public static void SetActive(this Transform this_, bool isActive_)
	{
		if (null == this_)
			return;

		if (isActive_ != this_.gameObject.activeSelf)
			this_.gameObject.SetActive(isActive_);
	}

	public static bool GetActive(this Transform this_)
	{
		if (null == this_)
			new UnityException ("Transform null where Extention method GetActive()");

		return this_.gameObject.activeSelf;
	}

	public static void Show(this Selectable this_)
	{
		if (null == this_)
			return;

		if (false == this_.gameObject.activeSelf)
			this_.gameObject.SetActive(true);
	}

	public static void Hide(this Selectable this_)
	{
		if (null == this_)
			return;

		if (true == this_.gameObject.activeSelf)
			this_.gameObject.SetActive(false);
	}

    public static void SetActive(this Selectable this_, bool isActive_)
    {
        if (null == this_)
            return;

        if (isActive_ != this_.gameObject.activeSelf)
            this_.gameObject.SetActive(isActive_);
    }

    public static void Show(this MaskableGraphic this_)
    {
        if (null == this_)
            return;

        if (false == this_.gameObject.activeSelf)
            this_.gameObject.SetActive(true);
    }

    public static void Hide(this MaskableGraphic this_)
    {
        if (null == this_)
            return;

        if (true == this_.gameObject.activeSelf)
            this_.gameObject.SetActive(false);
    }
    
    public static void Show(this Graphic this_)
    {
        if (null == this_)
            return;

        if (false == this_.gameObject.activeSelf)
            this_.gameObject.SetActive(true);
    }

    public static void Hide(this Graphic this_)
    {
        if (null == this_)
            return;

        if (true == this_.gameObject.activeSelf)
            this_.gameObject.SetActive(false);
    }

    public static void SetActive(this Graphic this_, bool isActive_)
    {
        if (null == this_)
            return;

        if (isActive_ != this_.gameObject.activeSelf)
            this_.gameObject.SetActive(isActive_);
    }

    public static void SetSprite(this Image this_, Sprite spr_)
    {
        if (null == this_)
            return;

        if (null == spr_)
        {
            this_.Hide();
        }
        else
        {
            this_.Show();
            this_.sprite = spr_;
        }
    }

    public static void AsyncOperator(this MonoBehaviour this_, UnityEngine.AsyncOperation operation_, Action<float> progress_, Action<AsyncOperation> complete_) {
        this_.StartCoroutine(AsynCoroutine(operation_, progress_, complete_));
    }

    private static IEnumerator AsynCoroutine(UnityEngine.AsyncOperation operation_, Action<float> progress_, Action<AsyncOperation> complete_)
    {
        while (!operation_.isDone)
        {
            progress_(operation_.progress);
            yield return null;
        }
        complete_(operation_);
    }

    public static void SetInteractable(this Button this_, bool isActive_, Sprite enable_ = null, Sprite disable_ = null)
    {
        if (null == this_)
            return;

        this_.interactable = isActive_;

        Image img = this_.GetComponent<Image>();
        if (null != img && null != enable_ && null != disable_)
        {
            img.sprite = isActive_ ? enable_ : disable_;
        }

        WhiteMaskButton white = this_.GetComponent<WhiteMaskButton>();
        if (null != white)
        {
            white.enabled = isActive_;
        }
    }
    #endregion


    #region String
    public static bool IsNullOrEmpty(this string this_)
	{
		return string.IsNullOrEmpty(this_);
	}
	#endregion


	#region Collection
	public static bool IsNullOrEmpty(this ICollection this_)
	{
        if (null == this_)
            return true;
		
		return 0 >= this_.Count;
	}

    public static int GetCount(this ICollection this_)
    {
        if (null == this_)
            return 0;

        return this_.Count;
    }
	
	public static int GetIndexValue(this int[,] this_, int index)
	{
		int w=index*this_.GetLength(0);
		return this_[index/w,index%w ];
	}
	
	#endregion


	#region JsonData
	//public static bool Contains(this Newtonsoft.Json.Linq.JToken this_, string key)
	//{
	//	if (this_ == null || this_[key] == null)
	//		return false;
	//	return this_[key].HasValues;
	//}
	//public static bool Contains(this LitJson.JsonData this_, string key)
	//{
	//	bool result = false;
	//	if (this_ == null)
	//		return result;
	//	if (!this_.IsObject)
	//	{
	//		return result;
	//	}
	//	IDictionary tdictionary = this_ as IDictionary;
	//	if (tdictionary == null)
	//		return result;
	//	if (tdictionary.Contains(key))
	//	{
	//		result = true;
	//	}
	//	return result;
	//}
	#endregion


	#region UniRx
	public static void DisposeEx(this IDisposable this_)
    {
        if (null == this_)
            return;

        this_.Dispose();
        this_ = null;
    }
    #endregion

}
