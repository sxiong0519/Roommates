﻿using System;
using System.Collections.Generic;
using Roommates.Models;
using Roommates.Repositories;

namespace Roommates
{
    class Program
    {
        //  This is the address of the database.
        //  We define it here as a constant since it will never change.
        private const string CONNECTION_STRING = @"server=localhost\SQLExpress;database=Roommates;integrated security=true";

       

        static void Main(string[] args)
        { 
            RoomRepository roomRepo = new RoomRepository(CONNECTION_STRING);
            ChoreRepository choreRepo = new ChoreRepository(CONNECTION_STRING);
            RoommateRepository roommateRepo = new RoommateRepository(CONNECTION_STRING);

            bool runProgram = true;
            while (runProgram)
            {
                string selection = GetMenuSelection();

                switch (selection)
                {
                    case ("Show all rooms"):
                        List<Room> rooms = roomRepo.GetAll();
                        foreach (Room r in rooms)
                        {
                            Console.WriteLine($"{r.Name} has an Id of {r.Id} and a max occupancy of {r.MaxOccupancy}");
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Search for room"):
                        Console.Write("Room Id: ");
                        int id = int.Parse(Console.ReadLine());

                        Room room = roomRepo.GetById(id);

                        Console.WriteLine($"{room.Id} - {room.Name} Max Occupancy({room.MaxOccupancy})");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Add a room"):
                        Console.Write("Room name: ");
                        string name = Console.ReadLine();

                        Console.Write("Max occupancy: ");
                        int max = int.Parse(Console.ReadLine());

                        Room roomToAdd = new Room()
                        {
                            Name = name,
                            MaxOccupancy = max
                        };

                        roomRepo.Insert(roomToAdd);

                        Console.WriteLine($"{roomToAdd.Name} has been added and assigned an Id of {roomToAdd.Id}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("See all chores"):
                        List<Chore> chores = choreRepo.GetAll();
                        foreach (Chore c in chores)
                        {
                            Console.WriteLine($"{c.Name} has an Id of {c.Id}");
                        }
                        Console.WriteLine("Press any key to continue.");
                        Console.ReadLine();
                        break;
                    case ("Search for a chore"):
                        Console.Write("Chore id: ");
                        int cId = int.Parse(Console.ReadLine());

                        Chore chore = choreRepo.GetChoreById(cId);

                        Console.WriteLine($"{chore.Id}: {chore.Name}");
                        Console.WriteLine("Press any key to continue.");
                        Console.ReadLine();
                        break;
                    case ("Add a chore"):
                        Console.Write("Chore:");
                        string cName = Console.ReadLine();

                        Chore choreToAdd = new Chore()
                        {
                            Name = cName
                        };

                        choreRepo.Insert(choreToAdd);
                        Console.WriteLine($"{choreToAdd.Id}: {choreToAdd.Name}");
                        Console.WriteLine("Press any key to continue.");
                        Console.ReadLine();
                        break;
                    case ("Search a roommate"):
                        Console.Write("Roommate id: ");
                        int rId = int.Parse(Console.ReadLine());

                        Roommate roo = roommateRepo.GetRoommateById(rId);

                        Console.WriteLine($"{roo.Id}: {roo.FirstName} {roo.LastName}, {roo.RentPortion}, {roo.MovedInDate}, {roo.Room.Name}");
                        Console.WriteLine("Press any key to continue.");
                        Console.ReadLine();
                        break;
                    case ("Exit"):
                        runProgram = false;
                        break;
                }
            }


        }

        static string GetMenuSelection()
        {
            Console.Clear();

            List<string> options = new List<string>()
            {
                "Show all rooms",
                "Search for room",
                "Add a room",
                "See all chores",
                "Search for a chore",
                "Add a chore",
                "Search a roommate",
                "Exit"
            };

            for (int i = 0; i < options.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {options[i]}");
            }

            while (true)
            {
                try
                {
                    Console.WriteLine();
                    Console.Write("Select an option > ");

                    string input = Console.ReadLine();
                    int index = int.Parse(input) - 1;
                    return options[index];
                }
                catch (Exception)
                {

                    continue;
                }
            }
        }
    }
}
