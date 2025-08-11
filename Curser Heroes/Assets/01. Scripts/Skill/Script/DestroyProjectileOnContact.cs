using UnityEngine;
public class DestroyProjectileOnContact : MonoBehaviour
{
    [SerializeField] private LayerMask ProjectileLayer;
    [SerializeField] private AudioClip blockSound;

    private AudioSource audioSource;
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void Init(AudioClip clip)
    {
        this.blockSound = clip;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & ProjectileLayer) != 0)
        {
            if (audioSource != null && blockSound != null)
            {
                audioSource.PlayOneShot(blockSound);
            }
            Destroy(other.gameObject);
        }
    }
}