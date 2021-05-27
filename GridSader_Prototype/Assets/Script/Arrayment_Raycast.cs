using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Arrayment_Raycast: MonoBehaviour
{
    private bool Character_instance = true;


    [Tooltip("프리펩된 캐릭터")]
    [SerializeField]
    private GameObject Prefeb_Caracter;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (Character_instance == true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 1000);
                Vector3 HitPos = hit.collider.gameObject.transform.position;//Hitpos에 충돌 위치를 저장함.
                if (hit.transform.CompareTag("Tile"))//타일로 태그 되어 있는 물체에게 raycast가 닿으면.
                {
                    //Instantiate(Prefeb_Caracter, HitPos, Quaternion.identity);//충돌된 위치에 있는 오브젝트에 Prefeb을 생성한다.
                    GameObject Character_Instantiate = hit.collider.gameObject;
                    Debug.Log(Character_Instantiate);
                    Character_Instantiate.GetComponent<Character_Script>().character_ID_Number=GetComponent<Inventory_ID>().Character_ID_Inventory;
                    //작동함.
                    Character_Instantiate.GetComponent<SpriteRenderer>().sprite = GetComponent<Inventory_ID>().Character_Image.sprite;
                    //모르겠음ㅎㅎ;
                    Character_instance = false;//두번 소환하지는 못하게 한다.
                }
                /*else if(hit.transform.CompareTag("Character"))
                {
                    //그리드에서 그리드로 배치한다.
                }*/
                else
                {
                    Debug.Log("대상이 없습니다");
                }

            }
        }
    }

    public void IsCharacterButton()
    {
        Character_instance = true;
    }
}