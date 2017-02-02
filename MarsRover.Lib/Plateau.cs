
namespace MarsRover.Lib
{
    public interface IPlateau
    {
        Position Boundary { get; }
        
        /// <summary>
        /// Null => throw an error if attemping to go outside of boundary,
        /// true => can go outside boundary, 
        /// false => cannot go outside, error ignored
        /// </summary>
        bool? AllowOutsideBoundary { get; set; }
    }

    public class Plateau : IPlateau
    {
        public Position Boundary { get; private set;}
        public bool? AllowOutsideBoundary { get; set; }

        public Plateau(Position boundary, bool allowOutsideBoundary)
        {
            Boundary = boundary;
            AllowOutsideBoundary = allowOutsideBoundary;
        }
    }
}
