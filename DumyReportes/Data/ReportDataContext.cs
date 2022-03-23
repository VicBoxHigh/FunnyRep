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
        public ErrorFlag Exists(int id, out string error)
        {
            throw new NotImplementedException();
        }
        public ErrorFlag Get(int iduser, out IReportObject reportObject, out string error)
        {
            throw new NotImplementedException();
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
