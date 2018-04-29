using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    #region CONSTANTS
    private const string TAG_LOBBY_KEY = "Lobby";
    private const string PATH_UI_PREFAB_LOBBY_BG = "UI/Prefab/Lobby/BG";
    #endregion

    #region METHODS - reserved
    private void Awake () {
        DontDestroyOnLoad(this.gameObject);
        GameObject LobbyCanvas = GameObject.FindGameObjectWithTag(TAG_LOBBY_KEY);
        if (null == LobbyCanvas)
        {
            Debug.LogError("CustomUI Tag Find Failed!!");
        }
        else
        {
            GameObject Bg = Resources.Load<GameObject>(PATH_UI_PREFAB_LOBBY_BG);
            if (null == Bg)
            {
                Debug.LogError("Path : " + PATH_UI_PREFAB_LOBBY_BG + ", Prefab Load Failed!!");
            }
            else
            {
                GameObject LobbyBg = Instantiate<GameObject>(Bg, LobbyCanvas.transform, false);
                LobbyBg.transform.SetAsFirstSibling();
            }
        }
	}
    #endregion
}
