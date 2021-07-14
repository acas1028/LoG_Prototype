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

    GameObject emoticon_Block_Mine; //�� �̸�Ƽ���� ��Ÿ���� ��ġ
    GameObject emoticon_Block_Enemy; //�� ��Ƽ���� ��Ÿ���� ��ġ

    public void Emoticon_click(int emoticonNum) //�̸�Ƽ�� Ŭ���� �����Ǵ� �Լ�
    {
        emoticonNum--;

        if (emoticon_Block_Mine)
            Destroy(emoticon_Block_Mine);

        emoticon_Block_Mine = Instantiate(prefab_Block_Mine, GameObject.Find("Canvas").transform);
        emoticon_Block_Mine.GetComponent<Image>().sprite = emoticon_Images[emoticonNum].sprite;
        photonView.RPC("Show_Enemy_Emoticon", RpcTarget.Others, emoticonNum);
    }

    [PunRPC]
    public void Show_Enemy_Emoticon(int emoticonNum)
    {
        if (emoticon_Block_Enemy)
            Destroy(emoticon_Block_Enemy);

        emoticon_Block_Enemy = Instantiate(prefab_Block_Enemy, new Vector3(450.0f, 450.0f, 0.0f), Quaternion.identity, GameObject.Find("Canvas").transform);
        emoticon_Block_Enemy.GetComponent<Image>().sprite = emoticon_Images[emoticonNum].sprite;
    }
}