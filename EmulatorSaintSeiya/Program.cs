using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmulatorSaintSeiya
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Emulator Saint Seiya";
            Config.Servers = new AuthServer[Config.IPs.Length];
            for(Byte i=0;i<Config.Servers.Length;i++)            
                Config.Servers[i] = new AuthServer(i, Config.IPs[i]);            
            Console.WriteLine("Saint Seiya Emulator Open Source BY: Finzik!");
            Process.GetCurrentProcess().WaitForExit();
            
        }
    }
}
