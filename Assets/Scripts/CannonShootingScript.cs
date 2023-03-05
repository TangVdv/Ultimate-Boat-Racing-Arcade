using System.Collections;
using UnityEngine;

public class CannonShootingScript : MonoBehaviour
{
    [SerializeField] private GameObject cannonBall;
    [SerializeField] private Transform [] barrels;
    public float initialVelocity = 20;
    public float reloadTime = 5f;
    public int ammo = 2;

    private bool _isReloading;
    private bool _isLoaded;
    private GameObject _bullet;

    void Start()
    {
        if (ammo > 0)
        {
            StartCoroutine(Reload());
        }
    }

    void Update()
    {
        // Fire bullet when left click mouse is down
        if (_isLoaded)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Fire();
            }
        }
        else if(ammo > 0 && _isReloading == false)
        {
            StartCoroutine(Reload());
        }
    }

    void Fire()
    {
        for (int i = 0; i < barrels.Length; i++) 
        {
            var localScale = new Vector3(
                barrels[i].parent.lossyScale.x, 
                barrels[i].parent.lossyScale.x, 
                barrels[i].parent.lossyScale.z
            );
            cannonBall.transform.localScale = localScale;
            _bullet = Instantiate(cannonBall, barrels[i].position, barrels[i].rotation);  
            _bullet.GetComponent<Rigidbody>().velocity = barrels[i].forward * initialVelocity;
        }
        print("fire !");
        _isLoaded = false;
    }

    IEnumerator Reload()
    {
        print("isReloading");
        _isReloading = true;
        yield return new WaitForSeconds(reloadTime);
        ammo--;
        _isLoaded = true;
        print("isLoaded");
        print("Ammo left : "+ammo);
        _isReloading = false;
    }
}
