using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using Assemblies.Configurations;
using Assemblies.Toolkit;
using TV2Lib;
using System.IO;

namespace Assemblies.DataContracts
{
    public static class NetWCFConverter
    {
        #region Space
        public static WCFPoint ToWCF(Point p)
        {
            return new WCFPoint(p.X, p.Y);
        }
        public static Point ToNET(WCFPoint p)
        {
            return new Point(p.X, p.Y);
        }
        public static WCFPointF ToWCF(PointF p)
        {
            return new WCFPointF(p.X, p.Y);
        }
        public static PointF ToNET(WCFPointF p)
        {
            return new PointF(p.X, p.Y);
        }

        public static WCFSize ToWCF(Size s)
        {
            return new WCFSize(s.Width, s.Height);
        }
        public static Size ToNET(WCFSize s)
        {
            return new Size(s.Width, s.Height);
        }

        public static WCFRectangle ToWCF(Rectangle r)
        {
            return new WCFRectangle(ToWCF(r.Location), ToWCF(r.Size));
        }
        public static Rectangle ToNET(WCFRectangle r)
        {
            return new Rectangle(ToNET(r.Location), ToNET(r.Size));
        }

        public static WCFDirection ToWCF(Direction direction)
        {
            return (WCFDirection)Convert.ToInt32(direction);
        }
        public static Direction ToNET(WCFDirection direction)
        {
            return (Direction)Convert.ToInt32(direction);
        }
        #endregion

        #region Fonts
        public static WCFFontStyle ToWCF(FontStyle style)
        {
            return (WCFFontStyle)Convert.ToInt32(style);
        }
        public static WCFGraphicsUnit ToWCF(GraphicsUnit unit)
        {
            return (WCFGraphicsUnit)Convert.ToInt32(unit);
        }
        public static WCFFont ToWCF(Font f)
        {
            return new WCFFont(f.Name, f.Size, ToWCF(f.Style), ToWCF(f.Unit), f.GdiCharSet, f.GdiVerticalFont);
        }

        public static FontStyle ToNET(WCFFontStyle style)
        {
            return (FontStyle)Convert.ToInt32(style);
        }
        public static GraphicsUnit ToNET(WCFGraphicsUnit unit)
        {
            return (GraphicsUnit)Convert.ToInt32(unit);
        }
        public static Font ToNET(WCFFont f)
        {
            return new Font(f.Name, f.Size, ToNET(f.Style), ToNET(f.Unit), f.GdiCharSet, f.GdiVerticalFont);
        }
        #endregion

        #region Cores
        public static WCFColor ToWCF(Color c)
        {
            return new WCFColor(c.A, c.R, c.G, c.B);
        }
        public static Color ToNET(WCFColor c)
        {
            return Color.FromArgb(c.A, c.R, c.G, c.B);
        }
        #endregion

        #region ItemConfiguration
        //Ter em atenção que o IEnumerable pode não ser serializável
        public static IEnumerable<WCFItemConfiguration> ToWCF(IEnumerable<ItemConfiguration> items)
        {
            List<WCFItemConfiguration> res = new List<WCFItemConfiguration>();

            foreach (var item in items)
            {
                res.Add(ToWCF(item));
            }

            return res;
        }
        public static IEnumerable<ItemConfiguration> ToNET(IEnumerable<WCFItemConfiguration> items)
        {
            List<ItemConfiguration> res = new List<ItemConfiguration>();

            foreach (var item in items)
            {
                res.Add(ToNET(item));
            }

            return res;
        }

