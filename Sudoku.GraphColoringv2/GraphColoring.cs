using System;
using System.Net.Sockets;
using Sudoku.Shared;
namespace Sudoku.GraphColoringv2
{
    /* La classe "GraphColoring" permet de : 
     * Convertir notre Sudoku en Graphe
     * Appliquer un algorithme de coloration de graphe pour résoudre le sudoku
     *
     */
    public class GrapheColoring
    {
        //---------------------------------------------------------------------------------//
        /*
         *                                  ATTRIBUTS
         */
        //---------------------------------------------------------------------------------//

        List<Sommet> sommets; //Liste des sommets du graphe
        SudokuGrid grid; //Construit à partir d'une grille de sudoku
        public const int nb_cases = 81; // Avec 81 cases (9x9)

        //---------------------------------------------------------------------------------//
        /*
         *                                  CONSTRUCTEURS
         */
        //---------------------------------------------------------------------------------//

        public GrapheColoring()
        {
        }

        //On ajoute l'ensemble des sommets dans le graphe


        //Constructeur surchargé

        public GrapheColoring(SudokuGrid grid) //Construction d'un Grpahe à partir d'une grille de sudoku
        {
            this.sommets = new List<Sommet>(); //Nouvelle liste vide

            //CONSTRUCTION DU GRAPHE
            this.grid = grid.CloneSudoku(); //On récupère la grille de sudoku de départ
            
            int ligne;
            int colonne;

            //On parcours le sudoku

            /*
             * On attribut au graphe chaque sommet (case du sudoku) en lui donnant:
             * sa position (dans le sudoku)
             * sa couleur (si 0 = pas de couleur)
             */
            for (int i = 0; i < nb_cases; i++) 
            {
                ligne = (int)(i / 9); //Nb de ligne du sudoku
                colonne = i % 9; //Nb de colonne du sudoku

                //Le graphe possède des Sommet (position, couleur)

                this.sommets.Add(new Sommet(i, this.grid.Cells[ligne][colonne]));
            }

            //On parcours tous les sommets de notre graphe
            //Pour chaque sommet, on lui attribut sa liste de sommets adjacent
            foreach (Sommet s in this.sommets)
            {
                s.AddSommetsAdjacents(this.sommets);
            }
        }

        //---------------------------------------------------------------------------------//
        /*
         *                                  GETTERS
         */
        //---------------------------------------------------------------------------------//

        public SudokuGrid getGrid() //Retourne le sudoku
        {
            return this.grid;
        }

        //---------------------------------------------------------------------------------//
        /*
         *                                  MÉTHODES
         */
        //---------------------------------------------------------------------------------//

        //Méthode qui affiche d'afficher le sudoku à tous moment
        public void AfficherGrille()
        {
            Console.WriteLine("----------------------------------");
            int i = 0;
            foreach (Sommet s in this.sommets)
            {
                if (i % 3 == 0)
                    Console.Write("| ");
                Console.Write("{0,2:#0} ", s.getCouleur());
                i++;
                if (i % 9 == 0)
                    Console.WriteLine("|");
                if (i % 27 == 0)
                    Console.WriteLine("----------------------------------");
            }
            Console.WriteLine();
        }

