using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;

public class Emoticon_Click : MonoBehaviourPun
{
    [SerializeField]
    GameObject prefab_Block_Mine; //�� �̸�Ƽ���� ��Ÿ���� ��ġ ��

    [SerializeField]
    GameObject prefab_Block_Enemy; //�� �̸�Ƽ���� ��Ÿ���� ��ġ ��

    [SerializeField]
    Image[] emoticon_Images;

    bool isReady;
    Queue<float> timeQueue;

    GameObject emoticon_Block_Mine; //�� �̸�Ƽ���� ��Ÿ���� ��ġ
    GameObject emoticon_Block_Enemy; //�� ��Ƽ���� ��Ÿ���� ��ġ

    private void Start()
    {
        isReady = true;
        timeQueue = new Queue<float>();
    }

    public void OnClick(int emoticonNum) //�̸�Ƽ�� Ŭ���� �����Ǵ� �Լ�
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
                Debug.Log("�̸�Ƽ�� ��Ÿ�� �߻�! 10�ʰ� �̸�Ƽ���� ����� �� ����.");
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