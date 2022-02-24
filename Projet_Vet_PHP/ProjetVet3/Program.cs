using System;
using MySql.Data.MySqlClient;
using System.Collections;

namespace Projet_1_Shell
{
    public class Program
    {
        ArrayList tableau = new ArrayList();
        static void Main(string[] args)
        {
            Program program = new Program();
            program.startTheMachine();
            program.connectToDatabase();
        }


        public MySqlConnection connectToDatabase()
        {
            string connectionString = null;
            MySqlConnection cnn;
            connectionString = "server=localhost;database=projetvet;uid=root;pwd=;";
            cnn = new MySqlConnection(connectionString);

            try
            {
                cnn.Open();
                if (cnn.State == System.Data.ConnectionState.Open)
                {
                    Console.WriteLine("La connexion à la BD est fonctionnelle");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Impossible d'ouvrir la connexion." + ex.Message);
            }
            return cnn;
        }

        private void afficherMenu()
        {
            string MenuChoice = "";
            do
            {
                Console.WriteLine("Menu");
                Console.WriteLine("1 - Add animal");
                Console.WriteLine("2 - Registered animals list");
                Console.WriteLine("3 - Owners list");
                Console.WriteLine("4 - Number of registered animals");
                Console.WriteLine("5 - Total weight of registered animals(in KG)");
                Console.WriteLine("6 - List of Animals specific colors");
                Console.WriteLine("7 - Withdraw an animal");
                Console.WriteLine("8 - Modify animal name");
                Console.WriteLine("9 - Quit");
                Console.WriteLine(" ");
                Console.WriteLine("Please enter option");
                MenuChoice = Console.ReadLine();
                selectChoice(MenuChoice);
            }
            while (MenuChoice != "8");
        }

        private void startTheMachine()
        {
            afficherMenu();
        }

        private void selectChoice(string choice)
        {
            switch (choice)
            {
                case  "1":
                    ajouterUnAnimal();
                    break;

                case "2":
                    voirListeAnimauxPension();
                    break;

                case "3":
                    voirListePropriétaire();
                    break;

                case "4":
                    voirNombreTotalAnimaux();
                    break;

                case "5":
                    voirPoidsTotalAnimaux();
                    break;

                case "6":
                    extraireAnimauxSelonCouleurs();
                    break;

                case "7":
                    retirerUnAnimalDeListe();
                    break;

                case "8":
                    modifierAnimal();
                    break;

                case "9":
                    quitprog();
                break;

                default:
                    afficherMessageErreur();
                    break;
            }

        }

        private void afficherMessageErreur()
            
        {
            Console.WriteLine("CHOICE INVALID TRY AGAIN ");
        }

        private void ajouterUnAnimal()
        {
            string animaltype, animalname, color, ownername;
            int age, weight;

            Console.WriteLine("Please enter animal type : ");
            animaltype = Console.ReadLine();
            Console.WriteLine("Please enter animal name : ");
            animalname = Console.ReadLine();
            Console.WriteLine("Please enter animal age : ");
            age = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Please enter animal weight : ");
            weight = Convert.ToInt32(Console.ReadLine());

            do
            {
                Console.WriteLine("Please enter animal color : ");
                color = Console.ReadLine();

                if(color != "purple" & color != "red" & color != "blue")
                {
                    afficherMessageErreur();
                }

            } while (color != "purple" & color != "red" & color != "blue");

            Console.WriteLine("Please enter owner's name : ");
            ownername = Console.ReadLine();


            /*
            tableau[i, 0] = Convert.ToString(i);
            tableau[i, 1] = animaltype;
            tableau[i, 2] = name;
            tableau[i, 3] = Convert.ToString(age);
            tableau[i, 4] = Convert.ToString(weight);
            tableau[i, 5] = color;
            tableau[i, 6] = ownername;
            break;
            */

            MySqlConnection conn = this.connectToDatabase();
            string sql = "INSERT INTO listeanimaux (animaltype, animalname, color, ownername, age, weight) " + " VALUES (@animaltype, @name, @color, @ownername, @age, @weight)";
            MySqlCommand command = new MySqlCommand(sql, conn);
            command.Parameters.AddWithValue("@animaltype", animaltype);
            command.Parameters.AddWithValue("@name", animalname);
            command.Parameters.AddWithValue("@color", color);
            command.Parameters.AddWithValue("@ownername", ownername);
            command.Parameters.AddWithValue("@age", age);
            command.Parameters.AddWithValue("@weight", weight);

            command.ExecuteReader();
            conn.Close();
            Console.WriteLine("Requête UPDATE terminé");
            Console.WriteLine();
                
  
        }

    private void voirListeAnimauxPension()
        {
            string ID = "ID", ANIMALTYPE = "ANIMAL TYPE", NAME = "NAME", AGE = "AGE", WEIGHT = "WEIGHT", COLOR = "COLOR", OWNER = "OWNER";

            Console.WriteLine("--------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("{0,10} {1,10} {2,10} {3,10} {4,10} {5,10} {6,10}", ID ,"|" + ANIMALTYPE + "|",NAME + "  |", AGE + " |"," " + WEIGHT + " |",COLOR + " |",OWNER + " ");
            Console.WriteLine("--------------------------------------------------------------------------------------------------------------");

            MySqlConnection conn = this.connectToDatabase();
            MySqlCommand command = new MySqlCommand("SELECT * FROM listeanimaux", conn);
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string animaltype = reader.GetString(1);
                        string name = reader.GetString(2);
                        string color = reader.GetString(3);
                        string ownername = reader.GetString(4);
                        int age = reader.GetInt32(5);
                        int weight = reader.GetInt32(6);
                        Console.WriteLine("Résultats : " + id + " " + animaltype + " " + name + " " + color + " " + ownername + " " + age + " " + weight);
                    }
                    Console.WriteLine();
                }
            }
            conn.Close();
        }
            
