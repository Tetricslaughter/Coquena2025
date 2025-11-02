using UnityEngine;

[System.Serializable]
public class PlayerPhysics
{
    [Header("Gravedad y salto")]
    public float gravity = -9.81f;
    public float fallMultiplier = 1f;
    //public float jumpForce = 3f;

    private Vector3 velocity;
    private bool isGrounded;

    /// <summary>
    /// Aplica gravedad, salto y caída personalizada.
    /// </summary>
    public Vector3 UpdatePhysics(CharacterController controller)
    {
        isGrounded = controller.isGrounded;

        // Si toca el suelo, resetea velocidad vertical
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        // Salto
        //if (jumpPressed && isGrounded)
            //velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);

        // Control de caída
        if (velocity.y < 0)
            velocity.y += gravity * fallMultiplier * Time.deltaTime;
        else
            velocity.y += gravity * Time.deltaTime;

        return velocity * Time.deltaTime;
    }
}
