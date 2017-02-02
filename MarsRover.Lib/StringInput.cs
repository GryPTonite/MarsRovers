using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarsRover.Lib
{
    public interface IInput
    {
        string[] GetStringArrayData();
    }

    public class StringInput : IInput
    {
        private string data;

        public StringInput(string data)
        {
            this.data = data;
        }

        public string[] GetStringArrayData()
        {
            return data.Split(new String[] { Environment.NewLine }, StringSplitOptions.None);
        }
    }
}
