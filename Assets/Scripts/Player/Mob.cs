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

    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private int speed = 10;
    [SerializeField] private float timeBetweenBullets = 2f;
    

    protected float timeToShoot = 0;

    [HideInInspector] public ObjectPool<Projectile> projectilePool;
    protected Vector3 spawnPosition;

    private void Awake()
    {
        projectilePool = new ObjectPool<Projectile>(InstantiateProjectile, GetProjectile, ReleaseProjectile, DestroyProjectile);
    }

    void Update()
    {
        spawnPosition = transform.position;

        if(timeToShoot > 0)
            timeToShoot -= Time.deltaTime;
    }

    protected void Move(Vector2 direction)
    {
        transform.Translate(direction * speed * Time.deltaTime);  
    }

    protected void Shoot()
    {
        if (timeToShoot <= 0)
        {
            timeToShoot = timeBetweenBullets;
            projectilePool.Get();
        }
    }
    public void Die()
    {
        Destroy(gameObject);
    }

    #region PoolMethods
    private Projectile InstantiateProjectile()
    {
        Projectile projectile = GameObject.Instantiate(projectilePrefab, spawnPosition, transform.rotation).GetComponent<Projectile>();
        projectile.MyPool = projectilePool;
        projectile.FatherObject = gameObject;
        return projectile;
    }
    private void GetProjectile(Projectile projectile)
    {
        projectile.gameObject.SetActive(true);
        projectile.transform.position = spawnPosition;
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
