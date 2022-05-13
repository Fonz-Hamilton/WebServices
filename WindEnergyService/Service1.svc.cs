using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WindEnergyService {
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1 {
        public double AverageWindEnergy(int lat, int lng) {

            int rowInTable = ((lat + 90) * 360) + (lng + 180) + 1;
            int rowInDoc = rowInTable + 8;
            int rowsToBeSkipped = rowInDoc - 1;     // probably delete

            string annualAverage = "";
            // string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "dataset.html");
            // Debug.WriteLine("this is the path: " + path);
            string path = "d:\\sites\\content\\website24\\Page0\\App_Data\\dataset.html";
            if (File.Exists(path)) {
                string data = File.ReadLines(path).Skip(rowsToBeSkipped).Take(1).First();
                annualAverage = data.Substring(data.LastIndexOf(" "));
            }


            return Convert.ToDouble(annualAverage);

        }


    }
}
