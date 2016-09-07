using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoBrick;
using MonoBrick.EV3;
using System.Threading;

namespace DynamoMonoBrick
{

    public class Eve3Robot
    {
        //for the hackathon we know we need a colorSensor type
        private MonoBrick.EV3.Brick<ColorSensor, Sensor, Sensor, Sensor> internalBrick;
      
        public static Eve3Robot byConnection(string connection = "usb")
        {
            var robot = new Eve3Robot();
            robot.internalBrick = new MonoBrick.EV3.Brick<ColorSensor, Sensor, Sensor, Sensor>(connection);
            robot.internalBrick.Connection.Open();
            return robot;
        }


        public string ReadColorSensor()
        {
            this.internalBrick.Sensor1.Mode = ColorMode.Color;
            return (this.internalBrick.Sensor1 as ColorSensor).ReadAsString();
           
        }

        public int MoveMotorsForwardInSync(int steps , int speed)
        {
            this.internalBrick.MotorA.ResetTacho();
            this.internalBrick.MotorD.ResetTacho();
            this.internalBrick.MotorSync.BitField = OutputBitfield.OutA | OutputBitfield.OutD;
            this.internalBrick.MotorSync.StepSync(Convert.ToSByte(speed), 0, Convert.ToUInt32(steps), false);
            WaitForMotorToStop(new List<Motor>() { this.internalBrick.MotorA, this.internalBrick.MotorD });
            return this.internalBrick.MotorA.GetTachoCount();
        }

        [Autodesk.DesignScript.Runtime.CanUpdatePeriodically(true)]
        public string MoveMotorsForwardAndSamplePeriodic(int steps)
        {
            this.internalBrick.MotorA.ResetTacho();
            this.internalBrick.MotorD.ResetTacho();
            this.internalBrick.MotorSync.BitField = OutputBitfield.OutA | OutputBitfield.OutD;
            this.internalBrick.MotorSync.StepSync(Convert.ToSByte(-30), 0, Convert.ToUInt32(steps), true);
            //while the motors are running keep sampling
          
            WaitForMotorToStop(new List<Motor>() { this.internalBrick.MotorA, this.internalBrick.MotorD });
            return this.ReadColorSensor();
        }
    

        /// <summary>
        /// This node moves motors A and D forward a number of steps, samples the color and button state
        /// and plays a tone if the button is pressed during sampling
        /// </summary>
        /// <param name="steps"></param>
        /// <returns></returns>
        public List<List<String>> MoveMotorsForwardAndSample(int steps)
        {
            var list = new List<List<String>>();

            this.internalBrick.MotorA.ResetTacho();
            this.internalBrick.MotorD.ResetTacho();
            this.internalBrick.MotorSync.BitField = OutputBitfield.OutA | OutputBitfield.OutD;
            this.internalBrick.MotorSync.StepSync(Convert.ToSByte(-30), 0, Convert.ToUInt32(steps), true);
            //while the motors are running keep sampling
            Thread.Sleep(100);
            do
            {
                Thread.Sleep(50);
                var buttonState = this.internalBrick.Sensor4.ReadAsString();
                list.Add(new List<string>() { this.ReadColorSensor(), buttonState });
                this.internalBrick.PlayTone(
                    (byte)(100*(Convert.ToInt32(buttonState))), (ushort)(10), 50);

            }
            while (new List<Motor>() { this.internalBrick.MotorA, this.internalBrick.MotorD }.Any(x => x.IsRunning()));

                //WaitForMotorToStop(new List<Motor>() { this.internalBrick.MotorA, this.internalBrick.MotorD });
                return list;
        }

        /// <summary>
        /// plays a tone in khz, this method should be synchronous
        /// </summary>
        /// <param name="volume"></param>
        /// <param name="khz"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        public int PlayTone(int volume, int khz, int duration)
        {
            this.internalBrick.PlayTone(Convert.ToByte(volume), Convert.ToUInt16(khz), Convert.ToUInt16(duration));
            Thread.Sleep(duration + 50);
            return volume;
        }

        public int MoveMotorTo(int position,int speed, string motorPort = "A")
        {
            var speedByte = (byte)Convert.ToSByte(speed);
            switch (motorPort)
            {
                case ("A"):
                    internalBrick.MotorA.ResetTacho();
                    internalBrick.MotorA.MoveTo(speedByte, position, true);
                    var realPos = internalBrick.MotorA.GetTachoCount();
                    WaitForMotorToStop(internalBrick.MotorA);
                    return realPos;

                case ("B"):
                    internalBrick.MotorB.ResetTacho();
                    internalBrick.MotorB.MoveTo(50, position, true);
                    return internalBrick.MotorB.GetTachoCount();

                case ("C"):
                    internalBrick.MotorC.ResetTacho();
                    internalBrick.MotorC.MoveTo(50, position, true);
                    return internalBrick.MotorC.GetTachoCount();

                case ("D"):
                    internalBrick.MotorD.ResetTacho();
                    internalBrick.MotorD.MoveTo(50, position, true);
                    return internalBrick.MotorD.GetTachoCount();

                default:
                    break;

            }
            return position;

        }

        public int MoveMotor(int degreesRotation, int speed, string motorPort = "A")
        {
            var speedByte = Convert.ToSByte(speed);
            switch (motorPort)
            {
                case ("A"):
                    internalBrick.MotorA.ResetTacho();
                    internalBrick.MotorA.On(speedByte, Convert.ToUInt32(degreesRotation), true);
                    WaitForMotorToStop(internalBrick.MotorA);
                    return internalBrick.MotorA.GetTachoCount();

                case ("B"):
                    internalBrick.MotorB.ResetTacho();
                    internalBrick.MotorB.On(speedByte, Convert.ToUInt32(degreesRotation), true);
                    WaitForMotorToStop(internalBrick.MotorB);

                    return internalBrick.MotorB.GetTachoCount();

                case ("C"):
                    internalBrick.MotorC.ResetTacho();
                    internalBrick.MotorC.On(speedByte, Convert.ToUInt32(degreesRotation), true);
                    WaitForMotorToStop(internalBrick.MotorC);

                    return internalBrick.MotorC.GetTachoCount();

                case ("D"):
                    internalBrick.MotorD.ResetTacho();
                    internalBrick.MotorD.On(speedByte, Convert.ToUInt32(degreesRotation), true);
                    WaitForMotorToStop(internalBrick.MotorD);

                    return internalBrick.MotorD.GetTachoCount();

                default:
                    break;

            }
            return degreesRotation;

        }

        void WaitForMotorToStop(Motor motor)
        {
            Thread.Sleep(500);
            while (motor.IsRunning()) { Thread.Sleep(50); }
        }

        void WaitForMotorToStop(List<Motor> motors)
        {
            Thread.Sleep(500);
            while (motors.Any(x=>x.IsRunning())) { Thread.Sleep(50); }
        }

       
    }

}
