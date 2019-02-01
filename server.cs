using System;
using System.Text;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace GuessNumber
{
	class Server
	{
        private int _listeningPort;
		private int number;
		private TcpListener _server;
		
        public Server(int port)
        {
            _listeningPort = port;
			number = 1 + new Random().Next() % 100;
        }
		
		public void start()
		{
			IPAddress ipAddre = IPAddress.Loopback;
			_server = new TcpListener(ipAddre, _listeningPort);
			_server.Start();

            Console.WriteLine("Server is running");
            Console.WriteLine("Listening on port " + _listeningPort);
 
            while (true)
            {
	            try
	            {
	                Console.WriteLine("Waiting for connections...");
					handleConnection();
					Thread.Sleep(1000);
				}
	            catch (Exception e)
	            {
	                Console.WriteLine(e.ToString());
	            }
            }
		}
		
		private async void handleConnection()
		{
			var tcpClient = await _server.AcceptTcpClientAsync();
			handleConnectionAsync(tcpClient);
		}
		
		private void handleConnectionAsync(TcpClient tcpClient)
		{
			NetworkStream stream = tcpClient.GetStream();
			try
			{
				while (true)
				{
					if (stream.CanRead)
					{
						byte[] myReadBuffer = new byte[4];
						stream.Read(myReadBuffer, 0, myReadBuffer.Length);
						int guess = BitConverter.ToInt32(myReadBuffer, 0);
						string response = "";
						if (guess == number)
							response = "Число угадано!";
						else if (guess < number)
							response = "Число меньше заданного";
						else if (guess > number)
							response = "Число больше заданного";
						Byte[] responseData = Encoding.UTF8.GetBytes(response);
						Console.WriteLine(response);
						stream.Write(responseData, 0, responseData.Length);
						if (guess == number)
							break;
					}
				}
			}
			catch (Exception exp)
			{
				System.Console.WriteLine(exp);
			}
			finally
			{
				stream.Close();
				tcpClient.Close();
			}
		}
	}

    class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server(8080);
            server.start();
        }
    }
}
