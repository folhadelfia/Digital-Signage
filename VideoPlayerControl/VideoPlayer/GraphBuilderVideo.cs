using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectShowLib;

namespace VideoPlayer
{
    class GraphBuilderVideo
    {
        #region Filtros

        private IBaseFilter source;
        private IBaseFilter splitter;
        private IBaseFilter audioDecoder;
        private IBaseFilter videoDecoder;
        private IBaseFilter audioRenderer;
        private IBaseFilter videoRenderer;

        private static Dictionary<string, DsDevice> audioDecoderDevices;
        private static Dictionary<string, DsDevice> audioRendererDevices;
        private static Dictionary<string, DsDevice> videoDecoderDevices;

        private DsDevice audioDecoderDevice;
        private DsDevice videoDecoderDevice;
        private DsDevice audioRendererDevice;

        #endregion
    }
}
