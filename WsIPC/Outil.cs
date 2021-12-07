namespace WsIPC
{
    public enum TypeOutil
    {
        Tournevis,
        CleAMolette
    }

    public class Outil
    {
        public string Nom { get; set; } = string.Empty;
        public TypeOutil Type { get; set; }
        public string Utilisateur { get; private set; } = string.Empty;

        public void Utiliser(string utilisateur)
        {
            Utilisateur = utilisateur;
            Console.WriteLine($"L'outil {Nom} est utilisé par {Utilisateur}");
        }
            
        public void Liberer()
        {
            Console.WriteLine($"L'outil {Nom} est libéré par {Utilisateur}");
            Utilisateur = string.Empty;
        }
    }
}
