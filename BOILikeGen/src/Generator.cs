namespace BOILikeGen;

public enum Direction { N, S, E, W }

public class Room(int id)
{
    public Dictionary<Direction, Room> Taken { get; set; } = [];
    public List<Direction> Available { get; set; } = [Direction.N, Direction.S, Direction.E, Direction.W];
    public int Id { get; set; } = id;
    public (int x, int y) Position { get; init; }
}

public class Generator
{
    private Dictionary<(int, int), Room> Rooms { get; set; } = [];
    private readonly Room _start = new Room(0) { Position = (0, 0) };
    public void Generate(int count)
    {
        Rooms.Add(_start.Position, _start);
        var lastSpawned = SpawnRoom(_start);
        for (int i = 1; i < count; i++)
        {
            lastSpawned = SpawnRoom(lastSpawned);
        }
    }

    private Room SpawnRoom(Room room)
    {
        var current = room;
        while (current.Available.Count == 0)
            current = current.Taken.Values.ElementAt(new Random().Next(Rooms.Keys.Count));
        
        var direction = GetRandomExit(current);
        while (current.Taken.ContainsKey(direction))
        {
            current = current.Taken[direction];
            direction = GetRandomExit(current);
        }

        var newId = current.Id + 1;
        var position = current.Position;
        switch (direction)
        {
            case Direction.N:
                position.y += 1;
                break;
            case Direction.S:
                position.y -= 1;
                break;
            case Direction.E:
                position.x += 1;
                break;
            case Direction.W:
                position.x -= 1;
                break;
        }
        var newRoom = new Room(newId) { Position = position };
        current.Taken.Add(direction, newRoom);
        current.Available.Remove(direction);
        newRoom.Taken.Add(GetOppositeDirection(direction), room);
        current.Available.Remove(GetOppositeDirection(direction));
        Rooms.Add(newRoom.Position, newRoom);
        
        return newRoom;
    }

    private Direction GetOppositeDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.N:
                return Direction.S;
            case Direction.S:
                return Direction.N;
            case Direction.E:
                return Direction.W;
            case Direction.W:
                return Direction.E;
            default:
                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }
    }

    private Direction GetRandomExit(Room room)
    {
        Random random = new();
        var availableCount = room.Available.Count;
        var selected = random.Next(availableCount);
        return room.Available[selected];
    }

    public void PrintRooms()
    {
        foreach (var pair in Rooms)
        {
            Console.Write($"{pair.Key}\t:\t{pair.Value.Id}\n");
            foreach (var taken in pair.Value.Taken)
            {
                Console.WriteLine($"\t{taken.Key}\t{taken.Value.Id}\t{taken.Value.Position}");
            }
        }
    }
}