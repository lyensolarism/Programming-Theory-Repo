using UnityEngine;

public class CleanUp : MonoBehaviour
{
    public int lifetime;

    // Start is called before the first frame update
    void Start()
    {
        currentLifetime = lifetime;
    }

    // Update is called once per frame
    void Update()
    {
        if (lifetime > 0)
        {
            currentLifetime -= Time.deltaTime;
        }

        if (transform.position.y < -20 || currentLifetime < 0)
        {
            Destroy(gameObject);
        }
    }

    private float currentLifetime;
}
