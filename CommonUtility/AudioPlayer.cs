using NAudio.Wave;

namespace CommonUtility
{
    public class AudioPlayer
    {
        private IWavePlayer _waveOutDevice;
        private AudioFileReader _audioFileReader;

        public AudioPlayer(string fileName)
        {
            // 初始化播放设备
            _waveOutDevice = new WaveOutEvent();
            // 打开要播放的文件
            _audioFileReader = new AudioFileReader(fileName);
            _waveOutDevice.Init(_audioFileReader);
        }

        public void Play()
        {
            // 开始播放
            _waveOutDevice.Play();
        }

        public void Pause()
        {
            // 暂停播放
            _waveOutDevice.Pause();
        }

        public void Stop()
        {
            // 完全停止播放
            _waveOutDevice.Stop();
        }

        public void Replay()
        {
            // 重播音频
            _audioFileReader.Position = 0; // 重置位置到文件开始
            _waveOutDevice.Play(); // 从头播放
        }

        public void Dispose()
        {
            // 垃圾回收音频资源
            if (_waveOutDevice != null)
            {
                _waveOutDevice.Dispose();
                _waveOutDevice = null;
            }

            if (_audioFileReader != null)
            {
                _audioFileReader.Dispose();
                _audioFileReader = null;
            }
        }
    }
}
