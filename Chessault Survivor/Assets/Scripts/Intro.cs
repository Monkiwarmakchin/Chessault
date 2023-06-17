using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Intro : MonoBehaviour
{
    private AudioSource Shot;
    public GameObject Arrows;
    public TMP_Text MOVE;

    // Start is called before the first frame update
    void Start()
    {
        Shot = GetComponent<AudioSource>();
        PlayerPrefs.DeleteKey("LOGO");
        StartCoroutine(waiter());
    }

    IEnumerator waiter()
    {
        yield return new WaitForSeconds(6);

        Arrows.gameObject.SetActive(true);
        MOVE.gameObject.SetActive(true);
    }

    public void Update()
    {

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.UpArrow))
            SceneManager.LoadScene("_Scenes/Menu");
    }
}
