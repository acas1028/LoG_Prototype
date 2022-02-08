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

    private IEnumerator Start() {
        yield return new WaitUntil(() => UserDataSynchronizer.Instance.isAllDataLoaded);

        nicknameText.text = UserDataSynchronizer.Instance.nickname;
        coinText.text = UserDataSynchronizer.Instance.coin.ToString();
        shopCoinText.text = UserDataSynchronizer.Instance.coin.ToString();
    }
}
