using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DumyReportes.Flags
{
    public enum ErrorFlag
    {
        ERROR_CONNECTION_DB,
        ERROR_CONNECTION_SERVER,
        ERROR_CREATION_ENITIY,
        ERROR_VALIDATION_ENTITY,
        ERROR_SERSSION_EXPIRED,
        ERROR_OK_RESULT,
        ERROR_RESULT 
           
    }

    public enum ReportStatus
    {
        STATUS_ESPERA,
        STATUS_EN_PROCESO,
        STATUS_COMPLETADA,
    }

    //Cambiar para que lo refleje como en DB
    public enum AccessLevel
    {
        PUBLIC = 0,
        AGENT = 10,
        ADMIN = 20
    }

    public class Flags
    {
        


    }
}