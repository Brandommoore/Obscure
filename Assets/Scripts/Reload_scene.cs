using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Reload_scene : MonoBehaviour
{

    private Neo_movement player;
    //private bool player_state;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Neo").GetComponent<Neo_movement>();
    }

    // Update is called once per frame
    void Update()
    {

        //player_state = player.dead;

        if(Input.GetKeyDown(KeyCode.Return) && !player.winned)
        {
            //Scene scene = SceneManager.GetActiveScene();
            //SceneManager.LoadScene(scene.name);
            this.GetComponent<Canvas>().enabled = false;
            player.dead = false;
            player.transform.position = player.originalPos;
            GameObject.Find("Neo").GetComponent<Animator>().SetBool("Dead", player.dead);
        }else if (Input.GetKeyDown(KeyCode.Return) && player.winned)
        {
            Debug.Log("Quit Game");
            SceneManager.LoadScene(1);
        }
    }

    /*public void ReloadScene()
    {
        
    }*/
}
