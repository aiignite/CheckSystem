using System.Collections.Generic;

namespace CheckSystem.HelperForms.TPS92994
{
    /// <summary>
    /// 单个通道的配置数据
    /// </summary>
    public class ChannelConfig
    {
        /// <summary>
        /// 通道号 (0-15)
        /// </summary>
        public int Channel { get; set; }

        /// <summary>
        /// 通道名称
        /// </summary>
        public string ChannelName { get; set; }

        /// <summary>
        /// 相位值 (0-1023)
        /// </summary>
        public ushort Phase { get; set; }

        /// <summary>
        /// PWM值 (0-1023)
        /// </summary>
        public ushort PWM { get; set; }

        public ushort Ts1 { get; set; }

        public ushort Ts2 { get; set; }

        public ChannelConfig()
        {
            ChannelName = "";
        }

        public ChannelConfig(int channel, string channelName, ushort phase, ushort pwm, ushort ts1, ushort ts2)
        {
            Channel = channel;
            ChannelName = channelName;
            Phase = phase;
            PWM = pwm;
            Ts1 = ts1;
            Ts2 = ts2;
        }
    }

    /// <summary>
    /// 光束配置（远光或近光）
    /// </summary>
    public class BeamConfig
    {
        /// <summary>
        /// 16个通道的配置
        /// </summary>
        public List<ChannelConfig> Channels { get; set; }

        public BeamConfig()
        {
            // 不在构造函数中初始化Channels，避免JSON反序列化时重复添加
            // 初始化工作由CreateDefault方法完成
            Channels = null;
        }

        /// <summary>
        /// 创建默认配置（16个通道，全部为0）
        /// </summary>
        public static BeamConfig CreateDefault()
        {
            var config = new BeamConfig();
            config.Channels = new List<ChannelConfig>();
            // 初始化16个通道，默认值为0，通道号从0开始
            for (int i = 2; i < 16; i++)
            {
                config.Channels.Add(new ChannelConfig(i, $"CH{i - 1}", 0, 0, 50, 50));
            }
            return config;
        }

        /// <summary>
        /// 获取指定通道的配置
        /// </summary>
        public ChannelConfig GetChannel(int channelIndex)
        {
            if (channelIndex >= 0 && channelIndex < 16)
                return Channels[channelIndex];
            return null;
        }

        /// <summary>
        /// 设置指定通道的配置
        /// </summary>
        public void SetChannel(int channelIndex, string channelName, ushort phase, ushort pwm)
        {
            if (channelIndex >= 0 && channelIndex < 16)
            {
                Channels[channelIndex].ChannelName = channelName;
                Channels[channelIndex].Phase = phase;
                Channels[channelIndex].PWM = pwm;
            }
        }
    }

    /// <summary>
    /// TPS92664完整配置数据
    /// </summary>
    public class Tps92664ConfigData
    {
        /// <summary>
        /// 远光配置
        /// </summary>
        public BeamConfig HighBeam { get; set; }

        /// <summary>
        /// 上次使用的PWM百分比 (80-100)
        /// </summary>
        public int LastPwmPercentage { get; set; }

        public int KEEPONTS1 { get; set; }
        public int KEEPOFFTS1 { get; set; }
        public int KEEPONTS2 { get; set; }

        public Tps92664ConfigData()
        {
            // 不在构造函数中初始化HighBeam，避免JSON反序列化时重复添加
            HighBeam = null;
            LastPwmPercentage = 100; // 默认100%
            KEEPONTS1 = 1000;
            KEEPONTS2 = 2500;
            KEEPOFFTS1 = 1500;
        }

        /// <summary>
        /// 创建默认配置
        /// </summary>
        public static Tps92664ConfigData CreateDefault()
        {
            var config = new Tps92664ConfigData();
            config.HighBeam = BeamConfig.CreateDefault();
            config.LastPwmPercentage = 100;
            return config;
        }
    }
}

