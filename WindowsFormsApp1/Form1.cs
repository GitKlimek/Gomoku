using System;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        Random rand = new Random();
        const int M_INF = -10000;
        const int P_INF = 10000;
        bool tura = true; //true - Black, false- Red
        bool win = false;
        bool AI = true;

        public struct area
        {
              public  int rectUpX;
              public  int rectUpY;
              public  int rectDownX;
              public int rectDownY;
        }
        area area_t = new area();
        
        int[,] board = new int[15, 15];
        int[,] tempboard = new int[15, 15];

        Button[] przyc = new Button[225];
        int[] plansza = new int[225];

        int[,] edgingB = new int[15, 15];
        int[,] edgingR = new int[15, 15];

        public Form1()
        {
            InitializeComponent();
            setLabel();
            init_area();
            initEdging();
            lastBlackMove.Text = lastRedMove.Text = "";
        }

        private void initEdging()
        {
            for(int i = 0; i < 15; i++)
            {
                for(int j = 0; j < 15; j++)
                {
                    edgingB[i, j] = 0;
                    edgingR[i, j] = 0;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Maciej Klimek 165508\nBartosz Głodkowski", "Gomoku about");
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button_clik(object sender, EventArgs e)
        {
            int c;
            int r;
            int ind;
            Button b = (Button)sender;
            if (tura)
            {
                b.BackColor = Color.Black;
                ind = b.TabIndex - 1;
                r = ind / 15;
                c = ind - (r * 15);
                edgingB = updateEdge(edgingB, c, r);
                area_t = setArea(area_t, c, r);
                lastBlackMove.Text = "[" + r.ToString() + " , " + c.ToString() + "]"; 
            }
            else
            {
             b.BackColor = Color.Red;
             ind = b.TabIndex - 1;
             r = ind / 15;
             c = ind - (r * 15);
             edgingR = updateEdge(edgingR, c, r);
             area_t = setArea(area_t, c, r);
             lastRedMove.Text = "[" + r.ToString() + " , " + c.ToString() + "]";
            }
            tura = !tura;
            b.Enabled = false;
            updateBoard();
            chcekResult();

            if (!tura && !win && AI)
            {
                setLabel();
                compMakeMove();
            }
            else
            {
              updateBoard();
                chcekResult();
                if (win)
                {
                    disableAllButtons();
                    string winner = "";
                    if (tura) winner = "Red";
                    else winner = "Black";
                    MessageBox.Show(winner + " won!!!", "The end!");
                }
                setLabel();
            }
        }

        private void disableAllButtons()
        {
          
                foreach (Control c in Controls)
                {
                try
                {
                    Button b = (Button)c;
                    b.Enabled = false;
                }
                catch { }
            }
           
        }

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            win = false;
            tura = true;   //player 1 zawsze czarny
            AI = true;
                foreach (Control c in Controls)
                {
                try
                {
                    Button b = (Button)c;
                    b.BackColor = SystemColors.Control;
                    b.Enabled = true;
                }
                catch { }
            }
            setLabel();
            init_area();
            initEdging();
        }

        private void twoPlayersModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            win = false;
            tura = true;   //player 1 zawsze czarny
            AI = false;
            foreach (Control c in Controls)
            {
                try
                {
                    Button b = (Button)c;
                    b.BackColor = SystemColors.Control;
                    b.Enabled = true;
                }
                catch { }
            }
            setLabel();
        }

        private void updateBoard()
        {

            foreach (Control c in Controls)
            {
                try
                {
                    Button b = (Button)c;
                    int a = b.TabIndex - 1;
                    przyc[a] = b;
                }
                catch { }
            }

            for (int i = 0; i < 225; i++)
            {
                if (przyc[i].BackColor == SystemColors.Control)
                {
                    plansza[i] = -1;
                }
                else if (przyc[i].BackColor == Color.Black)
                {
                    plansza[i] = 0;
                }
                else
                {
                    plansza[i] = 1;
                }
            }

            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    board[i, j] = plansza[i * 15 + j];
                }
            }
        }

        private void chcekResult()
        {
            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    int b = board[i,j];
                    if (b != -1)
                    {
                        //right
                        if (j + 4 < 15)
                        {
                            if (board[i, j + 1] == b && board[i, j + 2] == b && board[i, j + 3] == b && board[i, j + 4] == b) win = true;
                        }

                        //right-down
                        if (j + 4 < 15 && i + 4 < 15)
                        {
                            if (board[i + 1,j + 1] == b && board[i + 2,j + 2] == b && board[i + 3,j + 3] == b && board[i + 4,j + 4] == b) win = true;
                        }                                                                                                                 
                                                                                                                                          
                        //down                                                                                                            
                        if (i + 4 < 15)                                                                                                   
                        {                                                                                                                 
                            if (board[i + 1,j] == b && board[i + 2,j] == b && board[i + 3,j] == b && board[i + 4,j] == b) win = true;    
                        }                                                                                                                 
                                                                                                                                          
                        //left-down                                                                                                       
                        if (i + 4 < 15 && j > 3)                                                                                          
                        {                                                                                                                 
                            if (board[i + 1,j - 1] == b && board[i + 2,j - 2] == b && board[i + 3,j - 3] == b && board[i + 4,j - 4] == b) win = true;
                        }
                    }
                }
            }


        }

        private void setLabel()
        {
            if (!win)
            {
                if (tura) label2.Text = "Black";
                else label2.Text = "Red";
            }
            else label2.Text = "-----";
        }

        private void init_area()
        {
            area_t.rectUpX = 15;
            area_t.rectUpY = 15;
            area_t.rectDownX = 0;
            area_t.rectDownY = 0;
        }

        private area setArea(area area_t,int col, int row)
        {
            if (row - 1 < area_t.rectUpY) area_t.rectUpY = row - 1;
            if (row + 1 > area_t.rectDownY) area_t.rectDownY = row + 1;
            if (row == 0) area_t.rectUpY = 0;
            if (row == 14) area_t.rectDownY = 14;


            if (col - 1 < area_t.rectUpX) area_t.rectUpX = col - 1;
            if (col + 1 > area_t.rectDownX) area_t.rectDownX = col + 1;
            if (col == 0) area_t.rectUpX = 0;
            if (col == 14) area_t.rectDownX = 14;

            return area_t;
        }

        private unsafe void compMakeMove()
        {
            Button move = null;

            int newR = 0;
            int newC = 0;

            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    tempboard[i, j] = board[i, j];
                }
            }
         
            int v = doCertainMove(tempboard, &newC, &newR, 0, 1, M_INF, M_INF, P_INF, true, 1, M_INF);

            if (v < 17)
            {
                newC = 0;
                newR = 0;
                v=minmax(tempboard, &newC, &newR, 0, 5, M_INF, M_INF, P_INF, true);
            }
              
            int indexButton = newR * 15 + newC;
         
            foreach (Control c in Controls)
            {
                try
                {
                    Button b = (Button)c;
                    int a = b.TabIndex - 1;
                    if (a == indexButton)
                    {
                        move = b;
                        break;
                    }
                }
                catch { }
            }
            move.PerformClick();           
        }
       
        private unsafe int minmax(int[,] boardM, int *newColumn, int *newRow, int depth, int maxDepth, int v, int alfa, int beta, bool max)
        {

            if (depth < maxDepth)
            {
                if (max)
                {
                    v = M_INF;
                }
                else
                {
                    v = P_INF;
                }

                int n = 0;
                for (int i = area_t.rectUpY; i <= area_t.rectDownY; i++)
                {
                    for (int j = area_t.rectUpX; j <= area_t.rectDownX; j++)
                    {
                        int tempJ = j, tempI = i;
                        if (boardM[i,j] == -1 && (edgingR[i,j] != 0 || edgingB[i,j] != 0))
                        {
                            n++;
                            boardM[i,j] = max? 1:0;
                            area_t = setArea(area_t, i, j);
                            if (max && alfa < v) alfa = v;
                            else if (!max && beta > v) beta = v;

                            if ((max && v <= beta) || (!max && v >= alfa))
                            {
                                int t = minmax(boardM, &tempJ, &tempI, depth + 1, maxDepth, v, alfa, beta, !max);
                                if ((max && t >= v) || (!max && t <= v))
                                {
                                    if (t == v && v != M_INF && v != P_INF)
                                    {
                                        if (rand.Next() % 4 == 0)
                                        {
                                            v = t;
                                            *newRow = i;
                                            *newColumn = j;
                                        }
                                    }
                                    else
                                    {
                                        v = t;
                                        *newRow = i;
                                        *newColumn = j;
                                    }

                                }
                            }
                            else
                            {
                            }
                            boardM[i,j] = -1;
                        }
                    }

                }
                return v;
            }
            else
            {
                return heuristics3(boardM);
            }
        }

        private unsafe int doCertainMove(int[,] boardC, int *newColumn, int *newRow, int depth, int maxDepth, int v, int alfa, int beta, bool max, int turn, int maxHeur)
        {
            if (depth < maxDepth)
            {
                if (max)
                {
                    v = M_INF;
                }
                else
                {
                    v = P_INF;
                }
                
                int n = 0;
                *newColumn = -1;
                *newRow = -1;
                for (int i = area_t.rectUpY; i <= area_t.rectDownY; i++)
                {
                    for (int j = area_t.rectUpX; j <= area_t.rectDownX; j++)
                    {
                        int tempJ = j, tempI = i;
                        if (boardC[i, j] == -1 && (edgingR[i, j] == 0 || edgingB[i, j] == 0))
                        {
                            n++;
                            boardC[i, j] = max? 1: 0;
                            if (max && alfa < v) alfa = v;
                            else if (!max && beta > v) beta = v;

                            if ((max&& v <= beta) || (!max && v >= alfa))
                            {
                           
                                int t = doCertainMove(boardC, &tempJ, &tempI, depth + 1, maxDepth, v, alfa, beta, !max, turn, maxHeur);
                                if ((max && t >= v) || (!max && t <= v))
                                {
                                    if (t == v && (v == M_INF || v == P_INF))
                                    {
                                        maxHeur = heuristics3(boardC);
                                        v = t;
                                        *newRow = i;
                                        *newColumn = j;
                                    }
                                    else if (t == v)
                                    {
                                        int tempHeur = heuristics3(boardC);
                                        if (tempHeur > maxHeur)
                                        {
                                            maxHeur = tempHeur;
                                            v = t;
                                            *newRow = i;
                                            *newColumn = j;
                                        }
                                    }
                                    else
                                    {
                                        maxHeur = heuristics3(boardC);
                                        v = t;
                                        *newRow = i;
                                        *newColumn = j;
                                    }

                                }
                            }
                            else
                            { }
                            boardC[i, j] = -1;
                        }
                    }
                }
                return v;
            }
            else
            {
                return heuristics(boardC, *newRow, *newColumn, turn);
            }
        }

        private int[,] updateEdge(int[,] edging, int col, int row)
        {
            if (col > 0)
            { //bit 7                                               //------------------------
                edging[row, col - 1] |= 0b00000001;                 //--krawedzie wokol pola--
                if (row > 0)                                        //------------------------
                { //bit 0                                           //    0 1 2     ----------
                    edging[row - 1, col - 1] |= 0b10000000;         //    7 x 3     ----------
                }                                                   //    6 5 4     ----------
                if (row < 14)                                       //------------------------
                { //bit 6                                           //------------------------
                    edging[row + 1, col - 1] |= 0b00000010;
                }
            }
            if (col < 14)
            { //bit 3
                edging[row, col + 1] |= 0b00010000;
                if (row > 0)
                { //bit 2
                    edging[row - 1, col + 1] |= 0b00100000;
                }
                if (row < 14)
                { //bit 4
                    edging[row + 1, col + 1] |= 0b00001000;
                }
            }
            if (row > 0)
            { //bit 1
                edging[row - 1, col] |= 0b01000000;
            }
            if (row < 14)
            { //bit 5
                edging[row + 1, col] |= 0b00000100;
            }
            return edging;
        }

        private int heuristics(int[,] boardheur, int r, int k, int turn)
        {
            int[,] directions = new int[8, 4]; //0 black, 1 red, -1 empty, -2 out
            int yKr = -1;
            int xKr = 1;
            int yt = 1;
            int xt = 1;
            int posX, posY;
            for (int i = 0; i < 8; i++)
            {
                posX = k;
                posY = r;
                yKr += yt;
                xKr += xt;
                if (Math.Abs(yKr) == 4)
                {
                    yt *= -1;
                    yKr = 0;
                }
                if (Math.Abs(xKr) == 4)
                {
                    xt *= -1;
                    xKr = 0;
                }

                for (int j = 0; j < 4; j++)
                {
                    if (yKr > 0)
                    {
                        posY++;
                    }
                    else if (yKr < 0)
                    {
                        posY--;
                    }

                    if (xKr > 0)
                    {
                        posX++;
                    }
                    else if (xKr < 0)
                    {
                        posX--;
                    }

                    if (posX < 15 && posX >= 0 && posY < 15 && posY >= 0)
                    {
                        directions[i, j] = boardheur[posY, posX];
                    }
                    else
                    {
                        directions[i, j] = -2; //out of board
                    }
                }
            }

            // I, II - 1 condition: OOOO_
            for (int i = 0; i < 8; i++)
            {
                int jest = 1;
                int jest2 = 1;
                for (int j = 0; j < 4; j++)
                {
                    if (directions[i, j] != turn)
                    {
                        jest = 0;
                    }
                    if (directions[i, j] != ((turn + 1) % 2))
                    {
                        jest2 = 0;
                    }
                }
                if (jest == 1) return 20;
                else if (jest2 == 1) return 19;
            }
            // I, II - 3 condition OO_OO
            for (int i = 0; i < 4; i++)
            {
                int jest = 1;
                int jest2 = 1;
                for (int j = 0; j < 2; j++)
                {
                    if (directions[i, j] != turn || directions[i + 4, j] != turn)
                    {
                        jest = 0;
                    }
                    if (directions[i, j] != ((turn + 1) % 2) || directions[i + 4, j] != ((turn + 1) % 2))
                    {
                        jest2 = 0;
                    }
                }
                if (jest == 1) return 20;
                else if (jest2 == 1) return 19;
            }
            // I, II - 2 condition O_OOO
            for (int i = 0; i < 8; i++)
            {
                int jest = 1;
                int jest2 = 1;
                for (int j = 0; j < 3; j++)
                {
                    if (directions[i, j] != turn || directions[(i + 4) % 8, 0] != turn)
                    {
                        jest = 0;
                    }
                    if (directions[i, j] != ((turn + 1) % 2) || directions[(i + 4) % 8, 0] != ((turn + 1) % 2))
                    {
                        jest2 = 0;
                    }
                }
                if (jest ==1) return 20;
                else if (jest2 == 1) return 19;
            }
            // III, IV - 1 condition OOO_
            for (int i = 0; i < 8; i++)
            {
                int jest = 1;
                int jest2 = 1;
                for (int j = 0; j < 3; j++)
                {
                    if (directions[i, j] != turn)
                    {
                        jest = 0;
                    }
                    if (directions[i, j] != ((turn + 1) % 2))
                    {
                        jest2 = 0;
                    }
                }
                if (jest2 == 1)
                {
                    if (directions[i, 3] == turn || directions[i, 3] == -2) jest2 = 0;
                }
                if (jest == 1)
                {
                    if (directions[i, 3] == ((turn + 1) % 2) || directions[i, 3] == -2) jest = 0;
                }
                if (jest == 1) return 18;
                else if (jest2 == 1) return 17;
            }
            // III, IV - 2 condition OO_O
            for (int i = 0; i < 8; i++)
            {
                int jest = 1;
                int jest2 = 1;
                for (int j = 0; j < 2; j++)
                {
                    if (directions[i, j] != turn || directions[(i + 4) % 8, 0] != turn)
                    {
                        jest = 0;
                    }
                    if (directions[i, j] != ((turn + 1) % 2) || directions[(i + 4) % 8, 0] != ((turn + 1) % 2))
                    {
                        jest2 = 0;
                    }
                }
                if (jest == 1) return 18;
                else if (jest2 == 1) return 17;
            }
            // VI
            for (int i = 0; i < 8; i++)
            {
                int jest = 1;
                for (int j = 0; j < 3; j++)
                {
                    if (directions[i, j] != turn)
                    {
                        jest = 0;
                        break;
                    }
                }
                if (jest == 1 && (directions[i, 3] == ((turn + 1) % 2) || directions[i, 3] == -2)) return 15;
            }
            // V, VII - 1 warunek
            for (int i = 0; i < 8; i++)
            {
                int jest = 1;
                int jest2 = 1;
                for (int j = 0; j < 2; j++)
                {
                    if (directions[i, j] != turn)
                    {
                        jest = 0;
                    }
                    if (directions[i, j] != ((turn + 1) % 2))
                    {
                        jest2 = 0;
                    }
                }
                if (jest == 1) return 16;
                else if (jest2 ==1) return 14;
            }
            // V, VII - 2 warunek
            for (int i = 0; i < 4; i++)
            {
                int jest = 1;
                int jest2 = 1;
                if (directions[i, 0] != turn || directions[i + 4, 0] != turn)
                {
                    jest = 0;
                }
                if (directions[i, 0] != ((turn + 1) % 2) || directions[i + 4, 0] != ((turn + 1) % 2))
                {
                    jest2 = 0;
                }
                if (jest == 1) return 16;
                else if (jest2 == 1) return 14;
            }
            return 1;
        }

        private int heuristics3(int[,] boardHeur)
        {
            int sumaC = 0;
            for (int r = 0; r < 15; r++)
            {
                for (int k = 0; k < 15; k++)
                {
                    if (boardHeur[r, k] != -1)
                    {
                        int[,] directions = new int[8, 4]; //0 black, 1 white, -1 empty, -2 out
                        int yKr = -1;
                        int xKr = 1;
                        int yt = 1;
                        int xt = 1;
                        int posX, posY;
                        for (int i = 0; i < 8; i++)
                        {
                            posX = k;
                            posY = r;
                            yKr += yt;
                            xKr += xt;
                            if (Math.Abs(yKr) == 4)
                            {
                                yt *= -1;
                                yKr = 0;
                            }
                            if (Math.Abs(xKr) == 4)
                            {
                                xt *= -1;
                                xKr = 0;
                            }

                            for (int j = 0; j < 4; j++)
                            {
                                if (yKr > 0) posY++;
                                else if (yKr < 0) posY--;
                                if (xKr > 0) posX++;
                                else if (xKr < 0) posX--;
                                if (posX < 15 && posX >= 0 && posY < 15 && posY >= 0) directions[i, j] = boardHeur[posY, posX];
                                else directions[i, j] = -2; //out of board
                            }
                        }

                        for (int i = 0; i < 4; i++)
                        {
                            for (int j = 3; j >= 0; j--)
                            {
                                int ileA = 0;
                                int ileAN = 0;
                                //start point directions[i,j]
                                for (int jj = 0; jj <= j; jj++)
                                {
                                    if (directions[i, j - jj] == boardHeur[r, k])
                                    {
                                        ileA++;
                                        ileAN++;
                                    }
                                    else if (directions[i, j - jj] == -1)
                                    {
                                        ileA++;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                                for (int jj = 0; jj <= 2 - j; jj++)
                                {
                                    if (directions[i + 4, jj] == boardHeur[r, k])
                                    {
                                        ileA++;
                                        ileAN++;
                                    }
                                    else if (directions[i + 4, jj] == -1)
                                    {
                                        ileA++;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                                ileA++;
                                ileAN++;
                                if (ileA >= 5)
                                {
                                    if (boardHeur[r, k] == 1) sumaC += (ileAN * 4) - 1;
                                    else sumaC -= (ileAN * 4) - 1;
                                }
                            }
                            int ile = 1;
                            int ileN = 1;
                            //start od srodka
                            for (int jj = 0; jj < 4; jj++)
                            {
                                if (directions[i + 4, jj] == boardHeur[r, k])
                                {
                                    ile++;
                                    ileN++;
                                }
                                else if (directions[i + 4, jj] == -1)
                                {
                                    ile++;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            if (ile >= 5)
                            {
                                if (boardHeur[r, k] == 1) sumaC += ((ileN * 4) - 1);
                                else sumaC -= ((ileN * 4) - 1);
                            }
                        }
                    }
                }
            }
            return sumaC;
        }
      
    }
}
