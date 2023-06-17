using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public TMP_Text RETREAT, SHOOT, EXIT, RETRY, CONFIRM, MATE;
    public TMP_InputField Name;
    public GameObject CanvasO;
    public GameObject Escape, Arrows;
    private bool Dead = false, High = false;
    private int Position;

    public void Active(string MDead)
    {
        Dead = true;
        MATE.text = MDead;
    }

    public void NewHigh(int high)
    {
        High = true;
        Position = high;
    }

    void Update ()
    {
		if(Dead)
        {
            CanvasO.gameObject.SetActive(true);
            Escape.gameObject.SetActive(false);
            RETREAT.gameObject.SetActive(false);
            Arrows.gameObject.SetActive(false);
            SHOOT.gameObject.SetActive(false);

            if (High)
            {
                Name.gameObject.SetActive(true);
                Name.ActivateInputField();
                //Esto hace que las flechas no muevan el cursor del input field
                Name.MoveToEndOfLine(false, false);

                if(Name.text.Length==3)
                {
                    Arrows.gameObject.SetActive(true);
                    CONFIRM.gameObject.SetActive(true);
                    if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.UpArrow))
                    {
                        //Debug.Log("Posicionando");

                        for(int i=8; i>Position; i--)
                        {
                            //Debug.Log("Recorriendo");
                            PlayerPrefs.SetString("N"+i, PlayerPrefs.GetString( "N"+(i-1) ));
                            PlayerPrefs.SetInt("S"+i, PlayerPrefs.GetInt( "S"+(i-1) ));
                            PlayerPrefs.SetInt("M"+i, PlayerPrefs.GetInt( "M"+(i-1) ));
                        }

                        Tablero TScript = GameObject.Find("Tablero").GetComponent<Tablero>();

                        PlayerPrefs.SetString("N"+ Position, Name.text);
                        PlayerPrefs.SetInt("S"+ Position, TScript.S );
                        PlayerPrefs.SetInt("M"+ Position, TScript.NewM );

                        Name.gameObject.SetActive(false);
                        CONFIRM.gameObject.SetActive(false);

                        //Arrows.gameObject.SetActive(true);
                        //Escape.gameObject.SetActive(true);

                        High = false;

                        /*for (int i=8; i>=1;i--)
                        {
                            Debug.Log("N: "+PlayerPrefs.GetString("N"+i)+", S: "+PlayerPrefs.GetInt("S"+i)+", M: "+PlayerPrefs.GetInt("M"+i));
                        }*/

                    }
                }
                else
                {
                    Arrows.gameObject.SetActive(false);
                    CONFIRM.gameObject.SetActive(false);
                }
            }
            else
            {
                Arrows.gameObject.SetActive(true);
                Escape.gameObject.SetActive(true);

                EXIT.gameObject.SetActive(true);
                RETRY.gameObject.SetActive(true);
                if (Input.GetKeyDown(KeyCode.Escape))
                    SceneManager.LoadScene("_Scenes/Menu");
                if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.UpArrow))
                    SceneManager.LoadScene("_Scenes/Game");
            }
        }
	}
}
