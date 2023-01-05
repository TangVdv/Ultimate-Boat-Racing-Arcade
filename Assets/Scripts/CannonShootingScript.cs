using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonShootingScript : MonoBehaviour
{
    public GameObject cannonBall;
    
    public Transform [] barrels;

    public float force;
    // Update is called once per frame
    void Update()
    {
        // FIRE CANNONBALL WHEN LEFT CLICK IS DOWN
        if (Input.GetMouseButtonDown(0))
        {
            for (int i = 0; i < barrels.Length; i++)
            {
                var localScale = cannonBall.transform.localScale;
                localScale = new Vector3(
                    barrels[i].parent.lossyScale.x,
                    barrels[i].parent.lossyScale.x,
                    barrels[i].parent.lossyScale.z
                );
                cannonBall.transform.localScale = localScale;
                GameObject bullet = Instantiate(cannonBall, barrels[i].position, barrels[i].rotation);
                bullet.GetComponent<Rigidbody>().AddForce(barrels[i].forward * force, ForceMode.Impulse);
            }
        }
    }
}
