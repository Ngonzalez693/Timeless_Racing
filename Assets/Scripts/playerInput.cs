using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private CarMovement carMovement;

    private void Awake()
    {
        carMovement = GetComponent<CarMovement>();
    }

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        carMovement.SetInput(horizontal, vertical);
    }
}
