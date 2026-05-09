using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] private float horizontalSpeed = 7f;
    private Vector2 inputVector;
    private Vector3 moveDir;
    private bool isWalking;

    private void Update()
    {
        HandleMovement();
        Debug.Log(inputVector);
    }

    private void HandleMovement() {
        if (GameInput.Instance == null) return;
        
        inputVector = GameInput.Instance.GetMovementVectorNormalized();
        moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        transform.position += moveDir * horizontalSpeed * Time.deltaTime;
        
        isWalking = moveDir != Vector3.zero;

        // Slerp towards a location
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
    }

    public bool IsWalking()
    {
        return isWalking;
    }
}
