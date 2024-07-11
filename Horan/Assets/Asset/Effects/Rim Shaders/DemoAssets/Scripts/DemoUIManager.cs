using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemoUIManager : MonoBehaviour
{
    public Slider rim_StandardPBR_R_Slider;
    public Slider rim_StandardPBR_G_Slider;
    public Slider rim_SandardPBR_B_Slider;
    public Slider rim_StandardPBR_Power_Slider;

    public Slider rim_BumpedSpecular_R_Slider;
    public Slider rim_BumpedSpecular_G_Slider;
    public Slider rim_BumpedSpecular_B_Slider;
    public Slider rim_BumpedSpecular_Power_Slider;

    public GameObject rim_StandardPBR_Model;
    public GameObject rim_BumpedSpecular_Model;

    private SkinnedMeshRenderer[] m_Rim_StandardPBR_Model_MeshRenderers;
    private SkinnedMeshRenderer[] m_Rim_Bumped_Specular_Model_MeshRenderers;


    // Use this for initialization
    void Start()
    {
        m_Rim_StandardPBR_Model_MeshRenderers = rim_StandardPBR_Model.GetComponentsInChildren<SkinnedMeshRenderer>();

        for (int i = 0; i < m_Rim_StandardPBR_Model_MeshRenderers.Length;i++ )
        {
            SkinnedMeshRenderer meshRenderer = m_Rim_StandardPBR_Model_MeshRenderers[i];
            Material[] materials = meshRenderer.materials;

            for (int j = 0; j < materials.Length;j++ )
            {
                Material material = materials[j];
                rim_StandardPBR_R_Slider.value = material.GetColor("_RimColor").r;
                rim_StandardPBR_G_Slider.value = material.GetColor("_RimColor").g;
                rim_SandardPBR_B_Slider.value = material.GetColor("_RimColor").b;
                rim_StandardPBR_Power_Slider.value = material.GetFloat("_RimPower");
            }
           
        }

        m_Rim_Bumped_Specular_Model_MeshRenderers = rim_BumpedSpecular_Model.GetComponentsInChildren<SkinnedMeshRenderer>();

        for (int i = 0; i < m_Rim_Bumped_Specular_Model_MeshRenderers.Length; i++)
        {
            SkinnedMeshRenderer meshRenderer = m_Rim_Bumped_Specular_Model_MeshRenderers[i];
            Material[] materials = meshRenderer.materials;

            for (int j = 0; j < materials.Length; j++)
            {
                Material material = materials[j]; 
                rim_BumpedSpecular_R_Slider.value = material.GetColor("_RimColor").r;
                rim_BumpedSpecular_G_Slider.value = material.GetColor("_RimColor").g;
                rim_BumpedSpecular_B_Slider.value = material.GetColor("_RimColor").b;
                rim_BumpedSpecular_Power_Slider.value = material.GetFloat("_RimPower");
            }

            
        }

    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < m_Rim_StandardPBR_Model_MeshRenderers.Length; i++)
        {
            SkinnedMeshRenderer meshRenderer = m_Rim_StandardPBR_Model_MeshRenderers[i];
            Material[] materials = meshRenderer.materials;

            for (int j = 0; j < materials.Length; j++)
            {
                Material material = materials[j];
                Color rimColor = new Color(rim_StandardPBR_R_Slider.value,
                                         rim_StandardPBR_G_Slider.value,
                                         rim_SandardPBR_B_Slider.value,
                                         1.0f);
                material.SetColor("_RimColor", rimColor);
                material.SetFloat("_RimPower", rim_StandardPBR_Power_Slider.value);
            }
         
        }

        for (int i = 0; i < m_Rim_Bumped_Specular_Model_MeshRenderers.Length; i++)
        {
            SkinnedMeshRenderer meshRenderer = m_Rim_Bumped_Specular_Model_MeshRenderers[i];
            Material[] materials = meshRenderer.materials;
            for (int j = 0; j < materials.Length; j++)
            {
                Material material = materials[j];
                Color rimColor = new Color(rim_BumpedSpecular_R_Slider.value,
                                            rim_BumpedSpecular_G_Slider.value,
                                            rim_BumpedSpecular_B_Slider.value,
                                            1.0f);
                material.SetColor("_RimColor", rimColor);
                material.SetFloat("_RimPower", rim_BumpedSpecular_Power_Slider.value);
            }

            

          
        }

    }
}
