﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotFilesEditor
{
    public class Controler: INotifyPropertyChanged//: IControler
    {
        //public FilesOrganizer _productionCopiedFiles;
        //public FilesOrganizer _serviceCopiedFiles;
        //public FilesOrganizer _copiedOlpDataFiles;
        //public FilesOrganizer _copiedGlobalDataFiles;
        //public FilesOrganizer _removingDataFiles;    

        public string ContolerType
        {
            get { return _contolerType; }
            set
            {
                if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))                  
                {
                    throw new ArgumentNullException(nameof(ContolerType));
                }

                if(_contolerType!=value)
                {
                    _contolerType = value;
                    OnPropertyChanged(nameof(_contolerType));
                }
            }
        }

        public List<FilesOrganizer>Files
        {
            get { return _files; }
            set
            {
                if(value==null)
                {
                    _files=new List<FilesOrganizer>();
                }

                if(_files!=value && value!=null)
                {
                    _files = value;
                    OnPropertyChanged(nameof(Files));
                }
            }
        }

        private string DestinationPath
        {
            get { return _destinationPath; }
            set
            {
                if(string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException(nameof(DestinationPath));
                }

                if (_destinationPath!=value)
                {
                    if (Directory.Exists(value))
                    {
                        _destinationPath = value;
                        OnPropertyChanged(nameof(DestinationPath));
                    }else
                    {
                        throw new DirectoryNotFoundException($"Directory \'{value} \'not exist!");
                    }                
                }                     
            }
        }

        private string SourcePath
        {
            get { return _sourcePath; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException(nameof(DestinationPath));
                }

                if (_sourcePath != value)
                {
                    if (Directory.Exists(value))
                    {
                        _sourcePath = value;
                        OnPropertyChanged(nameof(SourcePath));
                    }
                    else
                    {
                        throw new DirectoryNotFoundException($"Directory: \'{value} \'not exist!");
                    }
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private List<FilesOrganizer> _files;
        private string _destinationPath;
        private string _sourcePath;
        private string _contolerType;


        [NotifyPropertyChangedInvocatorAttribute]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Controler()
        {
            Files = new List<FilesOrganizer>();
            //_productionCopiedFiles = new FilesOrganizer();
            //_serviceCopiedFiles = new FilesOrganizer();

            //_copiedOlpDataFiles = new FilesOrganizer();
            //_copiedGlobalDataFiles = new FilesOrganizer();

            //_removingDataFiles = new FilesOrganizer();
        }

        //public SingleControler LoadConfigurationSettingsForControler(string controlerType)
        //{
        //    var fs = new Serializer.FilesSerialization();
        //    return fs.GetControlerConfigration(controlerType);
        //}

        //public void LoadConfigurationSettingsForControler()
        //{
        //    throw new NotImplementedException();
        //}

        //public List<FilesTree> GetFilesExtensions()
        //{
        //    throw new NotImplementedException();
        //}

        //public void MoveProductionFiles()
        //{
        //    throw new NotImplementedException();
        //}

        //public void MoveServicesFiles()
        //{
        //    throw new NotImplementedException();
        //}

        //public void CreateDestinationFolders()
        //{
        //    throw new NotImplementedException();
        //}

        //public void RefreshDestinationPath()
        //{
        //    throw new NotImplementedException();
        //}

        //public void RefreshSourcePath()
        //{
        //    throw new NotImplementedException();
        //}

        //public bool CheckDestinationPath()
        //{
        //    throw new NotImplementedException();
        //}

        //public void OlpFilesDataCopy()
        //{
        //    throw new NotImplementedException();
        //}

        //public void GlobalFilesDataCopy()
        //{
        //    throw new NotImplementedException();
        //}

        //public void DeleteFiles()
        //{
        //    throw new NotImplementedException();
        //}     

        //public List<string> GetGroupedFiles()
        //{
        //    throw new NotImplementedException();
        //}

        //public void RefreshDestinationPath(string path)
        //{
        //    throw new NotImplementedException();
        //}

        //public void RefreshSourcePath(string path)
        //{
        //    throw new NotImplementedException();
        //}
        
        //#region IsPossible
        //public bool IsPossibleCopyProductionFiles()
        //{
        //    return (_productionCopiedFiles.FileExtensions?.Count > 0 && _productionCopiedFiles.DestinationFolder != null);
        //}
        //public bool IsPossibleCopyServicesFiles()
        //{
        //    return (_serviceCopiedFiles.FileExtensions?.Count > 0 && _serviceCopiedFiles.DestinationFolder != null);
        //}
        //public bool IsPossibleOlpFilesDataCopy()
        //{
        //    return (_copiedOlpDataFiles.FileExtensions?.Count > 0 && _copiedOlpDataFiles.DestinationFolder != null);
        //}
        //public bool IsPossibleGlobalFilesDataCopy()
        //{
        //    return (_copiedGlobalDataFiles.FileExtensions?.Count > 0 && _copiedGlobalDataFiles.DestinationFolder != null);
        //}
        //public bool IsPossibleDeleteFiles()
        //{
        //    return (_removingDataFiles.FileExtensions?.Count > 0);
        //}
        //#endregion IsPossible

    }
}
