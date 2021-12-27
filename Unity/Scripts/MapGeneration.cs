using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

/*
 * Goal: Add merchants
 * Steps:
 * - Merchant[0] -> put next to the player in the starting room
 * - Merchant[1] -> place randomly in rooms at a very low rate (DONE)
 * - Merchant[2-3] -> pick 2 random, separate rooms to place these in
 * 
 * Goal: Randomize floor tiles
 */



public enum Entity { Chaser, Shooter, Miner, Charger, Heal, Curse, Reward, Gateway };

public struct Room
{
    public double seed;
    public int width, height, x, y;
    public int[] entityX, entityY;
    public Entity[] entities;
}

public struct Door
{
    public int x, y, orientation; //orientation = 0 -> horizontal, 1 -> vertical
}

public class MapGeneration : MonoBehaviour
{
    /* Variables */
    [Range(0, 1)]
    public double MIN_POWER = .25; //area ^ MIN_POWER = min entities in a room
    [Range(0, 1)]
    public double MAX_POWER = .5; //area ^ MAX_POWER = max entities in a room
    [Range(0, 1000)]
    public int MAP_WIDTH = 150; //tile width of overall map
    [Range(0, 1000)]
    public int MAP_HEIGHT = 70; //tile height of overall map
    [Range(5, 100)]
    public int MIN_SIZE = 10; //min width/height of a room
    [Range(5, 100)]
    public int MAX_SIZE = 50; //max width/height of a room
    [Range(10, 1000)]
    public int MAX_PLACEMENT_FAILURES = 1000; //max # of times we try to place a room before we throw it away
    [Range(10, 100)]
    public int MAX_ROOM_FAILURES = 25; //max # of rooms we fail to place before giving up
    [Range(1, 100)]
    public int MAX_ROOMS = 9; //max # of rooms
    [Range(1, 10)]
    public int MIN_DISTANCE = 1; //min # of tiles between rooms
    [Range(3, 50)]
    public int ROOM_OVERLAP = 10; //how much the rooms will overlap
    [Range(10, 50)]
    public int BARRIER = 20;
    //The entity generation process picks a number between 0 and 1
    //These determine what the entity becomes
    [Range(0, 1)]
    public double CHASER_MAX = 0.35;    // 0.00 - 0.35 -> CHASER enemy (35% chance)
    [Range(0, 1)]
    public double SHOOTER_MAX = 0.55;    // 0.35 - 0.55 -> ranged enemy (20% chance)
    [Range(0, 1)]
    public double MINER_MAX = 0.75;   // 0.55 - 0.75 -> miner enemy (20% chance)
    [Range(0, 1)]
    public double CHARGER_MAX = 0.98;   // 0.75 - 0.98 -> charger enemy (23% chance)
                                        // 0.98 - 1.00 -> heal merchant (2% chance)
    public GameObject CHASER;
    public GameObject SHOOTER;
    public GameObject MINER;
    public GameObject CHARGER;
    public GameObject PLAYER;
    public GameObject[] MERCHANTS;
    public GameObject TUTORIAL;
    public GameObject GATEWAY;

    public Tilemap tmap;
    public Tile[] tile;
    static System.Random rand = new System.Random();


