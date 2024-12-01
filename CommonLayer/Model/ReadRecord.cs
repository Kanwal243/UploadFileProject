namespace UploadFileProject.CommonLayer.Model
{
    public class ReadRecord
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string EmailID { get; set; }
        public string MobileNumber { get; set; }
        public int Age { get; set; }
        public int Salary { get; set; }
        public string Gender { get; set; }
    }
    public class ReadRecordRequest
    {
    }
    public class ReadRecordResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public List<ReadRecord> data { get; set; }
    }
}
