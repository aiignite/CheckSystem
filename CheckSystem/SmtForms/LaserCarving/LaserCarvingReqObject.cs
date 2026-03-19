namespace CheckSystem.SmtForms.LaserCarving
{
    public class LaserCarvingReqObject
    {
        public int code { get; set; }
        public string info { get; set; }
        public Data data { get; set; }
    }

    public class Data
    {
        public string RES { get; set; }
        public string PROGRAM_NAME { get; set; }
        public object NG { get; set; }
        public SN_DETAIL[] SN_DETAIL { get; set; }
    }

    public class SN_DETAIL
    {
        public string SN { get; set; }
        public string CODE1 { get; set; }
        public string CODE2 { get; set; }
        public string CODE3 { get; set; }
        public string CODE4 { get; set; }
        public string CODE5 { get; set; }
        public string CODE6 { get; set; }
    }
}
