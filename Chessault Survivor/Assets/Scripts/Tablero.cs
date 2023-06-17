using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

enum Fases{
	inactivo,
	calculo,
	mover,
	coronacion,
	muerte,
	spawn,
	score
}

public class Tablero : MonoBehaviour
{
    private int num_spanw;
    Fases fase = Fases.score;

    public GameObject ToyO;
    public GameObject Balin;
    public GameObject Estela;
    public GameObject Pawn;
    public GameObject Horse;
    public GameObject Bishop;
    public GameObject Tower;
    public GameObject Queen;
    public GameObject King;

    public bool PawnS = true;
    public bool HorseS = true;
    public bool BishopS = true;
    public bool TowerS = true;
    public bool QueenS = true;
    public bool KingS = true;

    public TMP_Text High;
    public TMP_Text You;
    public GameObject CanvasO;

    private PiecesID[][] tablero;
    private int PiecesCounter = 0;
    private int NoSpawn = 1;
    private char Tutorial = 'S';
    private PiecesID Toy;
    public int S = 0, M = -1, Pos = 9, Tar = 9;
    private bool New = false;
    public int NewM;
    private char ToyS;
    private string Target;


    void Start()
    {
        num_spanw = 0;
        //construimos el tablero de PiecesID
        tablero = new PiecesID[12][];

        for (int i = 0; i < 12; i++)
            tablero[i] = new PiecesID[12];

        CalTar();
    }

    void Update()
    {
        int x, y;
        if (fase == Fases.calculo)
        {
            ciclo("Pawn_", "calculo");
            ciclo("Bisho", "calculo");
            ciclo("Horse", "calculo");
            ciclo("Tower", "calculo");
            ciclo("Queen", "calculo");
            ciclo("King_", "calculo");
            fase = Fases.mover;
        }
        else if (fase == Fases.mover)
        {
            ciclo("Pawn_", "mover");
            ciclo("Bisho", "mover");
            ciclo("Horse", "mover");
            ciclo("Tower", "mover");
            ciclo("Queen", "mover");
            ciclo("King_", "mover");
            fase = Fases.coronacion;
        }
        else if (fase == Fases.coronacion)
        {
            Coronacion();
            fase = Fases.muerte;
        }
        else if (fase == Fases.muerte)
        {
            x = Toy.getX();
            y = Toy.getY();
            if (x == 10 || x == 1 || y == 10 || y == 1)
            {
                Destroy(ToyO);
                Debug.Log("New High: " + Pos);
                if (New)
                    CanvasO.SendMessage("NewHigh", Pos);
                CanvasO.SendMessage("Active", "OUT");
            }
            fase = Fases.spawn;
        }
        else if (fase == Fases.spawn)
        {
            //AcomodarPiezas();
            Spawn();
            fase = Fases.score;
        }
        else if (fase == Fases.score)
        {
            //AcomodarPiezas();
            Score();
            High.text = Target;
            fase = Fases.inactivo;
        }
    }

    //Recibe un objeto de tipo PiecesID con los datos del soldado 
    public void GetToy(PiecesID datos)
    {
        if (datos.getFX() == 0) //El soldado aparece en el tablero
        {
            tablero[datos.getX()][datos.getY()] = datos;
            datos.setFXY(datos.getX(), datos.getY());
        }
        else if (tablero[datos.getFX()][datos.getFY()] == null)//el soldado se mueve en el tablero
        {
            tablero[datos.getX()][datos.getY()] = null;
            datos.setXY(datos.getFX(), datos.getFY());
            tablero[datos.getFX()][datos.getFY()] = datos;
            fase = Fases.calculo;
        }
        else //El soldado se queda en la misma posicion
        {
            datos.setFXY(datos.getX(), datos.getY());
            fase = Fases.calculo;
        }
        Toy = datos;
        ToyO.SendMessage("Move", Toy);

        M++;
    }

    //Funcion que recorre todo el tablero, recibe un tipo de pieza y una accion
    //realiza las funciones de calculo y de movimiento
    public void ciclo(string piece, string action)
    {
        int i, j;
        string nombre;

        /*foreach (PiecesID P in Piezas)
        {
            //Accion calculo1: calculo de movimientos
            if (action == "calculo" && P.getAction() == 'N')
            {
                //le preguntamos el nombre a la pieza en el tablero
                nombre = P.getName();
                //la pieza encontrada es del tipo de pieza que le mandamos
                if (nombre.Substring(0, 5) == piece.Substring(0, 5))
                    //llamamos de forma dinamica la funcion para mover ese tipo de pieza
                    this.gameObject.SendMessage(piece + "Move", P);
            }
            //Accion mover: movera las piezas fisicamente
            else if (action == "mover")
            {
                if (P.getAction() == 'M' || P.getAction() == 'A')
                    Move(P.getX(), P.getY());
                else if (P.getAction() != 'C')
                    P.setAction('N');
            }
        }*/
        
		//recorremos el tablero
		for(i = 0;i < 12; i++)
		{
			for(j = 0;j < 12; j++)
			{
				//revisamos si hay alguna pieza en esa casilla
				if(tablero[i][j]!=null)
				{
					if(tablero[i][j].getName()!="ToySoldier")
					{
						//Accion calculo1: calculo de movimientos
						if(action=="calculo" && tablero[i][j].getAction()=='N')
						{
							//le preguntamos el nombre a la pieza en el tablero
							nombre=tablero[i][j].getName();
							//la pieza encontrada es del tipo de pieza que le mandamos
							if(nombre.Substring(0,5) == piece.Substring(0,5))
								//llamamos de forma dinamica la funcion para mover ese tipo de pieza
								this.gameObject.SendMessage(piece+"Move",tablero[i][j]);
						}
						//Accion mover: movera las piezas fisicamente
						else if(action=="mover")
						{
							if(tablero[i][j].getAction()=='M' || tablero[i][j].getAction()=='A')
								Move(i,j);
							else if(tablero[i][j].getAction()!= 'C')
								tablero[i][j].setAction('N');
						}
					}
					
				}
			}
		}
    }

