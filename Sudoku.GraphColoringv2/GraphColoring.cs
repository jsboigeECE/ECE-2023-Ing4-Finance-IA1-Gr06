using Sudoku.Shared;
using System;

/*On crée un graph dont les sommets sont les cases du sudoku (1 sommet pour chaque case du sudoku) 
indépendemment de ce qu'il y'a dans le sudoku à résoudre. 
On a donc autant de sommet que de case dans le sudoku.

Objectif 1 : Mettre une arrête entre le sommet x et le sommet y si les 2 sommets sont contraintes d'avoir des valeurs différentes
(si elles sont sur la même ligne, colonne ou carré).
*/
namespace Sudoku.GraphColoringv2;
//On crée une classe sommet qui représente une case du sudoku
public class Sommet
{
    public int x { get; set; }
    public int y { get; set; }
    public int valeur { get; set; }
    public int couleur { get; set; }
    public Sommet(int x, int y, int valeur)
    {
        this.x = x;
        this.y = y;
        this.valeur = valeur;
        this.couleur = 0;
    }
}

//On crée une classe graph qui représente le sudoku de 'SudokuGrid' et qui contient les sommets
public class Graph
{
    public Sommet[] sommets { get; set; }
    public int[,] aretes { get; set; }
    public int nbSommets { get; set; }
    public int nbAretes { get; set; }

    /* On instancie un nouveau graph à partir d'un sudoku de la classe 'SudokuGrid' qui est la classe qui représente le sudoku.
    On crée autant de sommet que de case dans le sudoku.
    On crée une arrête entre 2 sommets si les 2 sommets sont contraintes d'avoir des valeurs différentes
    (si elles sont sur la même ligne, colonne ou carré).
    */
    public Graph(SudokuGrid sudoku)
    {
        this.nbSommets = 81;
        this.nbAretes = 0;
        this.sommets = new Sommet[nbSommets];
        this.aretes = new int[nbSommets, nbSommets];
        int cpt = 0;
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                this.sommets[cpt] = new Sommet(i, j, sudoku.Cells[i][j]);
                cpt++;
            }
        }
        for (int i = 0; i < nbSommets; i++)
        {
            for (int j = 0; j < nbSommets; j++)
            {
                if (i != j)
                {
                    if (sommets[i].x == sommets[j].x || sommets[i].y == sommets[j].y || (sommets[i].x / 3 == sommets[j].x / 3 && sommets[i].y / 3 == sommets[j].y / 3))
                    {
                        this.aretes[i, j] = 1;
                        this.nbAretes++;
                    }
                }
            }
        }
    }
}

/*
On va maintenant utiliser ce graphe et le problème de coloration sur ce graphe pour résoudre un problème de sudoku.

On crée une nouvelle classe pour résoudre notre sudoku grâce au graphe qu'on a créé, et qui va résoudre le sudoku
grâce à l'algorithme de coloration de graphes (Welsh-Powell).
Le principe ici, sachant qu'on a un sudoku 9x9, est d'arriver à avoir 9 couleurs différentes sur chaque ligne, colonne, et carré.
*/

/*On crée notre classe 'GraphColoring' qui va instancier un nouveau graph à partir d'un sudoku de la classe 'SudokuGrid'.
On considère les que un 0 dans une case correspond à une case vide.
On va ensuite initialiser les cases non vides du sudoku avec les couleurs correspondantes (par exemple, tous les '1' auront la couleurs 1, etc)
*/


public class GraphColor
{
    public Graph graph { get; set; }
    public int[] couleurs { get; set; }
    public int nbCouleurs { get; set; }






    public GraphColor(SudokuGrid sudoku)
    {
        this.graph = new Graph(sudoku);
        this.couleurs = new int[graph.nbSommets];
        this.nbCouleurs = 0;


        //On remplace les cases non vides du sudoku par les couleurs correspondantes, dans le sudoku


        for (int i = 0; i < graph.nbSommets; i++)
        {
            //On colorie les cases non vides du sudoku avec les couleurs correspondantes
            if (graph.sommets[i].valeur != 0)
            {
                graph.sommets[i].couleur = graph.sommets[i].valeur;
                couleurs[i] = graph.sommets[i].valeur;
                nbCouleurs++;
            }
        }


        //On colorie les cases vides du sudoku avec les couleurs correspondantes



        for (int i = 0; i < graph.nbSommets; i++)
        {

            if (graph.sommets[i].couleur == 0)
            {
                int couleur = 1;
                bool ok = false;
                while (!ok)
                {
                    ok = true;
                    for (int j = 0; j < graph.nbSommets; j++)
                    {

                        if (graph.aretes[i, j] == 1 && graph.sommets[j].couleur == couleur)
                        {
                            ok = false;
                        }


                        //Pour chaque sommet voisin, on vérie ses couleurs disponible
                        //Si le voisin n'a qu'une seule couleure disponible, on lui attribue cette couleur
                        //Et on élimine cette couleur des couleurs disponibles pour le sommet i

                        if (graph.aretes[i, j] == 1 && graph.sommets[j].couleur == 0)
                        {
                            int nbCouleursDispo = 0;
                            int couleurDispo = 0;
                            for (int k = 0; k < graph.nbSommets; k++)
                            {
                                if (graph.aretes[i, k] == 1 && graph.sommets[k].couleur == 0)
                                {
                                    nbCouleursDispo++;
                                    couleurDispo = k;
                                }
                            }
                            if (nbCouleursDispo == 1)
                            {
                                graph.sommets[couleurDispo].couleur = graph.sommets[j].couleur;
                                couleurs[couleurDispo] = graph.sommets[j].couleur;
                                nbCouleurs++;
                            }
                        }


                    }
                    if (!ok)
                    {
                        couleur++;
                    }




                }
                graph.sommets[i].couleur = couleur;
                couleurs[i] = couleur;
                nbCouleurs++;


            }

        }


    }





    //Fonction qui recupère un sudoku, le convertit en graphe, et qui retourne un sudoku résolu
    public SudokuGrid Convert()
    {
        //On crée un nouveau sudoku à partir du sudoku passé en paramètre
        SudokuGrid sudokuResolu = new SudokuGrid();

        //On remet chaque sommet du graph dans le sudoku
        for (int i = 0; i < graph.nbSommets; i++)
        {
            sudokuResolu.Cells[graph.sommets[i].x][graph.sommets[i].y] = graph.sommets[i].couleur;
        }
        //On retourne le sudoku résolu
        return sudokuResolu;


    }







}