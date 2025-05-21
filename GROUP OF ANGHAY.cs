using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking_Lot_Management_System
{
    internal class GROUP_OF_ANGHAY
    {
        class ParkingSlot
        {
            public string LicensePlate { get; set; }
            public DateTime? ParkedTime { get; set; }
            public bool IsOccupied => LicensePlate != null;
        }

        class ParkingLot
        {
            private ParkingSlot[,] slots;

            public ParkingLot(int rows, int cols)
            {
                slots = new ParkingSlot[rows, cols];
                for (int r = 0; r < rows; r++)
                    for (int c = 0; c < cols; c++)
                        slots[r, c] = new ParkingSlot();
            }

            public bool ParkCar(string licensePlate, int row, int col)
            {
                if (HasCarWithPlate(licensePlate))
                    return false;

                if (row < 0 || row >= slots.GetLength(0) || col < 0 || col >= slots.GetLength(1))
                    return false;

                if (slots[row, col].IsOccupied)
                    return false;

                slots[row, col].LicensePlate = licensePlate;
                slots[row, col].ParkedTime = DateTime.Now;
                return true;
            }

            public bool IsFull()
            {
                for (int r = 0; r < slots.GetLength(0); r++)
                    for (int c = 0; c < slots.GetLength(1); c++)
                        if (!slots[r, c].IsOccupied)
                            return false;
                return true;
            }

            public bool HasCarWithPlate(string licensePlate)
            {
                for (int r = 0; r < slots.GetLength(0); r++)
                    for (int c = 0; c < slots.GetLength(1); c++)
                        if (slots[r, c].IsOccupied && slots[r, c].LicensePlate == licensePlate)
                            return true;
                return false;
            }

            public bool IsSlotOccupied(int row, int col)
            {
                return slots[row, col].IsOccupied;
            }

            public bool IsSlotValid(int row, int col)
            {
                return row >= 0 && row < slots.GetLength(0) && col >= 0 && col < slots.GetLength(1);
            }

            public bool RemoveCar(int row, int col)
            {
                if (!IsSlotValid(row, col) || !slots[row, col].IsOccupied)
                    return false;

                slots[row, col] = new ParkingSlot();
                return true;
            }

            public void DisplayStatus()
            {
                for (int r = 0; r < slots.GetLength(0); r++)
                {
                    for (int c = 0; c < slots.GetLength(1); c++)
                        Console.Write(slots[r, c].IsOccupied ? "[X] " : "[ ] ");
                    Console.WriteLine();
                }
            }

            public void ShowRecords()
            {
                Console.WriteLine("\nParked Cars Records:");
                for (int r = 0; r < slots.GetLength(0); r++)
                    for (int c = 0; c < slots.GetLength(1); c++)
                        if (slots[r, c].IsOccupied)
                            Console.WriteLine($"License Plate: {slots[r, c].LicensePlate}, Parked Time: {slots[r, c].ParkedTime}");
            }
        }

        class Program
        {
            static void Main()
            {
                int rows = 3;
                int cols = 5;

                ParkingLot lot = new ParkingLot(rows, cols);

                Console.WriteLine("\nInitial Parking Lot Status:");
                lot.DisplayStatus();

                while (true)
                {
                    Console.WriteLine("\n1. Park Car\n2. Remove Car\n3. Show Status\n4. Show Records\n5. Exit");
                    Console.Write("Enter Choice: ");
                    string choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":

                            if (lot.IsFull())
                            {
                                Console.WriteLine("Parking lot is full!");
                                break;
                            }

                            string plate;
                            do
                            {
                                Console.Write("License Plate: ");
                                plate = Console.ReadLine();

                                if (lot.HasCarWithPlate(plate))
                                {
                                    Console.WriteLine("This car is already parked. Please enter a different plate number.");
                                }
                                else
                                {
                                    break;
                                }
                            } while (true);

                            int row, col;
                            while (true)
                            {
                                Console.Write("Row: ");
                                if (!int.TryParse(Console.ReadLine(), out row))
                                {
                                    Console.WriteLine("Invalid input. Please enter a number.");
                                    continue;
                                }

                                Console.Write("Column: ");
                                if (!int.TryParse(Console.ReadLine(), out col))
                                {
                                    Console.WriteLine("Invalid input. Please enter a number.");
                                    continue;
                                }

                                if (!lot.IsSlotValid(row, col))
                                {
                                    Console.WriteLine("Invalid slot: row or column is out of bounds.");
                                    continue;
                                }

                                if (lot.IsSlotOccupied(row, col))
                                {
                                    Console.WriteLine("Slot already occupied. Please choose another.");
                                    continue;
                                }

                                if (lot.ParkCar(plate, row, col))
                                {
                                    Console.WriteLine("Car parked.");
                                    break;
                                }
                            }
                            break;

                        case "2":
                            Console.Write("Row: ");
                            int remRow = int.Parse(Console.ReadLine());
                            Console.Write("Column: ");
                            int remCol = int.Parse(Console.ReadLine());
                            if (lot.RemoveCar(remRow, remCol))
                                Console.WriteLine("Car removed.");
                            else
                                Console.WriteLine("Invalid slot or empty.");
                            break;

                        case "3":
                            lot.DisplayStatus();
                            break;

                        case "4":
                            lot.ShowRecords();
                            break;

                        case "5":
                            return;

                        default:
                            Console.WriteLine("Invalid choice.");
                            break;
                    }
                }
            }
        }
    }
}

