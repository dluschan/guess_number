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

			// Инициализация
			IPAddress localAddr = IPAddress.Parse("127.0.0.1");
			int port = 8888;
			TcpListener server = new TcpListener(localAddr, port);
			// Запуск в работу
			server.Start();
			// Бесконечный цикл
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
							byte[] myReadBuffer = new byte[1024];
							StringBuilder myCompleteMessage = new StringBuilder();
							int numberOfBytesRead = 0;
							do
							{
								numberOfBytesRead = stream.Read(myReadBuffer, 0, myReadBuffer.Length);
								myCompleteMessage.AppendFormat("{0}", Encoding.UTF8.GetString(myReadBuffer, 0, numberOfBytesRead));
							}
							while (stream.DataAvailable);
							Console.WriteLine("Complete");
							Byte[] responseData = Encoding.UTF8.GetBytes("УСПЕШНО!");
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
