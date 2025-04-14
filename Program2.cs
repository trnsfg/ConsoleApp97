using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

class PartsPriceClient
{
    static void Main()
    {
        using TcpClient client = new TcpClient("127.0.0.1", 5050);
        using NetworkStream stream = client.GetStream();
        using StreamReader reader = new(stream, Encoding.UTF8);
        using StreamWriter writer = new(stream, Encoding.UTF8) { AutoFlush = true };

        Console.WriteLine(reader.ReadLine());

        while (true)
        {
            Console.Write("Введите название комплектующей: ");
            string input = Console.ReadLine();
            writer.WriteLine(input);

            if (input.ToLower() == "exit") break;

            string response = reader.ReadLine();
            Console.WriteLine("Ответ сервера: " + response);
        }
    }
}
