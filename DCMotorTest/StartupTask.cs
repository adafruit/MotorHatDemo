using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using Windows.ApplicationModel.Background;
using System.Threading.Tasks;
using AdafruitClassLibrary;

// The Background Application template is documented at http://go.microsoft.com/fwlink/?LinkID=533884&clcid=0x409

namespace DCMotorTest
{
    public sealed class StartupTask : IBackgroundTask
    {
        MotorHat motorHat;
        MotorHat.DCMotor dcMotor;
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            // 
            // TODO: Insert code to perform background work
            //
            // If you start any asynchronous methods here, prevent the task
            // from closing prematurely by using BackgroundTaskDeferral as
            // described in http://aka.ms/backgroundtaskdeferral
            //
            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();

            motorHat = new MotorHat();
            await motorHat.InitAsync(1600).ConfigureAwait(false);

            dcMotor = motorHat.GetMotor(4);  //motor port 4

            while (true)
            {
                dcMotor.Run(MotorHat.DCMotor.Command.FORWARD);
                for (uint i = 0; i < 255; i++)
                {
                    dcMotor.SetSpeed(i);
                    Task.Delay(10).Wait();

                }
                for (uint i = 255; i != 0; i--)
                {
                    dcMotor.SetSpeed(i);
                    Task.Delay(10).Wait();
                }

                dcMotor.Run(MotorHat.DCMotor.Command.BACKWARD);
                for (uint i = 0; i < 255; i++)
                {
                    dcMotor.SetSpeed(i);
                    Task.Delay(10).Wait();

                }
                for (uint i = 255; i != 0; i--)
                {
                    dcMotor.SetSpeed(i);
                    Task.Delay(10).Wait();
                }

                dcMotor.Run(MotorHat.DCMotor.Command.RELEASE);
                Task.Delay(1000).Wait();
            }
        }
    }
}
