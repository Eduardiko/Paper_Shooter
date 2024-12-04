using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Mob
{
    [SerializeField] private float timeBetweenBullets = 2f;
    private float timeToShoot = 0;

    // Continous Input Actions
    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction shootAction;


    void Start()
    {
        playerInput = GetComponent<PlayerInput>();

        moveAction = playerInput.actions.FindAction("Move");
        shootAction = playerInput.actions.FindAction("Shoot");
    }


    void Update()
    {
        if (timeToShoot > 0)
            timeToShoot -= Time.deltaTime;

        ReadContinuousInputs();

        spawnPosition = transform.position;

        if (timeToShoot > 0)
            timeToShoot -= Time.deltaTime;
    }

    public void ReadContinuousInputs()
    {
        Move(moveAction.ReadValue<Vector2>());

        if (shootAction.ReadValue<float>() == 1 && timeToShoot <= 0)
        {
            timeToShoot = timeBetweenBullets;
            Shoot();
        }
    }
}
