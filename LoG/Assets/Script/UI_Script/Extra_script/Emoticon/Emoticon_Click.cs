using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;

public class Emoticon_Click : MonoBehaviourPun
{
    [SerializeField]
    GameObject prefab_Block_Mine; //내 이모티콘이 나타나는 위치 블럭

    [SerializeField]
    GameObject prefab_Block_Enemy; //적 이모티콘이 나타나는 위치 블럭

    [SerializeField]
    Image[] emoticon_Images;

    bool isReady;
    Queue<float> timeQueue;

    GameObject emoticon_Block_Mine; //내 이모티콘이 나타나는 위치
    GameObject emoticon_Block_Enemy; //적 모티콘이 나타나는 위치

    private void Start()
    {
        isReady = true;
        timeQueue = new Queue<float>();
    }

    public void OnClick(int emoticonNum) //이모티콘 클릭시 구현되는 함수
    {
        if (!isReady)
            return;

        float time;

        time = Time.time;
        timeQueue.Enqueue(time);

        emoticonNum--;

        if (emoticon_Block_Mine)
            Destroy(emoticon_Block_Mine);

        emoticon_Block_Mine = Instantiate(prefab_Block_Mine, GameObject.Find("Canvas").transform);
        emoticon_Block_Mine.GetComponent<RectTransform>().anchoredPosition = new Vector2(-450.0f, 600.0f);
        emoticon_Block_Mine.transform.GetChild(0).GetComponent<Image>().sprite = emoticon_Images[emoticonNum].sprite;

        photonView.RPC("ShowEnemyEmoticon", RpcTarget.Others, emoticonNum);

        if (timeQueue.Count >= 5)
        {
            if (time - timeQueue.Dequeue() <= 6)
            {
                Debug.Log("이모티콘 쿨타임 발생! 10초간 이모티콘을 사용할 수 없다.");
                isReady = false;
                Invoke("SetReady", 10.0f);
            }
        }
    }

    [PunRPC]
    private void ShowEnemyEmoticon(int emoticonNum)
    {
        if (emoticon_Block_Enemy)
            Destroy(emoticon_Block_Enemy);

        emoticon_Block_Enemy = Instantiate(prefab_Block_Enemy, GameObject.Find("Canvas").transform);
        emoticon_Block_Enemy.GetComponent<RectTransform>().anchoredPosition = new Vector2(450.0f, 600.0f);
        emoticon_Block_Enemy.GetComponent<Image>().sprite = emoticon_Images[emoticonNum].sprite;
    }

    private void SetReady()
    {
        isReady = true;
    }
}