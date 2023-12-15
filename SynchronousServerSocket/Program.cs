using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SynchronousServerSocket
{
	internal class Program
	{
		public class SyncServerSocket
		{
			public static string data = null;
			public static void StartListener()
			{
				byte[] bytes = new byte[1024];

				IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
				IPAddress ipAddress = ipHost.AddressList[0];
				IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 5000);
				Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

				try
				{
					listener.Bind(localEndPoint);
					listener.Listen(10);
					while (true)
					{
						Console.WriteLine($"waiting for a connection ...");
						Socket handler = listener.Accept();
						data = null;
						while (true)
						{
							int byteRec = handler.Receive(bytes);
							data += Encoding.ASCII.GetString(bytes, 0, byteRec);
							if (data.IndexOf("<EQF>") > -1)
							{
								break;
							}
						}

						Console.WriteLine("Test recived : {data}");
						byte[] msg = Encoding.ASCII.GetBytes(data);
						handler.Send(msg);
						handler.Shutdown(SocketShutdown.Both);
						handler.Close();
					}
				}
				catch (Exception e)
				{
					Console.WriteLine(e.Message);
					throw;
				}

				Console.WriteLine("pess any to exit program");
				Console.ReadLine();
			}
		}
		static void Main(string[] args)
		{
			Console.WriteLine("enter to continue ");
			Console.ReadLine();
			SyncServerSocket.StartListener();
			Console.ReadLine();
		}


	}
}
