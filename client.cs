using System;
using System.Text;
using System.Net.Sockets;

namespace TCPSample
{
	class Client
	{
		static void Main()
		{
			string address = "127.0.0.1";
			int port = 8888;
			string outMessage = "Ping";
			
			// Инициализация
			TcpClient client = new TcpClient(address, port);
			Byte[] data = Encoding.UTF8.GetBytes(outMessage);
			NetworkStream stream = client.GetStream();
			try
			{
				// Отправка сообщения
				stream.Write(data, 0, data.Length);
				// Получение ответа
				Byte[] readingData = new Byte[256];
				String responseData = String.Empty;
				StringBuilder completeMessage = new StringBuilder();
				int numberOfBytesRead = 0;
				do
				{
					numberOfBytesRead = stream.Read(readingData, 0, readingData.Length);
					completeMessage.AppendFormat("{0}", Encoding.UTF8.GetString(readingData, 0, numberOfBytesRead));
				}
				while (stream.DataAvailable);
				responseData = completeMessage.ToString();
				Console.WriteLine(responseData);
			}
			finally
			{
				stream.Close();
				client.Close();
			}
		}
	}
}
