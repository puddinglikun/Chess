using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 点击测试脚本
/// </summary>
public class CheckTouchRay : MonoBehaviour
{
    public LayerMask gridMask;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit2 = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, gridMask);
            if (hit2.collider != null)
                Debug.Log(hit2.collider.gameObject.name);
        }
            
    }
}
