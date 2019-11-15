//   Copyright © 2019 | Vladimir Simonyan | simonyan.vlad@gmail.com | GPL v3.0   //


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze
{
    class MazeBot
    {
        public bool forward;


        public MazeBot()
        {
            resetBot();
        }

        public int stepBot(int b, int l, int t, int r)
        {
            while (true)
            {
                forward = true;

                if (temp[0] == 0 && temp[1] == 0 && temp[2] == 0 && temp[3] == 0)
                {
                    forward = false;
                    pop();
                    return temp[4];
                }

                if (temp[0] == 1)
                {
                    if (b != 1)
                    {
                        temp[0] = 0;
                        temp[4] = 2;
                        push();
                        temp[2] = 0;
                        return 0;
                    }
                    else temp[0] = 0;
                }

                if (temp[1] == 1)
                {
                    if (l != 1)
                    {
                        temp[1] = 0;
                        temp[4] = 3;
                        push();
                        temp[3] = 0;
                        return 1;
                    }
                    else temp[1] = 0;
                }

                if (temp[2] == 1)
                {
                    if (t != 1)
                    {
                        temp[2] = 0;
                        temp[4] = 0;
                        push();
                        temp[0] = 0;
                        return 2;
                    }
                    else temp[2] = 0;
                }

                if (temp[3] == 1)
                {
                    if (r != 1)
                    {
                        temp[3] = 0;
                        temp[4] = 1;
                        push();
                        temp[1] = 0;
                        return 3;
                    }
                    else temp[3] = 0;
                }
            }
        }



        //===[Stack]===
        private sbyte[,] stack = new sbyte[10000, 5];
        private int top = 0;
        private sbyte[] temp = new sbyte[5];


        public void resetBot()
        {
            temp[0] = 1;
            temp[1] = 1;
            temp[2] = 1;
            temp[3] = 1;
            temp[4] = -1;
            push();
        }

        private void push()
        {
            for (int i = 0; i < 5; i++)
                stack[top, i] = temp[i];
            top++;

            for (int i = 0; i < 4; i++)
                temp[i] = 1;
        }

        private void pop()
        {
            top--;
            for (int i = 0; i < 5; i++)
                temp[i] = stack[top, i];
        }
        //=======
    }
}
