using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using IO = System.IO;

namespace xxAMIDOxx.xxSTACKSxx.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Samples")]
    public class FilesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult Get(string path = null)
        {
            Log($"Received Arg: {path}");

            Log($"Current Directory: {IO.Directory.GetCurrentDirectory()}");
            if (string.IsNullOrEmpty(path))
                path = IO.Directory.GetCurrentDirectory();

            List<string> filesAndDirecotories = new List<string>();

            if (IO.File.Exists(path))
            {
                //string relativePath = path.Replace($"{IO.Directory.GetCurrentDirectory()}{IO.Path.PathSeparator}","");
                //Log($"Loading file: {relativePath}");

                IFileProvider provider = null;
                if (IO.Path.IsPathRooted(path))
                    provider = new PhysicalFileProvider(IO.Path.GetPathRoot(path));
                else
                    provider = new PhysicalFileProvider(IO.Directory.GetCurrentDirectory());

                IFileInfo fileInfo = provider.GetFileInfo(path);
                var readStream = fileInfo.CreateReadStream();

                return File(readStream, "text/plain");
            }
            else if (IO.Directory.Exists(path))
            {
                filesAndDirecotories.AddRange(IO.Directory.GetDirectories(path));
                filesAndDirecotories.AddRange(IO.Directory.GetFiles(path));
            }
            return Ok(filesAndDirecotories);
        }

        void Log(string text)
        {
            Console.WriteLine(text);
        }
    }
}