        public static WCFItemConfiguration ToWCF(ItemConfiguration item)
        {
            if (item is MarkeeConfiguration)
            {
                var footer = item as MarkeeConfiguration;

                return new WCFFooterConfiguration()
                {
                    Direction = ToWCF(footer.Direction),
                    FinalResolution = ToWCF(footer.FinalResolution),
                    Font = ToWCF(footer.Font),
                    FooterBackColor = ToWCF(footer.BackColor),
                    FooterTextColor = ToWCF(footer.TextColor),
                    Location = ToWCF(footer.Location),
                    Resolution = ToWCF(footer.Resolution),
                    Size = ToWCF(footer.Size),
                    Speed = footer.Speed,
                    TextList = footer.Text.ToArray()
                };
            }
            else if (item is TVConfiguration)
            {
                var tvConfig = item as TVConfiguration;

                return new WCFTVConfiguration
                {
                    Frequency = tvConfig.Frequency,
                    TunerDevicePath = tvConfig.TunerDevicePath,
                    AudioDecoder = tvConfig.AudioDecoder,
                    AudioRenderer = tvConfig.AudioRenderer,
                    H264Decoder = tvConfig.H264Decoder,
                    MPEG2Decoder = tvConfig.MPEG2Decoder,
                    FinalResolution = ToWCF(tvConfig.FinalResolution),
                    Location = ToWCF(tvConfig.Location),
                    Resolution = ToWCF(tvConfig.Resolution),
                    Size = ToWCF(tvConfig.Size)
                };
            }
            else return null;
        }
        public static ItemConfiguration ToNET(WCFItemConfiguration item)
        {
            if (item is WCFFooterConfiguration)
            {
                var footer = item as WCFFooterConfiguration;

                List<string> textos = new List<string>();

                foreach (var text in footer.TextList)
                    textos.Add(text);

                return new MarkeeConfiguration()
                {
                    Direction = ToNET(footer.Direction),
                    FinalResolution = ToNET(footer.FinalResolution),
                    Font = ToNET(footer.Font),
                    BackColor = ToNET(footer.FooterBackColor),
                    TextColor = ToNET(footer.FooterTextColor),
                    Location = ToNET(footer.Location),
                    Resolution = ToNET(footer.Resolution),
                    Size = ToNET(footer.Size),
                    Speed = footer.Speed,
                    Text = textos,
                    TransparentBackground = footer.TransparentBackground
                };
            }
            else if (item is WCFTVConfiguration)
            {
                var tvConfig = item as WCFTVConfiguration;

                return new TVConfiguration
                {
                    Frequency = tvConfig.Frequency,
                    TunerDevicePath = tvConfig.TunerDevicePath,
                    FinalResolution = ToNET(tvConfig.FinalResolution),
                    Location = ToNET(tvConfig.Location),
                    Resolution = ToNET(tvConfig.Resolution),
                    Size = ToNET(tvConfig.Size),

                    AudioDecoder = tvConfig.AudioDecoder,
                    AudioRenderer = tvConfig.AudioRenderer,
                    H264Decoder = tvConfig.H264Decoder,
                    MPEG2Decoder = tvConfig.MPEG2Decoder
                };
            }
            else return null;
        }

        #region OLD
                //public static WCFFooterConfiguration ToWCF(FooterConfiguration footer)
                //{
                //    return new WCFFooterConfiguration()
                //    {
                //        Direction = ToWCF(footer.Direction),
                //        FinalResolution = ToWCF(footer.FinalResolution),
                //        Font = ToWCF(footer.Font),
                //        FooterBackColor = ToWCF(footer.BackColor),
                //        FooterTextColor = ToWCF(footer.TextColor),
                //        Location = ToWCF(footer.Location),
                //        Resolution = ToWCF(footer.Resolution),
                //        Size = ToWCF(footer.Size),
                //        Speed = footer.Speed,
                //        TextList = footer.Text.ToArray()
                //    };
                //}
                //public static WCFTVConfiguration ToWCF(TVConfiguration config)
                //{
                //    return new WCFTVConfiguration
                //    {
                //        Frequency = config.Frequency,
                //        FinalResolution = ToWCF(config.FinalResolution),
                //        Location = ToWCF(config.Location),
                //        Resolution = ToWCF(config.Resolution),
                //        Size = ToWCF(config.Size)
                //    };
                //}
        //public static FooterConfiguration ToNET(WCFFooterConfiguration footer)
        //{
        //    List<string> textos = new List<string>();

        //    foreach (var text in footer.TextList)
        //        textos.Add(text);

        //    return new FooterConfiguration()
        //    {
        //        Direction = ToNET(footer.Direction),
        //        FinalResolution = ToNET(footer.FinalResolution),
        //        Font = ToNET(footer.Font),
        //        BackColor = ToNET(footer.FooterBackColor),
        //        TextColor = ToNET(footer.FooterTextColor),
        //        Location = ToNET(footer.Location),
        //        Resolution = ToNET(footer.Resolution),
        //        Size = ToNET(footer.Size),
        //        Speed = footer.Speed,
        //        Text = textos
        //    };
        //}
        //public static TVConfiguration ToNET(WCFTVConfiguration config)
        //{
        //    return new TVConfiguration
        //    {
        //        Frequency = config.Frequency,
        //        FinalResolution = ToNET(config.FinalResolution),
        //        Location = ToNET(config.Location),
        //        Resolution = ToNET(config.Resolution),
        //        Size = ToNET(config.Size)
        //    };
        //}
        #endregion
        #endregion

