using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraComponent : MonoBehaviour
{
    // 각 게임의 Scene스크립트에서 초기화 해줄것  
    public Vector3 delta = new Vector3(0.0f, 6.0f, -5.0f);

    GameObject player = null;

    private void LateUpdate()
    {
        if (player == null) return;

        RaycastHit hit;
        if (Physics.Raycast(player.transform.position, delta, out hit, delta.magnitude, LayerMask.GetMask("Block")))
        {
            float dist = (hit.point - player.transform.position).magnitude * 0.8f;
            transform.position = player.transform.position + delta.normalized * dist;
        }
        else
        {
            transform.position = player.transform.position + delta;
            transform.LookAt(player.transform);
        }
    }
    public void SetPlayer(GameObject p)
    {
        player = p;
    }
}
