using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpOrb : MonoBehaviour
{
    private int expHeld;

    public void DropExp(int expValue)
    {
        expHeld = expValue;
        StartCoroutine(WaitForPickup());
    }

    private IEnumerator WaitForPickup()
    {
        while (Vector3.Distance(transform.position, PlayerControl.CurrentPlayer.transform.position) > 3)
        {
            yield return new WaitForSeconds(0.1f);
        }
        PlayerControl.CurrentPlayer.GetExp(expHeld);
        Destroy(gameObject);
    }
}
