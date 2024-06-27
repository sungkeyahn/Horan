using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSocket : MonoBehaviour
{
    void Start()
    {
        FindObjectOfType<Weapon>().AttachWeapon();
    }
}
