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

    public void UpdateAccountInfo() {
        nicknameText.text = UserDataSynchronizer.nickname;
    }
    
    public void UpdateUserInventory() {
        coinText.text = UserDataSynchronizer.coin.ToString();
        shopCoinText.text = UserDataSynchronizer.coin.ToString();
    }
}
