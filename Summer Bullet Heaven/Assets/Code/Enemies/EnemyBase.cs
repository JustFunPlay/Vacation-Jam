using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] private float speed;
    private Rigidbody rb;
    bool canmove = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        canmove = true;
    }
    private void Update()
    {
        if (canmove)
        {
            transform.LookAt(PlayerControl.CurrentPlayer.transform.position, Vector3.up);
            transform.Translate(0, 0, speed * Time.deltaTime);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        PlayerControl.CurrentPlayer.OnHitEffect(this);
        if (health <= 0)
            OnDeath();
    }
    private void OnDeath()
    {
        Destroy(gameObject);
    }
    public void GetKnockedBack()
    {
        StartCoroutine(pushedAway());
    }
    public IEnumerator pushedAway()
    {
        canmove = false;
        for (int i = 40; i > 0; i--)
        {
            rb.MovePosition(transform.position - transform.forward * 0.01f * (i/2));
            yield return new WaitForSeconds(0.015f);
        }
        yield return new WaitForSeconds(0.15f);
        canmove = true;
    }
}
