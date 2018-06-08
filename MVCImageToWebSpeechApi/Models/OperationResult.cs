using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCImageToWebSpeechApi.Models
{
    public class OperationResult
    {
        public string ImageFileName { get; set; }
        public string ResultStr { get; set; }
        public OperationResultStatus StatusStr { get; set; }
        public string ImageStorageDirectory { get; set; }
        public string textToSpeechString { get; set; }
        public string AudioStorageDirectory { get; set; }
        public string AudioFileName { get; set; }

        public OperationResult()
        {
            ImageFileName = string.Empty;
            ResultStr = string.Empty;
            StatusStr = OperationResultStatus.Fail;
            ImageStorageDirectory = string.Empty;
            textToSpeechString = string.Empty;
            AudioStorageDirectory = string.Empty;
            AudioFileName = string.Empty;
        }

    }

    public enum OperationResultStatus
    {
        Fail = 0,
        Success = 1
    }
}