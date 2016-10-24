using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using Windows.Devices.Enumeration;
using Windows.ApplicationModel.Background;
using System.Threading.Tasks;
using System.Threading;
using AdafruitClassLibrary;
// The Background Application template is documented at http://go.microsoft.com/fwlink/?LinkID=533884&clcid=0x409

namespace MotorParty
{
    public sealed class StartupTask : IBackgroundTask
    {
        MotorHat motorHat;
        MotorHat.Stepper stepper1;
        MotorHat.Stepper stepper2;
        MotorHat.DCMotor dcMotor1;
        MotorHat.DCMotor dcMotor2;

        CancellationTokenSource Stepper1CancellationTokenSource;
        CancellationTokenSource Stepper2CancellationTokenSource;
        CancellationTokenSource Motor1CancellationTokenSource;
        CancellationTokenSource Motor2CancellationTokenSource;

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

            stepper1 = motorHat.GetStepper(200, 1);  //200 steps/revolution, stepper port 1
 //           stepper2 = motorHat.GetStepper(200, 2);  //200 steps/revolution, stepper port 2
            stepper1.SetSpeed(10); //10 rpm
//            stepper2.SetSpeed(10); //10 rpm

            //dcMotor1 = motorHat.GetMotor(3);  //motor port 3
            dcMotor2 = motorHat.GetMotor(4);  //motor port 4

            Stepper1CancellationTokenSource = new CancellationTokenSource();
            //Stepper2CancellationTokenSource = new CancellationTokenSource();

            //Motor1CancellationTokenSource = new CancellationTokenSource();
            Motor2CancellationTokenSource = new CancellationTokenSource();

            Task stepper1Task = Task.Run(() => RunStepper(stepper1, Stepper1CancellationTokenSource.Token));
            //Task stepper2Task = Task.Run(() => RunStepper(stepper2, Stepper2CancellationTokenSource.Token));

            //Task motor1Task = Task.Run(() => RunMotor(dcMotor1, Motor1CancellationTokenSource.Token));
            Task motor2Task = Task.Run(() => RunMotor(dcMotor2, Motor2CancellationTokenSource.Token));

            List<Task> tasks = new List<Task>();
            tasks.Add(stepper1Task);
           // tasks.Add(stepper2Task);

            //tasks.Add(motor1Task);
            tasks.Add(motor2Task);

            Task.WaitAll(tasks.ToArray());

            deferral.Complete();
        }

        private async Task RunStepper(MotorHat.Stepper stepper, CancellationToken cancellationToken)
        {
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

        private async Task RunMotor(MotorHat.DCMotor motor,  CancellationToken cancellationToken)
        {
            while (true)
            {
                motor.Run(MotorHat.DCMotor.Command.FORWARD);
                for (uint i = 0; i < 255; i++)
                {
                    motor.SetSpeed(i);
                    Task.Delay(10).Wait();

                }
                for (uint i = 255; i != 0; i--)
                {
                    motor.SetSpeed(i);
                    Task.Delay(10).Wait();
                }

                motor.Run(MotorHat.DCMotor.Command.BACKWARD);
                for (uint i = 0; i < 255; i++)
                {
                    motor.SetSpeed(i);
                    Task.Delay(10).Wait();

                }
                for (uint i = 255; i != 0; i--)
                {
                    motor.SetSpeed(i);
                    Task.Delay(10).Wait();
                }

                motor.Run(MotorHat.DCMotor.Command.RELEASE);
                Task.Delay(1000).Wait();
            }
        }
    }
}
