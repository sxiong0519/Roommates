using System;
using System.Collections.Generic;
using Roommates.Models;
using Roommates.Repositories;
using System.Linq;

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
                    case ("Update a room"):
                        List<Room> roomOptions = roomRepo.GetAll();
                        foreach (Room r in roomOptions)
                        {
                            Console.WriteLine($"{r.Id} - {r.Name} Max Occupancy({r.MaxOccupancy})");
                        }

                        Console.Write("Which room would you like to update? ");
                        int selectedRoomId = int.Parse(Console.ReadLine());
                        Room selectedRoom = roomOptions.FirstOrDefault(r => r.Id == selectedRoomId);

                        Console.Write("New Name: ");
                        selectedRoom.Name = Console.ReadLine();

                        Console.Write("New Max Occupancy: ");
                        selectedRoom.MaxOccupancy = int.Parse(Console.ReadLine());

                        roomRepo.Update(selectedRoom);

                        Console.WriteLine("Room has been successfully updated");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Remove a room"):
                        List<Room> deleteRoom = roomRepo.GetAll();
                        foreach (Room d in deleteRoom)
                        {
                            Console.WriteLine($"{d.Id} - {d.Name} Max Occupancy({d.MaxOccupancy})");
                        }

                        Console.Write("Which room would you like remove? ");
                        int selectRoomId = int.Parse(Console.ReadLine());

                        roomRepo.Delete(selectRoomId);

                        Console.WriteLine("Room has been successfully updated");
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
                        Console.WriteLine($"Added {choreToAdd.Id}: {choreToAdd.Name}");
                        Console.WriteLine("Press any key to continue.");
                        Console.ReadLine();
                        break;
                    case ("See all unassigned chores"):
                        List<Chore> chor = choreRepo.GetUnassignedChore();
                        foreach (Chore c in chor)
                        {
                            Console.WriteLine($"{c.Name} is unassigned.");
                        }
                        Console.WriteLine("Press any key to continue.");
                        Console.ReadLine();
                        break;
                    case ("Assign roommate to chore"):
                        List<Chore> ch = choreRepo.GetAll();
                        foreach (Chore cho in ch)
                        {
                            Console.WriteLine($"{cho.Id}: {cho.Name}");
                        }
                        Console.Write("Pick a chore: ");
                        int choId = int.Parse(Console.ReadLine());

                        List<Roommate> rm = roommateRepo.GetAll();
                        foreach (Roommate rM in rm)
                        {
                            Console.WriteLine($"{rM.Id}: {rM.FirstName} {rM.LastName}");
                        }
                        Console.Write("Assign the roommate: ");
                        int rMId = int.Parse(Console.ReadLine());

                        choreRepo.AssignChore(choId, rMId);
                        Console.WriteLine($"Assignment successful.");
                        Console.WriteLine("Press any key to continue.");
                        Console.ReadLine();
                        break;
                    case ("Update a chore"):
                        List<Chore> choreOptions = choreRepo.GetAll();
                        foreach (Chore c in choreOptions)
                        {
                            Console.WriteLine($"{c.Id} - {c.Name}");
                        }

                        Console.Write("Which chore would you like to update? ");
                        int selectedChoreId = int.Parse(Console.ReadLine());
                        Chore selectedChore = choreOptions.FirstOrDefault(c => c.Id == selectedChoreId);

                        Console.Write("New Name: ");
                        selectedChore.Name = Console.ReadLine();

                        choreRepo.UpdateChore(selectedChore);

                        Console.WriteLine("Room has been successfully updated");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Remove a Chore"):
                        List<Chore> deleteChore = choreRepo.GetAll();
                        foreach (Chore d in deleteChore)
                        {
                            Console.WriteLine($"{d.Id} - {d.Name}");
                        }

                        Console.Write("Which chore would you like remove? ");
                        int selectChoreId = int.Parse(Console.ReadLine());

                        choreRepo.DeleteChore(selectChoreId);

                        Console.WriteLine("Room has been successfully updated");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("See chore count by Roommate"):
                        List<Chore> choo = choreRepo.GetChoreCounts();
                        var grp = choo.GroupBy(r => r.Roommate.FirstName).Select(g => new { FirstName = g.Key, Count = g.Count() }).ToList();
                        foreach (var gr in grp )
                        {
                            Console.WriteLine($"{gr.FirstName} has {gr.Count} chores assigned.");
                        }
                        Console.WriteLine("Press any key to continue.");
                        Console.ReadLine();
                        break;
                    case ("Reassign a chore"):
                        List<RoommateChore> assignedChore = choreRepo.GetAssignedChore();
                        foreach (RoommateChore a in assignedChore)
                        {
                            Console.WriteLine($"{a.Chore.Id} - {a.Chore.Name}");
                        }
                        Console.Write("Pick a chore you would like to reassign:");
                        int aId = int.Parse(Console.ReadLine());
                        foreach (RoommateChore a in assignedChore)
                        {
                            if (a.Chore.Id == aId)
                            {
                                Console.WriteLine($"The chore is assigned to {a.Roommate.FirstName}. Who would you like to reassign it to?");
                            }
                        }
                        List<Roommate> choreRoommates = roommateRepo.GetAll();
                        foreach (Roommate roommate in choreRoommates)
                        {
                            Console.WriteLine($"{roommate.Id} - {roommate.FirstName}");
                        }
                        int arId = int.Parse(Console.ReadLine());

                        choreRepo.ReAssignChore(aId, arId);

                        Console.WriteLine("Chore has been reassigned");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Search a roommate"):
                        Console.Write("Roommate id: ");
                        int rId = int.Parse(Console.ReadLine());

                        Roommate roo = roommateRepo.GetRoommateById(rId);

                        Console.WriteLine($"{roo.Id}: {roo.FirstName} {roo.LastName} is paying {roo.RentPortion} to stay in the {roo.Room.Name} starting on {roo.MovedInDate}.");
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
                "Update a room",
                "Remove a room",
                "See all chores",
                "Search for a chore",
                "Add a chore",
                "See all unassigned chores",
                "Assign roommate to chore",
                "Update a chore",
                "Remove a Chore",
                "See chore count by Roommate",
                "Reassign a chore",
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