    /*public void Move(PiecesID P)
    {
        int x, y;
        char interC = 'n';
        GameObject Piece;
        PiecesID aux = null;

        x = P.getFX();
        y = P.getFY();

        if (tablero[x][y] != null)
        {
            if (tablero[x][y].getName() != "ToySoldier")
            {
                if (tablero[x][y].getFX() == P.getX() && tablero[x][y].getFX() == P.getX()) // ??????????????????
                {
                    interC = 's';
                    aux = tablero[x][y];
                }
                else
                    Move(tablero[x][y]);
            }

        }

        if (P.getAction() == 'A')
        {
            Piece = ToyO;
            Destroy(Piece);
            Debug.Log("New High: " + Pos);
            if (New)
                CanvasO.SendMessage("NewHigh", Pos);
            CanvasO.SendMessage("Active", "MATE");
        }

        tablero[x][y] = P;
        int i = P.getX(), j = P.getY();

        if (interC == 's')
        {
            tablero[i][j] = aux;
            tablero[i][j].setXY(i, j);
            Piece = GameObject.Find(tablero[i][j].getName());
            Piece.SendMessage("Move", tablero[i][j]);
            tablero[i][j].setAction('N');
        }
        else
        {
            tablero[i][j] = null;
            tablero[x][y].setXY(x, y);
            Piece = GameObject.Find(tablero[x][y].getName());
            Piece.SendMessage("Move", tablero[x][y]);
            tablero[x][y].setAction('N');
        }
    }*/

    public void Move(int i, int j)
    {
        int x, y;
        char interC = 'n';
        GameObject Piece;
        PiecesID aux = null;

        x = tablero[i][j].getFX();
        y = tablero[i][j].getFY();

        if (tablero[x][y] != null)
        {
            if (tablero[x][y].getName() != "ToySoldier")
            {
                if (tablero[x][y].getFX() == i && tablero[x][y].getFY() == j) //??????????????????????????????????????????
                {
                    interC = 's';
                    aux = tablero[x][y];
                }
                else
                    Move(x, y);
            }
        }

        if (tablero[i][j].getAction() == 'A')
        {
            Piece = ToyO;
            Destroy(Piece);
            Debug.Log("New High: " + Pos);
            if (New)
                CanvasO.SendMessage("NewHigh", Pos);
            CanvasO.SendMessage("Active", "MATE");
        }
        tablero[x][y] = tablero[i][j];
        if (interC == 's')
        {
            tablero[i][j] = aux;
            tablero[i][j].setXY(i, j);
            Piece = GameObject.Find(tablero[i][j].getName());
            Piece.SendMessage("Move", tablero[i][j]);
            tablero[i][j].setAction('N');
        }
        else
        {
            tablero[i][j] = null;
            tablero[x][y].setXY(x, y);
            Piece = GameObject.Find(tablero[x][y].getName());
            Piece.SendMessage("Move", tablero[x][y]);
            tablero[x][y].setAction('N');
        }
    }

    //Funcion para calcular el movimientos de los pawns. recibe los datos de la pieza
    public void Pawn_Move(PiecesID piece)
    {
        int x, y, x2 = 0, y2 = 0;
        char r, s;
        bool move = true;
        x = piece.getX();
        y = piece.getY();
        s = piece.getSentido();
        //Caso 3: el peon se encuentra hasta el otro extremo del tablero 
        if ((x == 9 && s == 'R') || (x == 2 && s == 'L') || (y == 9 && s == 'U') || (y == 2 && s == 'D'))
            piece.setAction('C');
        //Caso 1: el peon se encuentra en el tumor 
        else if ((x > 9 && s == 'L') || (x < 2 && s == 'R') || (y > 9 && s == 'D') || (y < 2 && s == 'U'))
            piece.setAction(SimpleMove(piece, 5));
        //Caso 2: el peon se encuentra dentro del tablero 
        else
        {
            if (s == 'R')
            {
                x++; x2 = x; y2 = y - 1; y++;
            }
            else if (s == 'L')
            {
                x--; x2 = x; y2 = y - 1; y++;
            }
            else if (s == 'U')
            {
                y++; y2 = y; x2 = x - 1; x++;
            }
            else if (s == 'D')
            {
                y--; y2 = y; x2 = x - 1; x++;
            }
            if (Validate("ATK", 0, 0, piece))
            {
                if (tablero[x][y] != null)
                {
                    if (tablero[x][y].getName() == "ToySoldier")
                    {
                        move = false;
                        piece.setAction('A');
                        piece.setFXY(x, y);
                    }

                }
                else if (tablero[x2][y2] != null)
                {
                    if (tablero[x2][y2].getName() == "ToySoldier")
                    {
                        move = false;
                        piece.setAction('A');
                        piece.setFXY(x2, y2);
                    }
                }
            }
            else
                move = true;
            if (move)
            {
                r = SimpleMove(piece, 5);
                if (r == 'M')
                    piece.setAction('M');
                else
                    piece.setAction('Q');
            }
        }
    }

    //Funcion que calcula un movimiento simple de una casilla en el sentido de la pieza
    //Recibe los datos de la pieza y devuelve un entero que significa si se puede mover o no y por quien (pieza o soldado)
    //M: se puede mover - P: bloqueado por una pieza - T: bloqueado por el soldado
    public char SimpleMove(PiecesID piece, int n)
    {
        int x, y;
        char r = 'Q';
        bool move = false;
        x = piece.getX();
        y = piece.getY();
        //Verificamos sentido
        if (piece.getSentido() == 'R')
            x++;
        else if (piece.getSentido() == 'L')
            x--;
        else if (piece.getSentido() == 'U')
            y++;
        else if (piece.getSentido() == 'D')
            y--;
        //vemos si hay una pieza en frente de la que se movera
        if (tablero[x][y] != null)
        {
            //Caso a: la pieza que nos bloquea es el soldado
            if (tablero[x][y].getName() == "ToySoldier" && Validate("ATK", 0, 0, piece))
            {
                r = 'A';
                move = true;
            }
            else
            {
                //Caso b: la pieza que nos bloquea no ha calculado su movimiento
                if (tablero[x][y].getAction() == 'N')
                {
                    if (tablero[x][y].getFX() != 100)
                    {
                        piece.setFXY(100, 0);
                        this.gameObject.SendMessage(tablero[x][y].getName().Substring(0, n) + "Move", tablero[x][y]);
                    }
                    else
                        piece.setAction('Q');
                }
                //Caso c: la pieza que nos bloquea no se moverá
                if (tablero[x][y].getAction() == 'Q' || tablero[x][y].getAction() == 'N')
                    r = 'Q';
                else//Caso d: la pieza que nos bloquea se moverá
                    move = true;
            }
        }
        else//Caso e: no hay piezas que nos bloqueen
            move = true;
        if (move && Validate("FPosition", x, y, piece))
        {
            if (r != 'A')
                r = 'M';
            piece.setFXY(x, y);
        }
        else
            piece.setFXY(0, 0);
        return r;
    }

