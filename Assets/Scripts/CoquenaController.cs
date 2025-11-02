using UnityEngine;
using System.Collections;

public class CoquenaController : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    public Transform cameraTransform;
    private CharacterController controller;

    public Animator animator;

    [Header("Física del jugador")]
    public PlayerPhysics playerPhysics = new PlayerPhysics();

    //[Header("Energia")]
    //public float maxEnergy = 100f;
    //public float currentEnergy;
    //public float regenRate = 5f;

    //[Header("Poderes")]
    //public float windRange = 8f;
    //public float windForce = 10f;
    //public float songRange = 15f;
    //public float manifestDuration = 3f;

    //[Header("Costos de energía")]
    //public float costWind = 20f;
    //public float costSong = 25f;
    //public float costManifest = 40f;

    //[Header("Referencias")]
    //public ParticleSystem windEffect;
    //public ParticleSystem manifestEffect;
    //public AudioSource songSound;

    //private float manifestTimer;
    //private bool isManifesting;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        //currentEnergy = maxEnergy;
    }

    private void Update()
    {
        Move();
        //HandleAbilities();
        //RegenerateEnergy();
    }

    private void Move()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        bool jumpPressed = Input.GetButtonDown("Jump");

        // Calculamos primero la física (gravedad, salto, etc.)
        Vector3 physicsMovement = playerPhysics.UpdatePhysics(controller, jumpPressed);

        // Dirección del movimiento según la cámara
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 desiredMove = (camForward * v + camRight * h).normalized;

        // Rotación suave hacia la dirección de movimiento
        if (desiredMove.magnitude > 0.1f)
        {
            animator.SetBool("isWalk", true);
            Quaternion targetRotation = Quaternion.LookRotation(desiredMove);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            animator.SetBool("isWalk", false);
        }

        // Movimiento horizontal + vertical (juntos)
        Vector3 totalMovement = desiredMove * moveSpeed * Time.deltaTime + physicsMovement;
        controller.Move(totalMovement);
    }


    //private void HandleAbilities()
    //{
    //    if (Input.GetKeyDown(KeyCode.Q) && currentEnergy >= costWind)
    //        CastWind();

    //    if (Input.GetKeyDown(KeyCode.E) && currentEnergy >= costSong)
    //        CastSong();

    //    if (Input.GetKeyDown(KeyCode.R) && currentEnergy >= costManifest && !isManifesting)
    //        StartCoroutine(Manifest());
    //}

    //private void CastWind()
    //{
    //    currentEnergy -= costWind;
    //    if (windEffect) windEffect.Play();

    //    Collider[] hits = Physics.OverlapSphere(transform.position, windRange);
    //    foreach (Collider hit in hits)
    //    {
    //        if (hit.CompareTag("Animal"))
    //        {
    //            Vector3 dir = (hit.transform.position - transform.position).normalized;
    //            hit.GetComponent<Rigidbody>()?.AddForce(dir * windForce, ForceMode.Impulse);
    //        }
    //    }
    //}

    //private void CastSong()
    //{
    //    currentEnergy -= costSong;
    //    if (songSound) songSound.Play();

    //    Collider[] hits = Physics.OverlapSphere(transform.position, songRange);
    //    foreach (Collider hit in hits)
    //    {
    //        if (hit.CompareTag("Hunter"))
    //            hit.GetComponent<HunterAI>()?.Distract(transform.position);
    //    }
    //}

    //private IEnumerator Manifest()
    //{
    //    isManifesting = true;
    //    currentEnergy -= costManifest;
    //    if (manifestEffect) manifestEffect.Play();

    //    Collider[] hits = Physics.OverlapSphere(transform.position, 10f);
    //    foreach (Collider hit in hits)
    //    {
    //        if (hit.CompareTag("Hunter"))
    //            hit.GetComponent<HunterAI>()?.Scare();
    //    }

    //    yield return new WaitForSeconds(manifestDuration);
    //    isManifesting = false;
    //}

    //private void RegenerateEnergy()
    //{
    //    if (currentEnergy < maxEnergy)
    //        currentEnergy += regenRate * Time.deltaTime;
    //}
}
