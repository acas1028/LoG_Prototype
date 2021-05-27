using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Arrayment_Raycast: MonoBehaviour
{
    private bool Character_instance = true;


    [Tooltip("������� ĳ����")]
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
                Vector3 HitPos = hit.collider.gameObject.transform.position;//Hitpos�� �浹 ��ġ�� ������.
                if (hit.transform.CompareTag("Tile"))//Ÿ�Ϸ� �±� �Ǿ� �ִ� ��ü���� raycast�� ������.
                {
                    //Instantiate(Prefeb_Caracter, HitPos, Quaternion.identity);//�浹�� ��ġ�� �ִ� ������Ʈ�� Prefeb�� �����Ѵ�.
                    GameObject Character_Instantiate = hit.collider.gameObject;
                    Debug.Log(Character_Instantiate);
                    Character_Instantiate.GetComponent<Character_Script>().character_ID_Number=GetComponent<Inventory_ID>().Character_ID_Inventory;
                    //�۵���.
                    Character_Instantiate.GetComponent<SpriteRenderer>().sprite = GetComponent<Inventory_ID>().Character_Image.sprite;
                    //�𸣰�������;
                    Character_instance = false;//�ι� ��ȯ������ ���ϰ� �Ѵ�.
                }
                /*else if(hit.transform.CompareTag("Character"))
                {
                    //�׸��忡�� �׸���� ��ġ�Ѵ�.
                }*/
                else
                {
                    Debug.Log("����� �����ϴ�");
                }

            }
        }
    }

    public void IsCharacterButton()
    {
        Character_instance = true;
    }
}