    public void HorseMove(PiecesID piece)
    {
        int x, y, xf, yf, i = -2;
        int[] moves;
        float min = 81, aux;
        char s, accion = 'N';
        s = piece.getSentido();
        x = piece.getX();
        y = piece.getY();

        //Caso 1: el caballo se encuentra en el tumor 
        if ((x > 9 && s == 'L') || (x < 2 && s == 'R') || (y > 9 && s == 'D') || (y < 2 && s == 'U'))
            piece.setAction(SimpleMove(piece, 5));
        //Caso 2: el caballo se encuentra dentro del tablero 
        else
        {
            if (piece.getSentido() == 'R')
                moves = new int[] { 1, 2, 2, 1, 2, -1, 1, -2, -1, -2, -2, -1, -2, 1, -1, 2 };
            else
                moves = new int[] { -1, 2, -2, 1, -2, -1, -1, -2, 1, -2, 2, -1, 2, 1, 1, 2 };
            for (i = 0; i < moves.Length - 1; i = i + 2)
            {
                if (Mathf.Abs(moves[i]) != Mathf.Abs(moves[i + 1]) && moves[i] != 0 && moves[i + 1] != 0)
                {
                    xf = x + moves[i];
                    yf = y + moves[i + 1];
                    if (1 < xf && xf < 10 && 1 < yf && yf < 10)
                    {
                        //Si no hay piezas en nuestra posicion futura
                        aux = Distance(xf, yf);
                        if (tablero[xf][yf] == null)
                        {
                            //Posicion valida y distancia menor
                            if (Validate("FPosition", xf, yf, piece) && aux < min)
                            {
                                min = aux;
                                piece.setFXY(xf, yf);
                                accion = 'M';
                            }
                        }
                        //Si una pieza esta en nuestra posicion futura
                        else
                        {
                            //Si es el soldado y no lo van a atacar
                            if (tablero[xf][yf].getName() == "ToySoldier" && Validate("ATK", xf, yf, piece))
                            {
                                min = aux;
                                piece.setFXY(xf, yf);
                                accion = 'A';
                            }
                            //Si es otra pieza
                            else if (aux < min)
                            {
                                //Recursividad si la pieza no ha calculado la accion
                                if (tablero[xf][yf].getAction() == 'N')
                                {
                                    if (tablero[xf][yf].getFX() != 100)
                                    {
                                        piece.setFXY(100, 0);
                                        this.gameObject.SendMessage(tablero[xf][yf].getName().Substring(0, 5) + "Move", tablero[xf][yf]);
                                    }
                                    else
                                    {
                                        min = aux;
                                        tablero[xf][yf].setAction('M');
                                        //accion='M';
                                    }
                                }
                                //Si la pieza que nos bloquea se movera
                                if (tablero[xf][yf].getAction() == 'M' && Validate("FPosition", xf, yf, piece))
                                {
                                    piece.setFXY(xf, yf);
                                    accion = 'M';
                                    min = aux;
                                }
                                else
                                    accion = 'Q';
                            }
                        }
                    }
                }
            }
            piece.setAction(accion);
            if (piece.getFX() < piece.getX())
                piece.setSentido('L');
            else
                piece.setSentido('R');
        }
    }

    public void BishoMove(PiecesID piece)
    {
        int x, y, i;
        char s;
        string res = "11";

        s = piece.getSentido();
        x = piece.getX();
        y = piece.getY();
        //Caso 1: el alfil se encuentra en el tumor 
        if ((x > 9 && s == 'L') || (x < 2 && s == 'R') || (y > 9 && s == 'D') || (y < 2 && s == 'U'))
            piece.setAction(SimpleMove(piece, 5));
        //Caso 2: el alfil se movera con sus reglas
        else
        {
            i = 1;
            do {
                if (res == "1")
                    break;
                else if (res == "11")
                    res = BishopRules(piece, i, x, y, 1, 1, 0);
                else if (res == "1-1")
                    res = BishopRules(piece, i, x, y, 1, -1, 0);
                else if (res == "-1-1")
                    res = BishopRules(piece, i, x, y, -1, -1, 0);
                else if (res == "-11")
                    res = BishopRules(piece, i, x, y, -1, 1, 0);
                i++;
            } while (i < 5);
        }
    }

    public void TowerMove(PiecesID piece)
    {
        int x, y, i = 0;
        char s;
        string res = "01";

        s = piece.getSentido();
        x = piece.getX();
        y = piece.getY();
        //Caso 1: la torre se encuentra en el tumor 
        if ((x > 9 && s == 'L') || (x < 2 && s == 'R') || (y > 9 && s == 'D') || (y < 2 && s == 'U'))
            piece.setAction(SimpleMove(piece, 5));
        //Caso 2: la torre se movera con sus reglas
        else
        {
            do {
                i++;
                if (res == "1")
                    break;
                else if (res == "01")
                    res = TowerRules(piece, i, x, y, 0, 1, 0);
                else if (res == "0-1")
                    res = TowerRules(piece, i, x, y, 0, -1, 0);
                else if (res == "-10")
                    res = TowerRules(piece, i, x, y, -1, 0, 0);
                else if (res == "10")
                    res = TowerRules(piece, i, x, y, 1, 0, 0);
            } while (i < 5);
            Debug.Log(piece.getName() + " fase: " + i + " accion: " + piece.getAction() + " res: " + res);
        }
    }

