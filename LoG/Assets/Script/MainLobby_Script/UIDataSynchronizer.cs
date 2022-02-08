using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;

public class UIDataSynchronizer : MonoBehaviour
{
    [SerializeField] Text nicknameText;
    [SerializeField] Text coinText;
    [SerializeField] Text shopCoinText;

    public void SetNickName(string nickname) {
        nicknameText.text = nickname;
    }

    public void SetCoin(string coin) {
        coinText.text = coin;
        shopCoinText.text = coin;
    }
}
