using EECT.Assets;
using EECT.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EECT.ElectricalCalculations
{
    class CableDataHandler
    {

        public List<CableData> FilterAviliableCables(List<CableData> CableList, CableProperties CableProperties)
        {
            if (CableProperties.Conductors > 0)
            {
                CableList = CableList.Where(x => x.Conductors == CableProperties.Conductors).ToList();
            }

            if (CableProperties.Material != null)
            {
                CableList = CableList.Where(x => x.Material == CableProperties.Material).ToList();
            }

            if (CableProperties.dimension > 0)
            {
                CableList = CableList.Where(x => x.Dimension == CableProperties.dimension).ToList();
            }

            if (CableProperties.CableType != null)
            {
                CableList = CableList.Where(x => x.CableType == CableProperties.CableType).ToList();
            }

            return CableList;
        }

        public CableData GetCableID(string CableName, List<CableData> CableList)
        {
            List<CableData> results = CableList.FindAll(x => x.Cable == CableName);

            return results[0];
        }


        public List<string> GetCableNameList(List<CableData> CableList)
        {
            var list = new List<string>();

            foreach (var item in CableList)
            {
                list.Add(item.Cable);
            }

            list = list.Distinct().ToList();

            var sortedList = from dbl in list
                             orderby dbl ascending
                             select dbl;

            return sortedList.ToList();

        }
        

        public List<int> GetCableConductorList(List<CableData> CableList)
        {
            var list = new List<int>();

            foreach (var item in CableList)
            {
                list.Add(item.Conductors);
            }

            list = list.Distinct().ToList();

            var sortedList = from dbl in list
                             orderby dbl ascending
                             select dbl;

            return sortedList.ToList();

        }


        public List<string> GetMaterialList(List<CableData> CableList)
        {
            var list = new List<string>();

            foreach (var item in CableList)
            {
                list.Add(item.Material);
            }

            list = list.Distinct().ToList();

            var sortedList = from dbl in list
                             orderby dbl ascending
                             select dbl;

            return sortedList.ToList();

        }

        public List<string> GetCableTypeList(List<CableData> CableList)
        {
            var list = new List<string>();

            foreach (var item in CableList)
            {
                list.Add(item.CableType);
            }

            list = list.Distinct().ToList();

            var sortedList = from dbl in list
                             orderby dbl ascending
                             select dbl;

            return sortedList.ToList();

        }
        

        public List<double> GetCableSizesList(List<CableData> CableList)
        {
            var list = new List<double>();

            foreach (var item in CableList)
            {
                list.Add(item.Dimension);
            }

            list = list.Distinct().ToList();

            var sortedList = from dbl in list
                             orderby dbl ascending
                             select dbl;

            return sortedList.ToList();

        }

        public List<double> GetPhases(List<CableData> CableList)
        {
            var list = new List<double>();

            foreach (var item in CableList)
            {
                list.Add(item.Phases);
            }

            list = list.Distinct().ToList();

            var sortedList = from dbl in list
                             orderby dbl ascending
                             select dbl;

            return sortedList.ToList();

        }

        public List<CableData> GetCableData(string _cableData)
        {
            var cableList = new List<CableData>();
            var errorlist = new List<string>();

            using (StreamReader sr = new StreamReader(_cableData, Encoding.GetEncoding("iso-8859-1")))
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
                 return cableList;
            }
        }
    }
}