    public void QueenMove(PiecesID piece)
    {
        int x, y, ix = 0, iy = 0, dx, dy, n, i;
        char s, des;
        string res = "0";

        s = piece.getSentido();
        x = piece.getX();
        y = piece.getY();

        //Caso 1: la reina se encuentra en el tumor 
        if ((x > 9 && s == 'L') || (x < 2 && s == 'R') || (y > 9 && s == 'D') || (y < 2 && s == 'U'))
            piece.setAction(SimpleMove(piece, 5));
        else
        {
            //revisa si conviene moverse en diagonal o fila o columna
            dx = x - Toy.getX();
            dy = y - Toy.getY();

            if (dx == 0 || dy == 0)
                des = 'T';
            else
                des = 'B';

            i = 1;
            do {
                n = Mathf.CeilToInt(i / 2f);
                if (res == "1")
                    break;

                if (n != 1)
                {
                    if (res == "01") {
                        ix = 0; iy = 1;
                    }
                    else if (res == "11") {
                        ix = 1; iy = 1;
                    }
                    else if (res == "0-1") {
                        ix = 0; iy = -1;
                    }
                    else if (res == "1-1") {
                        ix = 1; iy = -1;
                    }
                    else if (res == "-10") {
                        ix = -1; iy = 0;
                    }
                    else if (res == "-1-1") {
                        ix = -1; iy = -1;
                    }
                    else if (res == "-10") {
                        ix = -1; iy = 0;
                    }
                    else if (res == "-11") {
                        ix = -1; iy = 1;
                    }
                }

                if (i % 2 == 1 && des == 'T') {
                    res = TowerRules(piece, n, x, y, ix, iy, 0); des = 'T';
                }
                else if (i % 2 == 1 && des == 'B') {
                    res = BishopRules(piece, n, x, y, ix, iy, 0); des = 'B';
                }
                else if (i % 2 == 0 && des == 'T') {
                    res = BishopRules(piece, n, x, y, ix, iy, 0); des = 'B';
                }
                else if (i % 2 == 0 && des == 'B') {
                    res = TowerRules(piece, n, x, y, ix, iy, 0); des = 'T';
                }
                i++;
            } while (i < 9);
        }
    }

    public void King_Move(PiecesID piece)
    {
        int x, y, ix = 0, iy = 0, dx, dy, n, i;
        char s, des;
        string res = "0";

        s = piece.getSentido();
        x = piece.getX();
        y = piece.getY();

        //Caso 1: el rey se encuentra en el tumor 
        if ((x > 9 && s == 'L') || (x < 2 && s == 'R') || (y > 9 && s == 'D') || (y < 2 && s == 'U'))
            piece.setAction(SimpleMove(piece, 5));
        else
        {
            //revisa si conviene moverse en diagonal o fila o columna
            dx = x - Toy.getX();
            dy = y - Toy.getY();

            if (dx == 0 || dy == 0)
                des = 'T';
            else
                des = 'B';

            i = 1;
            do {
                n = Mathf.CeilToInt(i / 2f);
                if (res == "1")
                    break;

                if (n != 1)
                {
                    if (res == "01") {
                        ix = 0; iy = 1;
                    }
                    else if (res == "11") {
                        ix = 1; iy = 1;
                    }
                    else if (res == "0-1") {
                        ix = 0; iy = -1;
                    }
                    else if (res == "1-1") {
                        ix = 1; iy = -1;
                    }
                    else if (res == "-10") {
                        ix = -1; iy = 0;
                    }
                    else if (res == "-1-1") {
                        ix = -1; iy = -1;
                    }
                    else if (res == "-10") {
                        ix = -1; iy = 0;
                    }
                    else if (res == "-11") {
                        ix = -1; iy = 1;
                    }
                }

                if (i % 2 == 1 && des == 'T') {
                    res = TowerRules(piece, n, x, y, ix, iy, 1); des = 'T';
                }
                else if (i % 2 == 1 && des == 'B') {
                    res = BishopRules(piece, n, x, y, ix, iy, 1); des = 'B';
                }
                else if (i % 2 == 0 && des == 'T') {
                    res = BishopRules(piece, n, x, y, ix, iy, 1); des = 'B';
                }
                else if (i % 2 == 0 && des == 'B') {
                    res = TowerRules(piece, n, x, y, ix, iy, 1); des = 'T';
                }
                i++;
            } while (i < 9);
        }
    }

    public string BishopRules(PiecesID piece, int fase, int x, int y, int ix, int iy, int lim)
    {
        bool move;
        float min, aux;
        char des;
        string centro;

        min = Distance(x + ix, y - iy);
        aux = Distance(x - ix, y + iy);
        if (min < aux)
            des = 'y';
        else if (aux < min)
            des = 'x';
        else//si ambas distancias son iguales, escogemos la diagonal que nos acerque mas al centro
        {
            min = Mathf.Abs(x - ix - 5.5f);
            aux = Mathf.Abs(y - iy - 5.5f);
            if (min < aux)
                des = 'y';
            else
                des = 'x';
        }

        if (fase == 1)//Diagonal principal
        {
            //obtenemos el incremento en x para la diagonal principal
            if (Toy.getX() < x)
                ix = -1;
            else if (Toy.getX() > x)
                ix = 1;
            else
            {
                centro = Center(x, y);
                if (centro == "++" || centro == "+-")
                    ix = 1;
                else
                    ix = -1;
            }
            //obtenemos el incremento en y para la diagonal principal
            if (Toy.getY() < y)
                iy = -1;
            else if (Toy.getY() > y)
                iy = 1;
            else
            {
                centro = Center(x, y);
                if (centro == "++" || centro == "-+")
                    iy = 1;
                else
                    iy = -1;
            }
            if (!LargeMove(piece, x, y, ix, iy, lim))
                return ix + "" + iy;
        }
        else if (fase == 2)//Decide la mejor diagonal entre la 2 y 3
        {
            //probamos la segunda diagonal
            if (des == 'x')
                move = LargeMove(piece, x, y, -ix, iy, lim);
            else
                move = LargeMove(piece, x, y, ix, -iy, lim);
            //si no podemos movernos por la segunda diagonal
            if (!move)
                return ix + "" + iy;
        }
        else if (fase == 3)//Toma la diagonal no seleccionada en la fase anterior
        {
            //probamos la tercera diagonal
            if (des == 'x')
                move = LargeMove(piece, x, y, ix, -iy, lim);
            else
                move = LargeMove(piece, x, y, -ix, iy, lim);
            //si no podemos movernos por la tercera diagonal
            if (!move)
                return ix + "" + iy;
        }
        else if (fase == 4)//Toma la diagonal opuesta
        {
            //probamos la ultima diagonal si no pudimos movernos, no nos moveremos
            if (!LargeMove(piece, x, y, -ix, -iy, lim))
                piece.setAction('Q');
        }
        return "1";
    }

