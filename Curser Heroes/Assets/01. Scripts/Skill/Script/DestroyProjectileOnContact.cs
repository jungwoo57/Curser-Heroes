using UnityEngine;
public class DestroyProjectileOnContact : MonoBehaviour
{
    [SerializeField] private LayerMask projectileLayer;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & projectileLayer) != 0)
        {
            Destroy(other.gameObject);
        }
    }
}