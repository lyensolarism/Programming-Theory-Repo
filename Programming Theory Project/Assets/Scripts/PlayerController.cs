using System.Collections;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public GameObject powerupIndicator;
    public GameObject bulletPrefab;
    public AnimationCurve smashCurve;

    // Start is called before the first frame update
    void Start()
    {
        focalPoint = GameObject.Find("Focal Point");
        rigid = GetComponent<Rigidbody>();
        powerupStatus = powerupIndicator.GetComponent<PowerupIndicator>();
    }

    // Update is called once per frame
    void Update()
    {
        var rotateSpeed = 10;
        powerupIndicator.transform.position = transform.position;
        powerupIndicator.transform.Rotate(Vector3.up, rotateSpeed);

        if (smashTime > 0)
        {
            var y = smashCurve.Evaluate((Time.time - smashTime)) + originalY;
            if (y <= originalY)
            {
                SmashAttack();

                smashTime = float.MinValue;
                rigid.useGravity = true;
            }

            transform.position = new Vector3(transform.position.x, y, transform.position.z);
            return;
        }

        var horizontalInput = Input.GetAxis("Horizontal");
        var verticalInput = Input.GetAxis("Vertical");
        rigid.AddForce(horizontalInput * speed * focalPoint.transform.right);
        rigid.AddForce(verticalInput * speed * focalPoint.transform.forward);

        if (powerupStatus.IsActive && Input.GetKeyDown(KeyCode.Space))
        {
            if (powerupStatus.CanFireAttack)
            {
                FireAttack();
            }
            else if (powerupStatus.CanSmashAttack)
            {
                smashTime = Time.time;
                originalY = transform.position.y;
                rigid.useGravity = false;
            }
        }
    }

    private void FireAttack()
    {
        var maxBullets = 1;

        var aliveEnemies = GameObject.FindGameObjectsWithTag("Enemy").ToList();
        while (aliveEnemies.Count > maxBullets)
        {
            var index = Random.Range(0, aliveEnemies.Count);
            aliveEnemies.RemoveAt(index);
        }

        foreach(var enemy in aliveEnemies)
        {
            var bulletSpeed = 500;
            var bodySize = 1;

            var position = Vector3.MoveTowards(transform.position, enemy.transform.position, bodySize);
            var bullet = Instantiate(bulletPrefab, position, bulletPrefab.transform.rotation);

            var direction = enemy.transform.position - transform.position;
            bullet.GetComponent<Rigidbody>().AddForce(direction.normalized * bulletSpeed);
        }
    }

    private void SmashAttack()
    {
        var explosionForce = 1000;
        var explosionRadius = 10;
        var aliveEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (var enemy in aliveEnemies)
        {
            enemy.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRadius);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Powerup"))
        {
            if (routineEnumerator != null)
            {
                StopCoroutine(routineEnumerator);
            }

            var powerup = other.gameObject.GetComponent<Powerup>();
            powerupStatus.type = powerup.GetPowerupType();

            powerupIndicator.GetComponent<Renderer>().material.SetColor("_Color", powerup.Color);
            powerupIndicator.SetActive(true);

            Destroy(other.gameObject);
            routineEnumerator = PowerupCountdownRoutine();
            StartCoroutine(routineEnumerator);
        }
    }

    IEnumerator PowerupCountdownRoutine()
    {
        yield return new WaitForSeconds(7);
        powerupIndicator.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        var collider = collision.gameObject;

        if (collider.CompareTag("Enemy"))
        {
            if (powerupStatus.IsActive && powerupStatus.CanKickAttack)
            {
                var awayDirection = collider.transform.position - transform.position;
                var enemyRb = collider.GetComponent<Rigidbody>();

                enemyRb.AddForce(awayDirection * powerupStrength, ForceMode.Impulse);
            }
        }
    }

    private GameObject focalPoint;
    private Rigidbody rigid;
    private PowerupIndicator powerupStatus;
    private readonly float powerupStrength = 8;
    private IEnumerator routineEnumerator;
    private float smashTime = float.MinValue;
    private float originalY;
}