    public string TowerRules(PiecesID piece, int fase, int x, int y, int ix, int iy, int lim)
    {
        int dx, dy;
        bool move;

        dx = x - Toy.getX();
        dy = y - Toy.getY();
        if (fase == 1)//Decide la columna o fila principal
        {
            if (dy == 0 || (Mathf.Abs(dx) < Mathf.Abs(dy) && dx != 0))
            {
                iy = 0;
                if (dx < 0)
                    ix = 1;
                else
                    ix = -1;
            }
            else if (dx == 0 || (Mathf.Abs(dy) < Mathf.Abs(dx) && dy != 0))
            {
                ix = 0;
                if (dy < 0)
                    iy = 1;
                else
                    iy = -1;
            }
            else
            {
                if (Toy.getSentido() == 'R' || Toy.getSentido() == 'L')
                {
                    iy = 0;
                    if (dx < 0)
                        ix = 1;
                    else
                        ix = -1;
                }
                else
                {
                    ix = 0;
                    if (dy < 0)
                        iy = 1;
                    else
                        iy = -1;
                }
            }
            Debug.Log(piece.getName() + " fase 1: ix = " + ix + " iy = " + iy);
            if (!LargeMove(piece, x, y, ix, iy, lim))
                return ix + "" + iy;
        }
        else if (fase == 2) //cambia fila por columna y columna por fila
        {
            if (ix == 0)
            {
                iy = 0;
                if (dx < 0)
                    ix = 1;
                else
                    ix = -1;
            }
            else
            {
                ix = 0;
                if (dy < 0)
                    iy = 1;
                else
                    iy = -1;
            }
            Debug.Log(piece.getName() + " fase 2: ix = " + ix + " iy = " + iy);
            if (!LargeMove(piece, x, y, ix, iy, lim))
                return ix + "" + iy;
        }
        else if (fase == 3)//Toma el sentido contrario a la anterior
        {
            if (ix == 0)
                move = LargeMove(piece, x, y, ix, -iy, lim);
            else
                move = LargeMove(piece, x, y, -ix, iy, lim);

            Debug.Log(piece.getName() + " fase 3: ix = " + ix + " iy = " + iy);
            if (!move)
                return ix + "" + iy;
        }
        else if (fase == 4)
        {
            //programos la ultima diagonal si no pudimos movernos, no nos moveremos
            Debug.Log(piece.getName() + " fase 4: ix = " + ix + " iy = " + iy);
            if (!LargeMove(piece, x, y, -ix, -iy, lim))
                piece.setAction('Q');
        }
        return "1";
    }

    public bool LargeMove(PiecesID piece, int x, int y, int ix, int iy, int lim)
    {
        int xf, yf;
        float min, aux;

        xf = x + ix; yf = y + iy;
        min = Distance(xf, yf);
        aux = min;
        //ciclo para marcar el avance de este camino
        //nos detendremos si:
        //-nos alejamos del soldado
        //-salimos del tablero
        //-la posicion no es valida
        while (aux <= min && 1 < xf && xf < 10 && 1 < yf && yf < 10 && Validate("FPosition", xf, yf, piece))
        {
            //revisamos si nos topamos con el soldado o una pieza
            if (tablero[xf][yf] != null)
            {
                //si es el soldado, validamos si nos lo podemos comer
                if (tablero[xf][yf].getName() == "ToySoldier" && Validate("ATK", xf, yf, piece))
                {
                    piece.setFXY(xf, yf);
                    piece.setAction('A');
                    return true;
                }
                else
                {
                    //si es otra pieza preguntamos si se movera
                    if (tablero[xf][yf].getAction() == 'N')
                    {
                        Debug.Log("recursividad");
                        if (tablero[xf][yf].getFX() != 100)
                        {
                            piece.setFXY(100, 0);
                            this.gameObject.SendMessage(tablero[xf][yf].getName().Substring(0, 5) + "Move", tablero[xf][yf]);
                        }
                        else
                            return false;
                    }
                    //Si la pieza que nos bloquea no se movera
                    if (tablero[xf][yf].getAction() == 'Q')
                        return false;//descartamos este movimiento largo
                    Debug.Log("Se movera");
                }
            }
            xf += ix;
            yf += iy;
            min = aux;
            aux = Distance(xf, yf);
            if (lim == 1)
                break;
        }
        xf -= ix; yf -= iy;

        if (xf == x && yf == y)
            return false;
        else
        {
            Debug.Log(piece.getName() + " se movera a (" + xf + "," + yf + ")");
            piece.setFXY(xf, yf);
            piece.setAction('M');
            return true;
        }
    }

    /*public void AcomodarPiezas()
    {
        //Limpiamos el array
        Array.Clear(Piezas, 0, Piezas.Length);

        //recorremos el tablero
        for (int i = 0; i < 12; i++)
        {
            for (int j = 0; j < 12; j++)
            {
                //revisamos si hay alguna pieza en esa casilla
                if (tablero[i][j] != null)
                {
                    if (tablero[i][j].getName() != "ToySoldier")
                    {
                        Piezas[Piezas.Length] = tablero[i][j];
                    }

                }
            }
        }
    }*/

