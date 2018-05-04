using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minefield
{
    class Program
    {
        static void Main(string[] args)
        {
            bool finishedInput = false;
            List<string> mineField = new List<string>();
            List<int[]> finishLocations = new List<int[]>();
            int fieldWidth = -1;
            int[] location = { -1, -1 };

            int i = 0;
            while (!finishedInput)
            {
                string line = Console.ReadLine();

                if ((line.Length == fieldWidth || i == 0) && line.Length > 2)
                {
                    fieldWidth = line.Length;
                    
                    mineField.Add(line);

                    if (mineField.Count > 1 && OnlyThisCharacter('+', mineField[i]))
                    {
                        finishedInput = true;
                    }

                    i++;
                }
                else
                {
                    EndGame("Needs to be the same field length and to be greater than 2.");
                }
            }

            bool robotFound = false;

            for (int row = 0; row < mineField.Count; row++)
            {
                for (int col = 0; col < fieldWidth; col++)
                {
                    char current = mineField[row][col];

                    if (current != '0' && current != '+' && current != 'M' && current != '*')
                    {
                        EndGame("Wrong characters.");
                    }

                    if (current == 'M')
                    {
                        if (robotFound)
                        {
                            EndGame("More than one robot.");
                        }

                        location = new int[] { row, col };

                        robotFound = true;
                    }
                }

                if (mineField[row][fieldWidth - 1] == '0')
                {
                    finishLocations.Add(new int[] { row, fieldWidth - 1 });
                } 
            }

            if (finishLocations.Count < 1)
            {
                EndGame("There are no solutions!");
            }

            OutputMineField(mineField);

            if (!robotFound)
            {
                EndGame("Robot not found.");
            }

            string instructions = Console.ReadLine();
            string finalInstructions = "";
            int startingIndex = -1;
            bool startFound = false;
            bool finishFound = false;

            for (int inCount = 0; inCount < instructions.Length; inCount++)
            {
                if (instructions[inCount] == 'I' && startFound == false)
                {
                    startingIndex = inCount;
                    finishFound = false;
                    startFound = true;
                }

                if (instructions[inCount] == '-' && startFound == true)
                {
                    finalInstructions = finalInstructions + instructions.Substring(startingIndex + 1, inCount - startingIndex);
                    finishFound = true;
                    startFound = false;
                }
            }

            if (!instructions.Contains("I"))
            {
                EndGame("You didn't start the engine!");
            }

            if (!finishFound)
            {
                EndGame("You didn't turn the engine off at the end!");
            }

            finalInstructions = finalInstructions.Replace("I", "");
            finalInstructions = finalInstructions.Replace("-", "");
            char move = ' ';

            foreach (char inst in finalInstructions)
            {
                move = ' ';

                switch (inst)
                {
                    case 'W':
                        try
                        {
                            move = mineField[location[0] - 1][location[1]];
                            if (move == '0')
                            {
                                mineField[location[0]] = mineField[location[0]].Replace("M", "0");
                                location[0] -= 1;
                            }
                        }
                        catch { }
                        break;
                    case 'S':
                        try
                        {
                            move = mineField[location[0] + 1][location[1]];
                            if (move == '0')
                            {
                                mineField[location[0]] = mineField[location[0]].Replace("M", "0");
                                location[0] += 1;
                            }
                        }
                        catch { }
                        break;
                    case 'D':
                        try
                        {
                            move = mineField[location[0]][location[1] + 1];
                            if (move == '0')
                            {
                                mineField[location[0]] = mineField[location[0]].Replace("M", "0");
                                location[1] += 1;
                            }
                        }
                        catch { }
                        break;
                    case 'A':
                        try
                        {
                            move = mineField[location[0]][location[1] - 1];
                            if (move == '0')
                            {
                                mineField[location[0]] = mineField[location[0]].Replace("M", "0");
                                location[1] -= 1;
                            }
                        }
                        catch { }
                        break;
                    default:
                        EndGame("Incorrect instructions.");
                        break;
                }

                if (move == '*')
                {
                    EndGame("You walked into a mine");
                }

                if (move == '0')
                {
                    StringBuilder sb = new StringBuilder(mineField[location[0]]);
                    sb[location[1]] = 'M';
                    mineField[location[0]] = sb.ToString();

                    OutputMineField(mineField);
                }

                System.Threading.Thread.Sleep(100);

                foreach (int[] finish in finishLocations)
                {
                    if (Enumerable.SequenceEqual(finish, location))
                    {
                        EndGame("You got to the exit! You Win!");
                    }
                }
            }

            EndGame("You didn't reach the exit...");
        }

        public static bool OnlyThisCharacter(char ch, string line)
        {
            foreach (char c in line)
            {
                if (c != ch)
                {
                    return false;
                }
            }

            return true;
        }

        public static void EndGame(string message)
        {
            Console.WriteLine(message);
            Console.Read();
            Environment.Exit(1);
        }

        public static void OutputMineField(List<string> mineField)
        {
            Console.Clear();

            for (int row = 0; row < mineField.Count; row++)
            {
                for (int col = 0; col < mineField[row].Length; col++)
                {
                    if (mineField[row][col] == '0')
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                    }
                    else if (mineField[row][col] == '+')
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    }
                    else if (mineField[row][col] == '*')
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    else if (mineField[row][col] == 'M')
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                    }

                    Console.Write(mineField[row][col]);
                    Console.ResetColor();
                }

                Console.WriteLine();
            }
        }
    }
}