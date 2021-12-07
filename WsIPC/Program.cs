namespace WsIPC
{
    public class MyClass
    {
        private static readonly int faster = 1;

        private static readonly object x = new();

        static void Main(string[] args)
        {
            Console.WriteLine("Simulation de travail\n\n");

            //Création des outils
            Outil tournevis1 = new() { Nom = "Tournevis 1", Type = TypeOutil.Tournevis };
            Outil tournevis2 = new() { Nom = "Tournevis 2", Type = TypeOutil.Tournevis };
            Outil cle1 = new() { Nom = "Cle a molette 1", Type = TypeOutil.CleAMolette };
            Outil cle2 = new() { Nom = "Cle a molette 2", Type = TypeOutil.CleAMolette };

            //Création des threads
            Thread ouvrier1;
            Thread ouvrier2;
            Thread ouvrier3;
            Thread ouvrier4;

            var creerPiece = (object o) =>
            {
                Context c = (Context)o; //cast de l'objet necessaire car ParameterizedThreadStart accepte uniquement un seul param de type object
                var compteur = 0;

                //Methode de travail qui s'execute en continue (pas de repos pour les braves)
                while (true)
                {
                    if (Monitor.TryEnter(c.OutilA, 15000/faster) && Monitor.TryEnter(c.OutilB, 15000/faster))
                    {
                        c.OutilA.Utiliser(c.Ouvrier);
                        c.OutilB.Utiliser(c.Ouvrier);

                        Thread.Sleep(4000/faster); //Créer la pièce dure 4s
                        compteur++; //+1 pièce créée

                        lock (x) //Evite qu'un autre thread change la couleur avant d'afficher le message
                        {
                            Console.ForegroundColor = c.ConsoleColor;
                            Console.WriteLine($"L'{c.Ouvrier} a créé sa pièce n°{compteur}");
                            Console.ForegroundColor = ConsoleColor.Gray;
                        }

                        c.OutilA.Liberer();
                        c.OutilB.Liberer();

                        Counter.AfficherTotal();

                        //Ne pas oublier de liberer les ressources !
                        Monitor.Exit(c.OutilA); 
                        Monitor.Exit(c.OutilB);
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"L'{c.Ouvrier} est blocké");
                        Console.ForegroundColor = ConsoleColor.Gray;
                        
                        Thread.Sleep(1000/faster);
                    }
                }
            };

            ouvrier1 = new Thread(new ParameterizedThreadStart(creerPiece.Invoke));
            ouvrier2 = new Thread(new ParameterizedThreadStart(creerPiece.Invoke));
            ouvrier3 = new Thread(new ParameterizedThreadStart(creerPiece.Invoke));
            ouvrier4 = new Thread(new ParameterizedThreadStart(creerPiece.Invoke));

            var threadParams1 = new Context { Ouvrier = "Ouvrier 1", OutilA = tournevis1, OutilB = cle1, ConsoleColor = ConsoleColor.Blue };
            ouvrier1.Start(threadParams1);

            var threadParams2 = new Context { Ouvrier = "Ouvrier 2", OutilA = tournevis1, OutilB = cle2, ConsoleColor = ConsoleColor.Cyan };
            ouvrier2.Start(threadParams2);

            var threadParams3 = new Context { Ouvrier = "Ouvrier 3", OutilA = tournevis2, OutilB = cle1, ConsoleColor = ConsoleColor.Magenta };
            ouvrier3.Start(threadParams3);

            var threadParams4 = new Context { Ouvrier = "Ouvrier 4", OutilA = tournevis2, OutilB = cle2, ConsoleColor = ConsoleColor.Yellow };
            ouvrier4.Start(threadParams4);

            int temps = 0;

            while (temps <= 1000/faster) //while(true) pour execution infinie
            {
                Thread.Sleep(5000);
                temps += 5;

                lock (x)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Temps de production : {0}", temps);
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
            }
        }
    }
}
