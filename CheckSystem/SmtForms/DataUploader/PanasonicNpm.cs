namespace CheckSystem.SmtForms.DataUploader
{
    public class PanasonicNpm
    {
        public string Ip { get; set; }
        public string LotNameLane1 { get; set; }
        public string MachineName { get; set; }
        public Arrangement[] Arrangements { get; set; }
    }

    public class Arrangement
    {
        public string Pu { get; set; }
        public string Side { get; set; }
        public string SerialNo { get; set; }
    }
}
