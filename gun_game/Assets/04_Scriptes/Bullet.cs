using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private ParticleSystem _impact;
    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void Init(Vector3 velocity)
    {
        _rb.velocity = velocity;
        Destroy(this.gameObject, 5f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        var impactParticle = Instantiate(_impact, collision.contacts[0].point, Quaternion.identity);
        if (this.gameObject.CompareTag("killKing") && !collision.gameObject.CompareTag("King"))
        {
            GameManager.instance.isntKing();
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("King"))
        {
            collision.gameObject.GetComponent<King>().OnRagdoll();
            GameManager.instance.SotpSlowMotion();
        }
        Destroy(impactParticle.gameObject, 1);
        Destroy(this.gameObject);
    }
}