    void Start()
    {
        int[,] map = GenerateMap(MAP_WIDTH, MAP_HEIGHT);
        
        //pad the top/bottom of the map
        for (int i = 0; i < MAP_WIDTH; ++i)
            for (int j = -1 * BARRIER; j < 0; ++j)
            {
                tmap.SetTile(new Vector3Int(i, j, 1), tile[0]);
                tmap.SetTile(new Vector3Int(i, j + MAP_HEIGHT + BARRIER, 1), tile[0]);
            }
        //pad the left/right sides of the map
        for (int i = -1 * BARRIER; i < MAP_HEIGHT + BARRIER; ++i)
            for (int j = -1 * BARRIER; j < 0; ++j)
            {
                tmap.SetTile(new Vector3Int(j, i, 1), tile[0]);
                tmap.SetTile(new Vector3Int(j + MAP_WIDTH + BARRIER, i, 1), tile[0]);
            }
        
        //convert int array into tilemap
        for (int i = 0; i < MAP_WIDTH; ++i)
            for (int j = 0; j < MAP_HEIGHT; ++j)
            {
                if (map[i, j] != 0) //there should be floor under all non-blank tiles
                    tmap.SetTile(new Vector3Int(i, j, 0), tile[1]);
                if (map[i, j] >= 0) //tiles over 0 are actual tiles
                {
                    //if it's floor, randomize it appropriately
                    if (map[i, j] == 1)
                        map[i, j] = rand.Next(31, 42);
                    tmap.SetTile(new Vector3Int(i, j, 1), tile[map[i, j]]);
                }
                else //tiles under 0 have entities on them
                {
                    switch(map[i, j])
                    {
                        case -1: //CHASER
                            Instantiate(CHASER, new Vector3Int(i, j, 0), Quaternion.identity);
                            break;
                        case -2: //SHOOTER
                            Instantiate(SHOOTER, new Vector3Int(i, j, 0), Quaternion.identity);
                            break;
                        case -3: //MINER
                            Instantiate(MINER, new Vector3Int(i, j, 0), Quaternion.identity);
                            break;
                        case -4: //CHARGER
                            Instantiate(CHARGER, new Vector3Int(i, j, 0), Quaternion.identity);
                            break;
                        case -5: //HEAL
                            Instantiate(MERCHANTS[1], new Vector3Int(i, j, 0), Quaternion.identity);
                            break;
                        case -6: //CURSE
                            Instantiate(MERCHANTS[2], new Vector3Int(i, j, 0), Quaternion.identity);
                            break;
                        case -7: //REWARD
                            Instantiate(MERCHANTS[3], new Vector3Int(i, j, 0), Quaternion.identity);
                            break;
                        case -8: //BOSS GATEWAY
                            GameObject gate = Instantiate(GATEWAY, new Vector3(i - 1.5f, j + 0.5f, 0), Quaternion.identity);
                            //gate.transform.Find("Warning Text").gate;
                            //gate.transform.Find("Enter Text").gate;
                            tmap.SetTile(new Vector3Int(i, j, 1), tile[49]);
                            break;
                        case -99: //PLAYER
                            PLAYER.transform.position = new Vector3Int(i, j, 0);
                            //staff/shield merchant
                            Instantiate(MERCHANTS[0], new Vector3Int(i + 2, j, 0), Quaternion.identity);
                            //tutorial text
                            Instantiate(TUTORIAL, new Vector3Int(i, j + 3, 0), Quaternion.identity);
                            break;
                            
                    }
                }
            }
        
        //find the 
    }

    // Update is called once per frame
    void Update()
    {

    }

