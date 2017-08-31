using System.Threading;

namespace HashFiles
{

    public abstract class BaseWorker
    {
        protected Thread thread;

        public BaseWorker()
        {
            thread = new Thread(() => Work());
        }
        
        protected abstract void Work();
    }
}
