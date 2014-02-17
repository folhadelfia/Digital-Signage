using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assemblies.Configurations;
using Assemblies.Toolkit;
using Assemblies.XMLSerialization.Components;
using Assemblies.XMLSerialization.Drawing;
using TV2Lib;

namespace Assemblies.XMLSerialization
{
    public static class NetXMLConverter
    {
        #region Space
        public static XMLPoint ToXML(Point p)
        {
            return new XMLPoint(p.X, p.Y);
        }
        public static Point ToNET(XMLPoint p)
        {
            return new Point(p.X, p.Y);
        }
        public static XMLPointF ToXML(PointF p)
        {
            return new XMLPointF(p.X, p.Y);
        }
        public static PointF ToNET(XMLPointF p)
        {
            return new PointF(p.X, p.Y);
        }

        public static XMLSize ToXML(Size s)
        {
            return new XMLSize(s.Width, s.Height);
        }
        public static Size ToNET(XMLSize s)
        {
            return new Size(s.Width, s.Height);
        }

        public static XMLRectangle ToXML(Rectangle r)
        {
            return new XMLRectangle(ToXML(r.Location), ToXML(r.Size));
        }
        public static Rectangle ToNET(XMLRectangle r)
        {
            return new Rectangle(ToNET(r.Location), ToNET(r.Size));
        }

        public static XMLDirection ToXML(Direction direction)
        {
            return (XMLDirection)Convert.ToInt32(direction);
        }
        public static Direction ToNET(XMLDirection direction)
        {
            return (Direction)Convert.ToInt32(direction);
        }
        #endregion

        #region Fonts
        public static XMLFontStyle ToXML(FontStyle style)
        {
            return (XMLFontStyle)Convert.ToInt32(style);
        }
        public static XMLGraphicsUnit ToXML(GraphicsUnit unit)
        {
            return (XMLGraphicsUnit)Convert.ToInt32(unit);
        }
        public static XMLFont ToXML(Font f)
        {
            return new XMLFont(f.Name, f.Size, ToXML(f.Style), ToXML(f.Unit), f.GdiCharSet, f.GdiVerticalFont);
        }

        public static FontStyle ToNET(XMLFontStyle style)
        {
            return (FontStyle)Convert.ToInt32(style);
        }
        public static GraphicsUnit ToNET(XMLGraphicsUnit unit)
        {
            return (GraphicsUnit)Convert.ToInt32(unit);
        }
        public static Font ToNET(XMLFont f)
        {
            return new Font(f.Name, f.Size, ToNET(f.Style), ToNET(f.Unit), f.GdiCharSet, f.GdiVerticalFont);
        }
        #endregion

        #region Cores
        public static XMLColor ToXML(Color c)
        {
            return new XMLColor(c.A, c.R, c.G, c.B);
        }
        public static Color ToNET(XMLColor c)
        {
            return Color.FromArgb(c.A, c.R, c.G, c.B);
        }
        #endregion

        #region ItemConfiguration
        //Ter em atenção que o IEnumerable pode não ser serializável
        public static IEnumerable<XMLItemConfiguration> ToXML(IEnumerable<ItemConfiguration> items)
        {
            List<XMLItemConfiguration> res = new List<XMLItemConfiguration>();

            foreach (var item in items)
            {
                res.Add(ToXML(item));
            }

            return res;
        }
        public static IEnumerable<ItemConfiguration> ToNET(IEnumerable<XMLItemConfiguration> items)
        {
            List<ItemConfiguration> res = new List<ItemConfiguration>();

            foreach (var item in items)
            {
                res.Add(ToNET(item));
            }

            return res;
        }