    int[,] GenerateMap(int MAP_WIDTH, int MAP_HEIGHT)
    {
        int[,] map = new int[MAP_WIDTH, MAP_HEIGHT];
        /*
        * 0. Place premade start/finish rooms
        * 1. Generate an array of rooms
        *    - Every room has a position (Tuple<int, int>) and a seed (double)
        *       -> Position: None of the rooms overlap
        *       -> Seed: A number between 0 and 1. Dictates how interesting the room will be. 
        *           > 0 -> Starting room
        *           > Low seed -> not a very interesting room (smaller, fewer enemies, worse rewards, etc)
        *           > High seed -> very interesting room (larger room, more likely to have enemies and rewards)
        *           > 1 -> Boss room (should only have 1 path leading to it, requires a key to enter?)
        */
        List<Room> rooms = new List<Room>();
        List<Door> doors = new List<Door>();
        int roomFailures = 0;
        //Continue adding rooms until we fail 10 times
        while (roomFailures < MAX_ROOM_FAILURES && rooms.Count < MAX_ROOMS)
        {
            Room next = new Room(); //Make a new room
            SeedRoom(ref next); //Set its seed
            SizeRoom(ref next, MIN_SIZE, MAX_SIZE); //Give it a size
                                                    //if the room is successfully placed, fill it with entities
            if (PlaceRoom(ref next, ref rooms, MAP_WIDTH, MAP_HEIGHT, MAX_PLACEMENT_FAILURES)) //Attempt to place it X times
            {
                FillRoom(ref next); //Fill it with entities
                PlaceEntities(ref next); //Position all of the entities
                ConnectAdjacent(ref next, ref rooms, ref doors);
                rooms.Add(next); //Add it to our list of rooms
            }
            else
                roomFailures++;
        }
        //Now turn all the rooms into meaningful ints
        foreach (Room room in rooms)
        {
            //Floor
            for (int i = 0; i < room.width; ++i)
                for (int j = 0; j < room.height; ++j)
                    map[room.x + i, room.y + j] = 1;
            //Walls
            for (int i = 0; i < room.width; ++i)
            {
                map[room.x + i, room.y + room.height - 1] = 11;
                map[room.x + i, room.y + room.height - 2] = 10;
                map[room.x + i, room.y] = 14;
                map[room.x + i, room.y + 1] = 15;
            }
            for (int i = 0; i < room.height; ++i)
            {
                map[room.x, room.y + i] = 13;
                map[room.x + room.width - 1, room.y + i] = 12;
            }
                
            //Corners
            map[room.x, room.y] = 7;
            map[room.x, room.y + 1] = 6;
            map[room.x + room.width - 1, room.y] = 9;
            map[room.x + room.width - 1, room.y + 1] = 8;
            map[room.x, room.y + room.height - 2] = 3;
            map[room.x, room.y + room.height - 1] = 2;
            map[room.x + room.width - 1, room.y + room.height - 2] = 5;
            map[room.x + room.width - 1, room.y + room.height - 1] = 4;
        }
        foreach (Door door in doors)
        {
            //Horizontal doors
            if (door.orientation == 0)
            {
                map[door.x - 1, door.y + 2] = 24;
                map[door.x - 1, door.y + 1] = 25;
                map[door.x - 1, door.y] = 1;
                map[door.x - 1, door.y - 1] = 28;
                map[door.x, door.y - 1] = 30;
                map[door.x, door.y] = 1;
                map[door.x, door.y + 1] = 10;
                map[door.x, door.y + 2] = 11;
                map[door.x + 1, door.y + 2] = 26;
                map[door.x + 1, door.y + 1] = 27;
                map[door.x + 1, door.y] = 1;
                map[door.x + 1, door.y - 1] = 29;
            }
            else
            {
                //Vertical door layout:
                // 17 1 19
                // 16 1 18
                // 13 1 12
                // 21 1 23
                // 20 1 22
                //TODO: Set up longer corridors when min-distance != 1
                map[door.x - 1, door.y - 2] = 17;
                map[door.x, door.y - 2] = 1;
                map[door.x + 1, door.y - 2] = 19;
                map[door.x - 1, door.y - 1] = 16;
                map[door.x, door.y - 1] = 1;
                map[door.x + 1, door.y - 1] = 18;
                map[door.x - 1, door.y] = 13;
                map[door.x, door.y] = 1;
                map[door.x + 1, door.y] = 12;
                map[door.x - 1, door.y + 1] = 21;
                map[door.x, door.y + 1] = 1;
                map[door.x + 1, door.y + 1] = 23;
                map[door.x - 1, door.y + 2] = 20;
                map[door.x, door.y + 2] = 1;
                map[door.x + 1, door.y + 2] = 22;
            }
        }
        //clear room[0] entities; it's the start room.
        map[rooms[0].x + rooms[0].width / 2, rooms[0].y + rooms[0].height / 2] = -99;
        for (int i = 1; i < rooms.Count; ++i)
            for (int j = 0; j < rooms[i].entities.Length; ++j)
                map[rooms[i].entityX[j], rooms[i].entityY[j]] = -1 * (int)rooms[i].entities[j];
        //pick rooms for merchants[2-3]
        //merchant[2] placement:
        int temp = rand.Next(1, rooms.Count);
        //make sure it won't collide with any existing entities

        int x, y;
        bool colliding = false;
        //For each entity in the room
        while (true)
        {
            colliding = false;
            //randomly pick an x/y combo in the room
            x = rand.Next(rooms[temp].x + 3, rooms[temp].x + rooms[temp].width - 3);
            y = rand.Next(rooms[temp].y + 3, rooms[temp].y + rooms[temp].height - 3);
            //check if it collides with any existing entities
            for (int i = 0; i < rooms[temp].entities.Length; ++i)
            {
                if (rooms[temp].entityX[i] == x && rooms[temp].entityY[i] == y)
                {
                    colliding = true;
                    break;
                }
            }
            //if not, put merchant[2] there
            if (!colliding)
                break;
            //if so, restart
        }
        map[x, y] = ((int)Entity.Curse + 1) * -1;
        
        temp = rand.Next(1, rooms.Count);

        while (true)
        {
            colliding = false;
            //randomly pick an x/y combo in the room
            x = rand.Next(rooms[temp].x + 3, rooms[temp].x + rooms[temp].width - 3);
            y = rand.Next(rooms[temp].y + 3, rooms[temp].y + rooms[temp].height - 3);
            //check if it collides with any existing entities
            for (int i = 0; i < rooms[temp].entities.Length; ++i)
            {
                if (rooms[temp].entityX[i] == x && rooms[temp].entityY[i] == y)
                {
                    colliding = true;
                    break;
                }
            }
            //if not, put merchant[3] there
            if (!colliding)
                break;
            //if so, restart
        }
        map[x, y] = ((int)Entity.Reward + 1) * -1;

        //now find the room whose center is furthest from the start room's center
        int bossRoom = 0;
        float bestDist = 0;
        float roomDist = 0;
       
        for (int i = 1; i < rooms.Count; ++i)
        {
            if (bestDist == 0 || (roomDist = Vector3.Distance(new Vector3(rooms[0].x, rooms[0].y, 0), new Vector3(rooms[i].x, rooms[i].y, 0))) > bestDist)
            {
                bestDist = roomDist;
                bossRoom = i;
            }
        }

        //now bossRoom is the room that's furthest from the starting point
        //put the boss gateway in it
        temp = bossRoom;
        while (true)
        {
            colliding = false;
            //randomly pick an x/y combo in the room
            x = rand.Next(rooms[temp].x + 2, rooms[temp].x + rooms[temp].width - 2);
            y = rand.Next(rooms[temp].y + 2, rooms[temp].y + rooms[temp].height - 2);
            //check if it collides with any existing entities
            for (int i = 0; i < rooms[temp].entities.Length; ++i)
            {
                if (rooms[temp].entityX[i] == x && rooms[temp].entityY[i] == y)
                {
                    colliding = true;
                    break;
                }
            }
            //if not, put boss gateway there
            if (!colliding)
                break;
            //if so, restart
        }
        map[x, y] = (int)Entity.Gateway * -1 - 1;

        return map;
    }

