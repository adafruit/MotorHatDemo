using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using Windows.ApplicationModel.Background;
using Windows.System.Threading;
using AdafruitClassLibrary;

// The Background Application template is documented at http://go.microsoft.com/fwlink/?LinkID=533884&clcid=0x409

namespace StepperTest
{
    public sealed class StartupTask : IBackgroundTask
    {
        MotorHat motorHat;
        MotorHat.Stepper stepper;
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            // 
            // TODO: Insert code to perform background work
            //
            // If you start any asynchronous methods here, prevent the task
            // from closing prematurely by using BackgroundTaskDeferral as
            // described in http://aka.ms/backgroundtaskdeferral

            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();

            motorHat = new MotorHat();
            await motorHat.InitAsync(1600).ConfigureAwait(false);

            stepper = motorHat.GetStepper(200, 1);  //200 steps/revolution, motor port 1
            stepper.SetSpeed(10); //10 rpm

            while (true)
            {
                stepper.step(100, MotorHat.Stepper.Command.FORWARD, MotorHat.Stepper.Style.SINGLE);
                stepper.step(100, MotorHat.Stepper.Command.BACKWARD, MotorHat.Stepper.Style.SINGLE);

                stepper.step(100, MotorHat.Stepper.Command.FORWARD, MotorHat.Stepper.Style.DOUBLE);
                stepper.step(100, MotorHat.Stepper.Command.BACKWARD, MotorHat.Stepper.Style.DOUBLE);

                stepper.step(100, MotorHat.Stepper.Command.FORWARD, MotorHat.Stepper.Style.INTERLEAVE);
                stepper.step(100, MotorHat.Stepper.Command.BACKWARD, MotorHat.Stepper.Style.INTERLEAVE);

                stepper.step(100, MotorHat.Stepper.Command.FORWARD, MotorHat.Stepper.Style.MICROSTEP);
                stepper.step(100, MotorHat.Stepper.Command.BACKWARD, MotorHat.Stepper.Style.MICROSTEP);
            }
        }
    }
}
