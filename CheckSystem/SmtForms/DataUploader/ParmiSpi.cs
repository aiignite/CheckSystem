namespace CheckSystem.SmtForms.DataUploader
{
    public class ParmiSpi
    {
        public Info Info { get; set; }
        public PanelInspResult PanelInspResult { get; set; }

        public BoardInspResult[] BoardInspResult { get; set; }

        public ComponentInspResult[] ComponentInspResult { get; set; }
    }

    public class Info
    {
        public string LineName { get; set; }
        public string MachineSn { get; set; }
        public string MachineName { get; set; }
        public string OperatorId { get; set; }
    }

    public class PanelInspResult
    {
        public string ModelName { get; set; }
        public string ModelCode { get; set; }
        public string PanelSide { get; set; }
        public string Index { get; set; }
        public string Date { get; set; }
        public string Barcode { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string DefectName { get; set; }
        public string DefectCode { get; set; }
        public string Result { get; set; }
    }

    public class BoardInspResult
    {
        public string BoardNo { get; set; }
        public string Barcode { get; set; }
        public string DefectName { get; set; }
        public string DefectCode { get; set; }
        public string Badmark { get; set; }
        public string Result { get; set; }
    }

    public class ComponentInspResult
    {
        public string BoardNo { get; set; }
        public string LocationName { get; set; }
        public string PinNumber { get; set; }
        public string PosX { get; set; }
        public string PosY { get; set; }
        public string DefectName { get; set; }
        public string DefectCode { get; set; }
        public string Result { get; set; }
    }
}
