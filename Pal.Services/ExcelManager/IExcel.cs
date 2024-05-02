using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Services.ExcelManager
{
    public interface IExcel
    {

        string Documentupload(IFormFile formFile);
        DataTable DocumentDataTable(string path);
        DataTable ImportDocument(DataTable dt, string cmdText, DataTable subDt = null);
        DataTable GetDataFromExcel(string path, dynamic worksheet);
        DataTable excelToDt(string filePath);

    }
}
