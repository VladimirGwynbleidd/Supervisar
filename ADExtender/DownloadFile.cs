using System;
using System.IO;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADExtender
{
    public class DownloadFile : IHttpHandler
    {
        public void ProcessRequest(HttpContext context, byte[] fileBytes)
        {
            string mediaName = "myFile.zip"; // 600MB in file size
            if (string.IsNullOrEmpty(mediaName))
            {
                return;
            }

            string destPath = context.Server.MapPath("~/Downloads/" + mediaName);
            // Check to see if file exist
            FileInfo fi = new FileInfo(destPath);
            FileStream fs;

            try
            {
                if (fi.Exists)
                {
                    fs = fi.Create();
                    fs.Write(fileBytes, 0, fileBytes.Length);
                    HttpContext.Current.Response.ClearHeaders();
                    HttpContext.Current.Response.ClearContent();
                    HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename=" + fi.Name);
                    HttpContext.Current.Response.AppendHeader("Content-Length", fi.Length.ToString());
                    HttpContext.Current.Response.ContentType = "application/octet-stream";
                    HttpContext.Current.Response.TransmitFile(fi.FullName);
                    HttpContext.Current.Response.Flush();
                }
            }
            catch (Exception exception)
            {
                HttpContext.Current.Response.ContentType = "text/plain";
                HttpContext.Current.Response.Write(exception.Message);
            }
            finally
            {
                HttpContext.Current.Response.End();
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

    }
}
