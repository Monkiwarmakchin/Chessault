using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingMove : MonoBehaviour
{
    public int x, y, vida;
    public char s, color;
    public Sprite King0;
    public Sprite King1;
    public Sprite King2;
    /*
    public Sprite KingN0;
    public Sprite KingN1;
    public Sprite KingN2;*/

    private PiecesID datos;
    private SpriteRenderer SR;
    // Use this for initialization
    void Start()
    {
        int col;
        this.name = datos.getName();
        SR = GetComponent<SpriteRenderer>();
        SR.sprite = King0;
        /*
        col = Random.Range(0,2);
        if(col == 0)
        {
        	SR.sprite = King0;
            color = 'B';
        }
        else
        {
            SR.sprite = KingN0;
            color = 'N';
        }*/
        if (x < 2)
            this.gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
    }

    // Update is called once per frame
    void Update()
    {
        datos.setXY(x,y);

        if((x > 9 && s=='L') || (x < 2 && s=='R')  || (y > 9 && s=='D') || (y < 2 && s=='U') )
            SR.color = Color.gray;
        else
            SR.color = new Color(255, 255, 255);
    }

    public void Move(PiecesID datos)
    {
        this.datos = datos;
        vida = datos.getVida();
        x = datos.getFX();
        y = datos.getFY();
        s = datos.getSentido();

        if(x < 2)
            this.gameObject.transform.position = new Vector3(x-5.75f, y-5.5f, 0);
        else if(x > 9)
            this.gameObject.transform.position = new Vector3(x-5.25f, y-5.5f, 0);
        else if(y < 2)
            this.gameObject.transform.position = new Vector3(x-5.5f, y-5.75f, 0);
        else if(y > 9)
            this.gameObject.transform.position = new Vector3(x-5.5f, y-5.25f, 0);
        else
            this.gameObject.transform.position = new Vector3(x-5.5f, y-5.5f, 0);
    }

    public void Shoot(PiecesID datos)
    {
        this.datos = datos;
        vida = datos.getVida();

        /*if(vida == 2)
        {
        	SR.sprite = King1;
        	
            if(color == 'B')
            else
                SR.sprite = KingN1;
                
        }*/
        if(vida == 1)
        {
            SR.sprite = King2;
        	/*
            if(color == 'B')
            else
                SR.sprite = KingN2;
                */
        }
    }
}
