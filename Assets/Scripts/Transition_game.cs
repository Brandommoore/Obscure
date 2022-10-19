using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition_game : MonoBehaviour
{
    public float wait_time2;

    void Start()
    {
        StartCoroutine(Wait_for_intro_2());
    }

    IEnumerator Wait_for_intro_2()
    {
        yield return new WaitForSeconds(wait_time2);

        SceneManager.LoadScene(3);
    }
}
