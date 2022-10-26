using System.Net.Sockets;

namespace Pretpark
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create a new instance of the server
            // and start it
            // The server will listen on port 5000
            // and will use the default IP address
            TcpListener server = new TcpListener(new System.Net.IPAddress(new byte[] { 127,0,0,1 }), 5000);
            server.Start();
            while(true){
                //hier worden alle systemen gestart en verwerkt
                using Socket connectie = server.AcceptSocket();
                    Console.WriteLine("1) " + connectie);
                using Stream request = new NetworkStream(connectie);
                    Console.WriteLine("2) " + request);
                using StreamReader requestLezer = new StreamReader(request);
                    Console.WriteLine("3) " + requestLezer);
                string[]? regel1 = requestLezer.ReadLine()?.Split(" ");
                    Console.WriteLine("4) " + regel1);
                if (regel1 == null) return;

                //methode om te kijken wat de request is
                //url hier komt de url uit de request
                //versie is de versie van de request
                (string methode, string url, string httpversie) = (regel1[0], regel1[1], regel1[2]);
                string? regel = requestLezer.ReadLine();
                    Console.WriteLine("5) " + methode);
                    Console.WriteLine("6) " + url);
                    Console.WriteLine("7) " + httpversie);
                int contentLength = 0;
                while (!string.IsNullOrEmpty(regel) && !requestLezer.EndOfStream)
                {
                    string[] stukjes = regel.Split(":");
                    (string header, string waarde) = (stukjes[0], stukjes[1]);
                        Console.WriteLine("8) " + header);
                        Console.WriteLine("9) " + waarde);
                    if (header.ToLower() == "content-length")
                        contentLength = int.Parse(waarde);
                    regel = requestLezer.ReadLine();
                }
                if (contentLength > 0) {
                    char[] bytes = new char[(int)contentLength];
                    requestLezer.Read(bytes, 0, (int)contentLength);
                }
                //URL switch
                string page = "";
                Teller counter = new Teller();
                Console.WriteLine(url);
                if(url.Equals("/")){
                    page = File.ReadAllText("HTML\\Home.html");
                }else if (url.Contains("/Contact")){
                    page = File.ReadAllText("HTML\\Contact.html");
                }else if (url.Contains("/Teller")){
                    page = "<h1>" + counter + "</h1>";
                    counter.i++;
                }else if (url.Contains("/add?")){
                    page = File.ReadAllText("HTML\\add.html");
                }else if (url.Contains("/mijnteller")){
                    page = File.ReadAllText("HTML\\mijnteller.html");
                }else{
                    page = File.ReadAllText("HTML\\404.html");
                }
                Console.WriteLine(page);
                connectie.Send(System.Text.Encoding.ASCII.GetBytes("HTTP/1.0 200 OK\r\nContent-Type: text/html\r\nContent-Length: 11\r\n\r\n"+page));            
            }
        }
    }

    // A bit excessive but whatever
    public class Teller{
        public int i;
        public Teller (){
            this.i = 0;
        }
    }
}