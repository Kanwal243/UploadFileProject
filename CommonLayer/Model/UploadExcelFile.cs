namespace UploadFileProject.CommonLayer.Model
{
    public class UploadExcelFileRequest
    {
        public IFormFile File { get; set; }
    }
    public class UploadExcelFileResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
    public class ExcelBulkUploadParameter
    {
        public string UserName { get; set; }
        public string EmailID { get; set; }
        public string MobileNumber { get; set; }
        public int Age { get; set; }
        public int Salary { get; set; }
        public string Gender { get; set; }
    }
}
