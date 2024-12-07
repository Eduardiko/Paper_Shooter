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

    [SerializeField] private GameObject[] cannons = new GameObject[5];


    [SerializeField] private GameObject propulsorTop;
    [SerializeField] private GameObject propulsorBot;
    [SerializeField] private GameObject propulsorBack;

    private int phase = 0;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();

        moveAction = playerInput.actions.FindAction("Move");
        shootAction = playerInput.actions.FindAction("Shoot");

        initPosition = transform.position;

        UpgradeShip();
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
            AudioManager.Instance.PlaySFX(0, 0.8f);

            foreach (GameObject cannon in cannons)
            {
                if (cannon.activeSelf)
                {
                    Shoot(cannon.transform);
                }
            }
        }
    }

    public override void ApplyDamage()
    {
        health--;
        if (health <= 0)
        {
            AudioManager.Instance.PlaySFX(5);
            Die();
        } else if (!ReadyToGetDestroyed)
             AudioManager.Instance.PlaySFX(7, 0.2f);

        StartCoroutine(DisableColliderAndBlink(2f, 0.1f));
    }

    public void Heal()
    {
        health++;
        AudioManager.Instance.PlaySFX(3);
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

    public void UpgradeShip()
    {
        phase++;

        switch (phase)
        {
            case 1:
                break;
            case 2:
                cannons[0].SetActive(false);
                cannons[1].SetActive(true);
                cannons[2].SetActive(true);
                break;
            case 3:
                cannons[0].SetActive(true);
                cannons[1].SetActive(false);
                cannons[2].SetActive(false);
                cannons[3].SetActive(true);
                cannons[4].SetActive(true);
                break;
            default:
                cannons[0].SetActive(false);
                cannons[1].SetActive(true);
                cannons[2].SetActive(true);
                cannons[3].SetActive(true);
                cannons[4].SetActive(true);
                break;
        }

        AudioManager.Instance.PlaySFX(2, 0.5f);
    }
}