        private void voirListePropriétaire()
        {   
            Console.WriteLine("--------------------------------------------------------------------------");
            Console.WriteLine("| PROPRIÉTAIRE |");
            Console.WriteLine("--------------------------------------------------------------------------");

            MySqlConnection conn = this.connectToDatabase();
            MySqlCommand command = new MySqlCommand("SELECT * FROM listeanimaux", conn);
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string ownername = reader.GetString(4);
                        Console.WriteLine("Resultat du SELECT #1 : " + ownername);
                    }
                    Console.WriteLine();
                }
            }
            conn.Close();
        }

        private void voirNombreTotalAnimaux()
        {
            int nbranimal = 0;
                Console.WriteLine("----------------------------------------------------------------------");
                Console.WriteLine("| ANIMALS NUMBER |");
                Console.WriteLine("----------------------------------------------------------------------");

            MySqlConnection conn = this.connectToDatabase();
            MySqlCommand command = new MySqlCommand("SELECT count(*) FROM listeanimaux", conn);
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        int total = reader.GetInt32(0);
                        Console.WriteLine("Resultat du SELECT #1 : " + total);
                    }
                    Console.WriteLine();
                }
            }
            conn.Close();
        }
        
        private void voirPoidsTotalAnimaux()
        {

                Console.WriteLine("----------------------------------------------------------------------");
                Console.WriteLine("| TOTAL WEIGHT |");
                Console.WriteLine("----------------------------------------------------------------------");
            MySqlConnection conn = this.connectToDatabase();
            MySqlCommand command = new MySqlCommand("SELECT count(*) FROM listeanimaux", conn);
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        int total = reader.GetInt32(0);
                        Console.WriteLine("Resultat du SELECT #1 : " + total);
                    }
                    Console.WriteLine();
                }
            }
            conn.Close();
        }
        // Pas fini
        private void extraireAnimauxSelonCouleurs()
        {
            string color;
            string ID = "ID", ANIMALTYPE = "ANIMAL TYPE", NAME = "NAME", COLOR = "COLOR";
            Console.WriteLine("Please enter color: ");
            color = Console.ReadLine();

            MySqlConnection conn = this.connectToDatabase();
            MySqlCommand command = new MySqlCommand("SELECT * FROM listeanimaux WHERE color = @color", conn);
            command.Parameters.AddWithValue("@color", color);
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string targetcolor = reader.GetString(3);
                        Console.WriteLine("Resultat du SELECT #1 : " + targetcolor);
                    }

                    Console.WriteLine();
                }
            }
            conn.Close();
        }

        private void retirerUnAnimalDeListe()
        {
            string ID;
            Console.WriteLine("DELETE ANIMAL ID:");
            ID = Console.ReadLine();

            MySqlConnection conn = this.connectToDatabase();
            string sql = "DELETE FROM listeanimaux WHERE id = @id";
            MySqlCommand command = new MySqlCommand(sql, conn);
            command.Parameters.AddWithValue("@id", ID);

            command.ExecuteReader();
            conn.Close();
            Console.WriteLine("Requête DELETE terminé");
            Console.WriteLine();
        }

        private void modifierAnimal()
        {
            string ID;
            string nomAnimal;
            Console.WriteLine("Veuillez entrer l'ID de l'animal que vous désirez modifier génétiquement");
            ID = Console.ReadLine();
            Console.WriteLine("Veuillez entrer le nouveau nom que vous voulez donner à l'animal");
            nomAnimal = Console.ReadLine();

            MySqlConnection conn = this.connectToDatabase();
            

            string sql = "UPDATE listeanimaux SET animalname = @param_val_1 WHERE id = @param_val_2";
            MySqlCommand command = new MySqlCommand(sql, conn);
            command.Parameters.AddWithValue("@param_val_1", nomAnimal);
            command.Parameters.AddWithValue("@param_val_2", ID);

            command.ExecuteReader();
            conn.Close();
            Console.WriteLine("Requête UPDATE terminé");
            Console.WriteLine();
        }
        
        private void quitprog()
        {
            System.Environment.Exit(0);
        }

    }
}

