using UnityEngine;

// Este componente requiere que el GameObject tenga un Rigidbody
[RequireComponent(typeof(Rigidbody))]
public class CarMovement : MonoBehaviour
{
    // ====== AJUSTES DE MOVIMIENTO ======
    [Header("Ajustes de Movimiento")]
    public float acceleration = 1200f;    // Fuerza de aceleración hacia adelante
    public float brakeForce = 1200f;     // Fuerza de frenado (hacia atrás)
    public float maxSpeed = 200f;         // Velocidad máxima permitida
    public float turnSpeed = 40f;        // Velocidad de giro del vehículo
    public float maxSteerAngle = 30f;    // Ángulo máximo de giro de las ruedas delanteras

    // ====== REFERENCIAS DE RUEDAS ======
    [Header("Ruedas")]
    public Transform frontLeftWheel;     // Transform de la rueda delantera izquierda
    public Transform frontRightWheel;    // Transform de la rueda delantera derecha

    // ====== VARIABLES INTERNAS ======
    private float horizontalInput;       // Entrada horizontal (giro)
    private float verticalInput;         // Entrada vertical (acelerar/frenar)
    private float currentSteerAngle;     // Ángulo actual de las ruedas delanteras
    private Quaternion frontLeftInitialRotation;   // Rotación inicial de la rueda izquierda
    private Quaternion frontRightInitialRotation;  // Rotación inicial de la rueda derecha
    private Rigidbody rb;                // Referencia al Rigidbody

    // Permite controlar el coche con IA o jugador
    public bool usePlayerInput = true;

    void Start()
    {
        // Obtenemos el Rigidbody y bajamos el centro de masa para mayor estabilidad
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = Vector3.down;

        // Guardamos la rotación inicial de las ruedas delanteras
        frontLeftInitialRotation = frontLeftWheel.localRotation;
        frontRightInitialRotation = frontRightWheel.localRotation;
    }

    void Update()
    {
        // Si el coche es controlado por el jugador, obtenemos el input
        if (usePlayerInput)
            GetInput();

        // Calculamos el ángulo de giro y animamos las ruedas
        HandleSteering();
        AnimateWheels();
    }

    void FixedUpdate()
    {
        // Movimiento físico (siempre en FixedUpdate)
        Move();
    }

    // Obtiene la entrada del usuario (teclado, joystick, etc.)
    void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
    }

    // Permite que la IA pase los inputs directamente
    public void SetInput(float horizontal, float vertical)
    {
        horizontalInput = horizontal;
        verticalInput = vertical;
    }

    // Calcula el ángulo de giro de las ruedas delanteras
    void HandleSteering()
    {
        // Calcula la velocidad actual en el plano XZ (ignora la Y)
        Vector3 flatVelocity = rb.linearVelocity;
        flatVelocity.y = 0;
        float speed = flatVelocity.magnitude;

        // Calcula el ángulo máximo de giro según la velocidad (más giro a baja velocidad)
        float dynamicSteerAngle = Mathf.Lerp(maxSteerAngle, maxSteerAngle * 0.3f, speed / maxSpeed);

        currentSteerAngle = dynamicSteerAngle * horizontalInput;
    }


    // Anima las ruedas delanteras visualmente
    void AnimateWheels()
    {
        frontLeftWheel.localRotation = frontLeftInitialRotation * Quaternion.Euler(0f, 0f, currentSteerAngle);
        frontRightWheel.localRotation = frontRightInitialRotation * Quaternion.Euler(0f, 0f, currentSteerAngle);
    }

    // Movimiento físico del coche
   void Move()
    {
        // Calcula la velocidad actual en el plano XZ (ignora la Y)
        Vector3 flatVelocity = rb.linearVelocity;
        flatVelocity.y = 0;

        // Convierte la velocidad a km/h
        float speedKmh = flatVelocity.magnitude * 3.6f;

        // Solo acelera si no ha alcanzado la velocidad máxima (en km/h)
        if (speedKmh < maxSpeed)
        {
            // Aceleración hacia adelante
            if (verticalInput > 0)
                rb.AddForce(transform.forward * verticalInput * acceleration * Time.fixedDeltaTime, ForceMode.Acceleration);

            // Frenado (hacia atrás)
            else if (verticalInput < 0)
                rb.AddForce(transform.forward * verticalInput * brakeForce * Time.fixedDeltaTime, ForceMode.Acceleration);
        }

        // Gira el coche solo si se está moviendo
        if (Mathf.Abs(flatVelocity.magnitude) > 0.5f)
        {
            // Calcula el ángulo de giro, teniendo en cuenta la dirección del movimiento
            float steer = horizontalInput * turnSpeed * Time.fixedDeltaTime * Mathf.Sign(Vector3.Dot(rb.linearVelocity, transform.forward));
            Quaternion turnRotation = Quaternion.Euler(0f, steer, 0f);
            rb.MoveRotation(rb.rotation * turnRotation);
        }
    }

}