    /*
    * SeedRoom -> sets the seed
    * SizeRoom -> sets the size (based on seed)
    * PlaceRoom -> attempts to place the room; cannot be within X tiles of any other room; returns whether it succeeded (true/false)
    * FillRoom -> adds stuff to the room (enemies and rewards) based on its seed
    * PlaceEntities -> positions all the entities within the room
    */

    //Sets the seed of a room to a value between 0 and 1
    void SeedRoom(ref Room room)
    {
        room.seed = rand.NextDouble();
    }

    //Sets the size of a room based on its seed
    void SizeRoom(ref Room room, int min, int max)
    {
        room.width = min + (int)(rand.Next(0, max - min) * room.seed);
        room.height = min + (int)(rand.Next(0, max - min) * room.seed);
    }

    //Attempts to place a room (without overlapping) X times; returns whether it succeded
    bool PlaceRoom(ref Room room, ref List<Room> rooms, int MAP_WIDTH, int MAP_HEIGHT, int maxAttempts)
    {
        int failCount = 0, x, y, side;
        Room connector;
        bool colliding;
        //if this is the first room, pick a random spot for it
        //note - this should be the starting room!
        if (rooms.Count == 0)
        {
            room.x = rand.Next(0, MAP_WIDTH - room.width);
            room.y = rand.Next(0, MAP_HEIGHT - room.height);
            return true;
        }
        while (failCount < maxAttempts)
        {
            x = y = 0;
            colliding = false;
            /*
                * Steps:
                * 1. Pick a room
                * 2. Pick a side of that room
                * 3. Add a room that's 1 square away from that side and overlapping at least X squares (so we can have a reasonable door/corners in both rooms)
                * 4. Check if that room collides with any others
                * 5. If it doesn't collide with anything, add it to the list of rooms
                * 6. If it does, restart the process (or return false if we're out of attempts)
                */
            connector = rooms[rand.Next(0, rooms.Count)];
            side = rand.Next(0, 4); //0 = Up, 1 = Right, 2 = Down, 3 = Left
                                    //set the connected coordinate; if it puts the room out of map bounds, throw it out and start over 
            switch (side)
            {
                case 0:
                    y = connector.y - room.height - MIN_DISTANCE;
                    x = rand.Next(Math.Min(connector.x - room.width + ROOM_OVERLAP + 1, connector.x + connector.width - ROOM_OVERLAP - 1), Math.Max(connector.x - room.width + ROOM_OVERLAP + 1, connector.x + connector.width - ROOM_OVERLAP - 1));
                    break;
                case 1:
                    x = connector.x + connector.width + MIN_DISTANCE;
                    y = rand.Next(Math.Min(connector.y - room.height + ROOM_OVERLAP + 1, connector.y + connector.height - ROOM_OVERLAP - 1), Math.Max(connector.y - room.height + ROOM_OVERLAP + 1, connector.y + connector.height - ROOM_OVERLAP - 1));
                    break;
                case 2:
                    y = connector.y + connector.height + MIN_DISTANCE;
                    x = rand.Next(Math.Min(connector.x - room.width + ROOM_OVERLAP + 1, connector.x + connector.width - ROOM_OVERLAP - 1), Math.Max(connector.x - room.width + ROOM_OVERLAP + 1, connector.x + connector.width - ROOM_OVERLAP - 1));
                    break;
                case 3:
                    x = connector.x - room.width - MIN_DISTANCE;
                    y = rand.Next(Math.Min(connector.y - room.height + ROOM_OVERLAP + 1, connector.y + connector.height - ROOM_OVERLAP - 1), Math.Max(connector.y - room.height + ROOM_OVERLAP + 1, connector.y + connector.height - ROOM_OVERLAP - 1));
                    break;
            }
            //if the room is outside map bounds, throw it out
            if (x < 0 || x + room.width > MAP_WIDTH || y < 0 || y + room.height > MAP_HEIGHT)
            {
                failCount++;
                continue;
            }
            foreach (Room other in rooms)
            {
                //collision process:
                //R1 LEFT > R2.RIGHT ||
                //R1 RIGHT < R2.LEFT ||
                //R1 BOTTOM < R2.TOP ||
                //R1 TOP > R2.BOTTOM ||
                //if any of these is true, there's no collision and we continue
                //if all of them are false, set the collision flag and stop evaluating
                //The new room also has to be exactly 1 square away from at least 1 other connecting room
                if (!(x > other.x + other.width ||
                        x + room.width < other.x ||
                        y > other.y + other.height ||
                        y + room.height < other.y))
                {
                    colliding = true;
                    break;
                }

            }
            if (!colliding)
            {
                room.x = x;
                room.y = y;
                return true;
            }
            failCount++;
        }
        return false;
    }

