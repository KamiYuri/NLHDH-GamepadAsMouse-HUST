using System;
using System.Diagnostics;

namespace GamepadAsMouse
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            int index = 0;
            if (args.Length != 0)
            {
                if (args[0] == "1")
                {
                    AlternatePathOfExecution(index);
                }
                //add other options here and below
            }
            else
            {
                NormalPathOfExectution(ref index);
            }
        }

        private static void NormalPathOfExectution(ref int index)
        {
            InputDevices input = new InputDevices();
            index = input.getSelectedDeviceIndex();

            Process.Start("GamepadAsMouse.exe", "1");
            Console.ReadLine();
        }

        private static void AlternatePathOfExecution(int index)
        {
            new GamepadAsMouse(index);
        }
    }
}