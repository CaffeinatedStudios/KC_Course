using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] private float horizontalSpeed = 7f;
    private Vector2 inputVector;
    private Vector3 moveDir;
    private float playerHeight = 2f;
    private float playerRadius = 0.7f;
    private float moveDistance;
    private bool isWalking;
    private bool canMove;
    
    private void Update()
    {
        HandleMovement();
        Debug.Log(inputVector);
    }

    private void HandleMovement() {
        if (GameInput.Instance == null) return;
        moveDistance = Time.deltaTime * rotateSpeed;

        inputVector = GameInput.Instance.GetMovementVectorNormalized();
        moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        // See if the player can moveDir
        canMove = CanMove(moveDir);

        if (canMove)
{            transform.position += moveDir * horizontalSpeed * Time.deltaTime;}
        else {
            // Cannot move directly to predicted position
            // So we try to either move in the x or y depending on where is clear
        Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;

            // Check if we can move on the X axis
            if (moveDir.x != 0 && CanMove(moveDirX))
            {
                UpdateTransform(moveDirX);
            } 
            // Check if we can move on the Z axis
            else if (moveDir.z != 0 && CanMove(moveDirZ))
            {
                UpdateTransform(moveDirZ);
            }
        }
        
        isWalking = moveDir != Vector3.zero;

        // Slerp towards a location
        transform.forward = Vector3.Slerp(transform.forward, moveDir, moveDistance);
    }

    private void UpdateTransform(Vector3 _moveDir)
    {
        transform.position += _moveDir * horizontalSpeed * Time.deltaTime;
    }

    // Returns true if the raycast doesn't hit anything
    private bool CanMove(Vector3 _moveDir) {
        return !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, _moveDir, moveDistance);
    }

    public bool IsWalking()
    {
        return isWalking;
    }
}
