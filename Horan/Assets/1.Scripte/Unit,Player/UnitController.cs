using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UnitController : MonoBehaviour
{
    public string MyName;
    protected AudioSource Sound_Hit;

    protected ActComponent Act;
    protected MoveComponent move; //-> Nav�޽� ������Ʈ������� ������ vs ������ �ٵ� ����� ������ �����ϱ� 
    protected InputComponent input;
    //Input������Ʈ�� HUD���� ó���ϴ� ��� ���� �ش� 
}
