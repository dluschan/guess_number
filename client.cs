using System;
using System.Text;
using System.Net.Sockets;

namespace GuessNumber
{
	class Client
	{
		static void Main()
		{
			// Инициализация
			string address = "127.0.0.1";
			int port = 8080;
			
			TcpClient client = new TcpClient(address, port);
			String responseData = String.Empty;
			NetworkStream stream = client.GetStream();
			try
			{
				do
				{
					Console.Write("Enter your number:");
					int guess = int.Parse(Console.ReadLine());
					Byte[] data = BitConverter.GetBytes(guess);

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
				while(responseData != "Число угадано!");
			}
			finally
			{
				stream.Close();
				client.Close();
			}
			
		}
	}
}
