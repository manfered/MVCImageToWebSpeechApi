using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tesseract;

namespace MVCImageToWebSpeechApi.Models
{
    public class OcrClass
    {
        private string TessDataDir { get; set; }
        private string ImageAddress { get; set; }

        public OcrClass(string _imageDirectory, string _imageFileNAme)
        {
            TessDataDir = HttpContext.Current.Server.MapPath(@"~/tessdata");
            ImageAddress = HttpContext.Current.Server.MapPath(@"~/" + _imageDirectory + "/" + _imageFileNAme);
        }

        public void Ocr(OperationResult operationResult)
        {
            using (var engine = new TesseractEngine(TessDataDir, "eng", EngineMode.Default))
            using (var image = Pix.LoadFromFile(ImageAddress))
            using (var page = engine.Process(image))
            {
                try
                {
                    operationResult.textToSpeechString = page.GetText();
                    operationResult.ResultStr += "\r\nFile successfully read";
                    operationResult.StatusStr = OperationResultStatus.Success;
                }
                catch (Exception ex)
                {
                    operationResult.StatusStr = OperationResultStatus.Fail;
                    operationResult.ResultStr = $"Unable to read text from image file  - error : {ex.Message}";
                }
            }
        }
    }
}