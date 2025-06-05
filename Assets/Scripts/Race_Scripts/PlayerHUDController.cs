using UnityEngine;
using TMPro;

public class PlayerHUDController : MonoBehaviour
{
    [Header("Referencias UI")]
    [SerializeField] private RectTransform needleTransform;     // Aguja del velocímetro
    [SerializeField] private TMP_Text speedText;                // Texto de velocidad

    [Header("Configuración aguja")]
    [SerializeField] private float minAngle = 0f;               // Ángulo mínimo (velocidad 0)
    [SerializeField] private float maxAngle = -130f;            // Ángulo máximo (velocidad máxima)
    [SerializeField] private float maxSpeed = 200f;             // Velocidad máxima esperada (km/h)

    [Header("Referencia al vehículo")]
    [SerializeField] private Rigidbody playerRigidbody;         // Rigidbody del vehículo del jugador

    private void Update()
    {
        if (playerRigidbody == null) return;

        // Calcula la velocidad en km/h
        float speed = playerRigidbody.linearVelocity.magnitude * 3.6f;

        // Actualiza el texto de velocidad
        speedText.text = Mathf.RoundToInt(speed) + " km/h";

        // Calcula el ángulo de la aguja según la velocidad
        float angle = Mathf.Lerp(minAngle, maxAngle, Mathf.Clamp01(speed / maxSpeed));

        // Aplica la rotación a la aguja (en el eje Z)
        needleTransform.localRotation = Quaternion.Euler(0f, 0f, angle);
    }
}
