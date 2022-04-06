using DumyReportes.Flags;
using DumyReportes.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace DumyReportes.Util
{
    //Maneja I/O de las fotos de evidencia
    public class EvidenceHelper
    {
        private static string PATH_IMGS = "C:\\imgs\\";

        public static string lastImgSaved { get; set; }
        //Write img file from base64 img
        public static Flags.ErrorFlag CreateEvidenceImg(Report report)
        {
            ErrorFlag resultCreation = ErrorFlag.ERROR_OK_RESULT;
            string fileName = Guid.NewGuid().ToString() + ".png";

            string evidence = PATH_IMGS;

            if (String.IsNullOrEmpty(report.Pic64)) resultCreation = ErrorFlag.ERROR_NO_FILE_TO_WRITE;

            else
                try
                {
                    File.WriteAllBytes(report.PathEvidence + report.FileNameEvidence, Convert.FromBase64String(report.Pic64));

                }
                catch (DirectoryNotFoundException dnfe)
                {
                    resultCreation = ErrorFlag.ERROR_NOT_EXISTS;
                }

            return resultCreation;

        }

        //Read img file and return it as a base64string
        public static Flags.ErrorFlag GetEvidenceImg(string fileName, out string base64Img)
        {

            ErrorFlag resultRead = ErrorFlag.ERROR_OK_RESULT;

            base64Img = null;

            byte[] imgBytes = null;

            try
            {
                imgBytes = File.ReadAllBytes(PATH_IMGS + fileName);

            }
            catch (FileNotFoundException fnfe)
            {
                resultRead = ErrorFlag.ERROR_NOT_EXISTS;
            }

            if (imgBytes == null || imgBytes.Length == 0) resultRead = Flags.ErrorFlag.ERROR_NO_FILE_TO_READ;
            else base64Img = Convert.ToBase64String(imgBytes);

            return resultRead;

        }


    }
}