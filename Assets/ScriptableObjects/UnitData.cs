using UnityEngine;

[CreateAssetMenu]
public class UnitData : ScriptableObject
{
    [Header("UNIT")]
    public int Cost;

    [Header("ATTACK")]
    public float Damage;
    public float Range;
    public float AttackSpeed;

    [Header("MOVEMENT")]
    public float MovementSpeed;
}
