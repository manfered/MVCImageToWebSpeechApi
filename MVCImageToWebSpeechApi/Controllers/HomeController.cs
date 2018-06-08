using MVCImageToWebSpeechApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MVCImageToWebSpeechApi.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(IEnumerable<HttpPostedFileBase> files)
        {
            ///
            ///
            //step 1 : upload
            ///
            ///

            OperationResult operationResult = new OperationResult();
            Uploader uploader = new Uploader(files);
            uploader.Upload(operationResult);

            if (operationResult.StatusStr == OperationResultStatus.Fail)
            {
                // if file upload was not successful 
                // we return a JSON to the INDEX controller as a result of failure
                // and the process will stop here
                return FailJSON(operationResult);


            }


            ///
            ///
            //step 2 : ocr
            ///
            ///

            OcrClass ocr = new OcrClass(operationResult.ImageStorageDirectory, operationResult.ImageFileName);
            ocr.Ocr(operationResult);

            if (operationResult.StatusStr == OperationResultStatus.Fail)
            {
                // if file OCR process on the image file was not successfull
                // we return a JSON to the INDEX controller as a result of failure
                // and the process will stop here
                return FailJSON(operationResult);


            }


            ///
            ///
            //step 3 : speech
            ///
            ///

            Task<JsonResult> task = Task.Run(() =>
            {
                //SpeechClass speechClass = new SpeechClass();
                //speechClass.speech(operationResult);

                //using (SpeechSynthesizer speechSynthesizer = new SpeechSynthesizer())
                //{
                //    string fileName = "ImageToSpeech_" + Guid.NewGuid();
                //    speechSynthesizer.SetOutputToWaveFile(Server.MapPath("~/SpeechFiles/") + fileName + ".wav");
                //    speechSynthesizer.Speak(operationResult.textToSpeechString);
                //}

                //letting the server master file save the image on disk
                Task.Delay(500);

                if (operationResult.StatusStr == OperationResultStatus.Success)
                {
                    return SuccessJSON(operationResult);
                }
                else
                {
                    return FailJSON(operationResult);
                }

            });


            return await task;
        }

        private JsonResult SuccessJSON(OperationResult operationResult)
        {
            return Json(new
            {
                status = "Success",
                Data = operationResult.ResultStr,
                src = "../" + operationResult.ImageStorageDirectory + "/" + operationResult.ImageFileName,
                imageFileName = operationResult.ImageFileName,
                audioDirectory = operationResult.AudioStorageDirectory,
                audioFileName = operationResult.AudioFileName + ".wav",
                textToSpeechString = operationResult.textToSpeechString,
            }, JsonRequestBehavior.AllowGet);
        }

        private JsonResult FailJSON(OperationResult operationResult)
        {
            return Json(new
            {
                status = "Fail",
                Data = operationResult.ResultStr
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Farzad(Fred) Seifi";

            return View();
        }



    }
}