using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] private float speed;
    [SerializeField] private int pointRefund = 1;
    [SerializeField] private int expValue;
    [SerializeField] private ExpOrb expOrb;
    [SerializeField] private int killscore;
    private Rigidbody rb;
    bool canmove = false;

    public void ReadyUp(int difficultyMod)
    {
        health = Mathf.RoundToInt(health * 1 + 0.2f * difficultyMod);
        rb = GetComponent<Rigidbody>();
        canmove = true;
    }
    private void Update()
    {
        if (canmove)
        {
            transform.LookAt(PlayerControl.CurrentPlayer.transform.position, Vector3.up);
            rb.MovePosition(transform.position + transform.forward * speed * Time.deltaTime);
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
        ExpOrb newOrb = Instantiate(expOrb, transform.position, Quaternion.identity);
        newOrb.DropExp(expValue);
        CombatDirector.instance.AddPoints(pointRefund);
        CombatDirector.instance.AddHighScore(killscore);
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
