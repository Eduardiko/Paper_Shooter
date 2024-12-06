using System;
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
    protected Vector3 spawnPosition;

    private List<Projectile> spawnedProjectiles;

    protected Collider2D mobCollider;
    protected SpriteRenderer mobRenderer;

    private bool blinkCoroutineRunning = false;

    private void Awake()
    {
        mobCollider = GetComponent<Collider2D>();
        mobRenderer = GetComponent<SpriteRenderer>();
        projectilePool = new ObjectPool<Projectile>(InstantiateProjectile, GetProjectile, ReleaseProjectile, DestroyProjectile);
        spawnedProjectiles = new List<Projectile>();
    }

    void Update()
    {
        spawnPosition = transform.position;
    }

    protected void Move(Vector2 direction)
    {
        float verticalSpeedMultiplier = 1.5f;

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

        Destroy(gameObject);
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
