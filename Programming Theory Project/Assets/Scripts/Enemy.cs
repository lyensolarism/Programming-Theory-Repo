using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        rigid = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (player == null)
        {
            return;
        }

        var direction = player.transform.position - transform.position;

        rigid.AddForce(speed * direction.normalized);
    }

    protected Rigidbody rigid;
    protected GameObject player;
}
