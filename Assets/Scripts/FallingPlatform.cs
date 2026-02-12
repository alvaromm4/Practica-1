using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            rb.useGravity = true;
            rb.isKinematic = false;
        }
    }
}