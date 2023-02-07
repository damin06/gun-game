using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    private Rigidbody _rb;

    [Header("Bullet")]
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private float _bulletSpeed = 10f;
    [SerializeField] private ParticleSystem _muzzleFlash;
    //[SerializeField] private ParticleSystem _smokeParticle;
    [SerializeField] private AudioSource _fireSource;

    [Header("Torque")]
    [SerializeField] private float _torque = 10f;
    [SerializeField] private float _maxAngularVelocity = 50f;
    [SerializeField] private float _maxBonusTorque = 10f;

    [Header("Force")]
    [SerializeField] private float _forceAmount = 50f;
    [SerializeField] private float _maxY = 10;
    [SerializeField] private float _maxUpAssist = 30;

    [Space]
    [SerializeField] private LayerMask _targetLayer;
    [SerializeField] private GameObject _followObject;
    private Vector3 _offset;
    private bool isTarget = false;

    private void Awake()
    {
        _offset = _followObject.transform.position - transform.position;
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !GameManager.instance.isSlowMotion && !GameManager.instance.isGamePause)
        {
            var hitsTarget = Physics.Raycast(_spawnPoint.position, _spawnPoint.forward, float.PositiveInfinity, _targetLayer);
            var bullet = Instantiate(_bulletPrefab, _spawnPoint.position, _spawnPoint.rotation);
            if (hitsTarget)
            {
                bullet.tag = "killKing";
                GameManager.instance.SlowMotion();
                isTarget = true;
            }
            bullet.Init(_spawnPoint.forward * _bulletSpeed);
            _muzzleFlash.Play();
            _fireSource.PlayOneShot(_fireSource.clip);
            //_smokeParticle.Play();

            var assistPoint = Mathf.InverseLerp(0, _maxY, _rb.position.y);
            var assistAmount = Mathf.Lerp(_maxUpAssist, 0, assistPoint);
            var forceDir = -transform.right * _forceAmount + Vector3.up * assistAmount;
            if (_rb.position.y > _maxY) forceDir.y = Mathf.Min(0, forceDir.y);
            _rb.AddForce(forceDir);

            var dir = Vector3.Dot(_spawnPoint.right, Vector3.right) > 0 ? Vector3.forward : Vector3.back;
            var angularPoint = Mathf.InverseLerp(0, _maxAngularVelocity, Mathf.Abs(_rb.angularVelocity.z));
            var amount = Mathf.Lerp(0, _maxBonusTorque, angularPoint);
            var torque = _torque + amount;
            _rb.AddTorque(dir * torque);
        }

        if (!isTarget)
        {
            _followObject.transform.position = new Vector3(transform.position.x, 1.92f, 0);
        }
        else
        {
            if (GameObject.FindGameObjectWithTag("killKing") == null)
            {
                isTarget = false;
                return;
            }
            _followObject.transform.position = new Vector3(GameObject.FindGameObjectWithTag("killKing").transform.position.x, 1.92f, 0);
        }
    }
}
