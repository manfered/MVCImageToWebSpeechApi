using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace MVCImageToWebSpeechApi.Models
{
    public class Uploader
    {
        private IEnumerable<HttpPostedFileBase> files;

        private string StorageDirectory = "UploadedFiles";

        public Uploader(IEnumerable<HttpPostedFileBase> _files)
        {
            files = _files;
        }

        public OperationResult Upload(OperationResult operationResult)
        {

            if (files != null)
            {
                foreach (var file in files)
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        operationResult.ImageFileName = file.FileName;
                        operationResult.ImageStorageDirectory = StorageDirectory;

                        var path = Path.Combine(HttpContext.Current.Server.MapPath("~/" + operationResult.ImageStorageDirectory), operationResult.ImageFileName);

                        //if file exists we assign a new sequence of charachters at the end of file name
                        if (System.IO.File.Exists(path))
                        {
                            operationResult.ImageFileName = Path.GetFileNameWithoutExtension(file.FileName) + "_" + Guid.NewGuid() + Path.GetExtension(file.FileName);
                            path = Path.Combine(HttpContext.Current.Server.MapPath("~/" + operationResult.ImageStorageDirectory), operationResult.ImageFileName);
                        }

                        try
                        {
                            file.SaveAs(path);
                            operationResult.ResultStr = "File successfully uploaded";
                            operationResult.StatusStr = OperationResultStatus.Success;
                        }
                        catch (Exception ex)
                        {
                            //exception message
                            operationResult.ResultStr = ex.Message;
                            operationResult.StatusStr = OperationResultStatus.Fail;

                            break;
                        }
                    }
                }
            }

            return operationResult;
        }
    }
}