	//funcion para el spawneo de piezas
	public void Spawn()
	{
        int i, spawnx, spawny, piece;
        float Spawning;

        if (Tutorial == 'S' && PiecesCounter == 0)
        {
            Debug.Log("Tutorial");
            Tutorial = 'P';
            i = UnityEngine.Random.Range(0, 33);
            if (i <= 16)
            {
                spawnx = (i % 8) + 2;
                spawny = i <= 8 ? 0 : 11;
            }
            else
            {
                spawny = (i % 8) + 2;
                spawnx = i <= 24 ? 0 : 11;
            }
            //Creamos los datos del nuevo pawn
            tablero[spawnx][spawny] = CreateData("Pawn_" + num_spanw, spawnx, spawny, 1);
            //asignamos la posicion futura que tendra
            tablero[spawnx][spawny].setFXY(spawnx, spawny);
            //creamos el gameobject y llamamos a la funcion Move de pawn
            Instantiate(Pawn).SendMessage("Move", tablero[spawnx][spawny]);

            PiecesCounter++;
            Debug.Log("Pieces: " + PiecesCounter);
        }
        else if (Tutorial == 'P' && PiecesCounter == 0)
            Tutorial = 'O';

        if (Tutorial == 'O')
        {
            Debug.Log("Normal");


            Spawning = UnityEngine.Random.Range(0, 101) + (S * 0.25f);
            if (Spawning > 100)
                Spawning = Spawning - 100;
            Debug.Log("Spawning: " + Spawning);
            Debug.Log("Porcentaje de spawn: " + (100 * Mathf.Pow(1 - Mathf.Pow(0.6f, 1 + (0.1f * NoSpawn)), PiecesCounter)) );

            while (Spawning <= (100 * Mathf.Pow(1 - Mathf.Pow(0.6f, 1 + (0.1f * NoSpawn)), PiecesCounter)) )
            {
                //ramdom que decide la probabilidad de que este spawn spawnee
                do
                {
                    i = UnityEngine.Random.Range(0, 33);
                    //Calculos para obtener la spawnx y spawny de la pieza en la matriz
                    if (i <= 16)
                    {
                        spawnx = (i % 8) + 2;
                        spawny = i <= 8 ? 0 : 11;
                    }
                    else
                    {
                        spawny = (i % 8) + 2;
                        spawnx = i <= 24 ? 0 : 11;
                    }
                } while (tablero[spawnx][spawny] != null);

                //incrementamos contador de spawns
                num_spanw++;

                //Random para obtener la pieza a spawnear
                piece = UnityEngine.Random.Range(0, 100);

                //rey 2%
                //reina 5%
                //torre 12%
                //alfil 16%
                //caballo 22%
                //peon 43%

                if (piece < 43 && PawnS)//43-43%
                {
                    //Creamos los datos del nuevo pawn
                    tablero[spawnx][spawny] = CreateData("Pawn_" + num_spanw, spawnx, spawny, 1);
                    //asignamos la posicion futura que tendra
                    tablero[spawnx][spawny].setFXY(spawnx, spawny);
                    //creamos el gameobject y llamamos a la funcion Move de pawn
                    Instantiate(Pawn).SendMessage("Move", tablero[spawnx][spawny]);
                }
                else if (piece < 65 && BishopS)//65-22%
                {
                    //Creamos los datos del nuevo Bishop
                    tablero[spawnx][spawny] = CreateData("Bisho" + num_spanw, spawnx, spawny, 2);
                    //asignamos la posicion futura que tendra
                    tablero[spawnx][spawny].setFXY(spawnx, spawny);
                    //creamos el gameobject y llamamos a la funcion Move de pawn
                    Instantiate(Bishop).SendMessage("Move", tablero[spawnx][spawny]);
                }
                else if (piece < 81 && HorseS)//81-16%
                {
                    //Creamos los datos del nuevo Horse
                    tablero[spawnx][spawny] = CreateData("Horse" + num_spanw, spawnx, spawny, 3);
                    //asignamos la posicion futura que tendra
                    tablero[spawnx][spawny].setFXY(spawnx, spawny);
                    //creamos el gameobject y llamamos a la funcion Move de pawn
                    Instantiate(Horse).SendMessage("Move", tablero[spawnx][spawny]);
                }
                else if (piece < 93 && TowerS)//93-12%
                {
                    //Creamos los datos del nuevo Tower
                    tablero[spawnx][spawny] = CreateData("Tower" + num_spanw, spawnx, spawny, 4);
                    //asignamos la posicion futura que tendra
                    tablero[spawnx][spawny].setFXY(spawnx, spawny);
                    //creamos el gameobject y llamamos a la funcion Move de pawn
                    Instantiate(Tower).SendMessage("Move", tablero[spawnx][spawny]);
                }
                else if (piece < 98 && QueenS)//98-5%
                {
                    //Creamos los datos de la nueva Queen
                    tablero[spawnx][spawny] = CreateData("Queen" + num_spanw, spawnx, spawny, 5);
                    //asignamos la posicion futura que tendra
                    tablero[spawnx][spawny].setFXY(spawnx, spawny);
                    //creamos el gameobject y llamamos a la funcion Move de pawn
                    Instantiate(Queen).SendMessage("Move", tablero[spawnx][spawny]);
                }
                else if (KingS) //3%
                {
                    //Creamos los datos del nuevo King
                    tablero[spawnx][spawny] = CreateData("King_" + num_spanw, spawnx, spawny, 6);
                    //asignamos la posicion futura que tendra
                    tablero[spawnx][spawny].setFXY(spawnx, spawny);
                    //creamos el gameobject y llamamos a la funcion Move de pawn
                    Instantiate(King).SendMessage("Move", tablero[spawnx][spawny]);
                }
                NoSpawn = 1;
                PiecesCounter++;
                Debug.Log("Pieces: " + PiecesCounter);
                Spawning = UnityEngine.Random.Range(0, 101) + (S * 0.25f);
                if (Spawning > 100)
                    Spawning = Spawning - 100;
                Debug.Log("Spawning: " + Spawning);
                Debug.Log("Porcentaje de spawn: " + (int)(100 * Mathf.Pow(1 - Mathf.Pow(0.6f, 1 + (0.1f * NoSpawn)), PiecesCounter)) );
            }
            NoSpawn++;
            Debug.Log("NS: " + NoSpawn);

            /*//METODO 1:
            //Revisamos cada una de las casillas de spawns. 
            //38% de probabilidad de spawnear minimo una pieza por turno
            for (i = 1; i <= 32; i++)
            {
                //ramdom que decide la probabilidad de que este spawn spawnee
                appear = UnityEngine.Random.Range(0, 1001);
                //Probabilidad de 1.5% de cada spawn de spawnear algo. Lo que nos da un promedio de una pieza por turno. 
                if (appear <= 15)
                {
                    //Calculos para obtener la spawnx y spawny de la pieza en la matriz
                    if (i <= 16)
                    {
                        spawnx = (i % 8) + 2;
                        spawny = i <= 8 ? 0 : 11;
                    }
                    else
                    {
                        spawny = (i % 8) + 2;
                        spawnx = i <= 24 ? 0 : 11;
                    }
                    //Random para obtener la pieza a spawnear
                    if (tablero[spawnx][spawny] == null)
                    {
                        //incrementamos contador de spawns
                        num_spanw++;
                        piece = UnityEngine.Random.Range(0, 100);
                        
                        //rey 2%
                        //reina 5%
                        //torre 12%
                        //alfil 16%
                        //caballo 22%
                        //peon 43%

                        if (piece < 43 && PawnS)//43-43%
                        {
                            //Creamos los datos del nuevo pawn
                            tablero[spawnx][spawny] = CreateData("Pawn_" + num_spanw, spawnx, spawny, 1);
                            //asignamos la posicion futura que tendra
                            tablero[spawnx][spawny].setFXY(spawnx, spawny);
                            //creamos el gameobject y llamamos a la funcion Move de pawn
                            Instantiate(Pawn).SendMessage("Move", tablero[spawnx][spawny]);
                        }
                        else if (piece < 65 && BishopS)//65-22%
                        {
                            //Creamos los datos del nuevo Bishop
                            tablero[spawnx][spawny] = CreateData("Bisho" + num_spanw, spawnx, spawny, 2);
                            //asignamos la posicion futura que tendra
                            tablero[spawnx][spawny].setFXY(spawnx, spawny);
                            //creamos el gameobject y llamamos a la funcion Move de pawn
                            Instantiate(Bishop).SendMessage("Move", tablero[spawnx][spawny]);
                        }
                        else if (piece < 81 && HorseS)//81-16%
                        {
                            //Creamos los datos del nuevo Horse
                            tablero[spawnx][spawny] = CreateData("Horse" + num_spanw, spawnx, spawny, 3);
                            //asignamos la posicion futura que tendra
                            tablero[spawnx][spawny].setFXY(spawnx, spawny);
                            //creamos el gameobject y llamamos a la funcion Move de pawn
                            Instantiate(Horse).SendMessage("Move", tablero[spawnx][spawny]);
                        }
                        else if (piece < 93 && TowerS)//93-12%
                        {
                            //Creamos los datos del nuevo Tower
                            tablero[spawnx][spawny] = CreateData("Tower" + num_spanw, spawnx, spawny, 4);
                            //asignamos la posicion futura que tendra
                            tablero[spawnx][spawny].setFXY(spawnx, spawny);
                            //creamos el gameobject y llamamos a la funcion Move de pawn
                            Instantiate(Tower).SendMessage("Move", tablero[spawnx][spawny]);
                        }
                        else if (piece < 98 && QueenS)//98-5%
                        {
                            //Creamos los datos de la nueva Queen
                            tablero[spawnx][spawny] = CreateData("Queen" + num_spanw, spawnx, spawny, 5);
                            //asignamos la posicion futura que tendra
                            tablero[spawnx][spawny].setFXY(spawnx, spawny);
                            //creamos el gameobject y llamamos a la funcion Move de pawn
                            Instantiate(Queen).SendMessage("Move", tablero[spawnx][spawny]);
                        }
                        else if (KingS) //3%
                        {
                            //Creamos los datos del nuevo King
                            tablero[spawnx][spawny] = CreateData("King_" + num_spanw, spawnx, spawny, 6);
                            //asignamos la posicion futura que tendra
                            tablero[spawnx][spawny].setFXY(spawnx, spawny);
                            //creamos el gameobject y llamamos a la funcion Move de pawn
                            Instantiate(King).SendMessage("Move", tablero[spawnx][spawny]);
                        }
                        PiecesCounter++;
                        Debug.Log("Pieces: " + PiecesCounter);
                    }
                }
            }*/
        }
	}

