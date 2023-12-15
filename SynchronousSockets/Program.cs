using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SynchronousSockets
{
	internal class Program
	{
		public class SyncSocketClient
		{
			public static void StartClient()
			{
				byte[] bytes = new byte[1024];
				try
				{
					var hostName = Dns.GetHostName();
					IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
					Console.WriteLine($"Host: {hostName}");
					IPAddress ip = ipHost.AddressList[0];
					IPEndPoint remoteIP = new IPEndPoint(ip, 5000);

					Socket sender = new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
					try
					{
						sender.Connect(remoteIP);
						Console.WriteLine("socket connected");
						sender.RemoteEndPoint.ToString();
						byte[] msg = Encoding.ASCII.GetBytes("this is just a test");
						int byteSent = sender.Send(msg);
						int byteRec = sender.Receive(bytes);
						Console.WriteLine($"Echoned test {Encoding.ASCII.GetString(bytes, 0, byteRec)}");

						//release the socket
						sender.Shutdown(SocketShutdown.Both);
						sender.Close();
					}
					catch (ArgumentNullException e)
					{
						Console.WriteLine(e.Message);
						throw;
					}
					catch (SocketException e)
					{
						Console.WriteLine(e.Message);

					}
					catch (Exception e)
					{
						Console.WriteLine(e.Message);

					}
				}
				catch (Exception e)
				{
					Console.WriteLine(e.Message);
					throw;
				}
			}
		}
		static void Main(string[] args)
		{
			Console.WriteLine("press any key to  cont....");
			Console.ReadLine();
			SyncSocketClient.StartClient();
			Console.ReadLine();
		}


	}
}
