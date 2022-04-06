namespace ConduitServer
{
    internal interface ILog
    {
        bool HasMessages();
        string[] Messages();
    }
}