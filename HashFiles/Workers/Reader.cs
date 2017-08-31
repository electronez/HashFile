using System;
using System.IO;

namespace HashFiles
{
    /// <summary>
    /// Worker для считывания файлов из каталога
    /// </summary>
    public class Reader : BaseWorker
    {
        private Data _stats;
        private string _startDir;
        private IWriter _writer;

        public Reader(Data stats, string startDir, IWriter writer) : base()
        {
            _stats = stats;
            _startDir = startDir;
            _writer = writer;
            thread.Start();
        }

        protected override void Work()
        {
            GetFiles(_startDir);
        }

        private void GetFiles(string startDirectory)
        {
            try
            {
                var startDir = new DirectoryInfo(startDirectory);
                var dirs = startDir.GetDirectories("*", SearchOption.AllDirectories);
                AddToEqueue(startDir);
                foreach (var dir in dirs)
                {
                    AddToEqueue(dir);
                }
            }
            catch (Exception ex)
            {
                _writer.PushError(new EntityException { Class = this.GetType().ToString(), Method = "GetFiles", ExceptionMessage = ex.Message });
            }
            finally
            {
                _stats.State.IsDoneReading = true;
            }
        }

        private void AddToEqueue(DirectoryInfo dir)
        {
            try
            {
                var files = dir.GetFiles("*", SearchOption.TopDirectoryOnly);
                foreach (var file in files)
                {
                    lock (_stats.queueFile)
                    {
                        _stats.queueFile.Enqueue(file);

                    }
                }
            }
            catch (Exception ex)
            {
                _writer.PushError(new EntityException { Class = this.GetType().ToString(), Method = "AddToEqueue", ExceptionMessage = ex.Message });
            }
        }
    }
}
