using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Mob
{
    private PlayerInput playerInput;

    // Continous Input Actions
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
        ReadContinuousInputs();

        spawnPosition = transform.position;

        if (timeToShoot > 0)
            timeToShoot -= Time.deltaTime;
    }

    public void ReadContinuousInputs()
    {
        Move(moveAction.ReadValue<Vector2>());

        if (shootAction.ReadValue<float>() == 1)
            Shoot();
    }
}
