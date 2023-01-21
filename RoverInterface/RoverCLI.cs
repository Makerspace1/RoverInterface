using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoverInterface
{
    internal class RoverCLI
    {
        RoverCom? _com;

        public RoverCLI()
        {
            _com = RoverCom.CreateComByConsolePrompt();
        }

        private void MessageReceived(object? sender, RoverMessageReceivedEventArgs e)
        { 

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(e.Message);
            Console.ResetColor();
        }

        public void Run()
        {
            if (_com == null) return;

            _com.MessageReceived += MessageReceived;
            _com.Open();
            while (true)
            {
                string? input = Console.ReadLine();
                if (input == null) continue;
                if (input.ToLower().Equals("exit")) break;

                _com.Send(input);
            }
            _com.Close();
        }
    }
}
