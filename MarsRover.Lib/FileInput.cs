using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarsRover.Lib
{
    public class FileInput : IInput
    {
        private string fileName;

        public FileInput(string fileName)
        {
            this.fileName = fileName;
        }

        public string[] GetStringArrayData()
        {
            return System.IO.File.ReadAllLines(fileName);
        }
    }
}
