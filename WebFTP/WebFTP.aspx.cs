using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebFTP
{
    public partial class WebFTP : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string errmsg = null;

            if (Request.HttpMethod == "POST")
            {
                var command = Request.Params["command"];
                var argpath = Path.Combine(WebConfigurationManager.AppSettings["FTP_ROOT"], Request.Params["argpath"] ?? "");

                if (command == "ls")
                {
                    if (File.Exists(argpath))
                    {
                        Response.Write(Path.GetFileName(argpath));
                    }
                    else if (Directory.Exists(argpath))
                    {
                        Response.Write(string.Join("\n",
                            Directory.GetDirectories(argpath).Select(o => Path.GetFileName(o) + "/").Concat(Directory.GetFiles(argpath).Select(o => Path.GetFileName(o)))
                        ));
                    }
                    else
                    {
                        errmsg = "File Not Exist";
                    }
                }
                else if (command == "put")
                {
                    if (Request.Files.Count == 0)
                    {
                        errmsg = "No File Uploaded";
                    }
                    else if (Request.Files.Count > 1)
                    {
                        errmsg = "More Than One File Uploaded";
                    }
                    else if (Directory.Exists(argpath))
                    {
                        Request.Files[0].SaveAs(Path.Combine(argpath, Request.Files[0].FileName));
                    }
                    else if (!Directory.Exists(Path.GetDirectoryName(argpath)))
                    {
                        errmsg = "Folder Not Exist";
                    }
                    else
                    {
                        Request.Files[0].SaveAs(argpath);
                    }
                }
                else if (command == "get")
                {
                    if (File.Exists(argpath))
                    {
                        Response.Clear();
                        Response.ContentType = "application/octet-stream";
                        Response.AppendHeader("Content-Disposition", string.Format("attachment;filename={0}", Path.GetFileName(argpath)));
                        Response.Flush();
                        Response.TransmitFile(argpath);
                        Response.End();
                    }
                    else if (Directory.Exists(argpath))
                    {
                        errmsg = "Not a File";
                    }
                    else
                    {
                        errmsg = "File Not Exist";
                    }
                }
                else if (command == "delete")
                {
                    if (File.Exists(argpath))
                    {
                        File.Delete(argpath);
                    }
                    else if (Directory.Exists(argpath))
                    {
                        errmsg = "Not a File";
                    }
                    else
                    {
                        errmsg = "File Not Exist";
                    }
                }
                else if (command == "mkdir")
                {
                    if (File.Exists(argpath))
                    {
                        errmsg = "File Exist";
                    }
                    else if (Directory.Exists(argpath))
                    {
                        errmsg = "Folder Exist";
                    }
                    else
                    {
                        Directory.CreateDirectory(argpath);
                    }
                }
                else if (command == "rmdir")
                {
                    if (File.Exists(argpath))
                    {
                        errmsg = "Not a Folder";
                    }
                    else if (Directory.Exists(argpath))
                    {
                        if (Directory.EnumerateFileSystemEntries(argpath).Any())
                        {
                            errmsg = "Folder Not Empty";
                        }
                        else
                        {
                            Directory.Delete(argpath);
                        }
                    }
                    else
                    {
                        errmsg = "Folder Not Exist";
                    }
                }
                else
                {
                    errmsg = "Invalid Command";
                }
            }
            else
            {
                errmsg = "Bad Request";
            }

            if (errmsg != null)
            {
                Response.StatusCode = 400;
                Response.Write(errmsg);
            }
        }
    }
}