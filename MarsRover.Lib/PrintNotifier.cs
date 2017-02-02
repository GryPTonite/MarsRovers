using MarsRover.Lib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MarsRover.Lib
{

    public interface IResultNotifier
    {
        void Output(string data);
        void OutputLine(string data, params object[] formatArgs);
        void OutputLine();
    }

    // A memo notifier that prints messages to a text stream.
    public class PrintNotifier : IResultNotifier
    {         
        TextWriter _writer;

        // Construct the notifier with the stream onto which it will 
        // print notifications. 
        public PrintNotifier(TextWriter writer)
        {
            _writer = writer;
        }

        // Print the details of the rovers final position onto the text stream. 
        public void Output(string data)
        {
            _writer.Write(data);
        }

        public void OutputLine(string data, params object[] formatArgs)
        {
            _writer.WriteLine(data, formatArgs);
        }

        public void OutputLine()
        {
            _writer.WriteLine();
        }
    }
}
