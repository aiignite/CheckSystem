namespace CheckSystem.SmtForms.DataUploader
{
    public class TriAoi
    {
        public TriAoiBoard[] TriAoiBoard;
    }

    public class TriAoiBoard
    {
        public string Pname { get; set; }
        public string Barcode { get; set; }
        public string Type { get; set; }
        public string Position { get; set; }
        public string PbenTime { get; set; }
        public string EquipNo { get; set; }
        public string Status { get; set; }
        public string Pnum1 { get; set; }
        public string Pnum2 { get; set; }
        public string EndTime { get; set; }
        public string Pnum3 { get; set; }

        public TriFovComp[] TriFovComp { get; set; }
    }

    public class TriFovComp
    {
        public string Position { get; set; }
        public string PositionName { get; set; }
        public string Pnum0 { get; set; }
        public string MaterialNo { get; set; }
        public string Status { get; set; }
        public string Pnum1 { get; set; }
        public string Pnum2 { get; set; }
        public string Pnum3 { get; set; }
    }
}
