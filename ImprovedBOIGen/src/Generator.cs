using Spectre.Console;

namespace ImprovedBOIGen;

public class Room
{
    public Dictionary<(int x, int y), Room?> ConnectedRooms { get; set; }
    public int NumberOfConnections { get; set; }

    public Room()
    {
        ConnectedRooms = new Dictionary<(int x, int y), Room?>()
        {
            {(1, 0), null},
            {(-1, 0), null},
            {(0, 1), null},
            {(0, -1), null},
        };

        NumberOfConnections = 0;
    }
}

public class Generator
{
    private const int MinNumberRooms = 6;
    private const int MaxNumberRooms = 10;
    private const int GenerationChance = 20;

    public Dictionary<(int x, int y), Room> Generate()
    {
        var rand = new Random();
        var roomSeed = rand.Next();
        var dungeon = new Dictionary<(int x, int y), Room>();
        var size = rand.Next(MinNumberRooms, MaxNumberRooms + 1);
        
        dungeon.Add((0, 0), new Room());
        size -= 1;

        while (size > 0)
        {
            foreach (var i in dungeon.Keys.ToList())
            {
                if (rand.Next(0, 101) < GenerationChance)
                {
                    var direction = rand.Next(0, 5);
                    (int x, int y) newRoomPosition;
                    switch (direction)
                    {
                        case 0:
                            newRoomPosition = (i.x + 1, i.y + 0);
                            if (!dungeon.ContainsKey(newRoomPosition))
                            {
                                dungeon.Add(newRoomPosition, new Room());
                                size--;
                            }
                            if (dungeon[newRoomPosition].ConnectedRooms[(1, 0)] == null)
                            {
                                ConnectRooms(dungeon[i], dungeon[newRoomPosition], (1, 0));
                            }
                            break;
                        case 1:
                            newRoomPosition = (i.x - 1, i.y + 0);
                            if (!dungeon.ContainsKey(newRoomPosition))
                            {
                                dungeon.Add(newRoomPosition, new Room());
                                size--;
                            }
                            if (dungeon[newRoomPosition].ConnectedRooms[(-1, 0)] == null)
                            {
                                ConnectRooms(dungeon[i], dungeon[newRoomPosition], (-1, 0));
                            }                           
                            break;
                        case 2:
                            newRoomPosition = (i.x + 0, i.y + 1);
                            if (!dungeon.ContainsKey(newRoomPosition))
                            {
                                dungeon.Add(newRoomPosition, new Room());
                                size--;
                            }
                            if (dungeon[newRoomPosition].ConnectedRooms[(0, 1)] == null)
                            {
                                ConnectRooms(dungeon[i], dungeon[newRoomPosition], (0, 1));
                            }                           
                            break;
                        case 3:
                            newRoomPosition = (i.x + 0, i.y - 1);
                            if (!dungeon.ContainsKey(newRoomPosition))
                            {
                                dungeon.Add(newRoomPosition, new Room());
                                size--;
                            }
                            if (dungeon[newRoomPosition].ConnectedRooms[(0, -1)] == null)
                            {
                                ConnectRooms(dungeon[i], dungeon[newRoomPosition], (0, -1));
                            }                           
                            break;
                    }
                }
            }
        }

        while (!IsUnique(dungeon))
        {
            dungeon = Generate();
        }
        return dungeon;
    }

    private bool IsUnique(Dictionary<(int x, int y), Room> dungeon)
    {
        var roomsWithMany = 0;
        foreach (var pair in dungeon)
        {
            if (pair.Value.NumberOfConnections > 2) roomsWithMany++;
        }

        return roomsWithMany > 1;
    }

    private void ConnectRooms(Room room1, Room room2, (int x, int y) direction)
    {
        room1.ConnectedRooms[direction] = room2;
        room2.ConnectedRooms[(direction.x * -1, direction.y * -1)] = room1;
        room1.NumberOfConnections++;
        room2.NumberOfConnections++;
    }
    
    
}