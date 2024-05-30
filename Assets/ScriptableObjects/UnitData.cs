using UnityEngine;

[CreateAssetMenu]
public class UnitData : ScriptableObject
{
    [Header("UNIT")]
    public GameObject Prefab;
    public int Cost;

    [Header("ATTACK")]
    public float Damage;
    public float Range;
    public float AttackSpeed;
    public float InitialAttackDelay;

    [Header("MOVEMENT")]
    public float MovementSpeed;
}
