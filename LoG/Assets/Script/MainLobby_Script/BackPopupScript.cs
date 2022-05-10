using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackPopupScript : MonoBehaviour
{
    [SerializeField]
    GameObject backPopup;

    // Update is called once per frame
    void Update()
    {
        if(Application.platform == RuntimePlatform.Android)
        {
            if(Input.GetKey(KeyCode.Escape))
            {
                backPopup.SetActive(true);
            }
        }
    }

    public void GameQuit()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            Application.Quit();
        }
    }
}
