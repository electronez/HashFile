using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace HashFiles
{
    /// <summary>
    /// Worker для обрабатывания файлов (нахождения хэша)
    /// </summary>
    public class Worker : BaseWorker
    {
        
        Data _data;
        IWriter _writer;

        public Worker(Data data, IWriter writer) : base()
        {
            _data = data;
            _writer = writer;
            thread.Start();
        }

        protected override void Work()
        {
            while (true)
            {
                try
                {
                    FileInfo file = null;
                    lock (_data)
                    {
                        if (_data.queueFile.Count == 0 && _data.State.IsDoneReading)
                        {
                            _data.State.DoneWorkers++;
                            break;
                        }
                        if (_data.queueFile.Count > 0)
                        {
                            file = _data.queueFile.Dequeue();
                        }
                    }
                    if (file != null)
                    {
                        var hash = GetHashFile(file.FullName);
                        _writer.PushHash(file, hash);
                        _data.Done++;
                    }
                }
                catch (Exception ex)
                {
                    _writer.PushError(new EntityException { Class = this.GetType().ToString(), Method = "Work", ExceptionMessage = ex.Message });
                }
            }
        }

        private string GetHashFile(string fullname)
        {
            try
            {
                var sb = new StringBuilder();
                using (var md5 = MD5.Create())
                using (var stream = new BufferedStream(File.OpenRead(fullname), 1200000))
                {
                    var hash = md5.ComputeHash(stream);
                    foreach (byte bt in hash)
                    {
                        sb.Append(bt.ToString("x2"));
                    }
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                _writer.PushError(new EntityException { Class = this.GetType().ToString(), Method = "GetHashFile", ExceptionMessage = ex.Message });
                return "Exception";
            }
        }
    }
}
