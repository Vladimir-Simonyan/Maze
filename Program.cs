//   Copyright © 2019 | Vladimir Simonyan | simonyan.vlad@gmail.com | GPL v3.0   //


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze
{
    class Program
    {
        static void Main(string[] args)
        {
            Maze lab = new Maze();
            lab.createMaze(195, 67);
            lab.printMaze('@', ' ', '.');

            Console.ReadKey();
        }
    }
}
