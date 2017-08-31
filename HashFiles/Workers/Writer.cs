using System;
using System.Collections.Generic;
using System.IO;

namespace HashFiles
{
    /// <summary>
    /// Worker для отправки сообщений в БД
    /// </summary>
    public class Writer : BaseWorker, IWriter
    {
        Queue<EntityMessage> queueMessage = new Queue<EntityMessage>();
        Queue<EntityException> queueException = new Queue<EntityException>();

        string _connectionString;
        StateWorkers _state;

        public Writer(string connectionString, StateWorkers state) : base() {
            _connectionString = connectionString;
            _state = state;
            thread.Start();
        }

        protected override void Work()
        {
            while (true)
            {
                EntityMessage message = null;
                EntityException exception = null;
                lock (queueMessage)
                {
                    if (queueMessage.Count > 0)
                    {
                        message = queueMessage.Dequeue();
                    }
                }

                if (message != null)
                    SendMessageToDb(message);

                lock (queueException)
                {
                    if (queueException.Count > 0)
                    {
                        exception = queueException.Dequeue();
                    }
                }
                if (exception != null)
                    SendExceptionToDb(exception);
                if (_state.IsDoneReading && _state.IsDoneWorkers && queueMessage.Count == 0 && queueException.Count == 0)
                {
                    _state.IsDoneWriting = true;
                    break;
                }
            }
        }

        public void PushHash(FileInfo file, string hash)
        {
            lock (queueMessage)
            {
                if (file != null)
                queueMessage.Enqueue(new EntityMessage { FileName = file.Name, Hash = hash } );
            }
        }

        public void PushError(EntityException ex)
        {
            lock (queueException)
            {
                queueException.Enqueue(ex);
            }
        }

        private void SendMessageToDb(EntityMessage message)
        {
            ISqlInsert sqlInsert = new SqlInsert(_connectionString);
            sqlInsert.InsertFileHash(message);
        }

        private void SendExceptionToDb(EntityException ex)
        {
            ISqlInsert sqlInsert = new SqlInsert(_connectionString);
            sqlInsert.InsertException(ex);
        }
    }
}
