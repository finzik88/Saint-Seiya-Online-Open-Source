using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace EmulatorSaintSeiya
{
    public class AuthServer
    {
        public Boolean Active = false;
        public Socket socket = null;
        public Byte ServerID = 0;
        public AuthClient[] Clients = null;

        public AuthServer(Byte ServerID, String IP)
        {
            try
            {
                System.Net.IPAddress ipad = null;

                if (System.Net.IPAddress.TryParse(IP, out ipad))
                {
                    IPEndPoint ipep = new IPEndPoint(ipad, 29000);
                    this.Active = true;
                    this.ServerID = ServerID;
                    this.Clients = new AuthClient[Config.MaxConnection];
                    for (Int32 i = 0; i < this.Clients.Length; i++)
                        this.Clients[i] = null;
                    this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    this.socket.Bind(ipep);
                    this.socket.Listen(0);
                    Console.WriteLine($"Channel {this.ServerID} Server Turn On");
                    this.socket.BeginAccept(this.WaitConnection, null);
                }

                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}\n{ex.StackTrace}");
            }
        }

        private void WaitConnection(IAsyncResult ar)
        {
            try
            {
                if (this.Active)
                {
                    Socket newClient = this.socket.EndAccept(ar);
                    Int16 newClientID = this.GetFreeClientID();
                    if(newClientID > 0)
                    {
                        this.Clients[newClientID] = new AuthClient(newClient,this.ServerID, newClientID);
                    }
                    else 
                    {
                        newClient.Close();
                        newClient = null;
                    }
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}\n{ex.StackTrace}");
            }
            finally
            {
                if(this.Active)
                {
                    this.socket.BeginAccept(this.WaitConnection, null);
                }
            }
        }

        private Int16 GetFreeClientID()
        {
            try
            {
                for(Int16 i=1;i<this.Clients.Length;i++)                
                    if(this.Clients[i] == null)                    
                        return i;                                    
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}\n{ex.StackTrace}");
            }
            return 0;
        }
    }
}