        public static XMLItemConfiguration ToXML(ItemConfiguration item)
        {
            if (item is MarkeeConfiguration)
            {
                var footer = item as MarkeeConfiguration;

                return new XMLMarkeeConfiguration()
                {
                    Direction = ToXML(footer.Direction),
                    FinalResolution = ToXML(footer.FinalResolution),
                    Font = ToXML(footer.Font),
                    FooterBackColor = ToXML(footer.BackColor),
                    FooterTextColor = ToXML(footer.TextColor),
                    Location = ToXML(footer.Location),
                    Resolution = ToXML(footer.Resolution),
                    Size = ToXML(footer.Size),
                    Speed = footer.Speed,
                    TextList = footer.Text.ToArray()
                };
            }
            else if (item is TVConfiguration)
            {
                var tvConfig = item as TVConfiguration;

                return new XMLTVConfiguration
                {
                    Frequency = tvConfig.Frequency,
                    FinalResolution = ToXML(tvConfig.FinalResolution),
                    Location = ToXML(tvConfig.Location),
                    Resolution = ToXML(tvConfig.Resolution),
                    Size = ToXML(tvConfig.Size)
                };
            }
            else if(item is VideoConfiguration)
            {
                var videoConfig = item as VideoConfiguration;

                return new XMLVideoConfiguration
                {
                    FinalResolution = ToXML(videoConfig.FinalResolution),
                    Location = ToXML(videoConfig.Location),
                    Resolution = ToXML(videoConfig.Resolution),
                    Size = ToXML(videoConfig.Size),
                    ID = videoConfig.ID,
                    Aspect = ToXML(videoConfig.Aspect),
                    Playlist = videoConfig.Playlist.ToArray()
                };
            }
            else return null;
        }
        public static ItemConfiguration ToNET(XMLItemConfiguration item)
        {
            if (item is XMLMarkeeConfiguration)
            {
                var footer = item as XMLMarkeeConfiguration;

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
            else if (item is XMLTVConfiguration)
            {
                var tvConfig = item as XMLTVConfiguration;

                return new TVConfiguration
                {
                    Frequency = tvConfig.Frequency,
                    FinalResolution = ToNET(tvConfig.FinalResolution),
                    Location = ToNET(tvConfig.Location),
                    Resolution = ToNET(tvConfig.Resolution),
                    Size = ToNET(tvConfig.Size)
                };
            }
            else if (item is XMLVideoConfiguration)
            {
                var videoConfig = item as XMLVideoConfiguration;

                return new VideoConfiguration
                {
                    FinalResolution = ToNET(videoConfig.FinalResolution),
                    Location = ToNET(videoConfig.Location),
                    Resolution = ToNET(videoConfig.Resolution),
                    Size = ToNET(videoConfig.Size),
                    ID = videoConfig.ID,
                    Playlist = videoConfig.Playlist.ToList(),
                    Aspect = ToNET(videoConfig.Aspect)
                };
            }
            else return null;
        }

        #endregion
        #region Screens e Windows

        /*
        public static XMLScreenInformation ToXML(Screen screen)
        {
            return new XMLScreenInformation(new XMLRectangle(screen.Bounds.X, screen.Bounds.Y, screen.Bounds.Width, screen.Bounds.Height), screen.DeviceName, screen.Primary);
        }
        public static XMLScreenInformation ToXML(ScreenInformation screen)
        {
            return new XMLScreenInformation(ToXML(screen.Bounds), screen.DeviceID, screen.Primary);
        }

        public static ScreenInformation ToNET(XMLScreenInformation screen)
        {
            return new ScreenInformation(ToNET(screen.Bounds), screen.DeviceID, screen.Primary);
        }
        public static ScreenInformation[] ToNET(XMLScreenInformation[] screens)
        {
            List<ScreenInformation> lista = new List<ScreenInformation>();
            foreach (var screen in screens)
            {
                lista.Add(ToNET(screen));
            }
            return lista.ToArray();
        }

        public static XMLPlayerWindowInformation ToXML(PlayerWindowInformation info)
        {
            MemoryStream mem = new MemoryStream();

            if (info.Background != null)
                info.Background.Save(mem, info.Background.RawFormat);

            return new XMLPlayerWindowInformation() { Components = ToXML(info.Components).ToArray(), Display = ToXML(info.Display), Background = mem.ToArray(), BackgroundImageLayout = Convert.ToInt32(info.BackgroundImageLayout) };
        }
        public static PlayerWindowInformation ToNET(XMLPlayerWindowInformation info)
        {
            Image background = null;

            if (info.Background != null && info.Background.Length > 0)
                background = Image.FromStream(new MemoryStream(info.Background));

            return new PlayerWindowInformation() { Components = ToNET(info.Components), Display = ToNET(info.Display), Background = background, BackgroundImageLayout = (ImageLayout)info.BackgroundImageLayout };
        }

        public static XMLPlayerWindowInformation2 ToXML(PlayerWindowInformation2 info)
        {
            ScreenInformation[] keys = info.Configuration.Keys.ToArray();
            ItemConfiguration[][] values = new ItemConfiguration[keys.Length][];

            XMLScreenInformation[] newKeys = new XMLScreenInformation[keys.Length];
            XMLItemConfiguration[][] newValues = new XMLItemConfiguration[keys.Length][];

            int index = 0;

            foreach (var key in keys)
            {
                values[index] = info.Configuration[key].ToArray();

                newKeys[index] = ToXML(key);

                newValues[index++] = ToXML(values[index]).ToArray();
            }

            XMLPlayerWindowInformation2 res = new XMLPlayerWindowInformation2() { Displays = newKeys, Components = newValues };

            return res;
        }
        public static PlayerWindowInformation2 ToNET(XMLPlayerWindowInformation2 info)
        {
            PlayerWindowInformation2 res = new PlayerWindowInformation2();
            int index = 0;

            foreach (var key in info.Displays)
            {
                res.Configuration.Add(ToNET(key), ToNET(info.Components[index]));
            }

            return res;
        }
        
        */
        #endregion
        #region TV

        public static XMLChannel ToXML(Channel channel)
        {
            ChannelDVBT ch = channel as ChannelDVBT;

            return new XMLChannel
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
                VideoOffset = NetXMLConverter.ToXML(ch.VideoOffset),
                VideoPid = ch.VideoPid,
                VideoRendererDevice = ch.VideoRendererDevice,
                VideoZoom = ch.VideoZoom,
                VideoZoomMode = Convert.ToInt32(ch.VideoZoom)
            };
        }
        public static XMLChannel[] ToXML(Channel[] channels)
        {
            List<XMLChannel> res = new List<XMLChannel>();

            foreach (var ch in channels)
                res.Add(ToXML(ch));

            return res.ToArray();
        }

        public static Channel ToNET(XMLChannel channel)
        {
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
                VideoOffset = NetXMLConverter.ToNET(channel.VideoOffset),
                VideoPid = channel.VideoPid,
                VideoRendererDevice = channel.VideoRendererDevice,
                VideoZoom = channel.VideoZoom,
                VideoZoomMode = (VideoSizeMode)channel.VideoZoom
            };
        }
        public static Channel[] ToNET(XMLChannel[] channels)
        {
            List<Channel> res = new List<Channel>();

            foreach (var ch in channels)
                res.Add(ToNET(ch));

            return res.ToArray();
        }

        #endregion
        #region Video

        public static VideoPlayer.FileVideoPlayer.AspectMode ToNET(XMLAspect aspect)
        {
            return (VideoPlayer.FileVideoPlayer.AspectMode)Convert.ToInt32(aspect);
        }
        public static XMLAspect ToXML(VideoPlayer.FileVideoPlayer.AspectMode aspect)
        {
            return (XMLAspect)Convert.ToInt32(aspect);
        }

        #endregion
    }
}
