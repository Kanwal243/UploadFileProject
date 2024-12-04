using UploadFileProject.CommonLayer.Model;

namespace UploadFileProject.DataAccessLayer
{
    public interface IUploadFileDL
    {
        public Task<UploadExcelFileResponse> UploadExcelFile(UploadExcelFileRequest request,string Path);
        public Task<ReadRecordResponse> ReadRecord(ReadRecordRequest request);
        public Task<DeleteRecordResponse> DeleteRecord(DeleteRecordRequest request);
        public Task<UploadCsvFileResponse> UploadCsvFile(UploadCsvFileRequest request,string Path);


    }
}
