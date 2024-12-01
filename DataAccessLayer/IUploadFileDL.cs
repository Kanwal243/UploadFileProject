using UploadFileProject.CommonLayer.Model;

namespace UploadFileProject.DataAccessLayer
{
    public interface IUploadFileDL
    {
        public Task<UploadExcelFileResponse> UploadExcelFile(UploadExcelFileRequest request,string Path);
        public Task<ReadRecordResponse> ReadRecord();
    }
}
