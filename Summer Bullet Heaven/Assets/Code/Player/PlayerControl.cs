using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    public static PlayerControl CurrentPlayer;
    [Header("Input actions")]
    [SerializeField] private InputAction moveInput;
    [SerializeField] private InputAction attackInput;
    [SerializeField] private InputAction specialInput;
    [SerializeField] private InputAction dodgeInput;

    [Header("Stats")]
    [SerializeField] private int maxHealth = 5;
    private int currentHealth;
    [SerializeField] private float moveSpeed = 3;
    [SerializeField] private float attackSpeed = 1;
    [SerializeField] private int damage = 4;
    [SerializeField] private float dashCooldown = 1;
    [SerializeField] private float dashDistance = 1;
    [SerializeField] private float projectileSpeedModifier = 1;

    private Vector2 moveVector;
    private Rigidbody rb;
    bool isDodgeing;
    bool canDodge = true;
    bool keepAttacking;
    bool canAttack = true;
    float invulTime;
    [Header("Additional stuff")]
    [SerializeField] private Transform shootingThingy;
    [SerializeField] private PlayerProjectile projectile;

    private void Awake()
    {
        CurrentPlayer = this;
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody>();
        moveInput.performed += Move;
        moveInput.canceled += Move;
        moveInput.Enable();
        attackInput.performed += Shoot;
        attackInput.canceled += Shoot;
        attackInput.Enable();
        specialInput.Enable();
        dodgeInput.performed += Dash;
        dodgeInput.Enable();
    }

    private void Update()
    {
        if (invulTime > 0) invulTime -= Time.deltaTime;
        if (!isDodgeing)
            rb.MovePosition(new Vector3(transform.position.x + moveVector.x * moveSpeed * Time.deltaTime, transform.position.y, transform.position.z + moveVector.y * moveSpeed * Time.deltaTime));
    }

    private void Move(InputAction.CallbackContext callbackContext)
    {
        moveVector = callbackContext.ReadValue<Vector2>().normalized;
        if (moveVector != Vector2.zero)
        {
            shootingThingy.localPosition = new Vector3(moveVector.x, 0.5f, moveVector.y);
            shootingThingy.LookAt(shootingThingy.position + new Vector3(moveVector.x, 0, moveVector.y), Vector3.up);
        }
    }
    private void Shoot(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            keepAttacking = true;
            if (canAttack)
                StartCoroutine(Attacking());
        }
        else if (callbackContext.canceled)
        {
            keepAttacking = false;
        }
    }
    private IEnumerator Attacking()
    {
        yield return null;
        canAttack = false;
        while (keepAttacking)
        {
            PlayerProjectile newProjectile = Instantiate(projectile, shootingThingy.position, shootingThingy.rotation);
            newProjectile.Launch(this, damage, projectileSpeedModifier);
            yield return new WaitForSeconds(1f / attackSpeed);
        }
        canAttack = true;
    }

    private void Dash(InputAction.CallbackContext callbackContext)
    {
        if (!isDodgeing && canDodge)
            StartCoroutine(Dodgeing());
    }
    private IEnumerator Dodgeing()
    {
        isDodgeing = true;
        canDodge = false;
        Vector3 dodgeVector = new Vector3(moveVector.x * moveSpeed * dashDistance, 0, moveVector.y * moveSpeed * dashDistance);
        if (dodgeVector == Vector3.zero)
            dodgeVector = -shootingThingy.forward * moveSpeed * dashDistance;
        //GetComponent<Collider>().enabled = false;
        for (int i = 40; i > 0; i--)
        {
            rb.MovePosition(transform.position + dodgeVector * 0.01f * (i / 15 + 1f));
            yield return new WaitForSeconds(0.01f);
        }
        isDodgeing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDodge = true;
    }

    public void OnHitEffect(EnemyBase hitTarget)
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out EnemyBase enemy) && invulTime <= 0)
        {
            currentHealth--;
            //if (currentHealth <= 0)
            //    Destroy(gameObject);
            invulTime = 1f;
            Collider[] colliders = Physics.OverlapSphere(transform.position, 5);
            foreach (Collider collider in colliders)
            {
                if (collider.TryGetComponent(out EnemyBase closeTarget))
                {
                    closeTarget.GetKnockedBack();
                }
            }
        }
    }
}
