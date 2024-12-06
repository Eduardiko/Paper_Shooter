using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Swordfish : Mob
{
    [SerializeField] private GameObject propulsorBack;

    void Update()
    {
        if(canAct)
        {
            Move(Vector3.right);
            if (propulsorBack != null)
                propulsorBack.SetActive(true);
        }
    }
}
