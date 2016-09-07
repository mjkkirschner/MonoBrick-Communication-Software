using System;
using MonoBrick.EV3;//use this to run the example on the EV3
using System.Collections;
using System.Linq;

namespace simpleConsoleTest
{
    public static class Program
    {
        static void Main(string[] args)
        {

            var brick = new Brick<ColorSensor, Sensor, Sensor, Sensor>("usb");
            sbyte speed = 0;
            brick.Connection.Open();
            ConsoleKeyInfo cki;
            Console.WriteLine("Press Q to quit");
            brick.Sensor1.Mode = ColorMode.Color;
            //  cki = Console.ReadKey(true);

            while (true)

            {
                brick.MotorA.On(2, false);

            }
        }
    }
}