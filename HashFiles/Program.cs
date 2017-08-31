using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Threading;

namespace HashFiles
{
    class Program
    {
        static Data data = new Data { Done = 0, queueFile = new Queue<FileInfo>(), State = new StateWorkers() };
        static Random rnd = new Random((int)DateTime.Now.Ticks);

        static Timer t;

        static void Main(string[] args)
        {

            Console.WriteLine("Задайте каталог");
            var inputCatalog = Console.ReadLine();

            t = new Timer(Count, null, 0, 250);
            
            Init(inputCatalog);

            Console.ReadLine();
        }

        static void Init(string fileCatalog)
        {
            var countWorkers = int.Parse(ConfigurationManager.AppSettings["countWorkers"]);

            data.State.AllWorkers = countWorkers;

            var connectionString = ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString;


            IWriter writer = new Writer(connectionString, data.State);

            new Reader(data, fileCatalog, writer);

            for (int i = 0; i < countWorkers; i++)
                new Worker(data, writer);
        }



        /// <summary>
        /// Вывод статистики
        /// </summary>
        /// <param name="obj"></param>
        private static void Count(object obj)
        {
            Console.WriteLine(string.Format("В очереди: {0} / Обработано: {1}", data.Ready, data.Done));
            if (data.State.IsDoneReading && data.State.IsDoneWorkers && data.State.IsDoneWriting && data.Ready == 0)
            {
                t.Dispose();
                Console.WriteLine("Конец");
            }
        }

        
    }
}
