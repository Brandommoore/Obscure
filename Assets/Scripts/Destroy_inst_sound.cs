using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Destroy_inst_sound : MonoBehaviour
{
    public float life_time;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, life_time);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
