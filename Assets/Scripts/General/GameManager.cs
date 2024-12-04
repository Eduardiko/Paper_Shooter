using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;

    private ObjectPool<Projectile> playerPool;


    private void Awake()
    {
        //GameObject player = Instantiate(playerPrefab);
       // playerPool = new ObjectPool<Projectile>(InstantiateProjectile, GetProjectile, ReleaseProjectile, DestroyProjectile);
        //player.GetComponent<Player>().projectilePool = playerPool;

    }
}
