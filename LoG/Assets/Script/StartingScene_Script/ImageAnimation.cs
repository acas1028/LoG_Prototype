using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageAnimation : MonoBehaviour
{
    public GameObject TitleImage;
    public GameObject BackGroundImage;
    public GameObject MessageObject;
    public GameObject PressScreenButton;
    public GameObject OptionButton;
    public GameObject StartButton;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ActivateBackGround());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator ActivateBackGround()
    {
        int i = 0;

        while(i < 255)
        { 
            yield return new WaitForSeconds(0.01f);
            Color newcolor = new Color(1.0f,1.0f,1.0f,i/255f);
            BackGroundImage.GetComponent<Image>().color = newcolor;
            i++;
        }
       
        
        if(i == 255)
        {
            StartCoroutine(ActivateTitle());
            yield break;
        }
    }

    IEnumerator ActivateTitle()
    {
        int i = 0;

        while (i < 255)
        {
            yield return new WaitForSeconds(0.005f);
            Color newcolor = new Color(1.0f, 1.0f, 1.0f, i / 255f);
            TitleImage.GetComponent<Image>().color = newcolor;
            i++;
        }


        if (i == 255)
        {
            PressScreenButton.SetActive(true);
            MessageObject.SetActive(true);
            yield break;
        }
    }

    public void PressScreen()
    {

    }
    
    IEnumerator CPressScreen()
    {
        int i = 0;

        while(i < 350)
        {
            Vector3 position = new Vector3(0, i, 0);
            yield return new WaitForSeconds(0.005f);
            TitleImage.transform.position = position;
            i++;
        }

        if(i >= 350)
        {
            OptionButton.SetActive(true);
            yield break;
        }
    }

    IEnumerator ActivateButton()
    {
        int i = 0;

        while (i < 255)
        {
            yield return new WaitForSeconds(0.01f);
            Color newcolor = new Color(1.0f, 1.0f, 1.0f, i / 255f);
            OptionButton.GetComponent<Image>().color = newcolor;
            i++;
        }


        if (i == 255)
        {
            StartCoroutine(ActivateTitle());
            yield break;
        }
    }
}
