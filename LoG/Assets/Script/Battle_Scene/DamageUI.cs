using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageUI : MonoBehaviour
{
    private float moveSpeed;
    private float alphaSpeed;
    private float destroyTime;
    TextMeshProUGUI text;
    Color alpha;

    public int damage;
    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 55.0f;
        alphaSpeed = 2.0f;
        destroyTime = 2.0f;


        text = GetComponent<TextMeshProUGUI>();
        alpha = text.color;
        text.text = damage.ToString();
        Invoke("DestroyThis", destroyTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0, moveSpeed * Time.deltaTime, 0));

        alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * alphaSpeed); // 텍스트 알파값
        text.color = alpha;
    }

    private void DestroyThis()
    {
        Destroy(gameObject);
    }

    public Vector3 worldToUISpace(Canvas parentCanvas, Vector3 worldPos) //캔버스의 포지션과 월드의 포지션의 통로 역할을 해주는 함수
    {
        //Convert the world for screen point so that it can be used with ScreenPointToLocalPointInRectangle function
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        Vector2 movePos;

        //Convert the screenpoint to ui rectangle local point
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentCanvas.transform as RectTransform, screenPos, parentCanvas.worldCamera, out movePos);
        //Convert the local point to world point
        return parentCanvas.transform.TransformPoint(movePos);
    }
}
