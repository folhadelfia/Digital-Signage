using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Assemblies.Components;
using Assemblies.PlayerComponents;


namespace Assemblies.Factories
{
    //[Serializable()]
    public class DateTimeCreator : IComponentCreator
    {
        public ComposerComponent Instance
        {
            get
            {
                return new DateTimeComposer();
            }
        }


        public ComposerComponent FromConfiguration(Configurations.ItemConfiguration config)
        {
            throw new NotImplementedException();
        }
    }

    //[Serializable()]
    public class ImageCreator : IComponentCreator
    {
        public ComposerComponent Instance
        {
            get
            {
                return new ImageComposer();
            }
        }


        public ComposerComponent FromConfiguration(Configurations.ItemConfiguration config)
        {
            throw new NotImplementedException();
        }
    }

    //[Serializable()]
    public class WaitListCreator : IComponentCreator
    {
        public ComposerComponent Instance
        {
            get
            {
                return new WaitListComposer();
            }
        }


        public ComposerComponent FromConfiguration(Configurations.ItemConfiguration config)
        {
            throw new NotImplementedException();
        }
    }

    //[Serializable()]
    public class WeatherCreator : IComponentCreator
    {
        public ComposerComponent Instance
        {
            get
            {
                return new WeatherComposer();
            }
        }


        public ComposerComponent FromConfiguration(Configurations.ItemConfiguration config)
        {
            throw new NotImplementedException();
        }
    }

    //[Serializable()]
    public class MarkeeCreator : IComponentCreator
    {
        public ComposerComponent Instance
        {
            get
            {
                return new Markee(ComponentTargetSite.Builder);
            }
        }

        public ComposerComponent FromConfiguration(Configurations.ItemConfiguration config)
        {
            if (!(config is Assemblies.Configurations.MarkeeConfiguration)) return null;

            Markee m = new Markee(ComponentTargetSite.Builder);
            Assemblies.Configurations.MarkeeConfiguration mConfig = config as Assemblies.Configurations.MarkeeConfiguration;

            m.Configuration = mConfig;

            m.TextList = mConfig.Text;
            m.MarkeeFont = mConfig.Font;
            m.Speed = mConfig.Speed;
            m.Direction = mConfig.Direction;
            m.BackColor = mConfig.BackColor;
            m.TextColor = mConfig.TextColor;

            return m;
        }
    }

    //[Serializable()]
    public class SlideShowCreator : IComponentCreator
    {
        public ComposerComponent Instance
        {
            get
            {
                return new SlideShowComposer();
            }
        }


        public ComposerComponent FromConfiguration(Configurations.ItemConfiguration config)
        {
            throw new NotImplementedException();
        }
    }

    //[Serializable()]
    public class PriceListCreator : IComponentCreator
    {
        public ComposerComponent Instance
        {
            get
            {
                return new PriceListComposer();
            }
        }


        public ComposerComponent FromConfiguration(Configurations.ItemConfiguration config)
        {
            throw new NotImplementedException();
        }
    }

    //[Serializable()]
    public class TVCreator : IComponentCreator
    {
        public ComposerComponent Instance
        {
            get
            {
                return new TVComposer();
            }
        }


        public ComposerComponent FromConfiguration(Configurations.ItemConfiguration config)
        {
            if (!(config is Assemblies.Configurations.TVConfiguration)) return null;
            return new TVComposer(config as Assemblies.Configurations.TVConfiguration);
        }
    }

    //[Serializable()]
    public class VideoCreator : IComponentCreator
    {
        public ComposerComponent Instance
        {
            get
            {
                return new VideoComposer();
            }
        }


        public ComposerComponent FromConfiguration(Configurations.ItemConfiguration config)
        {
            throw new NotImplementedException();
        }
    }
}
