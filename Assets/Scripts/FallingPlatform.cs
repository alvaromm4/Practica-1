using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    // 1. La variable se declara AQUÍ (fuera de los métodos) para que todo el script la vea
    private Rigidbody rb;

    void Start()
    {
        // 2. Buscamos el componente al empezar
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // 3. Activamos la gravedad
            rb.useGravity = true;

            // CONSEJO: Si tu plataforma es "Kinematic", añade esta línea 
            // para que la gravedad realmente le afecte:
            rb.isKinematic = false;
        }
    }
}