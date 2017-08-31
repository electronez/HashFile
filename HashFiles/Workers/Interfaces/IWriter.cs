using System;
using System.IO;

namespace HashFiles
{
    public interface IWriter
    {
        /// <summary>
        /// Добавление файла и его хэша в очередь
        /// </summary>
        /// <param name="file"></param>
        /// <param name="hash"></param>
        void PushHash(FileInfo file, string hash);

        /// <summary>
        /// Добавление ошибки в очередь
        /// </summary>
        /// <param name="ex"></param>
        void PushError(EntityException ex);
    }
}
