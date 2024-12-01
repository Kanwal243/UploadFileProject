﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UploadFileProject.CommonLayer.Model;
using UploadFileProject.DataAccessLayer;

namespace UploadFileProject.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class UploadFileController : ControllerBase
    {
        public readonly IUploadFileDL _uploadFileDL;
        public UploadFileController(IUploadFileDL uploadFileDL)
        {
            _uploadFileDL = uploadFileDL;
        }
        [HttpPost]
        public async Task<IActionResult> UploadExcelFile([FromForm]UploadExcelFileRequest request)
        {
            UploadExcelFileResponse response = new UploadExcelFileResponse();
            string Path = "UploadFileFolder/" + request.File.FileName;
            try
            {
                using (FileStream stream = new FileStream(Path, FileMode.CreateNew))
                {
                    await request.File.CopyToAsync(stream);
                }
                response = await _uploadFileDL.UploadExcelFile(request, Path);
                string[]files = Directory.GetFiles(Path);
                foreach (string file in files)
                {
                    System.IO.File.Delete(file);
                    Console.WriteLine($"{file} is Deleted");
                }

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return Ok(response);
        }
        [HttpGet]
        public async Task<IActionResult> ReadRecord()
        {
            ReadRecordResponse response = new ReadRecordResponse();
            try
            {
                response = await _uploadFileDL.ReadRecord();
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                
            }
            return Ok(response);
        }
    }
}
