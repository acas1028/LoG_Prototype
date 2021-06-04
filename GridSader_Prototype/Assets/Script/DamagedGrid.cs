using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagedGrid : MonoBehaviour
{
    public GameObject Damaged_Grid;
    public Vector3[] Grid_Num; // 팀1 위치배열
    public GameObject go;
    // Start is called before the first frame update
    void Start()
    {
        Grid_Num = new Vector3[9];

        Damaged_Grid = Instantiate(Damaged_Grid);
        Damaged_Grid.GetComponent<SpriteRenderer>();
        BattleManager.Instance.bM_Character_Team1[0].GetComponent<SpriteRenderer>();

        Create_Grid();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void Create_Grid()
    {
        for (float x = -6.7f; x <= -2f;)
        {
            for (float y = -2f; y <= 3;)
            {
                Instantiate(Damaged_Grid, new Vector3(x, y, 0), Quaternion.identity);
                // BattleManager.Instance.bM_Character_Team1[0].material.color = Color.red;
                y += 2.25f;

            }

            x += 2.25f;
        }
        // (grid-1)%3 %3->열 결정
        for (float x = 2.2f; x <= 6.7f;)
        {

            for (float y = -2f; y <= 3;)
            {
                Instantiate(Damaged_Grid, new Vector3(x, y, 0), Quaternion.identity);
                y += 2.25f;

            }

            x += 2.25f;
        }
    }
}
