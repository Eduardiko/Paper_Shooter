using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private int speed;

    private GameObject fatherObject;
    private ObjectPool<Projectile> myPool;
    private Renderer projectileRenderer;

    public ObjectPool<Projectile> MyPool { get => myPool; set => myPool = value; }
    public GameObject FatherObject { get => fatherObject; set => fatherObject = value; }

    private void Start()
    {
        projectileRenderer = GetComponent<Renderer>();
    }

    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        if (!projectileRenderer.isVisible)
            myPool.Release(this);

        // El dispose de la pool parece no funcionar, esto es un workaround :(
            // El problema que tenía es que al destruir un GameObject que tiene una pool (los enemigos), sus "balas" se quedan existiendo en escena sin ser destruidas.
            // No he podido hacer que compartan una pool, que era mi objetivo.
        if(fatherObject == null && !projectileRenderer.isVisible)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Projectile" && fatherObject.tag != collision.gameObject.tag)
        {
            myPool.Release(this);

            Mob mob = collision.gameObject.GetComponent<Mob>();

            if(mob != null)
                mob.ApplyDamage();
        }
    }
}
