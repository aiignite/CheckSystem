namespace CheckSystem.SmtForms.DataUploader
{
    public class UploadPackage
    {
        public string ProcessName { get; set; }
        public string MaterialName { get; set; }
        public string MaterialUid { get; set; }
        public string MaterialBarcode { get; set; }
        public string MaterialCustomerBarcode { get; set; }
        public string SubMaterialsInfo { get; set; }
        public string DeviceNo { get; set; }
        public string CheckData { get; set; }
        public string CheckDateTiem { get; set; }
        public string Result { get; set; }
        public string Note { get; set; }
        public string MaterialNo { get; set; }
        public string MaterialLotNo { get; set; }
        public string DeviceName { get; set; }
    }

    public class UploadDetail
    {
        public string Type { get; set; }
        public string ParaName { get; set; }
        public string Range { get; set; }
        public string Value { get; set; }
        public string Result { get; set; }
    }
}
