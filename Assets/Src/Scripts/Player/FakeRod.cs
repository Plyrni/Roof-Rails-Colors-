using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeRod : MonoBehaviour
{
    public Rigidbody Rigidbody => _rigid;
    [SerializeField] private Rigidbody _rigid;
    
    // TODO : Set color or smthg like that
}
