using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InputManagerInterface : MonoBehaviour
{
    protected BoatControls boat;

    protected short directionX = 0;
    protected short directionZ = 0;
    
    // Start is called before the first frame update
    public void setController(BoatControls b)
    {
        boat = b;
    }

   protected virtual void setMovement()
   {
       Debug.Log("Parent");
   }
    
    void FixedUpdate()
    {
        setMovement();

        boat.Movement(directionZ, directionX);
    }
}
