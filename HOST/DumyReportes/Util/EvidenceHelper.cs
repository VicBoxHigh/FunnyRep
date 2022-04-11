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
            ErrorFlag resultCreation = CreateEvidenceImg(report.Pic64, out string fileName, out string path);
            report.PathEvidence = "";
            report.FileNameEvidence = "";
            if (resultCreation == ErrorFlag.ERROR_OK_RESULT)
            {
                report.FileNameEvidence = fileName;
                report.PathEvidence = path;

            }

            return resultCreation;



        }

        public static Flags.ErrorFlag CreateEvidenceImg(ReportDtlEntry reportDtlEntry)
        {

            ErrorFlag resultCreation = CreateEvidenceImg(reportDtlEntry.Pic64, out string fileName, out string path);
            reportDtlEntry.PathEvidence = "";
            reportDtlEntry.FileNameEvidence = "";
            if (resultCreation == ErrorFlag.ERROR_OK_RESULT)
            {
                reportDtlEntry.PathEvidence = path;
                reportDtlEntry.FileNameEvidence = fileName;


            }

            return resultCreation;

        }

        //filename = random, path the same
        private static ErrorFlag CreateEvidenceImg(string picB64, out string fileName, out string path)
        {
            ErrorFlag resultCreation = ErrorFlag.ERROR_OK_RESULT;
            fileName = Guid.NewGuid().ToString() + ".png";

            path = PATH_IMGS;

            if (String.IsNullOrEmpty(picB64)) resultCreation = ErrorFlag.ERROR_NO_FILE_TO_WRITE;

            else
                try
                {
                    File.WriteAllBytes(PATH_IMGS + fileName, Convert.FromBase64String(picB64));

                }
                catch (DirectoryNotFoundException dnfe)
                {
                    resultCreation = ErrorFlag.ERROR_NOT_EXISTS;
                }
                catch (UnauthorizedAccessException uae)
                {
                    resultCreation = ErrorFlag.ERROR_UNAUTHORIZED_ACCESS;
                }

            return resultCreation;
        }




        //Read img file and return it as a base64string
        public static Flags.ErrorFlag GetEvidenceImg(string fileName, out string base64Img)
        {

            ErrorFlag resultRead = ErrorFlag.ERROR_OK_RESULT;

            if (String.IsNullOrEmpty(fileName)) { base64Img = ""; return resultRead; }
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