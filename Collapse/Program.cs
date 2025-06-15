internal class Program
{
    private static void Main(string[] args)
    {
        using var game = new Collapse.Code.View.GameView();
        game.Run();
    }
}