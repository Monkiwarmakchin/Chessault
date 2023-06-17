using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Music : MonoBehaviour {

    static Music Instance;
    
    private GameObject Toy;

    public AudioClip [] Pistas = new AudioClip[5];
    public AudioSource AS;
    public AudioSource Shot;
    private int Playing = -1, NewPlaying;

    private bool Pause = false;

    // Use this for initialization
    void Start()
    {
        NewPlaying = Random.Range(0, 5);
        AS = this.gameObject.GetComponent<AudioSource>();
        if (Instance != null)
            GameObject.Destroy(gameObject);
        else
        {
            GameObject.DontDestroyOnLoad(gameObject);
            Instance = this;
        }
    }

    public void ActiveP()
    {
        Pause = true;
    }

    void Update ()
	{
        if (Playing != NewPlaying)
            StartCoroutine(Change());

        if(!Toy)
            Toy = GameObject.FindWithTag("ToySoldier");

        if (Toy && Toy.GetComponent<ShootnMove>() && Toy.GetComponent<ShootnMove>().Pause == true)
            Pause = true;

        if (Toy && Toy.activeSelf == true && !Pause)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.UpArrow))
                Shot.Play();
        }
        else if(Pause)
            Pause = false;
    }

    IEnumerator Change()
    {
        Playing = NewPlaying;
        AS.clip = Pistas[Playing];
        AS.Play(0);
        yield return new WaitForSeconds(AS.clip.length);
        while (NewPlaying == Playing)
            NewPlaying = Random.Range(0, 5);
    }
}
