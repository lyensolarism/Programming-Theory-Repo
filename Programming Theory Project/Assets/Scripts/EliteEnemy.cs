using UnityEngine;

// INHERITANCE
public class EliteEnemy : Enemy
{
    public float strength;

    protected void OnCollisionEnter(Collision collision)
    {
        var collider = collision.gameObject;

        if (collider.CompareTag("Player") || collider.CompareTag("Enemy"))
        {
            var awayDirection = collider.transform.position - transform.position;
            var playerRb = collider.GetComponent<Rigidbody>();

            playerRb.AddForce(awayDirection * strength, ForceMode.Impulse);
        }
    }
}