	public void Coronacion()
	{
		int i, j;
		GameObject Piece;
		for(i=2;i<10;i++)
		{
			for(j=2;j<10;j++)
			{
				if(tablero[i][j]!=null)
				{
					if(tablero[i][j].getName().Substring(0,5)=="Pawn_")
					{
						if(tablero[i][j].getAction()=='C')
						{
							Debug.Log(tablero[i][j].getName());
							Piece=GameObject.Find(tablero[i][j].getName());
							Destroy(Piece);

							num_spanw++;
							tablero[i][j] = new PiecesID("Queen",i,j,'R','N',5);
							tablero[i][j].setFXY(i, j);
							Instantiate(Queen).SendMessage("Move", tablero[i][j]);
							
						}
					}
				}
			}
		}
	}

	//Funcion que devuelve un objeto PiecesID con el nombre correspondientes, sus coordenadas, su orientacion y accion
	public PiecesID CreateData(string name, int x, int y, int p)
	{
		PiecesID data=null;;
		if(x==0)
         	data = new PiecesID(name,x,y,'R','N',p);
        else if(x==11)
         	data = new PiecesID(name,x,y,'L','N',p);
        else if(y==0)
         	data = new PiecesID(name,x,y,'U','N',p);
        else if(y==11)
         	data = new PiecesID(name,x,y,'D','N',p);
        return data;
	}

	public float Distance(int i, int j)
	{
		int x, y;

		x=Mathf.Abs(Toy.getX()-i);
		y=Mathf.Abs(Toy.getY()-j);
		return x*x+y*y;
	}

    public string Center(int x, int y)
    {
        string m;

        if (x < 5.5)
        {
            if (y < 5.5)
                m = "++";
            else
                m = "+-";
        }
        else
        {
            if (y < 5.5)
                m = "-+";
            else
                m = "--";
        }
        return m;
    }

	//Funcion que validaran si la posicion futura no esta asignada a otra pieza
	//Recibe un par de coordenadas
	//Devuelve True: si la posicion no esta asignada - False: si la posicion ya esta asignada
	public bool Validate(string accion, int x, int y, PiecesID piece)
	{
		int i, j;

		for(i=1;i<11;i++)
		{
			for(j=1;j<11;j++)
			{
				if(tablero[i][j]!=null)
				{
					if(tablero[i][j].getName()!="ToySoldier" && tablero[i][j].getName()!=piece.getName())
					{
						//validamos la posicion futura
						if(accion=="FPosition" && x==tablero[i][j].getFX() && y==tablero[i][j].getFY())
						{
							//si una pieza tiene la misma posicion futura revisamos su nivel
							if(piece.getLevel()<tablero[i][j].getLevel())
								tablero[i][j].setAction('N');//si tenemos prioridad tomamos la posicion
							else
								return false;//si tenemos igual o menos prioridad no la tomamos
						}
						else if(accion=="ATK" && tablero[i][j].getAction()=='A')
							return false;
					}
				}
			}
		}
		return true;
	}

