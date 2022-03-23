using DumyReportes.Flags;
using DumyReportes.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DumyReportes.Data
{
    class ReportDataContext : IDataOperation
    {
        public ErrorFlag Create(IReportObject reportObject, out string error)
        {
            throw new NotImplementedException();
        }

        public ErrorFlag Delete(int id, out string error)
        {
            throw new NotImplementedException();
        }

        //Get specific report details
        public ErrorFlag Detail(int id, out string error)
        {
            throw new NotImplementedException();
        }

        public ErrorFlag Exists(int id, out string error)
        {
            throw new NotImplementedException();
        }
        public ErrorFlag Get(int iduser, out IReportObject reportObject, out string error)
        {
            throw new NotImplementedException();
        }

        public static string QUERY_GET_REPORT_BY_USER_WHONOTIFIED =
            @"
                SELECT Rep.IdReport, Rep.IdUserWhoNotified, ,Rep.NotifiedDT,Loc.IdLocation, Loc.[Description], Loc.lat, Loc.long, 
                RS.IdStatus, RS.titleStatus, U.IdUser, U.NumEmpleado
                FROM 
	                [Report] Rep
                  LEFT JOIN [Location] Loc
	                on Rep.IdLocation = Loc.IdLocation
                  Left JOIN ReportStatus RS
	                ON Rep.IdStatus = RS.IdStatus
                  LEFT JOIN [User] U
	                ON Rep.IdUserWhoNotified = U.IdUser
                Where U.IdUser = @IdUser
            ";

        public ErrorFlag GetAllBy(bool isOwner , int idUser ,out List<IReportObject> reportObjects, out string error)
        {
            error = "";
            ErrorFlag result;





        }

        public ErrorFlag GetAll(out List<IReportObject> reportObjects, out string error)
        {
            throw new NotImplementedException();
        }

        public IReportObject InstanceFromReader(SqlDataReader reader)
        {
            throw new NotImplementedException();
        }

        public ErrorFlag Update(IReportObject reportObject, out string error)
        {
            throw new NotImplementedException();
        }
    }
}
