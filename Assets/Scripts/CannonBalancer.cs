using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New CannonBalancer", menuName = "CannonBalancer")]
public class CannonBalancer : ScriptableObject
{
    public float startDelay;
    public float shootForce;
    public float shootDelay;
    public float blastRadius;
    public float damage;

}