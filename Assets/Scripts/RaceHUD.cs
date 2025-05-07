using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RaceHUD : MonoBehaviour
{
    [Header("Velocímetro")]
    public Rigidbody carRigidbody;
    public float maxSpeed = 200f;
    public float minNeedleAngle = 0f;
    public float maxNeedleAngle = -130f;
    public Image needleImage;
    public TextMeshProUGUI speedText;

    void Update()
    {
        float speed = carRigidbody.linearVelocity.magnitude * 3.6f;
        float normalizedSpeed = Mathf.Clamp01(speed / maxSpeed);
        float angle = Mathf.Lerp(minNeedleAngle, maxNeedleAngle, normalizedSpeed);

        if (needleImage != null)
            needleImage.rectTransform.localEulerAngles = new Vector3(0, 0, angle);

        if (speedText != null)
            speedText.text = Mathf.RoundToInt(speed) + " km/h";
    }
}
