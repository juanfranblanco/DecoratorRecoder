using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecoratorRecorder
{
    class Program
    {
        static void Main(string[] args)
        {
            var innerRobot = new Robot();
            var recorder = new MethodInterceptorRecorder<IRobot>(innerRobot);
            IRobot robot = (IRobot)recorder.GetTransparentProxy();

            robot.Fire(10);
            robot.Jump(1);
            robot.Move(5, 6);

            Console.WriteLine("Playing recorded");
            recorder.ReplayMethods(innerRobot);

            Console.ReadLine();

            
        }

        public class Robot : IRobot
        {
            public void Fire(int numberOfTimes)
            {
                Console.WriteLine("Fired " + numberOfTimes);
            }

            public void Jump(int height)
            {
                Console.WriteLine("Jumped " + height);
            }

            public void Move(int x, int y)
            {
                Console.WriteLine("Moved x:" + x + " y" + y);
            }
        }

        public interface IRobot
        {
            void Move(int x, int y);
            void Jump(int height);
            void Fire(int numberOfTimes);
        }
    }
}
