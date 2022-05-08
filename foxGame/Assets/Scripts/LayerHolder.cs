using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerHolder : MonoBehaviour
{
    public static LayerHolder Instance { get; private set; }
    [SerializeField] LayerMask player;
    [SerializeField] LayerMask ground;
    [SerializeField] LayerMask enemy;

    public LayerMask Player => player;
    public LayerMask Ground => ground;
    public LayerMask Enemy => enemy;

    private void Awake()
    {
        Instance = this;
        Debug.Log(Instance);
    }
}
