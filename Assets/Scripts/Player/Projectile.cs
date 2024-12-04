using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private int speed;


    private ObjectPool<Projectile> myPool;
    private Renderer projectileRenderer;

    public ObjectPool<Projectile> MyPool { get => myPool; set => myPool = value; }

    private void Start()
    {
        projectileRenderer = GetComponent<Renderer>();
    }

    void Update()
    {
        transform.Translate(transform.right * speed * Time.deltaTime);

        if (!projectileRenderer.isVisible)
            myPool.Release(this);
    }
}
