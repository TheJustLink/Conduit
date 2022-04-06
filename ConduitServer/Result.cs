namespace ConduitServer
{
    internal readonly struct Result<T>
    {
        private readonly T _content;
        private readonly bool _success;

        public Result(T content, bool success)
        {
            _content = content;
            _success = success;
        }

        public T Content => _content;
        public bool Success => _success;
    }
}