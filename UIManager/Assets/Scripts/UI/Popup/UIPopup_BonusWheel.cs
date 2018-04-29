//using Data;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;


public class UIPopup_BonusWheel : UIPopup
{
    private const string WHEEL_PATH = "Dialog/BonusWheel/ALL/WHEELS/wheel/";
    private readonly long[,] setTable =
        {
            { 15000, 20000, 30000, 50000, 60000, 80000, 90000, 1000000 },
            { 30000, 40000, 60000, 100000, 120000, 160000, 180000, 2000000 },
            { 75000, 100000, 150000, 250000, 300000, 400000, 450000, 5000000 },
            { 150000, 200000, 300000, 500000, 600000, 800000, 900000, 100000000 }
        };
    private static readonly long[,] winTable =
        {
            { 20000, 60000, 80000, 15000, 90000, 50000, 30000, 1000000 },
            { 40000, 120000, 160000, 30000, 180000, 100000, 60000, 2000000 },
            { 100000, 300000, 400000, 75000, 450000, 250000, 150000, 5000000 },
            { 200000, 600000, 800000, 150000, 900000, 500000, 300000, 100000000 }
        };


    #region VARIABLES
    //private GameObject money_prefab;
    //private GameObject result1_prefab;
    //private GameObject result2_prefab;
    private GameObject WheelObject;
    private GameObject PinObject;
    private GameObject SpinAnimObj;
    private GameObject SpinButtonEffectObj;
    private Image bg;
    private Text[] wheelcoin = new Text[8];

    private int Type;
    private bool Working;
    private DG.Tweening.Tweener tween;

    private Queue<int> _queueBonusWheel = new Queue<int>();
    #endregion


        #region METHODS - reserved
    protected override void Awake()
    {
        base.Awake();

        //money_prefab = Resources.Load<GameObject>("meta/Prefab/BN_BonusWheel_money");
        //result1_prefab = Resources.Load<GameObject>("meta/Prefab/BN_BonusWheel_result_01");
        //result2_prefab = Resources.Load<GameObject>("meta/Prefab/BN_BonusWheel_result_02");
        WheelObject = transform.Find("Dialog/BonusWheel/ALL/WHEELS/wheel").gameObject;
        PinObject = transform.Find("Dialog/BonusWheel/ALL/Upper/pin").gameObject;
        SpinAnimObj = transform.Find("Dialog/Spin_Word").gameObject;
        SpinButtonEffectObj = transform.Find("Dialog/BonusWheel/ALL/WHEELS/center_white_eff").gameObject;
        bg = transform.Find(WHEEL_PATH + "bg").GetComponent<Image>();

        for (int i = 0; i < 8; i++)
        {
            wheelcoin[i] = transform.Find(WHEEL_PATH + (i + 1).ToString()).GetComponent<Text>();
        }

		transform.Find<Button>("Dialog/BonusWheel/ALL/WHEELS/wheel center").AddEvent(OnClick_BtnCollect);
    }
    #endregion


    #region METHODS - base
    public override void Init(params object[] args_)
    {
        base.Init(args_);

        if (args_.IsNullOrEmpty())
            return;

        Working = false;
        _queueBonusWheel = args_[0] as Queue<int>;
        SetUp(_queueBonusWheel.Dequeue().ToString());
    }
    #endregion


    #region METHODS - private
    private void SetUp(string bonusWheel_)
    {
        Type = Int32.Parse(bonusWheel_);

        bg.sprite = Resources.Load<Sprite>("meta/images/Bonus/wheel_bg_" + bonusWheel_);

        for (int i = 0; i < 8; i++)
        {
            wheelcoin[i].text = (setTable[Int32.Parse(bonusWheel_) - 1, i]).ToString("N0");
        }
    }

    private void OnSpin()
    {
        if (Working)
            return;

        Working = true;
        SpinAnimObj.SetActive(false);
        SpinButtonEffectObj.SetActive(false);
        
        //NetworkManager.AddActionLog(
            //ActionLog.USER_ACTION_CATEGORY_WHEEL,
            //ActionLog.USER_ACTION_WHEEL_SPIN_BUTTON,
            //String.Format("Type: {0}", Type));
        
        //NetworkManager.instance.CollectWheel(Type, result =>
        //{

            tween = WheelObject.transform.DORotate(new Vector3(0f, 0f, -360f * 8), 7, RotateMode.FastBeyond360).SetLoops(-1);
        Type = 1;
            if (Type < 1)  // 서버 값이 잘못 들어오면 0으로 들어와서 강제로 종료.
            {
				Close();
            }
            else
            {
                //Sound.Play("Meta_roulette");

            long win = 777;//result.Win;
                int index;
                for (index = 0; index < 8; index++)
                {
                    if (winTable[Type - 1, index] == win)
                        break;
                }

                Debug.LogFormat("OnSpin(Type:{0}, index:{1})", Type, index);

                if (index < 8)
                    Turn(index);
            }
        //});
    }

    private void Turn(int stopIndex)
    {
        Debug.LogFormat("Turn({0})", stopIndex);

        const int rotateNum = 8;
        float endPos = 360f * -rotateNum + (360 / 8 * (stopIndex + 1));
        long win = winTable[Type - 1, stopIndex];
        GameObject parentObj = transform.parent.gameObject;
        tween.Kill();
        tween = WheelObject.transform.DORotate(new Vector3(0f, 0f, -360f * 8), 7, RotateMode.FastBeyond360);
        tween.ChangeEndValue(new Vector3(0f, 0f, endPos), transform.rotation.eulerAngles.z).OnComplete(() => 
        {
            // show result
            //GameObject objMoney = Instantiate(money_prefab, transform.parent);
            //objMoney.transform.Find("Money_text/8").GetComponent<TextMeshProUGUI>().text = win.ToString("#,#");
            //objMoney.SetActive(true);

            //GameObject objResult;
            //if (Type == 4)
            //{
            //    objResult = Instantiate(result2_prefab, transform.parent);
            //}
            //else
            //{
            //    objResult = Instantiate(result1_prefab, transform.parent);
            //}
            //objResult.SetActive(true);

            Observable.Timer(TimeSpan.FromSeconds(5)).Subscribe(_ =>
            {
				// 동전올라가는 소리 추가
				//Sound.Play("meta_coin_2", false);
				//Profile.instance.UpdateCoinAnimation(win + UserInfoManager.Point.Coin, Profile.AniType.SPREAD, parentObj.transform);


                //Destroy(objMoney);
                //Destroy(objResult);
				
			//	AniController.Create("meta/Prefab/Coin_Collect", parentObj.transform);

                if (_queueBonusWheel.IsNullOrEmpty())
					Close();
                else
                {
                    Working = false;
                    SetUp(_queueBonusWheel.Dequeue().ToString());
                }

                //LobbyLogic.DoNextDailyBonus();
            });
        });
    }
    #endregion


    #region METHODS - event
    private void OnClick_BtnCollect()
    {
        OnSpin();

        //NetworkManager.AddActionLog(ActionLog.USER_ACTION_CATEGORY_WHEEL, ActionLog.USER_ACTION_WHEEL_SPIN_BUTTON);
    }
    #endregion
}
