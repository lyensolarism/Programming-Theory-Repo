using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// INHERITANCE
public class BigBoss : EliteEnemy
{
    public float attackRating;
    public int spawnCount;
    public GameObject bulletPrefab;

    // POLYMORPHISM
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        spawnManager = FindObjectOfType<SpawnManager>();

        InvokeRepeating(nameof(PerformAttack), attackRating, attackRating);
    }

    // Update is called once per frame
    protected override void Update()
    {
        var direction = Vector3.zero - transform.position;

        rigid.AddForce(speed * Time.deltaTime * direction.normalized);
    }

    private void PerformAttack()
    {
        if (player == null)
        {
            return;
        }

        if (Random.value < spawnPossiblity)
        {
            Spawn();
        }
        else
        {
            Shoot();
        }
    }

    private void Spawn()
    {
        spawnManager.SpawnEnemies(spawnCount);
    }

    private void Shoot()
    {
        var bulletSpeed = 500;
        var bodySize = 3;

        var position = Vector3.MoveTowards(transform.position, player.transform.position, bodySize);
        var bullet = Instantiate(bulletPrefab, position, bulletPrefab.transform.rotation);
        bullet.GetComponent<Renderer>().material.SetColor("_Color", Color.green);

        var direction = player.transform.position - transform.position;
        bullet.GetComponent<Rigidbody>().AddForce(direction.normalized * bulletSpeed);
    }

    private readonly float spawnPossiblity = 0.2f;
    private SpawnManager spawnManager;
}
