using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public TMP_Text[] NameA = new TMP_Text[8];
    public TMP_Text[] ScoreA = new TMP_Text[8];
    public TMP_Text[] MovesA = new TMP_Text[8];

    public GameObject Toy;
    public GameObject Balin;
    public GameObject Estela;
    public GameObject Pawn;
    public Sprite PawnDown;
    public Sprite PawnUp;
    public Sprite PawnLeft;
    public Sprite PawnRight;
    private SpriteRenderer SR;

    private GameObject Seg;
    private GameObject Bala;
    //private SpriteRenderer rend;

    public GameObject Arrows;
    public TMP_Text MOVE;

    public GameObject CanvasM;
    public GameObject CanvasH;

    private int move = 0;
    private int premove = 1;
    private float y = 0;

    private bool High = false;
    private bool Block = false;

    private void Start()
    {
        SR = Pawn.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        move = 0;

        move = Input.GetKeyDown(KeyCode.RightArrow) ? 1 : move;
        move = Input.GetKeyDown(KeyCode.UpArrow) ? 2 : move;
        move = Input.GetKeyDown(KeyCode.LeftArrow) ? 3 : move;
        move = Input.GetKeyDown(KeyCode.DownArrow) ? 4 : move;

        if (move != 0 && !High)
        {
            //rotacion
            if (move != premove)
            {
                if (move % 2 == premove % 2)
                    Toy.transform.Rotate(0, 180, 0);
                else
                {
                    if (premove == 4 && move == 1)
                        Toy.transform.Rotate(0, 0, 90, Space.World);
                    else if (premove == 1 && move == 4)
                        Toy.transform.Rotate(0, 0, -90, Space.World);
                    else if (move > premove)
                        Toy.transform.Rotate(0, 0, 90, Space.World);
                    else if (move < premove)
                        Toy.transform.Rotate(0, 0, -90, Space.World);
                }
            }
            premove = move;

            //move
            if (move == 1)
            {
                if (y == 0)
                    SceneManager.LoadScene("_Scenes/Game");
                else if (y == 1)
                {
                    High = true;
                    MenuHigh();
                }
                else if (y == 2)
                    Application.Quit();
                SR.sprite = PawnLeft;
                Block = true;
            }
            else if (move == 2)
            {
                if (y == 0)
                    Toy.gameObject.transform.position += new Vector3(0, -1, 0);
                else if (y == 1)
                    Toy.gameObject.transform.position += new Vector3(0, -2, 0);
                else if (y == 2)
                    y--;
                y++;
                SR.sprite = PawnDown;
            }
            else if (move == 3)
                SR.sprite = PawnRight;
            else if (move == 4)
            {
                if (y == 0)
                    y++;
                else if (y == 1)
                    Toy.gameObject.transform.position += new Vector3(0, 1, 0);
                else if (y == 2)
                    Toy.gameObject.transform.position += new Vector3(0, 2, 0);
                y--;
                SR.sprite = PawnUp;
            }
            Pawn.gameObject.transform.position = new Vector3(3.5f, Toy.transform.position.y, 0);
            move = 0;

            //Estela
            for (int i = 1; i <= 21; i++)
            {
                Seg = Instantiate(Estela, Toy.transform.position + Toy.transform.right * (i - 0.13f) + Toy.transform.up * 0.35f, Toy.transform.rotation);
                /*if (i == 7)
                {
                    Bala = Instantiate(Balin, Toy.transform.position + Toy.transform.right * (i + 0.42f) + Toy.transform.up * 0.35f, Toy.transform.rotation);
                    Destroy(Bala, 0.07f);
                }*/
                //rend = Piece.GetComponent<SpriteRenderer>();
                //StartCoroutine("FadeOut");
                Destroy(Seg, 0.07f);
            }
        }

        //HighscoresBack
        if (High && Input.GetKeyDown(KeyCode.Escape))
        {
            Toy.gameObject.SetActive(true);
            Pawn.gameObject.SetActive(true);
            CanvasM.gameObject.SetActive(true);
            CanvasH.gameObject.SetActive(false);
            High = false;
        }
    }

    /*IEnumerator FadeOut()
    {
        for(float f=1f; f >= -0.05f; f -= 0.05f)
        {
            Color c = rend.material.color;
            c.a = f;
            rend.material.color = c;
            yield return new WaitForSeconds(0.05f);
        }
    }*/

    public void MenuHigh()
    {
        Toy.gameObject.SetActive(false);
        Pawn.gameObject.SetActive(false);
        CanvasM.gameObject.SetActive(false);
        CanvasH.gameObject.SetActive(true);

        //Reiniciar los scores
        PlayerPrefs.DeleteKey("NEW");
        /*PlayerPrefs.DeleteKey("N1");
        PlayerPrefs.DeleteKey("N2");
        PlayerPrefs.DeleteKey("N3");
        PlayerPrefs.DeleteKey("N4");
        PlayerPrefs.DeleteKey("N5");
        PlayerPrefs.DeleteKey("N6");
        PlayerPrefs.DeleteKey("N7");
        PlayerPrefs.DeleteKey("N8");
        PlayerPrefs.DeleteKey("S1");
        PlayerPrefs.DeleteKey("S2");
        PlayerPrefs.DeleteKey("S3");
        PlayerPrefs.DeleteKey("S4");
        PlayerPrefs.DeleteKey("S5");
        PlayerPrefs.DeleteKey("S6");
        PlayerPrefs.DeleteKey("S7");
        PlayerPrefs.DeleteKey("S8");
        PlayerPrefs.DeleteKey("M1");
        PlayerPrefs.DeleteKey("M2");
        PlayerPrefs.DeleteKey("N3");
        PlayerPrefs.DeleteKey("M4");
        PlayerPrefs.DeleteKey("M5");
        PlayerPrefs.DeleteKey("M6");
        PlayerPrefs.DeleteKey("M7");
        PlayerPrefs.DeleteKey("M8");*/

        if (PlayerPrefs.GetInt("NEW") != 1)
		{
			//Real ones
			PlayerPrefs.SetString("N1", "GTG");
	        PlayerPrefs.SetString("N2", "IBO");
	        PlayerPrefs.SetString("N3", "GSZ");
	        PlayerPrefs.SetString("N4", "Ans");
	        PlayerPrefs.SetString("N5", "JAH");
	        PlayerPrefs.SetString("N6", "Fat");
	        PlayerPrefs.SetString("N7", "BxM");
	        PlayerPrefs.SetString("N8", "LGT");
	        PlayerPrefs.SetInt("S1", 77);
	        PlayerPrefs.SetInt("S2", 60);
	        PlayerPrefs.SetInt("S3", 56);
	        PlayerPrefs.SetInt("S4", 42);
	        PlayerPrefs.SetInt("S5", 33);
	        PlayerPrefs.SetInt("S6", 21);
	        PlayerPrefs.SetInt("S7", 15);
	        PlayerPrefs.SetInt("S8", 7);
	        PlayerPrefs.SetInt("M1", 217);
	        PlayerPrefs.SetInt("M2", 201);
	        PlayerPrefs.SetInt("M3", 173);
	        PlayerPrefs.SetInt("M4", 121);
	        PlayerPrefs.SetInt("M5", 99);
	        PlayerPrefs.SetInt("M6", 70);
	        PlayerPrefs.SetInt("M7", 54);
	        PlayerPrefs.SetInt("M8", 21);

			/*//Prove ones
	        PlayerPrefs.SetString("N1", "SNK");
	        PlayerPrefs.SetString("N2", "Ans");
	        PlayerPrefs.SetString("N3", "Fat");
	        PlayerPrefs.SetString("N4", "FUN");
	        PlayerPrefs.SetString("N5", "BxM");
	        PlayerPrefs.SetString("N6", "IBO");
	        PlayerPrefs.SetString("N7", "GSZ");
	        PlayerPrefs.SetString("N8", "JAH");
	        PlayerPrefs.SetInt("S1", 8);
	        PlayerPrefs.SetInt("S2", 7);
	        PlayerPrefs.SetInt("S3", 5);
	        PlayerPrefs.SetInt("S4", 5);
	        PlayerPrefs.SetInt("S5", 5);
	        PlayerPrefs.SetInt("S6", 4);
	        PlayerPrefs.SetInt("S7", 2);
	        PlayerPrefs.SetInt("S8", 2);
	        PlayerPrefs.SetInt("M1", 21);
	        PlayerPrefs.SetInt("M2", 20);
	        PlayerPrefs.SetInt("M3", 12);
	        PlayerPrefs.SetInt("M4", 12);
	        PlayerPrefs.SetInt("M5", 12);
	        PlayerPrefs.SetInt("M6", 9);
	        PlayerPrefs.SetInt("M7", 5);
	        PlayerPrefs.SetInt("M8", 5);*/

	        //Variable de permanencia
	        PlayerPrefs.SetInt("NEW", 1);
	    }

        for (int i=0; i<8;i++)
        {
            NameA[i].text = PlayerPrefs.GetString("N"+ (i+1).ToString());
            ScoreA[i].text = PlayerPrefs.GetInt("S" + (i+1).ToString()).ToString();
            MovesA[i].text = PlayerPrefs.GetInt("M" + (i+1).ToString()).ToString();
        }
    }
}
