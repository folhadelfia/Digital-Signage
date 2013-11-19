namespace TV2Lib
{
    public enum VideoRenderer { VMR9, EVR }
    public enum LoggingLevel { None, Log, Debug, Queixinhas }

    public delegate void BDAGraphEventHandler(string message);
    public delegate void ChannelEventHandler(object sender, ChannelEventArgs e);
    public delegate void LogEventHandler(string message);
}
