using UnityEngine;

public class PowerupIndicator : MonoBehaviour
{
    public bool IsActive => gameObject.activeSelf;

    public Powerup.Type type;

    public bool CanKickAttack => type == Powerup.Type.KickAttack;

    public bool CanFireAttack => type == Powerup.Type.FireAttack;

    public bool CanSmashAttack => type == Powerup.Type.SmashAttack;
}
