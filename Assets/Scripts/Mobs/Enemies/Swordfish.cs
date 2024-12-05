using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Swordfish : Mob
{

    void Update()
    {
        if(canAct)
            Move(Vector3.right);
    }
}
