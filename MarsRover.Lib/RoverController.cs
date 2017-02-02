using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarsRover.Lib
{
    public interface IController { }
    public class RoverController : IController
    {
        private IRover[] rovers;
        private string[] moves;
        private IResultNotifier notifier;

        public RoverController(IRover[] rovers, string[] moves, IResultNotifier notifier)
        {
            this.rovers = rovers;
            this.moves = moves;
            this.notifier = notifier;
        }

        public void Process()
        {
            notifier.OutputLine("Input");
            if (rovers != null && rovers.Length > 0)
            {
                notifier.OutputLine("{0} {1}", rovers[0].Plateau.Boundary.X, rovers[0].Plateau.Boundary.Y);

                for(int i = 0; i < rovers.Length; i++)
                {
                    //Display Input
                    notifier.OutputLine(rovers[i].ToString());
                    notifier.OutputLine(moves[i]);
                }
                notifier.OutputLine();
                //Process and Display Output
                notifier.OutputLine("Output");
                for (int i = 0; i < rovers.Length; i++)
                {
                    IRover rover = rovers[i];
                    rover.Process(moves[i]);
                    notifier.OutputLine(rover.ToString());
                }
            }
            else
            {
                notifier.OutputLine("No rovers!");
            }
        }
    }
}
