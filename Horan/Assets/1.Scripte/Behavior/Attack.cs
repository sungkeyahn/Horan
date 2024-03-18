using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : Act
{
    public override void Init()
    {
        //추후에 데이터테이블에서 로드하는 방식으로 변경하기 
        ID = 1;
        AllowActs.Add(2);

    }
    public override void Execution()
    {
    }

    public override void Finish()
    {
    }


}
