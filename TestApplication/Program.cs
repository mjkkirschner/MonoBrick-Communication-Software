
using System;
using MonoBrick.EV3;//use this to run the example on the EV3
using System.Collections;
using System.Linq;
//using MonoBrick.NXT;//use this to run the example on the NXT  
namespace MonoBrickTest  
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
                Console.WriteLine(brick.Sensor1.ReadAsString() );
              //  if (cki.Key == ConsoleKey.Q)
              //  {
              //      break;
              //  }
            }


            //  Console.WriteLine("Enter position to move to.");  
            /*     foreach(var pos in Enumerable.Range()
                 if(Int32.TryParse(input, out position)){  
                      Console.WriteLine("Move to " + position);  
                      brick.MotorA.MoveTo(50, position, false);
                      Console.WriteLine((brick.Sensor1).ReadAsString());
                  }  
                  else{  
                      Console.WriteLine("Enter a valid number");  
                  }  
                  */
       }
   }     
} 