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

    private Vector3 initPosition;

    [SerializeField] private GameObject propulsorTop;
    [SerializeField] private GameObject propulsorBot;
    [SerializeField] private GameObject propulsorBack;


    void Start()
    {
        playerInput = GetComponent<PlayerInput>();

        moveAction = playerInput.actions.FindAction("Move");
        shootAction = playerInput.actions.FindAction("Shoot");

        initPosition = transform.position;
    }

    void Update()
    {
        if (ReadyToGetDestroyed && !mobRenderer.isVisible)
            Destroy(gameObject);

        if (timeToShoot > 0)
            timeToShoot -= Time.deltaTime;

        ReadContinuousInputs();

        spawnPosition = transform.position;

        if (timeToShoot > 0)
            timeToShoot -= Time.deltaTime;
    }

    public void ReadContinuousInputs()
    {
        propulsorBack.SetActive(false);
        propulsorTop.SetActive(false);
        propulsorBot.SetActive(false);

        Vector2 direction = moveAction.ReadValue<Vector2>();
        
        if (direction.x > 0)
            propulsorBack.SetActive(true);

        if(direction.y > 0)
            propulsorBot.SetActive(true);

        if (direction.y < 0)
            propulsorTop.SetActive(true);

        Move(direction);

        if (shootAction.ReadValue<float>() == 1 && timeToShoot <= 0)
        {
            timeToShoot = timeBetweenBullets;
            Shoot();
        }
    }

    public override void ApplyDamage()
    {
        health--;
        if (health <= 0)
            Die();

        StartCoroutine(DisableColliderAndBlink(2f, 0.1f));
    }

    public IEnumerator DisableColliderAndBlink(float duration, float blinkInterval)
    {
        if (mobCollider != null && mobRenderer != null)
        {
            transform.position = initPosition;

            mobCollider.enabled = false;

            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                mobRenderer.enabled = !mobRenderer.enabled;

                yield return new WaitForSeconds(blinkInterval);

                elapsedTime += blinkInterval;
            }

            mobRenderer.enabled = true;
            mobCollider.enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy" && !collision.gameObject.GetComponent<Mob>().ReadyToGetDestroyed || collision.gameObject.tag == "MobKiller")
            ApplyDamage();
    }


}
