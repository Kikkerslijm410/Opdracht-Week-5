﻿using System.Net.Sockets;

namespace Pretpark
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpListener server = new TcpListener(new System.Net.IPAddress(new byte[] { 127,0,0,1 }), 5000);
            server.Start();
            while(true){
                using Socket connectie = server.AcceptSocket();
                using Stream request = new NetworkStream(connectie);
                using StreamReader requestLezer = new StreamReader(request);
                string[]? regel1 = requestLezer.ReadLine()?.Split(" ");
                if (regel1 == null) return;
                (string methode, string url, string httpversie) = (regel1[0], regel1[1], regel1[2]);
                string? regel = requestLezer.ReadLine();
                int contentLength = 0;
                while (!string.IsNullOrEmpty(regel) && !requestLezer.EndOfStream)
                {
                    string[] stukjes = regel.Split(":");
                    (string header, string waarde) = (stukjes[0], stukjes[1]);
                    if (header.ToLower() == "content-length")
                        contentLength = int.Parse(waarde);
                    regel = requestLezer.ReadLine();
                }
                if (contentLength > 0) {
                    char[] bytes = new char[(int)contentLength];
                    requestLezer.Read(bytes, 0, (int)contentLength);
                }
                //URL
                string page = "";
                Console.WriteLine(url);
                if(url.Contains("/")){
                    page = File.ReadAllText("Home.html");
                }
                Console.WriteLine(page);
                connectie.Send(System.Text.Encoding.ASCII.GetBytes("HTTP/1.1 200 OK\r\nContent-Type: text/html\r\nContent-Length: 11\r\n\r\n"+page));            
            }
        }
    }
}