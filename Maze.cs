//   Copyright 2019 | Vladimir Simonyan | simonyan.vlad@gmail.com   // 



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze
{
    class Maze
    {
        //===[Maze]===
        private const int MAX_HEIGHT = 200;
        private const int MAX_WIDTH = 200;
        private int[,] maze = new int[MAX_HEIGHT, MAX_WIDTH];
        private int mazeHeight, mazeWidth, intelligenceVector;
        private int x, y;
        private int[] temp = new int[4];


        public void createMaze(int width, int height)
        {
            if (width > MAX_WIDTH || height > MAX_HEIGHT || width < 5 || height < 5) throw new IndexOutOfRangeException();

            mazeHeight = height;
            mazeWidth = width;
            intelligenceVector = 1;
            createMaze();
        }

        public void createMaze(int width, int height, int vector)
        {
            if (width > 100 || height > 100 || width < 5 || height < 5) throw new IndexOutOfRangeException();

            mazeHeight = height;
            mazeWidth = width;
            intelligenceVector = vector;
            createMaze();
        }

        private void createMaze()
        {
            createWalls();
            createRoads();
            markTheWay();
        }

        private void createWalls()
        {
            for (int i = 0; i < mazeHeight; i++)
                for (int j = 0; j < mazeWidth; j++)
                    maze[i, j] = 1;
        }

        private void createRoads()
        {
            resetStack();
            temp[0] = temp[3] = 1;
            temp[1] = temp[2] = 0;
            x = 1; y = 1;
            Random rand = new Random();
            int stepDirection;

            while (true)
            {
                if (x == -1 || y == -1) break;

                stepDirection = (rand.Next(45 * intelligenceVector++)) % 4;

                // Double pass protection
                if (temp[stepDirection] == 0)
                {
                    // Step back if all roads are blocked
                    if (temp[0] == 0 && temp[1] == 0 && temp[2] == 0 && temp[3] == 0) pop();
                    continue;
                }

                // Step in *stepDirection*
                if (canIDoTwoSteps(stepDirection)) crashTwoBlocks(stepDirection);
                else if (canIDoOneStep(stepDirection)) crashOneBlock(stepDirection);
                else temp[stepDirection] = 0;
            }
        }

        private void markTheWay()
        {
            Random rand = new Random();

            // Creatig start and exit positions
            int startX, startY, finishX, finishY;
            startX = 0;
            do
            {
                startY = (rand.Next(45 * intelligenceVector++)) % mazeWidth;
            } while (maze[startX + 1, startY] == 1);

            finishX = mazeHeight - 1;
            do
            {
                finishY = (rand.Next(45 * intelligenceVector++)) % mazeWidth;
            } while (maze[finishX - 1, finishY] == 1);

            maze[startX, startY] = maze[finishX, finishY] = 2;
            

            // Maze passage through the bot
            MazeBot bot = new MazeBot();
            x = startX; y = startY;
            int stepDirection = bot.stepBot(0, 1, 1, 1);
            while (stepDirection != -1)
            {
                switch (stepDirection)
                {
                    case 0:
                        if (bot.forward) maze[x, y] = 2;
                        else maze[x, y] = 0;
                        x += 1;
                        break;
                    case 1:
                        if (bot.forward) maze[x, y] = 2;
                        else maze[x, y] = 0;
                        y -= 1;
                        break;
                    case 2:
                        if (bot.forward) maze[x, y] = 2;
                        else maze[x, y] = 0;
                        x -= 1;
                        break;
                    case 3:
                        if (bot.forward) maze[x, y] = 2;
                        else maze[x, y] = 0;
                        y += 1;
                        break;
                }
                if (x == finishX && y == finishY) break;
                stepDirection = bot.stepBot(maze[x + 1, y], maze[x, y - 1], maze[x - 1, y], maze[x, y + 1]);
                
            }
        }

        public void printMaze()
        {
            for (int i = 0; i < mazeHeight; i++)
            {
                for (int j = 0; j < mazeWidth; j++)
                {
                    if (maze[i, j] == 1) System.Console.Write("#");
                    else if (maze[i, j] == 0) System.Console.Write(" ");
                    else if (maze[i, j] == 2)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        System.Console.Write(".");
                        Console.ResetColor();
                    }
                    else System.Console.Write("?");
                }
                System.Console.WriteLine();
            }
        }

        public void printMaze(char wall, char way, char rightWay)
        {
            for (int i = 0; i < mazeHeight; i++)
            {
                for (int j = 0; j < mazeWidth; j++)
                {
                    if (maze[i, j] == 1) System.Console.Write(wall);
                    else if (maze[i, j] == 0) System.Console.Write(way);
                    else if (maze[i, j] == 2)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        System.Console.Write(rightWay);
                        Console.ResetColor();
                    }
                    else System.Console.Write("?");
                }
                System.Console.WriteLine();
            }
        }
        //=======



        //===[STACK]===
        private int[,] stack = new int[5000, 6];
        private int top;


        private void resetStack()
        {
            stack[0, 4] = stack[0, 5] = -1;
            top = 1;
        }

        private void push()
        {
            for (int i = 0; i < 4; i++)
                stack[top, i] = temp[i];
            stack[top, 4] = x;
            stack[top, 5] = y;
            top++;
        }

        private void pop()
        {
            top--;
            for (int i = 0; i < 4; i++)
                temp[i] = stack[top, i];
            x = stack[top, 4];
            y = stack[top, 5];
        }
        //=======



        //===[CanIStep]===
        private bool canIDoTwoSteps(int direction)
        {
            int sum = 0;
            if (direction == 0)     // Step Down
            {
                if (x + 3 >= mazeHeight) return false;
                for (int i = 1; i <= 3; i++)
                {
                    sum += maze[x + i, y];
                    sum += maze[x + i, y - 1];
                    sum += maze[x + i, y + 1];
                }
                if (sum == 9) return true;
            }
            else if (direction == 1)     // Step Left
            {
                if (y - 3 < 0) return false;
                for (int i = 1; i <= 3; i++)
                {
                    sum += maze[x, y - i];
                    sum += maze[x - 1, y - i];
                    sum += maze[x + 1, y - i];
                }
                if (sum == 9) return true;
            }
            else if (direction == 2)     // Step Up
            {
                if (x - 3 < 0) return false;
                for (int i = 1; i <= 3; i++)
                {
                    sum += maze[x - i, y];
                    sum += maze[x - i, y - 1];
                    sum += maze[x - i, y + 1];
                }
                if (sum == 9) return true;
            }
            else if (direction == 3)     // Step Right
            {
                if (y + 3 >= mazeWidth) return false;
                for (int i = 1; i <= 3; i++)
                {
                    sum += maze[x, y + i];
                    sum += maze[x - 1, y + i];
                    sum += maze[x + 1, y + i];
                }
                if (sum == 9) return true;
            }
            return false;
        }

        private bool canIDoOneStep(int direction)
        {
            int sum = 0;
            if (direction == 0)     // Step Down
            {
                if (x + 2 >= mazeHeight) return false;
                for (int i = 1; i <= 2; i++)
                {
                    sum += maze[x + i, y];
                    sum += maze[x + i, y - 1];
                    sum += maze[x + i, y + 1];
                }
                if (sum == 6) return true;
            }
            else if (direction == 1)     // Step Left
            {
                if (y - 2 < 0) return false;
                for (int i = 1; i <= 2; i++)
                {
                    sum += maze[x, y - i];
                    sum += maze[x - 1, y - i];
                    sum += maze[x + 1, y - i];
                }
                if (sum == 6) return true;
            }
            else if (direction == 2)     // Step Up
            {
                if (x - 2 < 0) return false;
                for (int i = 1; i <= 2; i++)
                {
                    sum += maze[x - i, y];
                    sum += maze[x - i, y - 1];
                    sum += maze[x - i, y + 1];
                }
                if (sum == 6) return true;
            }
            else if (direction == 3)     // Step Right
            {
                if (y + 2 >= mazeWidth) return false;
                for (int i = 1; i <= 2; i++)
                {
                    sum += maze[x, y + i];
                    sum += maze[x - 1, y + i];
                    sum += maze[x + 1, y + i];
                }
                if (sum == 6) return true;
            }
            return false;
        }
        //=======



        //===[CrashBlocks]===
        private void crashTwoBlocks(int direction)
        {
            temp[direction] = 0;
            push();
            temp[0] = temp[1] = temp[2] = temp[3] = 1;
            switch (direction)
            {
                case 0: maze[x, y] = maze[x + 1, y] = maze[x + 2, y] = 0; x += 2; temp[2] = 0; break;
                case 1: maze[x, y] = maze[x, y - 1] = maze[x, y - 2] = 0; y -= 2; temp[3] = 0; break;
                case 2: maze[x, y] = maze[x - 1, y] = maze[x - 2, y] = 0; x -= 2; temp[0] = 0; break;
                case 3: maze[x, y] = maze[x, y + 1] = maze[x, y + 2] = 0; y += 2; temp[1] = 0; break;
            }
        }

        private void crashOneBlock(int direction)
        {
            temp[direction] = 0;
            push();
            temp[0] = temp[1] = temp[2] = temp[3] = 1;
            switch (direction)
            {
                case 0: maze[x, y] = maze[x + 1, y] = 0; x += 1; temp[2] = 0; break;
                case 1: maze[x, y] = maze[x, y - 1] = 0; y -= 1; temp[3] = 0; break;
                case 2: maze[x, y] = maze[x - 1, y] = 0; x -= 1; temp[0] = 0; break;
                case 3: maze[x, y] = maze[x, y + 1] = 0; y += 1; temp[1] = 0; break;
            }
        }
        //=======
    }
}
