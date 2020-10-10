using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Text;

namespace ControlPanel.Controllers
{
    public class ReportsController : Controller
    {
        // GET: Reports
        public FileResult Index()
        {
            string report = "qq";
            Encoding encoding = Encoding.ASCII;
            byte[] reportStream = encoding.GetBytes(report);
            //FileStream fs = new FileStream();
            string file_type = "application/csv";
            string file_name = "1.csv";
            return File(reportStream, file_type, file_name);
        }
    }
}