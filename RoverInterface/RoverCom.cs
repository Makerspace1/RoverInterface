using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoverInterface
{
    public class RoverCom
    {
        const char EOLCHAR = '\n';
        const string RUNCOMMAND = "RUN";
        const string CLEARCOMMAND = "CLEAR";
        const int BAUDRATE = 115200;
        private readonly SerialPort _serial;
        private string _dataBuffer = string.Empty;

        public bool IsOpen => _serial.IsOpen;

        public event EventHandler<RoverMessageReceivedEventArgs>? MessageReceived;

        public RoverCom(string port)
        {
            _serial = new SerialPort(port, BAUDRATE, Parity.None, 8, StopBits.One);
            _serial.WriteBufferSize = 512;
            _serial.ReadBufferSize = 512;
            _serial.DataReceived += DataReceived;
        }

        public static RoverCom? CreateComByConsolePrompt()
        {
            int i = 0;
            Console.WriteLine("Choose a port:");
            string[] ports = SerialPort.GetPortNames();
            foreach (string port in ports)
            {
                Console.WriteLine($"{i} : {port}");
                i++;
            }
            int selectedIndex;
            if(!int.TryParse(Console.ReadLine(), out selectedIndex))
            {
                return null;
            }
            return new RoverCom(ports[selectedIndex]);
        }

        ~RoverCom(){      
            _serial.DataReceived -= DataReceived;
            Close();
        }

        private void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            _dataBuffer += _serial.ReadExisting();

            int eolPosition;
            while((eolPosition = _dataBuffer.IndexOf(EOLCHAR)) >= 0)
            {
                string currentCommand = _dataBuffer.Substring(0, eolPosition);
                _dataBuffer = _dataBuffer.Substring(eolPosition + 1);
                ProcessCommand(currentCommand);
            }
        }

        private void ProcessCommand(string currentCommand)
        {
            if(MessageReceived != null)
            {
                MessageReceived(this, new RoverMessageReceivedEventArgs(currentCommand));
            }
        }

        public bool Open()
        {
            _serial.Open();
            return IsOpen;
        }

        public void Send(string message)
        {
            _serial.Write(message + EOLCHAR);
        }

        public void RunQueue()
        {
            Send(RUNCOMMAND);
        }
        public void ClearQueue()
        {
            Send(CLEARCOMMAND);
        }

        internal void Close()
        {
            if (!IsOpen) return;
            _serial.Close();
        }
    }
}
