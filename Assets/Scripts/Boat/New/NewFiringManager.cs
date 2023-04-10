using UnityEngine;

namespace Boat.New
{
    public class NewFiringManager: MonoBehaviour
    {
        public GameObject body;
        public GameObject barrels;
        public Transform[] barrelOutputs;
        [SerializeField] private GameObject cannonBall; // DONC C'EST CA LA MUNITION A CHANGER, LA RAMENER AU DESSUS ???
        
        public float initialVelocity = 20;
        private GameObject _bullet;

        public void Fire( /*Ajouter en argument la munition ?*/)
        {
            foreach (var barrelOutput in barrelOutputs)
            {
                var lossyScale =  barrelOutput.parent.lossyScale;
                var localScale = new Vector3(
                    lossyScale.x, 
                    lossyScale.x, 
                    lossyScale.z
                );
                cannonBall.transform.localScale = localScale;
                _bullet = Instantiate(cannonBall, barrelOutput.position, barrelOutput.rotation);  
                _bullet.GetComponent<Rigidbody>().velocity = barrelOutput.forward * initialVelocity;
            }
        }
    }
}