namespace WsIPC
{
    public static class Counter
    {
        private static int nombrePieces = 0;
        private static object x = new object();

        public static void AfficherTotal()
        {
            lock (x)
            {
                nombrePieces++;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"Nombre de pièces -> {nombrePieces}");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }
    }
}