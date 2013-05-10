using System;
using System.Collections.Generic;

namespace ConDep.Dsl.Operations.Application.Deployment.CopyFile
{
    public class FileInfoList : IOfferFileInfo
    {
        private readonly List<Tuple<string, string>> _files = new List<Tuple<string, string>>();
 
        public void Add(string srcFile, string destFile)
        {
            _files.Add(new Tuple<string, string>(srcFile, destFile));
        }
    }
}