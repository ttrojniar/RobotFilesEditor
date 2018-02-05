﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RobotFilesEditor
{
    public class Operations
    {
        public List<DataOperation> DataOperations;
        public List<FileOperation> FilesOperations;
        public FileOperation ActiveFileOperation;
        public DataOperation ActiveDataOperation;
        public List<string> OperatedFiles;
        public List<FileLineProperties> OperatedDataFiles;

        public Operations()
        {
            DataOperations = new List<DataOperation>();
            FilesOperations = new List<FileOperation>();
        }
        public void FollowOperation(string operation)
        {             
            List<FileOperation>fileOperations = FilesOperations.Where(x => x.OperationName==operation).ToList();
            string NestedSourcePath = "";

            foreach(FileOperation fileOperation in fileOperations)
            {
                if(fileOperation.NestedSourcePath)
                {
                    fileOperation.SourcePath=NestedSourcePath;
                }              

                if (fileOperation.ActionType.ToString().Contains("Data"))
                {
                    List<string>filesToPrepare=fileOperation.FollowOperation();
                    DataOperation operationData = DataOperations.FirstOrDefault(x => x.OperationName == fileOperation.OperationName && x.Priority==fileOperation.Priority);
                    operationData.FollowOperation(filesToPrepare, Path.Combine(fileOperation.DestinationPath));
                }else
                {
                    fileOperation.FollowOperation();
                }
                NestedSourcePath = Path.Combine(fileOperation.DestinationPath, fileOperation.DestinationFolder);
            }           
        }

        public List<string> ShowFilesToChanged()
        {
            List<string> filesToChange = new List<string>();



            return filesToChange;
        }
    }
}