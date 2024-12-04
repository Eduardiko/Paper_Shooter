using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pufferfish : Mob
{
    [SerializeField] private float timeBetweenBullets = 2f;
    private float timeToShoot = 0;

    void Update()
    {
        spawnPosition = transform.position;

        if (timeToShoot > 0)
            timeToShoot -= Time.deltaTime;
        else
        {
            timeToShoot = timeBetweenBullets;
            Shoot();
        }
    }
}
