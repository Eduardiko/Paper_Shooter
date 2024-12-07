using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool;



public class Mob : MonoBehaviour
{
    //Mob Specs
    public int health = 5;
    [SerializeField] private float speed = 10f;
    [SerializeField] private GameObject projectilePrefab;
    public bool canAct = false;

    [HideInInspector] public ObjectPool<Projectile> projectilePool;
    [SerializeField] private int pointsWhenDead;
    [SerializeField] private Sprite deadSprite;
    protected Vector3 spawnPosition;

    private List<Projectile> spawnedProjectiles;

    protected Collider2D mobCollider;
    protected SpriteRenderer mobRenderer;
    protected Rigidbody2D mobRigidBody;

    private bool blinkCoroutineRunning = false;
    private bool readyToGetDestroyed = false;

    public bool ReadyToGetDestroyed { get => readyToGetDestroyed; set => readyToGetDestroyed = value; }

    private void Awake()
    {
        mobCollider = GetComponent<Collider2D>();
        mobRenderer = GetComponent<SpriteRenderer>();
        mobRigidBody = GetComponent<Rigidbody2D>();
        projectilePool = new ObjectPool<Projectile>(InstantiateProjectile, GetProjectile, ReleaseProjectile, DestroyProjectile);
        spawnedProjectiles = new List<Projectile>();
    }

    void Update()
    {
        spawnPosition = transform.position;

        if (readyToGetDestroyed && !mobRenderer.isVisible)
            Destroy(gameObject);
    }

    protected void Move(Vector2 direction)
    {
        float verticalSpeedMultiplier = 1.75f;

        if(direction.y != 0)
            transform.Translate(direction * speed * verticalSpeedMultiplier * Time.deltaTime);
        else
            transform.Translate(direction * speed * Time.deltaTime);
    }

    protected void Shoot()
    {
        Projectile projectile = projectilePool.Get();
        projectile.MyPool = projectilePool;
        projectile.FatherObject = gameObject;
        projectile.transform.eulerAngles = transform.eulerAngles;

        if (!spawnedProjectiles.Contains(projectile))
            spawnedProjectiles.Add(projectile);
    }

    protected void Shoot(Vector3 bulletRotation)
    {
        Projectile projectile = projectilePool.Get();
        projectile.MyPool = projectilePool;
        projectile.FatherObject = gameObject;
        projectile.transform.eulerAngles = bulletRotation;

        if (!spawnedProjectiles.Contains(projectile))
            spawnedProjectiles.Add(projectile);
    }

    protected void Shoot(Transform cannonTransform)
    {
        Projectile projectile = projectilePool.Get();
        projectile.MyPool = projectilePool;
        projectile.FatherObject = gameObject;
        projectile.transform.eulerAngles = cannonTransform.eulerAngles;
        projectile.transform.position = cannonTransform.position;

        if (!spawnedProjectiles.Contains(projectile))
            spawnedProjectiles.Add(projectile);
    }

    public virtual void ApplyDamage()
    {
        health--;

        if (health <= 0)
        {
            GameManager.playerScore += pointsWhenDead;
            Die();
        }
        else if (!blinkCoroutineRunning)
            StartCoroutine(FlashOpacity(0.1f, 0.8f));
    }

    public virtual void Die()
    {
        if (gameObject == null)
            return;

        foreach(Projectile projectile in spawnedProjectiles)
        {
            if(projectile != null && !projectile.gameObject.activeSelf)
                Destroy(projectile.gameObject);
        }

        ThrowDeadAway();
    }

    public void ThrowDeadAway()
    {
        if(mobRigidBody != null)
        {
            float rotSpeed = 0;
            float gravityMagnitude = 5f;

            Vector2 direction = new Vector2((float)Random.Range(transform.position.x - 180, transform.position.x + 180), (float)Random.Range(transform.position.y, transform.position.y + 180));
            float force = (float)Random.Range(1, 7.5f);
            mobRigidBody.AddForce(direction * force);
            mobRigidBody.gravityScale = gravityMagnitude;

            rotSpeed = (float)Random.Range(-25, 25);

            mobRigidBody.AddTorque(force * rotSpeed);

            if(mobRenderer != null && deadSprite != null)
                mobRenderer.sprite = deadSprite;
        }
        readyToGetDestroyed = true;
        canAct = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "MobKiller")
            Die();
    }

    private IEnumerator FlashOpacity(float duration, float targetOpacity)
    {
        if (mobRenderer != null)
        {
            blinkCoroutineRunning = true;

            Color originalColor = mobRenderer.color;
            Color fadedColor = originalColor;

            fadedColor.a = targetOpacity;
            mobRenderer.color = fadedColor;

            yield return new WaitForSeconds(duration);

            blinkCoroutineRunning = false;
            mobRenderer.color = originalColor;
        }
    }

    #region PoolMethods
    private Projectile InstantiateProjectile()
    {
        Projectile projectile = GameObject.Instantiate(projectilePrefab, spawnPosition, Quaternion.identity).GetComponent<Projectile>();
        return projectile;
    }
    private void GetProjectile(Projectile projectile)
    {
        projectile.gameObject.SetActive(true);
        projectile.transform.position = spawnPosition;
        if(projectile.InitSprite != null)
            projectile.ProjectileRenderer.sprite = projectile.InitSprite;

        projectile.CanMove = true;
    }

    private void ReleaseProjectile(Projectile projectile)
    {
        projectile.gameObject.SetActive(false);
    }

    private void DestroyProjectile(Projectile projectile)
    {
        Destroy(projectile);
    }
    #endregion
}
