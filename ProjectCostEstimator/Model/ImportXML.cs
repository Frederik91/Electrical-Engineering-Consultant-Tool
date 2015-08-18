using EECT.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace EECT.ViewModel
{
    class ImportXML
    {
        public ImportXML()
        {

        }

        public List<string> GetChapterTypes(string XMLpath)
        {
            XDocument xdocument = XDocument.Load(XMLpath);
            XNamespace df = xdocument.Root.Name.Namespace;

            var AllChapter = from c in xdocument.Descendants(df + "Pristilbud")
                             select c;

            List<string> _chapterNumberTypeList = new List<string>();

            foreach (var Chapter in AllChapter)
            {
                var ChapterNumberPart = from c in Chapter.Descendants(df + "Postnrdel")
                                        select c;

                foreach (var ChapterNumber in ChapterNumberPart)
                {
                    _chapterNumberTypeList.Add(ChapterNumber.Element(df + "Type").Value);
                }
            }
            return _chapterNumberTypeList.Distinct().ToList();
        }

        public List<string> GetChapterNumber(string SelectedChapterType, string XMLPath)
        {
            List<string> SelectedChapterNumber = new List<string>();

            XDocument xdocument = XDocument.Load(XMLPath);
            XNamespace df = xdocument.Root.Name.Namespace;

            var AllChapter = from c in xdocument.Descendants(df + "Pristilbud")
                             select c;

            List<string> _chapterNumberTypeList = new List<string>();

            foreach (var Chapter in AllChapter)
            {
                var ChapterNumberPart = from c in Chapter.Descendants(df + "Postnrdel")
                                        select c;

                foreach (var ChapterNumber in ChapterNumberPart)
                {
                    if (ChapterNumber.Element(df + "Type").Value == SelectedChapterType)
                    {
                        SelectedChapterNumber.Add(ChapterNumber.Element(df + "Kode").Value);
                    }
                }
            }

            return SelectedChapterNumber.Distinct().ToList();
        }

        public List<DisciplineValues> ImportProjectFile(string SelectedChapterType, string XMLPath, int _area)
        {
            List<DisciplineValues> ImportedDataList = new List<DisciplineValues>();
            

            XDocument xdocument = XDocument.Load(XMLPath);
            XNamespace df = xdocument.Root.Name.Namespace;

            var AllChapter = from c in xdocument.Descendants(df + "Post")
                             select c;

            List<string> _chapterNumberTypeList = new List<string>();

            foreach (var Chapter in AllChapter)
            {
                string chapterNumber = string.Empty;
                string comment = string.Empty;
                double Sum = 0;


                var ChapterNumberPart = from c in Chapter.Descendants(df + "Postnrdel")
                                        select c;

                foreach (var ChapterNumber in ChapterNumberPart)
                {
                    if (ChapterNumber.Element(df + "Type").Value == SelectedChapterType)
                    {
                        chapterNumber = ChapterNumber.Element(df + "Kode").Value;
                    }
                }

                var ChapterCostValues = from c in Chapter.Descendants(df + "Prisinfo") select c;

                foreach (var ChapterCost in ChapterCostValues)
                {
                    double.TryParse(ChapterCost.Element(df + "Sum").Value, out Sum);
                }

                var ChapterComment = from c in Chapter.Descendants(df + "Tekst") select c;

                foreach (var Comment in ChapterComment)
                {
                    try
                    {
                        comment = Comment.Element(df + "Uformatert").Value;
                    }
                    catch (Exception e)
                    {
                        comment = string.Empty;
                        
                    }

                }

                ImportedDataList.Add(new DisciplineValues
                {
                    Chapter = chapterNumber,
                    Cost = Sum,
                    SqmCost = Sum / _area,
                    Comment = comment
                });

            }

            return ImportedDataList;

        }

        public string GetImportFileName(string XMLPath)
        {
            string ProjectName = string.Empty;

            XDocument xdocument = XDocument.Load(XMLPath);
            XNamespace df = xdocument.Root.Name.Namespace;

            var ProjectInfo = from c in xdocument.Descendants(df + "ProsjektNS")
                             select c;


            foreach (var Info in ProjectInfo)
            {
                ProjectName = Info.Element(df + "Navn").Value;
            }

            return ProjectName;
        }

        public DateTime GetImportDate(string XMLPath)
        {
            DateTime ImportDate = new DateTime();

            XDocument xdocument = XDocument.Load(XMLPath);
            XNamespace df = xdocument.Root.Name.Namespace;

            var ProjectInfo = from c in xdocument.Descendants(df + "Generelt")
                              select c;


            foreach (var Info in ProjectInfo)
            {
                ImportDate = DateTime.ParseExact(Info.Element(df + "Dato").Value, "yyyy-MM-dd", null);
            }

            return ImportDate;
        }

    }
}