        #region Screens e Windows
        public static WCFScreenInformation ToWCF(Screen screen, string name)
        {
            return new WCFScreenInformation(new WCFRectangle(screen.Bounds.X, screen.Bounds.Y, screen.Bounds.Width, screen.Bounds.Height), screen.DeviceName, screen.Primary, name);
        }
        public static WCFScreenInformation ToWCF(ScreenInformation screen)
        {
            return new WCFScreenInformation(ToWCF(screen.Bounds), screen.DeviceID, screen.Primary, screen.Name);
        }

        public static ScreenInformation ToNET(WCFScreenInformation screen)
        {
            return new ScreenInformation(ToNET(screen.Bounds), screen.DeviceID, screen.Primary, screen.Name);
        }
        public static ScreenInformation[] ToNET(WCFScreenInformation[] screens)
        {
            List<ScreenInformation> lista = new List<ScreenInformation>();
            foreach (var screen in screens)
            {
                lista.Add(ToNET(screen));
            }
            return lista.ToArray();
        }

        public static WCFPlayerWindowInformation ToWCF(PlayerWindowInformation info)
        {
            MemoryStream mem = new MemoryStream();
            
            if(info.Background != null)
                info.Background.Save(mem, info.Background.RawFormat);

            return new WCFPlayerWindowInformation() { Components = ToWCF(info.Components).ToArray(), Display = ToWCF(info.Display), Background = mem.ToArray(), BackgroundImageLayout = Convert.ToInt32(info.BackgroundImageLayout) };
        }
        public static PlayerWindowInformation ToNET(WCFPlayerWindowInformation info)
        {
            Image background = null;

            if (info.Background != null && info.Background.Length > 0)
                background = Image.FromStream(new MemoryStream(info.Background));

            return new PlayerWindowInformation() { Components = ToNET(info.Components), Display = ToNET(info.Display), Background = background, BackgroundImageLayout = (ImageLayout)info.BackgroundImageLayout };
        }

        public static WCFPlayerWindowInformation2 ToWCF(PlayerWindowInformation2 info)
        {
            ScreenInformation[] keys = info.Configuration.Keys.ToArray();
            ItemConfiguration[][] values = new ItemConfiguration[keys.Length][];

            WCFScreenInformation[] newKeys = new WCFScreenInformation[keys.Length];
            WCFItemConfiguration[][] newValues = new WCFItemConfiguration[keys.Length][];

            int index = 0;

            foreach (var key in keys)
            {
                values[index] = info.Configuration[key].ToArray();

                newKeys[index] = ToWCF(key);

                newValues[index++] = ToWCF(values[index]).ToArray();
            }

            WCFPlayerWindowInformation2 res = new WCFPlayerWindowInformation2() { Displays = newKeys, Components = newValues };

            return res;
        }
        public static PlayerWindowInformation2 ToNET(WCFPlayerWindowInformation2 info)
        {
            PlayerWindowInformation2 res = new PlayerWindowInformation2();
            int index = 0;

            foreach (var key in info.Displays)
            {
                res.Configuration.Add(ToNET(key), ToNET(info.Components[index]));
            }

            return res;
        }

        #endregion

        #region TV

        public static WCFChannel ToWCF(Channel channel)
        {
            if (channel == null) return null;

            ChannelDVBT ch = channel as ChannelDVBT;

            return new WCFChannel
            {
                AudioDecoderDevice = ch.AudioDecoderDevice,
                AudioDecoderType = Convert.ToInt32(ch.AudioDecoderType),
                AudioPid = ch.AudioPid,
                AudioPids = ch.AudioPids,
                AudioRendererDevice = ch.AudioRendererDevice,
                Bandwidth = ch.Bandwidth,
                CaptureDevice = ch.CaptureDevice,
                ChannelNumber = ch.ChannelNumber,
                EcmPid = ch.EcmPid,
                EcmPids = ch.EcmPids,
                Frequency = ch.Frequency,
                Guard = Convert.ToInt32(ch.Guard),
                H264DecoderDevice = ch.H264DecoderDevice,
                HAlpha = Convert.ToInt32(ch.HAlpha),
                InnerFEC = Convert.ToInt32(ch.InnerFEC),
                InnerFECRate = Convert.ToInt32(ch.InnerFECRate),
                Logo = ch.Logo,
                LPInnerFEC = Convert.ToInt32(ch.LPInnerFEC),
                LPInnerFECRate = Convert.ToInt32(ch.LPInnerFECRate),
                Mode = Convert.ToInt32(ch.Mode),
                Modulation = Convert.ToInt32(ch.Modulation),
                MPEG2DecoderDevice = ch.MPEG2DecoderDevice,
                Name = ch.Name,
                ONID = ch.ONID,
                OtherFrequencyInUse = ch.OtherFrequencyInUse,
                OuterFEC = Convert.ToInt32(ch.OuterFEC),
                OuterFECRate = Convert.ToInt32(ch.OuterFECRate),
                PcrPid = ch.PcrPid,
                PmtPid = ch.PmtPid,
                ReferenceClock = Convert.ToInt32(ch.ReferenceClock),
                SID = ch.SID,
                SymbolRate = ch.SymbolRate,
                TeletextPid = ch.TeletextPid,
                TSID = ch.TSID,
                TunerDevice = ch.TunerDevice,
                VideoAspectRatioFactor = ch.VideoAspectRatioFactor,
                VideoDecoderType = Convert.ToInt32(ch.VideoDecoderType),
                VideoKeepAspectRatio = ch.VideoKeepAspectRatio,
                VideoOffset = NetWCFConverter.ToWCF(ch.VideoOffset),
                VideoPid = ch.VideoPid,
                VideoRendererDevice = ch.VideoRendererDevice,
                VideoZoom = ch.VideoZoom,
                VideoZoomMode = Convert.ToInt32(ch.VideoZoom)
            };
        }
        public static WCFChannel[] ToWCF(Channel[] channels)
        {
            if (channels == null) return null;

            List<WCFChannel> res = new List<WCFChannel>();

            foreach (var ch in channels)
                res.Add(ToWCF(ch));

            return res.ToArray();
        }

