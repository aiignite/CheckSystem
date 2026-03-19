using System;
using System.Collections.Generic;
using System.Linq;
using NationalInstruments.Vision;
using NationalInstruments.Vision.Acquisition.Imaqdx;

namespace CheckSystem.VisionDetection.Vision
{
    public class NiImaqdx
    {
        public readonly List<NiImaqdxObj> ImaqdxObjs = new List<NiImaqdxObj>();

        public NiImaqdx()
        {
            var cameraList = ImaqdxSystem.GetCameraInformation(true);

            if (cameraList.Length <= 0)
                return;
            foreach (var camInfo in cameraList)
            {
                if (camInfo.Type == ImaqdxBusType.Ethernet)
                {
                    var niImaqdxObj = new NiImaqdxObj(camInfo.Name);
                    ImaqdxObjs.Add(niImaqdxObj);
                }
            }
        }

        public void OpenAllCamera()
        {
            foreach (var t in ImaqdxObjs)
                t.OpenCamera();
        }

        public void CloseAllCamera()
        {
            foreach (var t in ImaqdxObjs)
                t.CloseCamera();
        }

        public void Sequence(string deviceUserId)
        {

        }

        public void StartGrab(string deviceUserId, VisionImage image)
        {
            var c = ImaqdxObjs.Find(f => deviceUserId == f.DeviceUserId && !f.IsGrab);
            if (c != null)
            {
                c.StartGrab(image);
            }
        }

        public void StopGrab(string deviceUserId)
        {
            var c = ImaqdxObjs.Find(f => deviceUserId == f.DeviceUserId && f.IsGrab);
            if (c != null)
            {
                c.StopGrab();
            }
        }

        public class NiImaqdxObj
        {
            public string DeviceUserId;
            private readonly string _name;
            private ImaqdxSession _session;
            public bool IsGrab { get; private set; }
            public decimal ExposureTimeMin = 20;
            public decimal ExposureTimeMax = 1000000;

            public NiImaqdxObj(string name)
            {
                _name = name;
            }

            public void OpenCamera()
            {
                if (_session != null)
                    return;
                _session = new ImaqdxSession(_name);

                var deviceUserId = _session.Attributes.ToList().Find(f => f.Name.EndsWith("DeviceUserID"));
                if (deviceUserId != null)
                    DeviceUserId = deviceUserId.GetValue().ToString();

                var packetSize = _session.Attributes.ToList().Find(f => f.Name.EndsWith("PacketSize"));
                if (packetSize != null)
                {
                    var packetSizeCmd = (ImaqdxNumericAttribute)packetSize;
                    packetSizeCmd.Value = 1500;
                }

                //var vendor = _session.Attributes.ToList().Find(f => f.Name.EndsWith("Vendor"));
                //if (vendor != null)
                //    Vendor = vendor.GetValue().ToString();

                var exposureTime = _session.Attributes.ToList().Find(f => f.Name.EndsWith("ExposureTime"));
                var time = exposureTime as ImaqdxDoubleAttribute;
                if (time != null)
                {
                    ImaqdxNumericAttribute numericAttribute = time;

                    ExposureTimeMin = numericAttribute.Minimum;
                    ExposureTimeMax = numericAttribute.Maximum;
                }
                //_session.ConfigureGrab();
                //_session.GrabCompleted += _session_GrabCompleted;

                var pattern =
                    _session.Attributes.ToList()
                        .Find(f => f.Name.ToLower().EndsWith(@"Pattern".ToLower()));
                if (pattern != null)
                {
                    var attribute = (ImaqdxEnumAttribute)pattern;
                    //var ss = attribute.GetSupportedValues();

                    var values = attribute.GetSupportedValues();

                    // Use hardware value
                    //attribute.Value = attribute.GetSupportedValues().First(f => f.Name.ToLower().Contains("none"));
                    attribute.Value = attribute.GetSupportedValues().First(f => f.Name.ToLower().Contains("Use hardware value".ToLower()));
                }
            }

