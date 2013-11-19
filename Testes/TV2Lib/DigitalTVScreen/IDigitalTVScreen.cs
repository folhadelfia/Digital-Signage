using System;
namespace TV2Lib
{
    interface IDigitalTVScreen
    {
        event global::TV2Lib.ChannelEventHandler ChannelListChanged;
        global::System.Collections.Generic.List<global::TV2Lib.Channel> Channels { get; }
        global::TV2Lib.Channel CurrentChannel { get; }
        int Frequencia { get; set; }
        int ONID { get; set; }
        int SID { get; set; }
        int TSID { get; set; }
        IntPtr GetBaseHandle();
        global::System.Drawing.Rectangle GetBaseRectangle();
        void ModifyBlackBands(global::System.Drawing.Rectangle[] borders, global::System.Drawing.Color videoBackgroundColor);
        event global::TV2Lib.BDAGraphEventHandler NewLogMessage;
        void SaveGraphToFile();
        void Start();
        void Stop();
        void Tune();
        void Tune(global::TV2Lib.Channel channel);
        void UpdateChannelList();
        bool UseBlackBands { get; set; }
        double VideoAspectRatio { get; set; }
        bool VideoKeepAspectRatio { get; set; }
        global::System.Drawing.PointF VideoOffset { get; set; }
        global::TV2Lib.VideoSizeMode VideoZoomMode { get; set; }
        double VideoZoomValue { get; set; }
    }
}
