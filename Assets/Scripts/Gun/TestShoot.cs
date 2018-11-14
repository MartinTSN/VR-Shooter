using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestShoot : MonoBehaviour
{
    public GameObject bulletPrefab;
    void Update()
    {
        Ray raycast = new Ray(transform.position, transform.forward);
        Debug.DrawRay(raycast.origin, raycast.direction * 1000);

        if (Input.GetMouseButtonDown(1) == true)
        {
            Instantiate(bulletPrefab, new Vector3(transform.position.x + 0.10f, transform.position.y, transform.position.z), Quaternion.identity);
        }
    }
}
