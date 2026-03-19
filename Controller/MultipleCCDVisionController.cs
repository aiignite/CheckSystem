namespace Controller
{
    public sealed class MultipleCCDVisionController : ControllerBase
    {
        public MultipleCCDVisionController(string name) : base(name)
        {
        }

        /// <summary>
        /// 当前使用的载具
        /// </summary>
        public string UsingCarrier { get; set; }
    }
}
