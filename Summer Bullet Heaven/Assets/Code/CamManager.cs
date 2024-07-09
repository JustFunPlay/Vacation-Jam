using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamManager : MonoBehaviour
{
    [SerializeField, Range(0.2f, 1f)]private float catchUpSpeed;
    private Transform playerPos;

    private void Start()
    {
        playerPos = PlayerControl.CurrentPlayer.transform;
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, playerPos.position, catchUpSpeed * Time.deltaTime);
    }
}
