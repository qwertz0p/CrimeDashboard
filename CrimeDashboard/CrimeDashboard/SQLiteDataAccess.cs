using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using LiveCharts;
using LiveCharts.Wpf;

namespace CrimeDashboard
{
    public class SQLiteDataAccess
    {
        private List<string> delikte = new List<string>();

        // Properties:
        public List<string> Delikte { get => delikte; set => delikte = value; }
        public string SelectedYear { get; set; }

        // Konstruktor:
        public SQLiteDataAccess() { }


        // Verbindung, Laden und Lesen der Daten für ein ausgewähltes Delikt und ausgewähltes Jahr
        public static List<double> SelectBerlinData(string delikt, string selyear)
        {
            List<double> temp = new List<double>();
            string berlin = @"Data Source =..\Crime_Berlin.db;Version=3;";
            string select = "SELECT ";
            string inter = " FROM BerlinFZ";
            string year = selyear;
            string end = " WHERE LORID LIKE '%0000%'";

            string stm_get_quant_kiez = String.Concat(select, delikt, inter, year, end);

            using (var ber_con = new SQLiteConnection(berlin))
            using (var get_quant_kiez = new SQLiteCommand(stm_get_quant_kiez, ber_con))
            {
                ber_con.Open();

                using (SQLiteDataReader rdr = get_quant_kiez.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        int ord = rdr.GetOrdinal(delikt);
                        temp.Add(rdr.GetDouble(ord));
                    }
                }
            }

            return temp;
        }


        // Verbindung, Laden und Lesen der Kiez-Bezeichnungen aus der Datenbank
        public static List<string> SelectRegions()
        {
            List<string> temp = new List<string>();
            string berlin = @"Data Source= D:\.NET\CrimeDashboard\CrimeDashboard\CrimeDashboard\CrimeDashboard\bin\Crime_Berlin.db;Version=3;";

            string select = "SELECT ";
            string year = " FROM BerlinFZ2012 WHERE LORID LIKE '%0000%'";

            string stm_get_quant_kiez = String.Concat(select, "Region", year);                     

            using (var ber_con = new SQLiteConnection(berlin))
            using (var get_quant_kiez = new SQLiteCommand(stm_get_quant_kiez, ber_con))
            {
                ber_con.Open();

                using (SQLiteDataReader rdr = get_quant_kiez.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        int ord = rdr.GetOrdinal("Region");
                        temp.Add(rdr.GetString(ord));
                    }

                    return temp;
                }
            }

        }
    }
}
