using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoverInterface
{
    public class RoverMessageReceivedEventArgs : EventArgs
    {
        public readonly string Message;
        internal RoverMessageReceivedEventArgs(string message)
        {
            Message = message;
        }
    }
}
