using System;
using System.Text;
using System.Net.Sockets;

namespace TCPSample
{
	class Client
	{
		static void Main()
		{
			// Инициализация
			string address = "127.0.0.1";
			int port = 8888;
			
			String responseData = String.Empty;
			do
			{
				Console.Write("Enter your number:");
				int guess = int.Parse(Console.ReadLine());
				Byte[] data = BitConverter.GetBytes(guess);

				TcpClient client = new TcpClient(address, port);
				NetworkStream stream = client.GetStream();
				try
				{
					// Отправка сообщения
					stream.Write(data, 0, data.Length);
					// Получение ответа
					Byte[] readingData = new Byte[256];
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
			while(responseData != "Число угадано!");
		}
	}
}
