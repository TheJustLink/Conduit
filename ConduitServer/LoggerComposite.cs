namespace ConduitServer
{
    internal abstract class LoggerComposite : ILogger
    {
        protected readonly ILogger[] Childs;

        protected LoggerComposite(ILogger[] childs)
        {
            Childs = childs;
        }

        public abstract void Update();
    }
}