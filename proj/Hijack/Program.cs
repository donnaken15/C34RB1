using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Hijack
{
    class Program
    {
        static OpenFileDialog ISO = new OpenFileDialog();

        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("C34RB1\n--------------------");
            Console.WriteLine("Select the Metal Track Pack ISO...");
            ISO.Filter = "Wii Game (*.iso;*.wbfs)|*.iso;*.wbfs|Any type|*.*";
            ISO.Title = "Select a Metal Track Pack ISO";
            ISO.ShowDialog();
            if (ISO.FileName != string.Empty)
            {
                Process.Start(args[0], '"'+ISO.FileName+'"');
            }
            else
                Console.WriteLine("None provided, exiting");
        }
    }
}
