using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiecesID
{
    private int x;
    private int y;
    private int futx;
    private int futy;
    private int vida;
    private int level;
    private char sentido; //R: derecha - L: izquierda - U: arriba - D: abajo N: no tiene sentido
    private char action;  //A: atacar - M: moverse - N: no moverse - Q: quieto
    private string name;  //nombre de la pieza

    public PiecesID(string n, int x, int y, char s, char a, int p)
    {
        name = n;
        this.x = x;
        this.y = y;
        futx=0;
        futy=0;
        action = a;
        sentido = s;
        vida = 2;
        level = p;
    }

    public void setSentido(char s)
    {
        sentido = s;
    }

    public void setAction(char a)
    {
        action = a;
    }

    public void setXY(int x, int y)
    {
        this.x=x;
        this.y=y;
    }

    public void setFXY(int fx, int fy)
    {
        futx = fx;
        futy = fy;
    }

    public void setVida(int vida)
    {
        this.vida = vida;
    }

    public string getName()
    {
        return name;
    }

    public int getX()
    {
        return x;
    }

    public int getY()
    {
        return y;
    }

    public char getSentido()
    {
        return sentido;
    }

    public char getAction()
    {
        return action;
    }

    public int getFX()
    {
        return futx;
    }

    public int getFY()
    {
        return futy;
    }
    public int getVida()
    {
        return vida;
    }

    public int getLevel()
    {
        return level;
    }
}
