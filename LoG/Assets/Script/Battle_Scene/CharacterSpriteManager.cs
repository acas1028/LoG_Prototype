using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpriteManager : MonoBehaviour
{
    public Sprite[] characterSprites;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetInitialSprite(int id)
    {
        // 현재 ID 개수(14개)만큼의 스프라이트가 없어서 임시로 적용함
        // gameObject.GetComponent<SpriteRenderer>().sprite = Character_Sprite[Character_ID];
        transform.GetComponentInParent<SpriteRenderer>().sprite = characterSprites[(id - 1) % 5 + 1];
    }

    public void SetDeadSprite()
    {
        transform.GetComponentInParent<SpriteRenderer>().sprite = characterSprites[0];
    }
}
