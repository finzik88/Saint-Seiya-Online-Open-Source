using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace EmulatorSaintSeiya
{
    public class AuthClient
    {
        public Boolean Active = false;
        public Socket socket = null;
        public Byte ServerID = 0;
        public Int16 ClientID = 0;
        public Byte[] Buffer = null;
        public AuthClient(Socket socket, Byte ServerID, Int16 ClientID)
        {
            try
            {
                this.Active = true;
                this.socket = socket;
                this.ServerID = ServerID;
                this.ClientID = ClientID;

                Console.WriteLine($"Client {this.ClientID} connected on channel {this.ServerID}!");

                this.Buffer = new Byte[4000];
                this.socket.BeginReceive(this.Buffer, 0, this.Buffer.Length, SocketFlags.None, this.WaitData, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}\n{ex.StackTrace}");
            }
        }

        private void WaitData(IAsyncResult ar)
        {
            try
            {
                if (this.Active)
                {
                    Int32 size = this.socket.EndReceive(ar);
                    if(size > 0)
                    {
                        Array.Resize(ref this.Buffer, size);
                        Console.WriteLine($"{String.Join(", ", this.Buffer)}");
                    }
                    else
                    {
                        this.Close();
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}\n{ex.StackTrace}");
            }
            finally
            {
                this.Buffer = new Byte[4000];
                this.socket.BeginReceive(this.Buffer, 0, this.Buffer.Length, SocketFlags.None, this.WaitData, null);
            }
        }        
        public void Close()
        {
            try
            {
                if (this.Active)
                {
                    this.Active = false;
                    this.socket.Close();
                    this.socket = null;
                    Config.Servers[this.ServerID].Clients[this.ClientID] = null;
                    Console.WriteLine($"Client {this.ClientID} disconnected from channel {this.ServerID}");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}\n{ex.StackTrace}");
            }
        }
    }
}
