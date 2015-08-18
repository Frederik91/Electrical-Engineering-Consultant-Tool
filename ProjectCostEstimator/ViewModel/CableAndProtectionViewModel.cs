using ProjectCostEstimator.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EECT.ViewModel
{
    class CableAndProtectionViewModel : ViewModelBase
    {
        private string _cableData = ConfigurationManager.AppSettings["CableDataFolderPath"];

        List<CableData> _cableList = new List<CableData>();

        public CableAndProtectionViewModel()
        {

            GetCableData();
        }


        private void GetCableData()
        {
            var cableList = new List<CableData>();
            var errorlist = new List<string>();

            using (StreamReader sr = new StreamReader(_cableData))
            {
                sr.ReadLine();
                while (sr.Peek() >= 0)
                {
                    var cable = sr.ReadLine();
                    var cableArray = cable.Split('\t');

                    for (int i = 0; i < cableArray.Length; i++)
                    {
                        if (cableArray[i] == string.Empty)
                        {
                            cableArray[i] = "-1";
                        }
                    }

                    try
                    {
                        cableList.Add(new CableData
                        {
                            ID = Convert.ToInt32(cableArray[0]),
                            Cable = cableArray[1],
                            CableID = cableArray[2],
                            CableType = cableArray[3],
                            Isolation = cableArray[4],
                            OperatingTemp = Convert.ToInt32(cableArray[5]),
                            MaxTemp = Convert.ToInt32(cableArray[6]),
                            Voltage = Convert.ToInt32(cableArray[7]),
                            Phases = Convert.ToInt32(cableArray[8]),
                            Conductors = Convert.ToInt32(cableArray[9]),
                            Dimension = Convert.ToDouble(cableArray[10]),
                            Material = cableArray[11],
                            Jacket = Convert.ToBoolean(cableArray[12]),
                            Narea = Convert.ToDouble(cableArray[13]),
                            Nmaterial = cableArray[14],
                            PEArea = cableArray[15],
                            PEmaterial = cableArray[16],
                            CenterDistance = Convert.ToDouble(cableArray[17]),
                            ConductiorDiameter = Convert.ToDouble(cableArray[18]),
                            CableDiameter = Convert.ToDouble(cableArray[19]),
                            JacketDiameter = Convert.ToDouble(cableArray[20]),
                            CableWeight = Convert.ToDouble(cableArray[21]),
                            Rpos = Convert.ToDouble(cableArray[22]),
                            Lpos = Convert.ToDouble(cableArray[23]),
                            R0N = Convert.ToDouble(cableArray[24]),
                            L0N = Convert.ToDouble(cableArray[25]),
                            R0PEN = Convert.ToDouble(cableArray[26]),
                            L0PEN = Convert.ToDouble(cableArray[27]),
                            RPE = Convert.ToDouble(cableArray[28]),
                            LPE = Convert.ToDouble(cableArray[29]),
                            RPhasePhase = Convert.ToDouble(cableArray[30]),
                            LPhasePhase = Convert.ToDouble(cableArray[31]),
                            RPhaseN = Convert.ToDouble(cableArray[32]),
                            LPhaseN = Convert.ToDouble(cableArray[33]),
                            RPhasePEN = Convert.ToDouble(cableArray[34]),
                            LPhasePEN = Convert.ToDouble(cableArray[35]),
                            PhasePhaseGmd = Convert.ToDouble(cableArray[36]),
                            PhasePhaseGroupGmd = Convert.ToDouble(cableArray[37]),
                            PhaseNGmd = Convert.ToDouble(cableArray[38]),
                            PhasePEGmd = Convert.ToDouble(cableArray[39]),
                            NGmd = Convert.ToDouble(cableArray[40]),
                            PEGmd = Convert.ToDouble(cableArray[41]),
                            Capacitance = Convert.ToDouble(cableArray[42]),
                            ELnumber = Convert.ToInt32(cableArray[43]),
                            CENELEC = cableArray[44]
                        });
                    }
                    catch (Exception)
                    {
                        errorlist.Add(cable);
                    }

                }

                CableList = cableList;

                MessageBox.Show("There was an error reading " + errorlist.Count + " lines");

            }
        }


        #region Properties

        public List<CableData> CableList
        {
            get { return _cableList; }
            set
            {
                _cableList = value;
                OnPropertyChanged("cableList");
            }
        }

        #endregion

    }
}
