using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ShootnMove : MonoBehaviour
{
    public int x;
    public int y;
    private int start;
    private int move;
    private int premove;

    public GameObject tablero;
    public GameObject spawn;
    private GameObject[] pieces;
    public PiecesID Datatoy;
    public TMP_Text High;

    public GameObject Shot;
    public GameObject CanvasR;
    public TMP_Text RETREAT, SHOOT;
    public bool Pause = false;

    public void Start()
    {
        //El soldado inicia en una de las 4 casillas centrales al azar
        start = Random.Range(0, 4);
        if (start == 0)
        {
        	x=6;
        	y=5;
            this.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            premove = 1;
        }
        else if (start == 1)
        {
            x=y=5;
            this.gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
            premove = 3;
        }
        if (start == 2)
        {
            x=5;
        	y=6;
            this.gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
            premove = 3;
        }
        if (start == 3)
        {
            x=y=6;
            this.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            premove = 1;
        }
        //creamos los datos del soldado con su nombre, coordenas, sentido y accion
        Datatoy = new PiecesID("ToySoldier",x,y,'N','A',0);
        //llamamos al metodo GetData que esta en el tablero mandando los datos del soldado
        tablero.SendMessage("GetToy", Datatoy);
        //movemos al soldado fisicamente a su posicion
        //this.gameObject.transform.position = new Vector3(x-5.5f, y-5.5f, 0);

        High.text = "TARGET:\n8."+PlayerPrefs.GetString("N8")+"\nScore: "+PlayerPrefs.GetInt("S8")+"\nMoves: "+PlayerPrefs.GetInt("M8");
    }

    void Update()
    {
        char s='N';
    	move=0;

        if (Input.GetKeyDown(KeyCode.Escape) && Pause)
            SceneManager.LoadScene("Menu");

        if (Input.GetKeyDown(KeyCode.Escape) && !Pause)
        {
            CanvasR.gameObject.SetActive(true);
            RETREAT.gameObject.SetActive(false);
            SHOOT.gameObject.SetActive(false);
            Pause = true;
        }

        //Registro de tecla presionada
        if (!Pause)
        {
            move = Input.GetKeyDown(KeyCode.RightArrow) ? 1 : move;
            move = Input.GetKeyDown(KeyCode.UpArrow) ? 2 : move;
            move = Input.GetKeyDown(KeyCode.LeftArrow) ? 3 : move;
            move = Input.GetKeyDown(KeyCode.DownArrow) ? 4 : move;
        }

        if (Pause && (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.DownArrow)))
        {
            CanvasR.gameObject.SetActive(false);
            RETREAT.gameObject.SetActive(true);
            SHOOT.gameObject.SetActive(true);
            Pause = false;
        }

        //Revision y movimiento de las coordenadas con respecto a la tecla presionada
        if (move!=0)
        {
            //rotacion
            if (move != premove)
            {
                if (move % 2 == premove % 2)
                    this.transform.Rotate(0, 180, 0);
                else
                {
                    if (premove == 4 && move == 1)
                        this.transform.Rotate(0, 0, 90, Space.World);
                    else if (premove == 1 && move == 4)
                        this.transform.Rotate(0, 0, -90, Space.World);
                    else if (move > premove)
                        this.transform.Rotate(0, 0, 90, Space.World);
                    else if (move < premove)
                        this.transform.Rotate(0, 0, -90, Space.World);
                }
            }
            premove = move;

            //move
            if (move == 1)
            {
            	x--;
                s='R';
            }
            if (move == 2)
            {
                y--;
                s = 'U';
            }
            if (move == 3)
            {
                x++;
                s = 'L';
            }
            if (move == 4)
            {
            	y++;
                s='D';
            }
            Datatoy.setFXY(x,y);

            tablero.SendMessage("GetToy",Datatoy);
            tablero.SendMessage("DP",s);
            move = 0;

            tablero.SendMessage("Score");
        }
    }

    public void Move(PiecesID data)
    {
        Datatoy=data;
        x=Datatoy.getFX();
        y=Datatoy.getFY();
        //Debug.Log("me movere al "+data.getFX()+","+data.getFY());
        this.gameObject.transform.position = new Vector3(data.getFX()-5.5f, data.getFY()-5.5f, 0);
    }
}
