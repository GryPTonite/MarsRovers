using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MarsRover.Lib;


namespace MarsRover.Lib
{
    //using enum with a string attibute, an enum extension method will allow to retrive the string value
    public enum Orientations
    {
        [Utils.StringValueAttribute("N")]
        N = 1,
        [Utils.StringValueAttribute("E")]
        E = 2,
        [Utils.StringValueAttribute("S")]
        S = 3,
        [Utils.StringValueAttribute("W")]
        W = 4
    }


    public interface IRover
    {
        IPosition Position { get; set; }
        Orientations Orientation { get; set; }
        IPlateau Plateau { get; set; }
        bool IsRobotInsideBoundaries { get; }
        void Process(string commands);
        string ToString();
    }

    public class Rover : IRover
    {
        public IPosition Position { get; set; }
        public Orientations Orientation { get; set; }
        public IPlateau Plateau { get; set; }


        public Rover(IPosition roverPosition, Orientations roverOrientation, IPlateau roverPlateau)
        {
            Position = roverPosition;
            Orientation = roverOrientation;
            Plateau = roverPlateau;
        }

        public void Process(string commands)
        {
            foreach (var command in commands)
            {
                switch (command)
                {
                    case ('L'):
                        TurnLeft();
                        break;
                    case ('R'):
                        TurnRight();
                        break;
                    case ('M'):
                        Move();
                        break;
                    default:
                        throw new ArgumentException(string.Format("Invalid value: {0}", command));
                }
            }
        }

        public bool IsRobotInsideBoundaries
        {
            get
            {
                bool isInsideBoundaries = false;
                if (0 <= Position.X && Position.X <= Plateau.Boundary.X &&
                    0 <= Position.Y && Position.Y <= Plateau.Boundary.Y)
                    isInsideBoundaries = true;
                return isInsideBoundaries;
            }
        }

        private void TurnLeft()
        {
            Orientation = (Orientation - 1) < Orientations.N ? Orientations.W : Orientation - 1;
        }

        private void TurnRight()
        {
            Orientation = (Orientation + 1) > Orientations.W ? Orientations.N : Orientation + 1;
        }

        private void Move()
        {
            int newY = Position.Y, newX = Position.X;
            if (Orientation == Orientations.N)
            {
                newY++;
            }
            else if (Orientation == Orientations.E)
            {
                newX++;
            }
            else if (Orientation == Orientations.S)
            {
                newY--;
            }
            else if (Orientation == Orientations.W)
            {
                newX--;
            }

            bool proceed = false;
            if ((Plateau.AllowOutsideBoundary.HasValue && Plateau.AllowOutsideBoundary.Value) || 
                (
                    0 <= newX && newX <= Plateau.Boundary.X
                    &&
                    0 <= newY && newY <= Plateau.Boundary.Y
                ))
            {
                proceed = true;
            }
            
            if (proceed)
            {
                Position.X = newX;
                Position.Y = newY;
            }
            else if (!Plateau.AllowOutsideBoundary.HasValue)
            {
                throw new InvalidOperationException("Rover has been asked to move outside of plateau boundary which is not permitted");
            }
        }

        public override string ToString()
        {
            string printedRoverPosition = string.Format("{0} {1} {2}", Position.X, Position.Y, Orientation.GetStringValue());
            if (!IsRobotInsideBoundaries)
                printedRoverPosition =
                    string.Format("Rover outside the plateau, Rover position: {0} , plateau limit {1}",
                                  printedRoverPosition, Plateau.Boundary.ToString());

            return printedRoverPosition;


        }


    }
}

