namespace BOILikeGen;

public static class Program
{
    public static void Main(string[] args)
    {
        var generator = new Generator();
        generator.Generate(2);
        generator.PrintRooms();
    }
}