using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckGround : MonoBehaviour
{

    private Neo_movement player;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInParent<Neo_movement>();    //Para buscar un componente en el objeto padre

    }

    void OnCollisionStay2D(Collision2D col)
    {
        player.grounded = true;
    }

    void OnCollisionExit2D(Collision2D col)
    {
        player.grounded = false;
    }
}
