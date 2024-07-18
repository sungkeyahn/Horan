using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMaterial : MonoBehaviour
{
    protected Renderer Renderer; 
    protected Material ori;

    [SerializeField]
    protected Material mat;
    //protected MeshRenderer meshRenderer;
    //protected SkinnedMeshRenderer skinnedMeshRenderer;

    private void Awake()
    {
        Renderer = GetComponent<Renderer>();
        ori = Renderer.material;
        //meshRenderer = GetComponent<MeshRenderer>();
        //skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        //ori = meshRenderer.material;
        //ori = skinnedMeshRenderer.material;
    }
    protected void Swap(bool reteurnFlag=false)
    {
        if (reteurnFlag)
            Renderer.material = ori;
        else
            Renderer.material = mat;
        /*        if (reteurnFlag)
            meshRenderer.material = ori;
        else
            meshRenderer.material = mat;*/
    }

}
