using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, IDataBind
{
    //그냥 프리팹으로 만들어서 넣자 
    /*
     * 어차피 아이템은 데이터만 존재하면 됨 
     * 오브젝트일 필요 없음 
     * 픽업 아이템이 오브젝트 인거지 
     */
    public int id;
    
    public void BindData()
    {  
        
    }


}
