using UnityEngine;

public class Powerup : MonoBehaviour
{
    public enum Type
    {
        KickAttack = 0,
        FireAttack,
        SmashAttack,
        End
    }

    // ENCAPSULATION
    public Color Color => colors[(int)type];

    // ABSTRACTION
    public Type GetPowerupType()
    {
        return type;
    }

    // Start is called before the first frame update
    void Start()
    {
        var index = Random.Range(0, (int)Type.End);
        GetComponent<Renderer>().material.SetColor("_Color", colors[index]);
        type = (Type)index;
    }

    private readonly Color[] colors =
    {
        Color.yellow,
        Color.red,
        Color.green
    };
    private Type type;
}
