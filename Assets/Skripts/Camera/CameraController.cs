using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float speed;

    private void Update()
    {
        Vector3 targetPosition = target.position;
        //targetPosition.y = transform.position.y;
        targetPosition.z = -10;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed);

    }
}
