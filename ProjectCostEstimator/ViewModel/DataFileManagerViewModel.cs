using ProjectCostEstimator.Commands;
using ProjectCostEstimator.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml;
using System.Xml.Linq;

namespace ProjectCostEstimator.ViewModel
{
    class DataFileManagerViewModel : ViewModelBase
    {
        private List<FilePathList> _xmlFilePathsList = new List<FilePathList>();

        private DateTime _tenderDate = new DateTime();
        private DateTime _completionDate = new DateTime();

        private string _projectName;
        private string _atr;
        private string _XMLPath = ConfigurationManager.AppSettings["XMLFolderPath"];
        private int _selectedProjectIndex;

        public DataFileManagerViewModel()
        {
            SaveChangesCommand = new DelegateCommand(o => SaveChanges());

            collectXMLFilePaths();
        }


        private void collectXMLFilePaths()
        {
            XmlReader xmlReader = XmlReader.Create(_XMLPath);
            while (xmlReader.Read())
            {
                if ((xmlReader.NodeType == XmlNodeType.Element) && (xmlReader.Name == "ProjectFile"))
                {
                    if (xmlReader.HasAttributes)
                    {
                        XMLFilePathList.Add(new FilePathList() { Name = xmlReader.GetAttribute("Name"), FilePath = xmlReader.GetAttribute("Path") });
                    }
                }
            }
        }

        private void SaveChanges()
        {
            XElement xele = XElement.Load(XMLFilePathList[SelectedProjectIndex].FilePath);

            xele.Element("BuildingInformation").SetElementValue("ProjectName", ProjectName);
            xele.Element("BuildingInformation").SetElementValue("ATR", ATR);
            xele.Element("BuildingInformation").SetElementValue("TenderDate", TenderDate.ToShortDateString());
            xele.Element("BuildingInformation").SetElementValue("CompletionDate", CompletionDate.ToShortDateString());

            xele.Save(XMLFilePathList[SelectedProjectIndex].FilePath);
        }

        private void getSelectedProjectInfo()
        {
            XDocument xdocument = XDocument.Load(XMLFilePathList[SelectedProjectIndex].FilePath);
            IEnumerable<XElement> ProjectInfoFile = xdocument.Elements();
            foreach (var info in ProjectInfoFile)
            {
                ProjectName = info.Element("BuildingInformation").Element("ProjectName").Value;
                ATR = info.Element("BuildingInformation").Element("ATR").Value;
                TenderDate = Convert.ToDateTime(info.Element("BuildingInformation").Element("TenderDate").Value);
                CompletionDate = Convert.ToDateTime(info.Element("BuildingInformation").Element("CompletionDate").Value);
            }


        }

        public string ProjectName
        {
            get { return _projectName; }
            set {
                _projectName = value;
                OnPropertyChanged("ProjectName");
            }
        }

        public string ATR
        {
            get { return _atr; }
            set
            {
                _atr = value;
                OnPropertyChanged("ATR");
            }
        }

        public DateTime TenderDate
        {
            get { return _tenderDate; }
            set
            {
                _tenderDate = value;
                OnPropertyChanged("TenderDate");
            }
        }


        public DateTime CompletionDate
        {
            get { return _completionDate; }
            set
            {
                _completionDate = value;
                OnPropertyChanged("CompletionDate");
            }
        }

        public List<FilePathList> XMLFilePathList
        {
            get { return _xmlFilePathsList; }
            set
            {
                _xmlFilePathsList = value;
                OnPropertyChanged("XMLFilePathList");
            }
        }

        public int SelectedProjectIndex
        {
            get { return _selectedProjectIndex; }
            set
            {
                _selectedProjectIndex = value;
                OnPropertyChanged("SelectedProject");
                getSelectedProjectInfo();
            }
        }


        public ICommand SaveChangesCommand { get; private set; }
    }
}
