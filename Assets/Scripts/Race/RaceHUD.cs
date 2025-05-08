using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RaceHUD : MonoBehaviour
{
    [Header("Velocímetro")]
    public Rigidbody carRigidbody;
    public float maxSpeed = 200f;
    public float minNeedleAngle = -130f;
    public float maxNeedleAngle = 130f;
    public Image needleImage;
    public TextMeshProUGUI speedText;
    
    [Header("Información de Carrera")]
    public TextMeshProUGUI positionText;
    public TextMeshProUGUI lapText;
    private RaceManager raceManager;

    [System.Obsolete]
    void Start()
    {
        raceManager = FindObjectOfType<RaceManager>();
    }
    
    void Update()
    {
        // Actualizar velocímetro
        float speed = carRigidbody.linearVelocity.magnitude * 3.6f; // m/s a km/h
        float normalizedSpeed = Mathf.Clamp01(speed / maxSpeed);
        float angle = Mathf.Lerp(minNeedleAngle, maxNeedleAngle, normalizedSpeed);

        if (needleImage != null)
            needleImage.rectTransform.localEulerAngles = new Vector3(0, 0, angle);

        if (speedText != null)
            speedText.text = Mathf.RoundToInt(speed) + " km/h";
        
        // Actualizar información de carrera
        if (raceManager != null)
        {
            var playerInfo = raceManager.GetPlayerInfo();
            if (playerInfo != null)
            {
                // Mostrar posición actual
                if (positionText != null)
                {
                    string suffix = GetPositionSuffix(playerInfo.position);
                    positionText.text = playerInfo.position + suffix;
                }
                
                // Mostrar vuelta actual/total
                if (lapText != null)
                {
                    lapText.text = playerInfo.currentLap + "/" + raceManager.totalLaps;
                }
            }
        }
    }
    
    // Método para obtener el sufijo correcto (1ro, 2do, 3ro, etc.)
    private string GetPositionSuffix(int position)
    {
        switch (position)
        {
            case 1: return "ro";
            case 2: return "do";
            case 3: return "ro";
            default: return "to";
        }
    }
}
