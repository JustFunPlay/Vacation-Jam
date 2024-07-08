using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    [SerializeField] private float baseSpeed;
    [SerializeField] private int basePierce;
    [SerializeField] private float lifeTime;
    private int damage;
    private PlayerControl player;
    private List<EnemyBase> hitEnemies = new List<EnemyBase>();

    public void Launch(PlayerControl playerControl, int damage, float speedMod, int bonusPierce = 0)
    {
        player = playerControl;
        this.damage = damage;
        baseSpeed *= speedMod;
        basePierce += bonusPierce;
        StartCoroutine(ProjectileInAir());
    }
    private IEnumerator ProjectileInAir()
    {
        for (float f = 0; f <= lifeTime; f += Time.fixedDeltaTime)
        {
            transform.Translate(0, 0, baseSpeed * Time.fixedDeltaTime);
            yield return new WaitForFixedUpdate();
        }
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out EnemyBase hit))
        {
            if (!hitEnemies.Contains(hit))
            {
                hitEnemies.Add(hit);
                hit.TakeDamage(damage);
                if (hitEnemies.Count > basePierce) Destroy(gameObject);
            }
        }
    }
}
