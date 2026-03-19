using CommonUtility;
using System.Collections.Generic;
using System.ComponentModel;

namespace Controller
{
    public sealed class CycloneController : ControllerBase
    {
        [Description("R,烧写结果")]
        public string ProgramResult;

        public CycloneController(string name)
            : base(name) { }

        ~CycloneController()
        {
            Dispose();
        }

        private string DeviceName { get; set; }
        private bool IsConnected { get; set; }
        private uint Handle { get; set; }

        public void ConnectDevice(string name)
        {
            DeviceName = name;
            Handle = CycloneControlApi.ConnectToCyclone(DeviceName);
            IsConnected = Handle != 0;
        }

        [Description("按索引启动烧写")]
        public void StartProgram(string imageIndex)
        {
            ProgramResult = @"NG";

            byte imgIndexByte;
            if (!byte.TryParse(imageIndex, out imgIndexByte) || !IsConnected)
                return;

            if (string.IsNullOrEmpty(CycloneControlApi.GetImageDescription(Handle, imgIndexByte)))
            {
                ProgramResult += " No Image";
                return;
            }

            CycloneControlApi.StartImageExecution(Handle, imgIndexByte);
            var cycloneDone = false;

            do
            {
                if (CycloneControlApi.checkCycloneExecutionStatus(Handle) != 0)
                    continue;

                if (CycloneControlApi.getNumberOfErrors(Handle) == 0)
                    ProgramResult = @"OK";
                else
                    ProgramResult += " Error Code " + CycloneControlApi.getErrorCode(Handle, 1);
                cycloneDone = true;
            } while (!cycloneDone);
        }

        [Description("按名称启动烧写")]
        public void StartProgramByName(string imageName)
        {
            ProgramResult = @"NG";

            var programNameList = new List<string>();

            uint imageIndex = 1;

            while (true)
            {
                var name = CycloneControlApi.GetImageDescription(Handle, imageIndex);

                if (!string.IsNullOrEmpty(name))
                {
                    programNameList.Add(name);
                }
                else
                {
                    break;
                }

                imageIndex += 1;
            }

            if (programNameList.Count == 0)
            {
                ProgramResult += " No Image";
                return;
            }

            if (!programNameList.Contains(imageName) || !IsConnected)
                return;

            var imgIndexByte = (byte)(programNameList.IndexOf(imageName) + 1);

            CycloneControlApi.StartImageExecution(Handle, imgIndexByte);

            var cycloneDone = false;

            do
            {
                if (CycloneControlApi.checkCycloneExecutionStatus(Handle) != 0)
                    continue;

                if (CycloneControlApi.getNumberOfErrors(Handle) == 0)
                {
                    ProgramResult = @"OK";
                }
                else
                {
                    ProgramResult += " Error Code " + CycloneControlApi.getErrorCode(Handle, 1);
                }

                cycloneDone = true;

            } while (!cycloneDone);
        }
    }
}
