using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MarsRover.Lib;
using Autofac;
using System.Reflection;

namespace MarsRover.ConsoleApp
{
    class Program
    {
        private static void Process(string[] inputDataArray, out IRover[] rovers, out string[] moves)
        {
            int plateauWidth, plateauHeight;
            string[] plateauDimensions = inputDataArray[0].Split(' ');

            plateauWidth = int.Parse(plateauDimensions[0]);
            plateauHeight = int.Parse(plateauDimensions[1]);

            rovers = new Rover[(inputDataArray.Length - 1) / 2];
            moves = new String[rovers.Length];
            for (int i = 0; i < rovers.Length; i++)
            {
                string[] roverPosition = inputDataArray[i * 2 + 1].Split(' ');
                int roverPositionX = int.Parse(roverPosition[0]);
                int roverPositionY = int.Parse(roverPosition[1]);
                Orientations roverOrientation = Utils.ParseEnum<Orientations>(roverPosition[2]);
                //Initializing the rover with the rover position, the rover orientation and the plateau limit;
                rovers[i] = new Rover(new Position(roverPositionX, roverPositionY), roverOrientation, new Plateau(new Position(plateauWidth, plateauHeight), true));
                moves[i] = inputDataArray[(i + 1) * 2];
            }
        }

        static void Main(string[] args)
        {
            try
            {
                //Dependency Injection by Hand
                //NB: Data could be read in from a file with File.ReadAllLines(path)
                //if you create say a FileInput class that implements IInput              
                string inputData = @"5 5
1 2 N
LMLMLMLMM
3 3 E
MMRMMRMRRM";                
                //Alternative input scenario
                //string inputData = "RoverInputFile.txt";
                //IInput data = new FileInput(inputData);

                //string[] inputDataArray = data.GetStringArrayData();

                IRover[] rovers = null;
                string[] moves = null;
                //Process(inputDataArray, out rovers, out moves);

                //var controller = new RoverController(rovers, moves, new PrintNotifier(Console.Out));
                //controller.Process();

                //Dependency Injection with a "Autofac" Container
                //NB: It would seem using Autofac has more overhead then using DI by hand however,
                //remember this is a trivial example with only one class that needs to be registered.
                //The purpose here if you use the alternative method below that I have commented out
                //which can handle registering all the classes in 1 or more assemblies and additionally use a filter
                //to register only ones of say a certain interface, in our case that implement IController.
                //It all comes down to is the arguments i.e. where I have called RegisterInstance(...)
                //that might make things a little tedious and counterintuitive.
                
                var builder = new ContainerBuilder();
                builder.RegisterType<StringInput>().As<IInput>();
                builder.RegisterInstance(inputData);

                using (var container = builder.Build())
                {
                    var inputDataArray = container.Resolve<IInput>().GetStringArrayData();

                    Process(inputDataArray, out rovers, out moves);
                    using (var scope = container.BeginLifetimeScope(b =>
                        {
                            b.RegisterInstance(rovers);
                            b.RegisterInstance(moves);
                            b.RegisterType<PrintNotifier>().As<IResultNotifier>();                              

                            //var MarsRoverLib = Assembly.GetAssembly(typeof(IController));
                            //b.RegisterAssemblyTypes(MarsRoverLib).Where(t => t.IsClass && t.IsAssignableTo<IController>());
                            b.RegisterType<RoverController>();
                            b.RegisterInstance(Console.Out).As<TextWriter>().ExternallyOwned();
                        }))
                    {
                        scope.Resolve<RoverController>().Process();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine();
            Console.Write("Press any key to close: "); Console.ReadKey();

        }
    }
}
