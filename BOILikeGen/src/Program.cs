namespace BOILikeGen;

public static class Program
{
    public static void Main(string[] args)
    {
        // var generator = new Generator();
        // generator.Generate(5);
        // generator.PrintRooms();
        var str = "h-h";
        str = str.PadCenter(10);
        Console.WriteLine("." + str + ".");
    }

    public static string PadCenter(this string str, int length)
    {
        var leftSpaces = (length - str.Length) / 2;
        var rightSpaces = length - str.Length - leftSpaces;
        return str.PadLeft(leftSpaces + str.Length).PadRight(length);
    }
}