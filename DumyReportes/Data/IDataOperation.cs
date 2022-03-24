using DumyReportes.Models;
using System.Collections.Generic;

namespace DumyReportes.Data
{
    internal interface IDataOperation
    {
        Flags.ErrorFlag Get(int id, out IReportObject reportObject, out string error);
        Flags.ErrorFlag GetAll(out List<IReportObject> reportObjects, out string error);

        IReportObject InstanceFromReader(System.Data.SqlClient.SqlDataReader reader);

        Flags.ErrorFlag Create(IReportObject reportObject, out string error);

        Flags.ErrorFlag Update(IReportObject reportObject, out string error);

        Flags.ErrorFlag Delete(int id, out string error);

        Flags.ErrorFlag Exists(int id, out string error);

        Flags.ErrorFlag Detail(int id, out List<IReportObject> reportObjects, out string error);

    }
}