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
    [SerializeField] private float timeToStartMoving = 2f;



    [HideInInspector] public ObjectPool<Projectile> projectilePool;
    protected Vector3 spawnPosition;

    private List<Projectile> spawnedProjectiles;

    private void Awake()
    {
        projectilePool = new ObjectPool<Projectile>(InstantiateProjectile, GetProjectile, ReleaseProjectile, DestroyProjectile);
        spawnedProjectiles = new List<Projectile>();
    }

    void Update()
    {
        spawnPosition = transform.position;
    }

    protected void Move(Vector2 direction)
    {
        transform.Translate(direction * speed * Time.deltaTime);  
    }

    protected void Shoot()
    {
        Projectile projectile = projectilePool.Get();
        projectile.MyPool = projectilePool;
        projectile.FatherObject = gameObject;
        projectile.transform.eulerAngles = transform.eulerAngles / 2;

        if (!spawnedProjectiles.Contains(projectile))
            spawnedProjectiles.Add(projectile);
    }
    public void Die()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        foreach(Projectile projectile in spawnedProjectiles)
        {
            if(!projectile.gameObject.activeSelf)
                Destroy(projectile.gameObject);
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
