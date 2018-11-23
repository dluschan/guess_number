using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace TCPSample
{
	class Server
	{
		static void Main()
		{
			Console.WriteLine("Hello World!");

			int port = 8080;
			int number = 1 + new Random().Next() % 100;		
			TcpListener server = new TcpListener(IPAddress.Any, port);
			server.Start();
			while (true)
			{
				try
				{
					// Подключение клиента
					TcpClient client = server.AcceptTcpClient();
					NetworkStream stream = client.GetStream();
					// Обмен данными
					try
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
						}
					}
					finally
					{
						stream.Close();
						client.Close();
					}
				}
				catch
				{
					server.Stop();
					break;
				}
			}
		}
	}
}
