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

        rb.constraints = RigidbodyConstraints.FreezeRotation;

    }

    private void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (h != 0)
        {
            transform.Rotate(0, h * rotationSpeed * Time.deltaTime, 0);
        }

        bool estaCorriendo = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        float currentSpeed = estaCorriendo ? speed * 2.5f : speed;

        Vector3 moveDir = transform.forward * v * currentSpeed;

        Vector3 velocidadFinal = new Vector3(moveDir.x, rb.linearVelocity.y, moveDir.z);

        if (transform.parent != null)
        {
            Rigidbody rbPadre = transform.parent.GetComponent<Rigidbody>();
            if (rbPadre != null)
            {
                Vector3 velPadre = rbPadre.linearVelocity;

                velocidadFinal.x += velPadre.x;
                velocidadFinal.z += velPadre.z;
            }
        }

        rb.linearVelocity = velocidadFinal;

        if (modelo != null && v != 0)
        {
            float anguloLocal = (v > 0) ? 0 : 180;
            Quaternion rotacionObjetivo = Quaternion.Euler(0, anguloLocal, 0);
            modelo.localRotation = Quaternion.RotateTowards(modelo.localRotation, rotacionObjetivo, 600f * Time.deltaTime);
        }

        if (anim != null)
        {
            float animSpeed = (estaCorriendo && v != 0) ? 2.5f : Mathf.Abs(v);
            anim.SetFloat("Speed", animSpeed);
            anim.SetBool("Suelo", suelo);
        }

        if (Input.GetKeyDown(KeyCode.Space) && (suelo || n_saltos < 2))
        {
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


    private void OnCollisionEnter(Collision collision)
    {
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