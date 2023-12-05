using System;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class CarCollision : MonoBehaviour
{
    [Header("CollisionRay")] 
    [SerializeField] private LayerMask _whatIsWall;
    
    [Header("Collision")]
    private CarController _carController;

    private void Awake()
    {
        _carController = GetComponent<CarController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fuel"))
        {
            //_carController.CurrentFuel += 250;
            other.GetComponent<Fuel>().FuelCollision();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        //UnityEngine.Debug.Log($"{other.transform.gameObject.layer}    {_whatIsWall.value}");
        if (other.transform.gameObject.layer != _whatIsWall.value - 1)
            return;
        ContactPoint contact = other.contacts[0];
        CarFollowCamera.Instance.CameraShake(0.5f, 0.35f);
        Particle particle = PoolManager.Instance.Pop("CollisionSparkEffect") as Particle;
        particle.transform.position = contact.point;
    }
}