        public static Channel ToNET(WCFChannel channel)
        {
            if (channel == null) return null;

            return new ChannelDVBT
            {
                AudioDecoderDevice = channel.AudioDecoderDevice,
                AudioDecoderType = (TV2Lib.ChannelDVB.AudioType)channel.AudioDecoderType,
                AudioPid = channel.AudioPid,
                AudioPids = channel.AudioPids,
                AudioRendererDevice = channel.AudioRendererDevice,
                Bandwidth = channel.Bandwidth,
                CaptureDevice = channel.CaptureDevice,
                ChannelNumber = channel.ChannelNumber,
                EcmPid = channel.EcmPid,
                EcmPids = channel.EcmPids,
                Frequency = channel.Frequency,
                Guard = (DirectShowLib.BDA.GuardInterval)channel.Guard,
                H264DecoderDevice = channel.H264DecoderDevice,
                HAlpha = (DirectShowLib.BDA.HierarchyAlpha)channel.HAlpha,
                InnerFEC = (DirectShowLib.BDA.FECMethod)channel.InnerFEC,
                InnerFECRate = (DirectShowLib.BDA.BinaryConvolutionCodeRate)channel.InnerFECRate,
                Logo = channel.Logo,
                LPInnerFEC = (DirectShowLib.BDA.FECMethod)channel.LPInnerFEC,
                LPInnerFECRate = (DirectShowLib.BDA.BinaryConvolutionCodeRate)channel.LPInnerFECRate,
                Mode = (DirectShowLib.BDA.TransmissionMode)channel.Mode,
                Modulation = (DirectShowLib.BDA.ModulationType)channel.Modulation,
                MPEG2DecoderDevice = channel.MPEG2DecoderDevice,
                Name = channel.Name,
                ONID = channel.ONID,
                OtherFrequencyInUse = channel.OtherFrequencyInUse,
                OuterFEC = (DirectShowLib.BDA.FECMethod)channel.OuterFEC,
                OuterFECRate = (DirectShowLib.BDA.BinaryConvolutionCodeRate)channel.OuterFECRate,
                PcrPid = channel.PcrPid,
                PmtPid = channel.PmtPid,
                ReferenceClock = (TV2Lib.ChannelDVB.Clock)channel.ReferenceClock,
                SID = channel.SID,
                SymbolRate = channel.SymbolRate,
                TeletextPid = channel.TeletextPid,
                TSID = channel.TSID,
                TunerDevice = channel.TunerDevice,
                VideoAspectRatioFactor = channel.VideoAspectRatioFactor,
                VideoDecoderType = (TV2Lib.ChannelDVB.VideoType)channel.VideoDecoderType,
                VideoKeepAspectRatio = channel.VideoKeepAspectRatio,
                VideoOffset = NetWCFConverter.ToNET(channel.VideoOffset),
                VideoPid = channel.VideoPid,
                VideoRendererDevice = channel.VideoRendererDevice,
                VideoZoom = channel.VideoZoom,
                VideoZoomMode = (VideoSizeMode)channel.VideoZoom
            };
        }
        public static Channel[] ToNET(WCFChannel[] channels)
        {
            if (channels == null) return null;

            List<Channel> res = new List<Channel>();

            foreach (var ch in channels)
                res.Add(ToNET(ch));

            return res.ToArray();
        }

        #endregion
    }
}