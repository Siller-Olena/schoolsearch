using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schoolsearch
{
    internal class Program
    {
        public static school myschool;

        static void ShowMainMenu()
        {
            ConsoleKeyInfo result;
            Console.Clear();
            Console.Clear();
            Console.WriteLine("         ╔══════════════════════════════════════════════════════════╗");
            Console.WriteLine("         ║                         MAIN MENU                        ║");
            Console.WriteLine("         ╠══════════════════════════════════════════════════════════╣");
            Console.WriteLine("         ║           T - Search teacher by lastname                 ║");
            Console.WriteLine("         ║           S - Search student by lastname                 ║");
            Console.WriteLine("         ║           U - Search student by lastname and bus         ║");
            Console.WriteLine("         ║           C - Search classroom (students & teachers)     ║");
            Console.WriteLine("         ║           B - Search bus - show only students            ║");
            Console.WriteLine("         ╠══════════════════════════════════════════════════════════╣");
            Console.WriteLine("         ║           Q - Quit application                           ║");
            Console.WriteLine("         ╚══════════════════════════════════════════════════════════╝");
            Console.WriteLine();
            Console.Write("Select an option (T, S, U, C, B, Q):");
            while (true)
            {
                result = Console.ReadKey();
                if (result.KeyChar == 'T' || result.KeyChar == 't' || result.KeyChar == 'S' ||
                    result.KeyChar == 's' || result.KeyChar == 'U' || result.KeyChar == 'u' ||
                    result.KeyChar == 'C' || result.KeyChar == 'c' || result.KeyChar == 'B' ||
                    result.KeyChar == 'b' || result.KeyChar == 'Q' || result.KeyChar == 'q')
                    break;
            }
            Console.WriteLine();

            switch (result.KeyChar)
            {               
                case 'T':
                case 't':
                    ShowSearchTeacher();
                    ShowMainMenu();
                    break;

                case 'S':
                case 's':
                    ShowSearchStudent();
                    ShowMainMenu();
                    break;

                case 'U':
                case 'u':
                    ShowSearchStudentAndBus();
                    ShowMainMenu();
                    break;

                case 'C':
                case 'c':
                    ShowSearchClassroom();
                    ShowMainMenu();
                    break;

                case 'B':
                case 'b':
                    ShowSearchBus();
                    ShowMainMenu();
                    break;

                case 'Q':
                case 'q':
                    {
                        Console.WriteLine("Do you really want to exit the application? (Y/N)");
                        while (true)
                        {
                            ConsoleKeyInfo result1 = Console.ReadKey();
                            if (result1.KeyChar == 'y' || result1.KeyChar == 'Y' ||
                                result1.KeyChar == 'n' || result1.KeyChar == 'N')
                            {
                                if (result1.KeyChar == 'y' || result1.KeyChar == 'Y')
                                    Environment.Exit(0);
                                else
                                {
                                    ShowMainMenu();
                                    break;
                                }
                            }
                        }
                    }
                    break;
            }
        }
        static void ShowSearchTeacher()
        {
            string lastname = "";
            string data = "";
            int RecordCount = 0;

            Console.Clear();
            Console.WriteLine("         ╔══════════════════════════════════════════════════════════╗");
            Console.WriteLine("         ║                 SEARCH TEACHER BY LASTNAME               ║");
            Console.WriteLine("         ╚══════════════════════════════════════════════════════════╝");
            Console.WriteLine();
            Console.WriteLine("If the lastname is empty, all data will be shown!");
            Console.WriteLine();
            Console.Write("Last name: ");
            lastname = Console.ReadLine();
            Console.WriteLine();

            var timer = new System.Diagnostics.Stopwatch();
            timer.Start();
            myschool.GetFilteredData(ref data, FilterType.FT_Teacher, ref RecordCount, lastname);
            timer.Stop();

            Console.WriteLine(data);

            if (RecordCount > 0)
                Console.WriteLine("╠════════════╩═════════════════════════╩═════════════════════════╩════════════╩════════╩════════╣");
            else
                Console.WriteLine("╔═══════════════════════════════════════════════════════════════════════════════════════════════╗");

            Console.WriteLine("║             Elapsed time: {0,9:##0.00000} ms                   Record count: {1,5:###0}                  ║",
                timer.Elapsed.TotalMilliseconds, RecordCount);
            Console.WriteLine("╚═══════════════════════════════════════════════════════════════════════════════════════════════╝");
            Console.WriteLine();
            Console.WriteLine("     ----------------------------- Press any key to continue! ------------------------------");
            Console.ReadKey();
        }

        static void ShowSearchStudent()
        {
            string lastname;
            string data = "";
            int RecordCount = 0;

            Console.Clear();
            Console.WriteLine("         ╔══════════════════════════════════════════════════════════╗");
            Console.WriteLine("         ║                 SEARCH STUDENT BY LASTNAME               ║");
            Console.WriteLine("         ╚══════════════════════════════════════════════════════════╝");
            Console.WriteLine();
            Console.WriteLine("If the lastname is empty, all data will be shown!");
            Console.WriteLine();
            Console.Write("Last name: ");
            lastname = Console.ReadLine();

            var timer = new System.Diagnostics.Stopwatch();
            timer.Start();
            myschool.GetFilteredData(ref data, FilterType.FT_Student, ref RecordCount, lastname);
            timer.Stop();

            Console.WriteLine(data);

            if (RecordCount > 0)
                Console.WriteLine("╠════════════╩═════════════════════════╩═════════════════════════╩════════════╩════════╩════════╣");
            else
                Console.WriteLine("╔═══════════════════════════════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║             Elapsed time: {0,9:##0.00000} ms                   Record count: {1,5:###0}                  ║",
                timer.Elapsed.TotalMilliseconds, RecordCount);
            Console.WriteLine("╚═══════════════════════════════════════════════════════════════════════════════════════════════╝");
            Console.WriteLine();
            Console.WriteLine("     ----------------------------- Press any key to continue! ------------------------------");
            Console.ReadKey();
        }

        static void ShowSearchStudentAndBus()
        {
            string lastname, tmpValue;
            int bus;
            string data = "";
            int RecordCount = 0;

            Console.Clear();
            Console.WriteLine("         ╔══════════════════════════════════════════════════════════╗");
            Console.WriteLine("         ║             SEARCH STUDENT BY LASTNAME AND BUS           ║");
            Console.WriteLine("         ╚══════════════════════════════════════════════════════════╝");
            Console.WriteLine();
            Console.WriteLine("If the lastname is empty, no data will be shown!");
            Console.WriteLine();
            Console.Write("Last name: ");
            lastname = Console.ReadLine();
            while (true)
            {
                Console.Write("Bus: ");
                tmpValue = Console.ReadLine();
                if (int.TryParse(tmpValue, out bus))
                    if (bus >= 0)
                        break;
                Console.CursorTop = 8;
                Console.CursorLeft = 5;
                Console.Write("       ");
                Console.CursorTop = 8;
                Console.CursorLeft = 5;
            }

            var timer = new System.Diagnostics.Stopwatch();
            timer.Start();
            myschool.GetFilteredData(ref data, FilterType.FT_StudentBus, ref RecordCount, lastname, bus);
            timer.Stop();

            Console.WriteLine(data);

            if (RecordCount > 0)
                Console.WriteLine("╠════════════╩═════════════════════════╩═════════════════════════╩════════════╩════════╩════════╣");
            else
                Console.WriteLine("╔═══════════════════════════════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║             Elapsed time: {0,9:##0.00000} ms                   Record count: {1,5:###0}                  ║",
                timer.Elapsed.TotalMilliseconds, RecordCount);
            Console.WriteLine("╚═══════════════════════════════════════════════════════════════════════════════════════════════╝");
            Console.WriteLine();
            Console.WriteLine("     ----------------------------- Press any key to continue! ------------------------------");
            Console.ReadKey();
        }

        static void ShowSearchClassroom()
        {
            string tmpValue;
            int classroom;
            string data = "";
            int RecordCount = 0;

            Console.Clear();
            Console.WriteLine("         ╔══════════════════════════════════════════════════════════╗");
            Console.WriteLine("         ║                 SEARCH PERSON BY CLASSROOM               ║");
            Console.WriteLine("         ╚══════════════════════════════════════════════════════════╝");
            Console.WriteLine();
            Console.WriteLine();
            while (true)
            {
                Console.Write("Classroom: ");
                tmpValue = Console.ReadLine();
                if (int.TryParse(tmpValue, out classroom))
                    if (classroom >= 0)
                        break;
                Console.CursorTop = 7;
                Console.CursorLeft = 11;
                Console.Write("       ");
                Console.CursorTop = 7;
                Console.CursorLeft = 11;
            }

            var timer = new System.Diagnostics.Stopwatch();
            timer.Start();
            myschool.GetFilteredData(ref data, FilterType.FT_Classroom, ref RecordCount, "", classroom);
            timer.Stop();

            Console.WriteLine(data);

            if (RecordCount > 0)
                Console.WriteLine("╠════════════╩═════════════════════════╩═════════════════════════╩════════════╩════════╩════════╣");
            else
                Console.WriteLine("╔═══════════════════════════════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║             Elapsed time: {0,9:##0.00000} ms                   Record count: {1,5:###0}                  ║",
                timer.Elapsed.TotalMilliseconds, RecordCount);
            Console.WriteLine("╚═══════════════════════════════════════════════════════════════════════════════════════════════╝");
            Console.WriteLine();
            Console.WriteLine("     ----------------------------- Press any key to continue! ------------------------------");
            Console.ReadKey();
        }

        static void ShowSearchBus()
        {
            string tmpValue;
            int bus;
            string data = "";
            int RecordCount = 0;

            Console.Clear();
            Console.WriteLine("         ╔══════════════════════════════════════════════════════════╗");
            Console.WriteLine("         ║                    SEARCH STUDENT BY BUS                 ║");
            Console.WriteLine("         ╚══════════════════════════════════════════════════════════╝");
            Console.WriteLine();
            Console.WriteLine();
            while (true)
            {
                Console.Write("Bus: ");
                tmpValue = Console.ReadLine();
                if (int.TryParse(tmpValue, out bus))
                    if (bus >= 0)
                        break;
                Console.CursorTop = 6;
                Console.CursorLeft = 5;
                Console.Write("       ");
                Console.CursorTop = 6;
                Console.CursorLeft = 5;
            }

            var timer = new System.Diagnostics.Stopwatch();
            timer.Start();
            myschool.GetFilteredData(ref data, FilterType.FT_Bus, ref RecordCount, "", bus);
            timer.Stop();

            Console.WriteLine(data);

            if (RecordCount > 0)
                Console.WriteLine("╠════════════╩═════════════════════════╩═════════════════════════╩════════════╩════════╩════════╣");
            else
                Console.WriteLine("╔═══════════════════════════════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║             Elapsed time: {0,9:##0.00000} ms                   Record count: {1,5:###0}                  ║",
                timer.Elapsed.TotalMilliseconds, RecordCount);
            Console.WriteLine("╚═══════════════════════════════════════════════════════════════════════════════════════════════╝");
            Console.WriteLine();
            Console.WriteLine("     ----------------------------- Press any key to continue! ------------------------------");
            Console.ReadKey();
        }

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.Title = "Schoolsearch";

            Console.WindowWidth = 120;
            Console.WindowHeight = 40;

            Console.Clear();
            Console.WriteLine("Application is trying to load the data from file...");
            Console.WriteLine();

            myschool = new school();
            if (myschool.ReadSchoolData())
                ShowMainMenu();
            else
            {
                Console.WriteLine("No data file \"students.txt\" found! Application will terminate now!");
                Console.WriteLine("---- Press any key to continue! -----");
                Console.ReadKey();
            }
        }
    }
}
