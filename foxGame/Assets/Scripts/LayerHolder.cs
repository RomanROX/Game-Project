using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerHolder : MonoBehaviour
{
    [SerializeField] LayerMask player;
    [SerializeField] LayerMask ground;
    [SerializeField] LayerMask enemy;

    public LayerMask Player => player;
    public LayerMask Ground => ground;
    public LayerMask Enemy_ => enemy;
}