    //Determines what's inside the room (how many and what they are)
    void FillRoom(ref Room room)
    {
        //First, determine how many things are in this room
        //Every room should have at least 1 thing in it
        //It should be a range with the min/max both dependent on the size of the room (e.g. min area^(1/4), max area^(1/2))
        //Then determine what each entity is - enemy (of each type) or reward. Note - this chance will be based on game difficulty
        //For now it uses preset test variables
        int area = room.width * room.height;
        int minEntities = (int)Math.Pow(area, MIN_POWER);
        int maxEntities = (int)Math.Pow(area, MAX_POWER);
        int entityCount = rand.Next(minEntities, maxEntities);
        double entitySeed;
        //entityCount says how many things are in the room
        //now decide what they are
        room.entities = new Entity[entityCount];
        for (int i = 0; i < entityCount; ++i)
        {
            //Generate a random number between 0 and 1
            entitySeed = rand.NextDouble();
            //Convert that seed to an entity type
            if (entitySeed < CHASER_MAX)
                room.entities[i] = Entity.Chaser + 1;
            else if (entitySeed < SHOOTER_MAX)
                room.entities[i] = Entity.Shooter + 1;
            else if (entitySeed < MINER_MAX)
                room.entities[i] = Entity.Miner + 1;
            else if (entitySeed < CHARGER_MAX)
                room.entities[i] = Entity.Charger + 1;
            else
                room.entities[i] = Entity.Heal + 1;

        }
        //Result: room.entities is an array that says what's in the room
    }