            public void CloseCamera()
            {
                if (_session == null)
                    return;

                IsGrab = false;
                _session.GrabCompleted -= _session_GrabCompleted;
                _session.Close();
                _session = null;
            }

            public void SetExposureTime(int exposureTime)
            {
                if (_session != null)
                {
                    var exposureTimeAttr =
                        _session.Attributes.ToList()
                            .Find(
                                f =>
                                    string.Equals(f.Name, "CameraAttributes::AcquisitionControl::ExposureTime",
                                        StringComparison.CurrentCultureIgnoreCase));
                    if (exposureTimeAttr != null)
                    {
                        var exposureTimeAttrCmd = (ImaqdxNumericAttribute)exposureTimeAttr;

                        if (exposureTime < ExposureTimeMin)
                        {
                            exposureTimeAttrCmd.Value = (int)ExposureTimeMin;
                            return;
                        }

                        if (exposureTime > ExposureTimeMax)
                        {
                            exposureTimeAttrCmd.Value = (int)ExposureTimeMax;
                            return;
                        }

                        exposureTimeAttrCmd.Value = exposureTime;
                    }
                }
            }

            public VisionImage[] Sequence(int count)
            {
                if (_session == null)
                    return new VisionImage[0];

                if (IsGrab)
                {
                    IsGrab = false;
                    _session.GrabCompleted -= _session_GrabCompleted;
                }

                // Create array of images
                // 超过300帧会报错
                // 大恒，第一张：01:20.13，第三百张：01:23.15
                // 海康，第一张：01:20.13，第三百张：01:22.56

                var images = new VisionImage[count];
                for (var i = 0; i < images.Length; ++i)
                    images[i] = new VisionImage();

                // Acquire the sequence of images
                _session.Sequence(images, count);

                return images;

                //// Configure and start the camera
                //var images = new VisionImage[count];
                //for (var i = 0; i < images.Length; ++i)
                //    images[i] = new VisionImage();
                //_session.Acquisition.Configure(ImaqdxAcquisitionType.Continuous, count);
                //_session.Acquisition.Start();

                //for (uint i = 0; i < images.Length; ++i)
                //{
                //    _session.Acquisition.GetImageAt(images[i], i);
                //}

                //// Stop, Unconfigure, and Close the camera
                //_session.Acquisition.Stop();
                //_session.Acquisition.Unconfigure();
                //return images;
            }

            public void StartGrab(VisionImage visionImage)
            {
                if (_session == null)
                    return;

                if (IsGrab)
                    return;
                IsGrab = true;

                _session.ConfigureGrab();
                _session.GrabCompleted += _session_GrabCompleted;

                //if (DeviceUserId.ToLower()!="hik2")
                //{
                //    return;
                //}

                //lock (_lockGrab)
                _session.GrabAsync(visionImage, true, null);
            }

            public void StopGrab()
            {
                if (_session == null)
                    return;

                if (!IsGrab)
                    return;

                IsGrab = false;
                //_session.s
                _session.GrabCompleted -= _session_GrabCompleted;

                var tempBuff = new VisionImage[1];
                _session.Sequence(tempBuff, 1);
                foreach (var t in tempBuff)
                    t.Dispose();
            }

            private void _session_GrabCompleted(object sender, ImaqdxGrabEventArgs e)
            {
                //lock (_lockGrab)
                {
                    var session = sender as ImaqdxSession;
                    if (session != null)
                    {
                        //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ffff"));
                        //var temp = new VisionImage();
                        //Algorithms.Copy(e.Image, temp);
                        //lock (_locker)
                        //{
                        //    queue.Enqueue(temp);
                        //}
                        //_wh.Set();
                        //_buffer.Add(temp);

                        //Console.WriteLine("name={0},cout={1}", DeviceUserId, _buffer.Count);

                        //Console.WriteLine("{0}: name={1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ffff"), DeviceUserId);
                        session.GrabAsync(e.Image, true, null);
                    }
                }
            }
        }
    }
}
