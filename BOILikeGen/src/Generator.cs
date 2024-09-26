namespace BOILikeGen;

public enum DIRECTION
{
    N,
    S,
    E,
    W
}

public class Room
{
    public Dictionary<DIRECTION, Room> taken { get; set; }
    public List<DIRECTION> available { get; set; }
    public int Id { get; set; }
    public (int x, int y) position { get; set; }

    public Room(int id)
    {
        Id = id;
        taken = new Dictionary<DIRECTION, Room>();
        available = [DIRECTION.N, DIRECTION.S, DIRECTION.E, DIRECTION.W];
    }
}

public class Generator
{
    private Dictionary<(int, int), Room> Rooms { get; set; } = [];
    private readonly Room _start = new Room(0) { position = (0, 0) };
    public void Generate(int count)
    {
        Rooms.Add(_start.position, _start);
        var lastSpawned = SpawnRoom(_start);
        for (int i = 1; i < count; i++)
        {
            lastSpawned = SpawnRoom(lastSpawned);
        }
    }

    private Room SpawnRoom(Room room)
    {
        var current = room;
        while (current.available.Count == 0)
            current = current.taken.Values.ElementAt(new Random().Next(Rooms.Keys.Count));
        
        var direction = GetRandomExit(current);
        while (current.taken.ContainsKey(direction))
        {
            current = current.taken[direction];
            direction = GetRandomExit(current);
        }

        var newId = current.Id + 1;
        var position = current.position;
        switch (direction)
        {
            case DIRECTION.N:
                position.y += 1;
                break;
            case DIRECTION.S:
                position.y -= 1;
                break;
            case DIRECTION.E:
                position.x += 1;
                break;
            case DIRECTION.W:
                position.x -= 1;
                break;
        }
        var newRoom = new Room(newId) { position = position };
        current.taken.Add(direction, newRoom);
        current.available.Remove(direction);
        newRoom.taken.Add(GetOppositeDirection(direction), room);
        current.available.Remove(GetOppositeDirection(direction));
        Rooms.Add(newRoom.position, newRoom);
        
        return newRoom;
    }

    private DIRECTION GetOppositeDirection(DIRECTION direction)
    {
        switch (direction)
        {
            case DIRECTION.N:
                return DIRECTION.S;
            case DIRECTION.S:
                return DIRECTION.N;
            case DIRECTION.E:
                return DIRECTION.W;
            case DIRECTION.W:
                return DIRECTION.E;
            default:
                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }
    }

    private DIRECTION GetRandomExit(Room room)
    {
        Random random = new();
        var availableCount = room.available.Count;
        var selected = random.Next(availableCount);
        return room.available[selected];
    }

    public void PrintRooms()
    {
        foreach (var pair in Rooms)
        {
            Console.Write($"{pair.Key}\t:\t{pair.Value.Id}\n");
            foreach (var taken in pair.Value.taken)
            {
                Console.WriteLine($"\t{taken.Key}\t{taken.Value.Id}\t{taken.Value.position}");
            }
        }
    }
}