	public void DP(char sentido)
	{
		if(sentido=='U')
			DestroyPieces(Toy.getY(),Toy.getX(),10,1,'y');
		else if(sentido=='D')
			DestroyPieces(Toy.getY(),Toy.getX(),1,-1,'y');
		else if(sentido=='R')
			DestroyPieces(Toy.getX(),Toy.getY(),10,1,'x');
		else if(sentido=='L')
			DestroyPieces(Toy.getX(),Toy.getY(),1,-1,'x');
	}

	public void DestroyPieces(int inicio, int c, int final, int signo, char dir)
	{
		int i=inicio+signo;
        int j = 1, cont = 0;
		bool bloqueo = false;
		GameObject Piece=null;
		GameObject Aux;
		
		while((signo*i)<(signo*final))
		{
			if(dir=='x')
			{
				if(tablero[i][c]!=null)
				{
					Piece=GameObject.Find(tablero[i][c].getName());
					Debug.Log(tablero[i][c].getName().Substring(0,5));
					if(tablero[i][c].getName().Substring(0,5)=="King_")
					{
						tablero[i][c].setVida(tablero[i][c].getVida()-1);
						Piece.SendMessage("Shoot",tablero[i][c]);
						bloqueo = true;
					}
					else
						tablero[i][c].setVida(0);

					if(tablero[i][c].getVida()==0)
					{
						tablero[i][c]=null;
                        Destroy(Piece);
                        PiecesCounter--;
                        Debug.Log("Pieces: " + PiecesCounter);
                        S++;
                        if (Pos == 1)
                            NewM = M;
						Score();
					}
				}
				/*Aux=Instantiate(Estela, new Vector3(i-5.5f+signo*(-0.13f), c-5.15f, 0.0f), Quaternion.Euler(0, 0, 0));
				if(i==9 || i==2)
					Piece=Instantiate(Balin, new Vector3(i-5.5f+signo*(0.42f), c-5.15f, 0.0f), Quaternion.Euler(0, 0, 0));*/
			}
			else
			{
				if(tablero[c][i]!=null)
				{
					Piece=GameObject.Find(tablero[c][i].getName());
					if(tablero[c][i].getName().Substring(0,5)=="King_")
					{
						tablero[c][i].setVida(tablero[c][i].getVida()-1);
						Piece.SendMessage("Shoot",tablero[c][i]);
						bloqueo = true;
					}
					else
						tablero[c][i].setVida(0);

					if(tablero[c][i].getVida()==0)
					{
						tablero[c][i]=null;
                        Destroy(Piece);
                        PiecesCounter--;
                        Debug.Log("Pieces: " + PiecesCounter);
                        S++;
                        if (Pos == 1)
                            NewM = M;
                        Score();
					}
				}
				/*Aux=Instantiate(Estela, new Vector3((c-5.5f)-signo*(0.35f), i-5.5f+signo*(-0.13f), 0.0f), Quaternion.Euler(0, 0, 270));
				if(i==9 || i==2)
					Piece=Instantiate(Balin, new Vector3((c-5.5f)-signo*(0.35f), i-5.5f+signo*(0.42f), 0.0f), Quaternion.Euler(0, 0, 270));*/
			}
			/*Destroy(Aux,0.07f);
			if(i==9 || i==2)
				Destroy(Piece,0.07f);*/
			i+=signo;
            cont++;

			if(bloqueo)
				break;
		}

        while(j <= 21)
        {
            Aux = Instantiate(Estela, ToyO.transform.position + ToyO.transform.right * (j - 0.13f) + ToyO.transform.up * 0.35f, ToyO.transform.rotation);
            Destroy(Aux, 0.07f);
            if (bloqueo && j==cont)
            {
                Piece = Instantiate(Balin, ToyO.transform.position + ToyO.transform.right * (j + 0.42f) + ToyO.transform.up * 0.35f, ToyO.transform.rotation);
                Destroy(Piece, 0.07f);
                break;
            }
            j++;
        }
	}

	public void Score()
	{
		//Score actual del jugador
		You.text = "YOURS:\nScore: "+S+"\nMoves: "+M;

		//siempre y cuando la posicion no sea igual a uno
	 	if(Pos!=1)
	 	{
            //Revisando si el jugador ha superado el Target actual, se compara si ha superado los puntos de este o si los ha igualado en menos movimientos
            if (PlayerPrefs.GetInt("S" + Tar) < S || (PlayerPrefs.GetInt("S" + Tar) == S && PlayerPrefs.GetInt("M" + Tar) > M))
            {
                New = true; //Notificamos que se ha superdo minimo una posicion
                NewM = M; //Guardamos el numero de movimientos con el que el jugador supero el target actual
                Pos = Tar; //Actualizamos la posicion del jugador a ser la del target
                if (Pos == 1)
                    Target = "TARGET:\n1. You\nScore: " + S + "\nMoves: " + NewM;
                else
                    CalTar(); //Calculamos el nuevo Target
            }
            else
                Target = "TARGET:\n"+Tar+". "+PlayerPrefs.GetString("N"+Tar)+"\nScore: "+PlayerPrefs.GetInt("S"+Tar)+"\nMoves: "+PlayerPrefs.GetInt("M"+Tar);
	 	}
    	else//Si el jugador supera la maxima puntuacion, su propio score se vuelve el Target
 			Target = "TARGET:\n1. You\nScore: "+S+"\nMoves: "+NewM;
		
	}

	public void CalTar()
	{
  		do
  		{
  			Tar--; //Disminuimos la posicion del target
  		}
  		while( Tar!= 1 && PlayerPrefs.GetInt("S"+Tar)==PlayerPrefs.GetInt("S"+(Tar-1)) && PlayerPrefs.GetInt("M"+Tar)==PlayerPrefs.GetInt("M"+(Tar-1)) );
		//Si vemos que el Tar actual tiene los mismos valores que la posicion inmediatamente superior a el, disminuimos el Tar hasta que la posicion superior sea diferente
		//quedandonos asi con el Tar mas alto de los que son iguales pero con la posicion del jugador en el puesto mas bajo de esos highscores iguales
	}
}
