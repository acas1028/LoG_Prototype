using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagedGridControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damaged()
    {
        StartCoroutine(CDamaged());
    }

    IEnumerator CDamaged()
    {
        this.gameObject.GetComponent<SpriteRenderer>().color = Color.red;

        yield return new WaitForSeconds(BattleManager.Instance.bM_AttackTimegap);

        this.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }
}
