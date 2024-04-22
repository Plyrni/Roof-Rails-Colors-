using System;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class Character : MonoBehaviour
{
    public UnityEvent<GameObject> onCreateRagdoll;
    public Rigidbody RigidRagdoll => _rigibodyRagdoll;
    
    [SerializeField] private Player _player;
    [SerializeField] private GameObject visualCharacter;
    [SerializeField] private Rigidbody _rigibodyRagdoll;
    [SerializeField] private PhysicMaterial matNoFriction;
    private CapsuleCollider _collider;
    private GameObject _ragdoll;
    private Vector3 _baseLocalPos;
    private bool _isRagdoll = false;

    private void Awake()
    {
        _collider = GetComponent<CapsuleCollider>();
        _baseLocalPos = visualCharacter.transform.localPosition;
    }

    public void EnableMatNoFricton(bool enable) {
        if (enable)
        {
            _collider.material = matNoFriction;
        }
        else
        {
            _collider.material = null;
        }
    }
    public void EnableRagdoll()
    {
        _isRagdoll = true;
        Debug.Log("Ragdoll");
        // _ragdoll = Instantiate(visualCharacter, _player.transform.position, _player.transform.rotation, Game.Instance.transform);
        visualCharacter.GetComponent<Animator>().enabled = false;
        foreach (var rigid in visualCharacter.GetComponentsInChildren<Rigidbody>())
        {
            rigid.isKinematic = false;
            rigid.velocity = _player.Rigidbody.velocity * Random.Range(0f,1f);
        }
        foreach (var collider in GetComponentsInChildren<Collider>())
        {
            collider.enabled = true;
        }

        onCreateRagdoll?.Invoke(_ragdoll);
    }
    public void DisableRagdoll()
    {
        _isRagdoll = false;
        
        visualCharacter.GetComponent<Animator>().enabled = true;
        
        foreach (var rigid in visualCharacter.GetComponentsInChildren<Rigidbody>())
        {
            rigid.isKinematic = true;
        }
        
        foreach (var collider in GetComponentsInChildren<Collider>())
        {
            if (collider.gameObject == this.gameObject)
            {
                continue;
            }
            collider.enabled = false;
            collider.material = matNoFriction;
        }
        
        visualCharacter.gameObject.SetActive(true);
    }
}