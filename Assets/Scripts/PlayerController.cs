using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidadCaminar = 5f;
    public float velocidadCorrer = 8f;

    [Header("Salto")]
    public float fuerzaSalto = 8f;
    public float gravedad = -20f;
    public Transform checkSuelo;
    public float radioCheckSuelo = 0.5f;
    public LayerMask capaSuelo;

    [Header("Cámara")]
    public Transform camara;
    public float sensibilidad = 2f;
    public float limiteVertical = 80f;

    private CharacterController controller;
    private Vector3 velocidadVertical;
    private bool enSuelo;
    private float rotacionX = 0f;
    private float rotacionY = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        rotacionY = transform.eulerAngles.y;
    }

    void Update()
    {
        RotarCamara();
        Mover();
        Saltar();
    }

    void Mover()
    {
        enSuelo = Physics.CheckSphere(checkSuelo.position, radioCheckSuelo, capaSuelo);

        if (enSuelo && velocidadVertical.y < 0)
        {
            velocidadVertical.y = -2f;
        }
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direccion = transform.right * horizontal + transform.forward * vertical;
        float velocidadActual = Input.GetKey(KeyCode.LeftShift) ? velocidadCorrer : velocidadCaminar;

        controller.Move(direccion * velocidadActual * Time.deltaTime);

        // Aplicar gravedad
        velocidadVertical.y += gravedad * Time.deltaTime;
        controller.Move(velocidadVertical * Time.deltaTime);
    }

    void Saltar()
    {
        if (Input.GetKeyDown(KeyCode.Space) && enSuelo)
        {
            velocidadVertical.y = Mathf.Sqrt(fuerzaSalto * -2f * gravedad);
        }
    }

    void RotarCamara()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensibilidad;
        float mouseY = Input.GetAxis("Mouse Y") * sensibilidad;
        rotacionY += mouseX;
        transform.rotation = Quaternion.Euler(0f, rotacionY, 0f);

        rotacionX -= mouseY;
        rotacionX = Mathf.Clamp(rotacionX, -limiteVertical, limiteVertical);
        camara.localRotation = Quaternion.Euler(rotacionX, 0f, 0f);
    }

    void OnDrawGizmosSelected()
    {
        if (checkSuelo != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(checkSuelo.position, radioCheckSuelo);
        }
    }
}