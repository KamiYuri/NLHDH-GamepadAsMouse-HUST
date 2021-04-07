using SharpDX.DirectInput;
using System;
using System.Collections.Generic;

namespace GamepadAsMouse
{
    internal class InputDevices
    {
        private List<DeviceInstance> _devices;
        private int index;
        private static DeviceInstance _selectedDevice;

        private void getDevices()
        {
            _devices = new List<DeviceInstance>(new DirectInput().GetDevices(SharpDX.DirectInput.DeviceType.Gamepad, DeviceEnumerationFlags.AllDevices));

            if (_devices.Count == 0)
            {
                Console.Write("Gampad not found. Press any key to exit");
                Console.ReadLine();
                System.Environment.Exit(0);
            }
            else
            {
                Console.WriteLine("Gamepads has found.\n");

                int i = 0;
                while (i < _devices.Count)
                {
                    Console.WriteLine($"Device {i + 1}: {_devices[i].ProductName}");
                    i++;
                }
            }
        }

        private void selectGamepad()
        {
            ConsoleKeyInfo key;

            do
            {
                Console.Write("\nSelect one device: ");
                key = Console.ReadKey();
                if (!char.IsDigit(key.KeyChar))
                {
                    Console.Write("\nInput is not a numer.\n");
                    continue;
                }
                if (int.Parse(key.KeyChar.ToString()) - 1 > _devices.Count)
                {
                    Console.Write("\nInput is out of bound.\n");
                }
                if (int.Parse(key.KeyChar.ToString()) - 1 < 0)
                {
                    Console.Write("\nInput has not be small than 1.\n");
                }
            } while (!char.IsDigit(key.KeyChar) || 0 > int.Parse(key.KeyChar.ToString()) - 1 || int.Parse(key.KeyChar.ToString()) - 1 > _devices.Count);

            index = int.Parse(key.KeyChar.ToString()) - 1;

            _selectedDevice = _devices[index];

            Console.WriteLine($"\n{_selectedDevice.ProductName} has selected.\n");
            Console.WriteLine("Use Right Analog to move.");
            Console.WriteLine("Use Left Analog to scroll");
            Console.WriteLine("Use A button as left mouse");
            Console.WriteLine("Use B button as right mouse");
        }

        public int getSelectedDeviceIndex()
        {
            return index;
        }

        public static DeviceInstance getSelectedGamepad()
        {
            return _selectedDevice;
        }

        public InputDevices()
        {
            getDevices();
            selectGamepad();
        }

        public static bool IsKeyPressed(ConsoleKey key)
        {
            return Console.KeyAvailable && Console.ReadKey(true).Key == key;
        }
    }
}