using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 1.5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private Transform modelo;

    private bool suelo;
    private Rigidbody rb;
    private Animator anim;
    private int n_saltos = 0;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();

        // Evitamos que el monigote se caiga de lado o rote por choques físicos
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        // Es recomendable que el Rigidbody del jugador tenga Interpolate activado en el Inspector
    }

    private void Update()
    {
        // 1. ROTACIÓN DEL CUERPO (Eje Y)
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (h != 0)
        {
            transform.Rotate(0, h * rotationSpeed * Time.deltaTime, 0);
        }

        // 2. CÁLCULO DE VELOCIDAD DE MOVIMIENTO
        bool estaCorriendo = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        float currentSpeed = estaCorriendo ? speed * 2.5f : speed;

        // Dirección basada en hacia dónde mira el objeto principal
        Vector3 moveDir = transform.forward * v * currentSpeed;

        // 3. MOVIMIENTO RELATIVO A LA PLATAFORMA (La clave del problema)
        Vector3 velocidadFinal = new Vector3(moveDir.x, rb.linearVelocity.y, moveDir.z);

        if (transform.parent != null)
        {
            // Si somos hijos de algo, buscamos su Rigidbody
            Rigidbody rbPadre = transform.parent.GetComponent<Rigidbody>();
            if (rbPadre != null)
            {
                // Sumamos la velocidad del padre a la nuestra para no quedarnos atrás
                // Usamos la velocidad del rigidbody del padre para mayor precisión
                Vector3 velPadre = rbPadre.linearVelocity;

                // Si la plataforma usa MovePosition (cinemática), a veces linearVelocity es 0. 
                // En ese caso, el emparentado normal debería funcionar si NO sobreescribimos la velocidad X/Z.
                // Pero con esta suma nos aseguramos en la mayoría de casos:
                velocidadFinal.x += velPadre.x;
                velocidadFinal.z += velPadre.z;
            }
        }

        rb.linearVelocity = velocidadFinal;

        // 4. ROTACIÓN DEL MODELO (Visual: para que mire adelante o atrás)
        if (modelo != null && v != 0)
        {
            float anguloLocal = (v > 0) ? 0 : 180;
            Quaternion rotacionObjetivo = Quaternion.Euler(0, anguloLocal, 0);
            modelo.localRotation = Quaternion.RotateTowards(modelo.localRotation, rotacionObjetivo, 600f * Time.deltaTime);
        }

        // 5. ANIMACIONES
        if (anim != null)
        {
            float animSpeed = (estaCorriendo && v != 0) ? 2.5f : Mathf.Abs(v);
            anim.SetFloat("Speed", animSpeed);
            anim.SetBool("Suelo", suelo);
        }

        // 6. SALTO (Simple y Doble)
        if (Input.GetKeyDown(KeyCode.Space) && (suelo || n_saltos < 2))
        {
            // Reseteamos velocidad vertical para que el segundo salto sea consistente
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            n_saltos++;
            suelo = false;

            if (anim != null)
            {
                anim.SetBool("Suelo", false);
                anim.SetTrigger("Jump");
            }
        }
    }

    // --- DETECCIÓN DE SUELO Y PLATAFORMAS ---

    private void OnCollisionEnter(Collision collision)
    {
        // Recuerda poner el Tag "Floor" a tus plataformas también
        if (collision.gameObject.CompareTag("Floor"))
        {
            suelo = true;
            n_saltos = 0;
            if (anim != null) anim.SetBool("Suelo", true);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            suelo = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            suelo = false;
            if (anim != null) anim.SetBool("Suelo", false);
        }
    }
}