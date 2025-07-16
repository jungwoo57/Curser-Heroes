using UnityEngine;
public class DestroyProjectileOnContact : MonoBehaviour
{
    [SerializeField] private LayerMask ProjectileLayer;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & ProjectileLayer) != 0)
        {
            Destroy(other.gameObject);
        }
    }
}