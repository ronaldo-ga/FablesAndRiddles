using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    [SerializeField] private ParticleSystem _explosionParticle;

    [SerializeField] private MeshRenderer _meshRenderer;

    [SerializeField] private Collider _collider;

    private float _blastRadius;
    private float _damage;

    private Rigidbody _rb;



    private void Start()
    {
        //_rb = GetComponent<Rigidbody>();
    }

    /*private void OnCollisionEnter(Collision collision)
    {
        _explosionParticle.Play();
        _rb.velocity = Vector3.zero;

        Collider[] colliders = Physics.OverlapSphere(transform.position, _blastRadius);
        foreach (Collider enemies in colliders)
        {
            if(enemies.GetComponent<IDamageable>() != null)
            {
                _collider.enabled = false;
                Damage(enemies.GetComponent<IDamageable>());
            }
        }

        _meshRenderer.enabled = false;
        StartCoroutine(DestroyObject(_explosionParticle.main.duration));
    }*/

    private IEnumerator DestroyObject(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }

    /*private void Damage(IDamageable idamageable)
    {
        idamageable.TakeDamage(_damage);
    }*/

    public void SetShootAttributes(float blastradius, float damage)
    {
        /*_collider.enabled = true;
        _meshRenderer.enabled = true;
        _blastRadius = blastradius;
        _damage = damage;*/
    }
}
