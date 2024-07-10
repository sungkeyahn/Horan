using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraComponent : MonoBehaviour
{
    GameObject player = null;
    // 각 게임의 Scene스크립트에서 초기화 해줄것  
    [SerializeField] Vector3 delta = new Vector3(0.0f, 6.0f, -5.0f);

    [SerializeField] Vector3 minCameraBoundary;
    [SerializeField] Vector3 maxCameraBoundary;

    private void LateUpdate()
    {
        if (player == null) return;

        
        RaycastHit hit;
        if (Physics.Raycast(player.transform.position, delta, out hit, delta.magnitude, LayerMask.GetMask("Block")))
        {
            Debug.Log("Camera Zoom In");
            float dist = (hit.point - player.transform.position).magnitude * 0.8f;
            transform.position = player.transform.position + delta.normalized * dist;
        }
        else
        {
            Vector3 pos = player.transform.position + delta;
           
            pos.x = Mathf.Clamp(pos.x, minCameraBoundary.x, maxCameraBoundary.x);
            pos.z = Mathf.Clamp(pos.z, minCameraBoundary.z, maxCameraBoundary.z);
            transform.position = pos;
            //transform.LookAt(player.transform);
        }

    }
    public void SetPlayer(GameObject p)
    {
        player = p;
    }
}