    //Chooses where to put each of the entities
    //For now this will be random as long as they don't overlap
    //Later on they should be more coordinated (e.g. shooters are near the edges, CHASER enemies are between you and rewards/curses)
    void PlaceEntities(ref Room room)
    {
        //Initialize the entity coordinate arrays
        room.entityX = new int[room.entities.Length];
        room.entityY = new int[room.entities.Length];
        int x, y;
        bool colliding = false;
        //For each entity in the room
        for (int i = 0; i < room.entities.Length; ++i)
        {
            while (true)
            {
                colliding = false;
                x = rand.Next(room.x + 2, room.x + room.width - 2);
                y = rand.Next(room.y + 2, room.y + room.height - 2);
                for (int j = 0; j < i; ++j)
                {
                    if (room.entityX[j] == x && room.entityY[j] == y)
                    {
                        colliding = true;
                        break;
                    }
                }
                if (!colliding)
                    break;
            }
            room.entityX[i] = x;
            room.entityY[i] = y;
        }
        //Result: room.entityX and room.entityY have the position of each entity in the room, and none of them overlap
    }

    //Connects this room to the all adjacent rooms (i.e. the spanning tree) via doorways
    void ConnectAdjacent(ref Room room, ref List<Room> rooms, ref List<Door> doors)
    {
        Door next = new Door();
        int overlap;
        //for every existing room
        for (int i = 0; i < rooms.Count; ++i)
        {
            //if this room is properly X-aligned,
            if (room.x == rooms[i].x + rooms[i].width + MIN_DISTANCE || room.x + room.width + MIN_DISTANCE == rooms[i].x)
            {
                //if the overlap is sufficient,
                if ((overlap = Math.Min(room.y + room.height, rooms[i].y + rooms[i].height) - Math.Max(room.y, rooms[i].y)) > ROOM_OVERLAP)
                {
                    next.y = Math.Max(room.y, rooms[i].y) + overlap / 2;
                    if (room.x > rooms[i].x)
                        next.x = room.x - 1;
                    else
                        next.x = rooms[i].x - 1;
                    next.orientation = 0;
                    doors.Add(next);
                }
            }
            //If this room is properly Y-aligned,
            if (room.y == rooms[i].y + rooms[i].height + MIN_DISTANCE || room.y + room.height + MIN_DISTANCE == rooms[i].y)
            {
                //if the overlap is sufficient,
                if ((overlap = Math.Min(room.x + room.width, rooms[i].x + rooms[i].width) - Math.Max(room.x, rooms[i].x)) > ROOM_OVERLAP)
                {
                    next.x = Math.Max(room.x, rooms[i].x) + overlap / 2;
                    if (room.y > rooms[i].y)
                        next.y = room.y - 1;
                    else
                        next.y = rooms[i].y - 1;
                    next.orientation = 1;
                    doors.Add(next);
                }
            }
        }
    }
}