        /* Méthode qui renvoie :
             - true si le graphe a pu être entièrement complété
             - false sinon
        */
        bool attribuerCouleurGraphe(int numSommet, int nbCouleurs, int couleur)
        {

                                //Initialisation
           
            Sommet s_actuel = this.sommets.ElementAt(numSommet); //On récupère le sommet à traiter parmis les 81 sommets
            int couleur_enregistre = 0;     //On initialise une variable qui sauvegarde la couleur

                                //vérification initiale 
                                 
            if (s_actuel.getCouleur() != 0) //Si le sommet a une couleur
            {
                couleur_enregistre = s_actuel.getCouleur(); //On la garde
            }
            else //Si le sommet est vide
            {
                if (s_actuel.verifCouleurAdj(couleur)) //SI la couleur passé en paramètre n'est pas déja attribué à un voisin
                {
                    // La couleur n'est pas déjà utilisée par un voisin
                    couleur_enregistre = 0; // On sauvegarde l'absence de couleur
                    s_actuel.setCouleur(couleur); //  on affecte la couleur au Sommet
                }
            }

                            //Vérification suivante

            /* SI le sommet a maintenant une couleur 
             * On met a jours les valeurs et on passe au sommet suivant pour effectuer la même manip
             * 
             * SINON
             * Le Sommet garde sa même couleur initiale (une couleure ou 0) et returne False
             */

            if (s_actuel.getCouleur() != 0)  // On vérifie si le sommet a désormais une couleur
            {
                // MISE À JOUR
                nbCouleurs++; //Une nouvelle couleur a été utilisée
                numSommet++; //On passe au sommet suivante

                //VERIF : SUDOKU COMPLET
                if (numSommet == nb_cases) //Si on est arrivée au dernier sommet 
                {
                    return true; //On a fini

                }

                // VERIF : Si le nombre de couleurs est à 9 ==> on a une ligne complète
                nbCouleurs = nbCouleurs % 9; //On remet le nb de couleur à 0


                // Si le sommet suivante contient déjà une couleur
                if (this.sommets.ElementAt(numSommet).getCouleur() != 0) // on lance l'algorithme sur cette couleur
                {
                    if (attribuerCouleurGraphe(numSommet, nbCouleurs, this.sommets.ElementAt(numSommet).getCouleur()))
                    {
                        return true;
                    }
                        
                }
                else //Sinon; On lance l'algorithme en testant l'ensemble des couleurs de 1 à 9 sur la case suivante
                {
                    
                    for (int colour = 1; colour <= 9; colour++)
                    {
                        if (attribuerCouleurGraphe(numSommet, nbCouleurs, colour))
                        {
                            return true;
                        }    
                            
                    }
                }

                                        //DERNIERE VERIF : SI toujours à "False"

                
               
                s_actuel.setCouleur(couleur_enregistre); // On restaure la couleur initiale de la case (ou l'absence de couleur)
            }
            return (numSommet == nb_cases);
        }

        //ALOGORITHME DE COLORATION DE GRAPHE "NAÏF"

        public void AlgoNaifOptimise()
        {
            
     
            if (this.sommets.First().getCouleur() != 0) //SI le 1er sommet contient déja une couleure 
            {
                attribuerCouleurGraphe(0, 0, this.sommets.First().getCouleur()); //On lance l'Algo sur cette couleure
            }
            else //SINON
            {
                // On lance l'algorithme en testant l'ensemble des couleurs de 1 à 9 sur le 1er Sommet
                for (int colour = 1; colour <= 9; colour++) 
                {
                    if (attribuerCouleurGraphe(0, 0, colour))
                    {
                        break;
                    }
                }
            }

            // On a trouvé la solution. On met à jour la grille
            for (int i = 0; i < this.sommets.Count; i++)
            {
                this.grid.Cells[(int)(i / 9)][i % 9] = this.sommets.ElementAt(i).getCouleur();

            }
        }

        //ALGORITHME DE COLORATION DE GRAPHE "WELSH-POWELL"


        public void WelshPowell()
        {
            //1)TRIE DES SOMMETS PAR ORDRE DÉCROISSANT DE DEGRÉS
            List<Sommet> sommetsTries = this.sommets.OrderByDescending(s => s.getCouleur()).ToList();

     
            if (sommetsTries.First().getCouleur() != 0) //SI le 1er sommet contient déja une couleure 
            {
                attribuerCouleurGraphe(0, 0, this.sommets.First().getCouleur()); //On lance l'Algo sur cette couleure
            }

            else //SINON
            {
                // On lance l'algorithme en testant l'ensemble des couleurs de 1 à 9 sur le 1er Sommet
                for (int colour = 1; colour <= 9; colour++)
                {
                    if (attribuerCouleurGraphe(0, 0, colour))
                    {
                        break;
                    }
                }
            }

            // On a trouvé la solution. On met à jour la grille
            for (int i = 0; i < sommetsTries.Count; i++)
            {
                this.grid.Cells[(int)(i / 9)][i % 9] = this.sommets.ElementAt(i).getCouleur();

            }
        }


    }
}
