using UnityEngine;

public class cameraController : MonoBehaviour
{
    public Transform myPlayer;       // El transform del coche a seguir
    public Vector3 offset = new Vector3(0, 5, -10); // Distancia y altura respecto al coche
    public float smoothSpeed = 150f;   // Suavidad del seguimiento

    void LateUpdate()
    {
        // Calcula la posición deseada detrás del coche, alineada con su rotación
        Vector3 desiredPosition = myPlayer.TransformPoint(offset);

        // Interpolación suave para un movimiento más natural
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // La cámara siempre mira hacia el coche (puedes ajustar el target si quieres que mire un poco más arriba)
        transform.LookAt(myPlayer.position + Vector3.up * 1.0f); // 1.0f es para mirar un poco arriba del centro del coche
    }
}
