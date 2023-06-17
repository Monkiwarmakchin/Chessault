using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BishopMove : MonoBehaviour
{
    public int x, y;
    public char s;
    public Sprite Bishop;
    //public Sprite BishopN;
    private PiecesID datos;
    private SpriteRenderer SR;
    // Use this for initialization
    void Start()
    {
        int col;
        SR = GetComponent<SpriteRenderer>();
        col = Random.Range(0,2);
        this.name = datos.getName();
        SR.sprite = Bishop;
        /*
        if(col == 0)
        	SR.sprite = Bishop;
        else
            SR.sprite = BishopN;*/
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
}
