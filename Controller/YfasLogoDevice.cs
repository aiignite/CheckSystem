namespace Controller
{
    public sealed class YfasLogoDevice : ControllerBase
    {
        public YfasLogoDevice(string name)
            : base(name) { }

        public bool Execute;
        public bool Complete;
        public bool IsOk;

        public bool IsPingBiQiGangDaoWeiXinHao;

        public int ProductType;

        public bool IsScanEnable;

        public bool IsInReset;
        public bool IsInAuto;
        public bool IsLedOpen;
        public bool IsInConfig;

        public bool IsMatch10Min;
    }
}