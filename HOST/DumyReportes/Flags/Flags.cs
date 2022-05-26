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
        ERROR_CREATION_ENITITY,
        ERROR_VALIDATION_ENTITY,
        ERROR_SERSSION_EXPIRED,
        ERROR_OK_RESULT,
        ERROR_INVALID_OBJECT,
        ERROR_INVALID_ID,
        ERROR_INVALID_TOKEN,
        ERROR_NOT_EXISTS,
        ERROR_RECORD_EXISTS,
        ERROR_DATABASE,
        ERROR_NO_DELETED_RECORDS,
        ERROR_NO_AFECTED_RECORDS,
        ERROR_RESULT,
        ERROR_DENIED_OPERATION,
        ERROR_NO_UPDATED_RECORDS,
        ERROR_INVALID_CREDENTIALS,
        ERROR_EXPIRED_TOKEN,
        ERROR_NO_FILE_TO_WRITE,
        ERROR_NO_FILE_TO_READ,
        ERROR_UNAUTHORIZED_ACCESS,
        ERROR_PARSE,
        FATAL,
        ERROR_NULL_TOKEN,
        ERROR_CONFLICT_CANT_DELETE,
        UNKNOWN
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
        ADMIN = 20,
        TI = 30
    }

    public class Flags
    {
        


    }
}