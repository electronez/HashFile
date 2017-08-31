using System;

namespace HashFiles
{
    public class EntityMessage
    {
        public string FileName { get; set; }

        public string Hash { get; set; }

        public DateTime Date { get { return DateTime.Now; } }
    }

    public class EntityException
    {
        public string Class { get; set; }

        public string Method { get; set; }

        public string ExceptionMessage { get; set; }

        public DateTime Date { get { return DateTime.Now; } }
    }
}
