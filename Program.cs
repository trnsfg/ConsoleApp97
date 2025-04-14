using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System.Threading;

class PartsPriceServer
{
    static readonly Dictionary<string, decimal> partsPrices = new()
    {
        { "CPU", 150.99m },
        { "GPU", 299.50m },
        { "RAM", 79.99m },
        { "SSD", 99.90m },
        { "HDD", 59.40m },
        { "MOTHERBOARD", 120.00m },
        { "PSU", 60.00m }
    };

    static void Main()
    {
        TcpListener listener = new TcpListener(IPAddress.Any, 5050);
        listener.Start();
        Console.WriteLine("Сервер запущен. Ожидание подключений...");

        while (true)
        {
            TcpClient client = listener.AcceptTcpClient();
            Thread thread = new(() => HandleClient(client));
            thread.Start();
        }
    }

    static void HandleClient(TcpClient client)
    {
        using NetworkStream stream = client.GetStream();
        using StreamReader reader = new(stream, Encoding.UTF8);
        using StreamWriter writer = new(stream, Encoding.UTF8) { AutoFlush = true };

        writer.WriteLine("Добро пожаловать! Введите название комплектующей (например, CPU, GPU, SSD) или 'exit' для выхода.");

        while (true)
        {
            string? request = reader.ReadLine();
            if (request == null || request.ToLower() == "exit") break;

            string part = request.ToUpper();

            if (partsPrices.TryGetValue(part, out decimal price))
            {
                writer.WriteLine($"Цена на {part}: {price:C}");
            }
            else
            {
                writer.WriteLine("Данная комплектующая не найдена.");
            }
        }

        Console.WriteLine("Клиент отключился.");
        client.Close();
    }
}
