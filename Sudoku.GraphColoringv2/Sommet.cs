using System;
namespace Sudoku.GraphColoringv2
{
    /* La classe "Sommet" représente un Sommet du graphe.
     * 
     * 
     *
     */
    public class Sommet
    {
        //---------------------------------------------------------------------------------//
        /*
         *                                  ATTRIBUTS
         */
        //---------------------------------------------------------------------------------//

        List<Sommet> sommets_adj; //Liste des sommets adjacents

        //Caractéristiques d'un Sommet (indice & couleur)

        int indice;      // son indice dans la grille (81 cases = indice 0 à 80)
        int couleur;     // sa couleur (0 = pas de couleur)

        //Position du sommet par rapport à la grille du sudoku

        int ligne;       // Sa ligne (x)
        int colonne;     // Sa colonne (y)
        int carre;       // son carre (3x3)

        //---------------------------------------------------------------------------------//
        /*
         *                                  CONSTRUCTEURS
         */
        //---------------------------------------------------------------------------------//

        public Sommet()
        {

        }

        //Constructeur Surchargé (chaque sommet possède une position et une couleur)

        public Sommet(int indice, int couleur)
        {
            this.indice = indice; //Sa position
            this.couleur = couleur; //Sa couleur

            //Sudoku (ligne x colonne) = Sudoku (9x9)

            this.ligne = (int)(this.indice / 9); //Sa ligne (9 lignes)
            this.colonne = this.indice % 9; //Sa colonne (9 colonnes)

            //Son carre 
            if (this.ligne < 3)
                this.carre = (int)(this.colonne / 3);
            else if (this.ligne < 6)
                this.carre = 3 + (int)(this.colonne / 3);
            else if (this.ligne < 9)
                this.carre = 6 + (int)(this.colonne / 3);


            //Ses voisins 
            this.sommets_adj = new List<Sommet>();
        }


        //---------------------------------------------------------------------------------//
        /*
         *                                  GETTERS & SETTERS
         */
        //---------------------------------------------------------------------------------//

        public int getCouleur() //Retourne la couleur du sommet
        {
            return this.couleur;
        }

        public void setCouleur(int couleur) //Modifie la couleur du sommet
        {
            this.couleur = couleur;
        }

        //---------------------------------------------------------------------------------//
        /*
         *                                  MÉTHODES
         */
        //---------------------------------------------------------------------------------//


        //Test la couleur affectée au sommet
        //Retourne "True" si la valeur est accepté et respecte les contraintes
        //Retourne "False" sinon
        public bool verifCouleurAdj(int couleur)
        {
            foreach (Sommet s in this.sommets_adj) //On parcours les sommets de la liste des sommets adjacents
            {
                if (s.couleur == couleur)
                {
                    return false;
                }

            }

            return true;
        }


        // Méthode qui détermine les sommets adjacents
        public void AddSommetsAdjacents(List<Sommet> sommets)
        {
            this.sommets_adj = new List<Sommet>(); //On crée une nouvelle liste vide

            foreach (Sommet s in sommets) //Pour chaque sommet de liste attribuée en paramètre
            {
                if (s != this) //Si le sommet n'est pas ajouté dans la nouvelle liste 
                {
                    /*Si le sommet est :
                     * Sur la même ligne
                     * Sur la même colonne
                     * Dans le même carré
                     * ==> On l'ajoute à notre nouvelle liste 
                    */
                    if (s.ligne == this.ligne || s.colonne == this.colonne || s.carre == this.carre)
                    {
                        this.sommets_adj.Add(s);
                    }

                }
            }
        }
    }
}
