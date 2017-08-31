using System.Collections.Generic;
using System.IO;

namespace HashFiles
{
    public class Data
    {
        /// <summary>
        /// Файлов в очереди
        /// </summary>
        public int Ready { get { return queueFile.Count; } }

        /// <summary>
        /// Обработанных файлов
        /// </summary>
        public int Done { get; set; }

        /// <summary>
        /// Очередь файлов
        /// </summary>
        public Queue<FileInfo> queueFile { get; set; }

        /// <summary>
        /// Информация по worker's
        /// </summary>
        public StateWorkers State { get; set; }
    }

    public class StateWorkers
    {
        public bool IsDoneReading { get; set; }

        public int DoneWorkers { get; set; }

        public int AllWorkers { get; set; }

        public bool IsDoneWorkers { get { return DoneWorkers == AllWorkers; } }

        public bool IsDoneWriting { get; set; }
    }
}
