using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IEquip
{
    public void Equip(int id);
}
public class Item : MonoBehaviour
{
    public int ID;
}
