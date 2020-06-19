using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Defaults;
using LiveCharts.Helpers;
using LiveCharts.Configurations;
using System.Data;

namespace CrimeDashboard
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            

            // Bezirke werden als Labels gesetz
            RegionLabel = SQLiteDataAccess.SelectRegions().ToArray();

            // Anlage eines Datenbereichs für das Diagramm
            crimeBar = new SeriesCollection();

            DataContext = this;

        }

        // Properties:
        public string[] RegionLabel { get; set; }
        public SeriesCollection crimeBar { get; set; }
        public List<string> LegendTitles { get => legendTitles; set => legendTitles = value; }
        private List<string> legendTitles = new List<string>();
        SQLiteDataAccess access = new SQLiteDataAccess();

        // Schließen der App bei Klick auf das Kreuz
        private void Click_Exit_App(object sender, RoutedEventArgs e) => this.Close();

        // Anpassen der Deliktsauswahl und Laden der Daten aus der Datenbank
        public void cbAllCrimes_CheckedChanged(object sender, RoutedEventArgs e)
        {
            access.Delikte.Clear();
            LegendTitles.Clear();

            bool newVal = (cbAllFeatures.IsChecked == true);
            Raub.IsChecked = newVal;
            access.Delikte.Add(Raub.Name);
            LegendTitles.Add(Raub.Content.ToString());
            Straßen_Handtaschen_raub.IsChecked = newVal;
            access.Delikte.Add(Straßen_Handtaschen_raub.Name);
            LegendTitles.Add(Straßen_Handtaschen_raub.Content.ToString());
            Koerperverletzungen_GES.IsChecked = newVal;
            access.Delikte.Add(Koerperverletzungen_GES.Name);
            LegendTitles.Add(Koerperverletzungen_GES.Content.ToString());
            Gefährl_schwere_KV.IsChecked = newVal;
            access.Delikte.Add(Gefährl_schwere_KV.Name);
            LegendTitles.Add(Gefährl_schwere_KV.Content.ToString());
            FB_NOE_BED_NACH.IsChecked = newVal;
            access.Delikte.Add(FB_NOE_BED_NACH.Name);
            LegendTitles.Add(FB_NOE_BED_NACH.Content.ToString());
            Diebstahl_Ges.IsChecked = newVal;
            access.Delikte.Add(Diebstahl_Ges.Name);
            LegendTitles.Add(Diebstahl_Ges.Content.ToString());
            KFZDiebstahl.IsChecked = newVal;
            access.Delikte.Add(KFZDiebstahl.Name);
            LegendTitles.Add(KFZDiebstahl.Content.ToString());
            DiebstahlAUSKFZ.IsChecked = newVal;
            access.Delikte.Add(DiebstahlAUSKFZ.Name);
            LegendTitles.Add(DiebstahlAUSKFZ.Content.ToString());
            FRDDiebstahl.IsChecked = newVal;
            access.Delikte.Add(FRDDiebstahl.Name);
            LegendTitles.Add(FRDDiebstahl.Content.ToString());
            Einbruch.IsChecked = newVal;
            access.Delikte.Add(Einbruch.Name);
            LegendTitles.Add(Einbruch.Content.ToString());
            Branddelikte_GES.IsChecked = newVal;
            access.Delikte.Add(Branddelikte_GES.Name);
            LegendTitles.Add(Branddelikte_GES.Content.ToString());
            Brandstiftung.IsChecked = newVal;
            access.Delikte.Add(Brandstiftung.Name);
            LegendTitles.Add(Brandstiftung.Content.ToString());
            Sachbeschaedigung_GES.IsChecked = newVal;
            access.Delikte.Add(Sachbeschaedigung_GES.Name);
            LegendTitles.Add(Sachbeschaedigung_GES.Content.ToString());
            SBGraffiti.IsChecked = newVal;
            access.Delikte.Add(SBGraffiti.Name);
            LegendTitles.Add(SBGraffiti.Content.ToString());
            Rauschgift.IsChecked = newVal;
            access.Delikte.Add(Rauschgift.Name);
            LegendTitles.Add(Rauschgift.Content.ToString());

            MainWindow.LoadData(access, LegendTitles, crimeBar);

        }

        // Unterstützung des Three-Way-Checkbox-Modes; Laden der ausgewählten Datensätze
        private void cbCrime_CheckedChanged(object sender, RoutedEventArgs e)
        {
            // Check, ob alle/keine ausgewählt sind
            cbAllFeatures.IsChecked = null;
            if ((Raub.IsChecked == true) && (Straßen_Handtaschen_raub.IsChecked == true) && (Koerperverletzungen_GES.IsChecked == true) && (Gefährl_schwere_KV.IsChecked == true) && (FB_NOE_BED_NACH.IsChecked == true)
                && (Diebstahl_Ges.IsChecked == true) && (KFZDiebstahl.IsChecked == true) && (DiebstahlAUSKFZ.IsChecked == true) && (FRDDiebstahl.IsChecked == true) && (Einbruch.IsChecked == true) && (Branddelikte_GES.IsChecked == true)
                && (Brandstiftung.IsChecked == true) && (Sachbeschaedigung_GES.IsChecked == true) && (SBGraffiti.IsChecked == true) && (Rauschgift.IsChecked == true))
                cbAllFeatures.IsChecked = true;
            if ((Raub.IsChecked == false) && (Straßen_Handtaschen_raub.IsChecked == false) && (Koerperverletzungen_GES.IsChecked == false) && (Gefährl_schwere_KV.IsChecked == false) && (FB_NOE_BED_NACH.IsChecked == false)
                && (Diebstahl_Ges.IsChecked == false) && (KFZDiebstahl.IsChecked == false) && (DiebstahlAUSKFZ.IsChecked == false) && (FRDDiebstahl.IsChecked == false) && (Einbruch.IsChecked == false) && (Branddelikte_GES.IsChecked == false)
                && (Brandstiftung.IsChecked == false) && (Sachbeschaedigung_GES.IsChecked == false) && (SBGraffiti.IsChecked == false) && (Rauschgift.IsChecked == false))
                cbAllFeatures.IsChecked = false;

            // Löschen der letzten Auswahl
            access.Delikte.Clear();
            LegendTitles.Clear();

            // Anpassen der Liste, die die Auswahl speichert
            if (Raub.IsChecked == true) { access.Delikte.Add(Raub.Name); LegendTitles.Add((String)Raub.Content); }
            if (Straßen_Handtaschen_raub.IsChecked == true) { access.Delikte.Add(Straßen_Handtaschen_raub.Name); LegendTitles.Add(Straßen_Handtaschen_raub.Content.ToString()); }
            if (Koerperverletzungen_GES.IsChecked == true) { access.Delikte.Add(Koerperverletzungen_GES.Name); LegendTitles.Add(Koerperverletzungen_GES.Content.ToString()); }
            if (Gefährl_schwere_KV.IsChecked == true) { access.Delikte.Add(Gefährl_schwere_KV.Name); LegendTitles.Add(Gefährl_schwere_KV.Content.ToString()); }
            if (FB_NOE_BED_NACH.IsChecked == true) { access.Delikte.Add(FB_NOE_BED_NACH.Name); LegendTitles.Add(FB_NOE_BED_NACH.Content.ToString()); }
            if (Diebstahl_Ges.IsChecked == true) { access.Delikte.Add(Diebstahl_Ges.Name); LegendTitles.Add(Diebstahl_Ges.Content.ToString()); }
            if (KFZDiebstahl.IsChecked == true) { access.Delikte.Add(KFZDiebstahl.Name); LegendTitles.Add(KFZDiebstahl.Content.ToString()); }
            if (DiebstahlAUSKFZ.IsChecked == true) { access.Delikte.Add(DiebstahlAUSKFZ.Name); LegendTitles.Add(DiebstahlAUSKFZ.Content.ToString()); }
            if (FRDDiebstahl.IsChecked == true) { access.Delikte.Add(FRDDiebstahl.Name); LegendTitles.Add(FRDDiebstahl.Content.ToString()); }
            if (Einbruch.IsChecked == true) { access.Delikte.Add(Einbruch.Name); LegendTitles.Add(Einbruch.Content.ToString()); }
            if (Branddelikte_GES.IsChecked == true) { access.Delikte.Add(Branddelikte_GES.Name); LegendTitles.Add(Branddelikte_GES.Content.ToString()); }
            if (Brandstiftung.IsChecked == true) { access.Delikte.Add(Brandstiftung.Name); LegendTitles.Add(Brandstiftung.Content.ToString()); }
            if (Sachbeschaedigung_GES.IsChecked == true) { access.Delikte.Add(Sachbeschaedigung_GES.Name); LegendTitles.Add(Sachbeschaedigung_GES.Content.ToString()); }
            if (SBGraffiti.IsChecked == true) { access.Delikte.Add(SBGraffiti.Name); LegendTitles.Add(SBGraffiti.Content.ToString()); }
            if (Rauschgift.IsChecked == true) { access.Delikte.Add(Rauschgift.Name); LegendTitles.Add(Rauschgift.Content.ToString()); }

            // Laden der Datensätze
            MainWindow.LoadData(access, LegendTitles, crimeBar);

        }

        // Lädt die ausgewählten Delikte für das ausgewählte Jahr
        private void Z_Selected(object sender, RoutedEventArgs e)
        {
            access.SelectedYear = cbYears.Text;

            MainWindow.LoadData(access, LegendTitles, crimeBar, access.SelectedYear);

        }

        // Routine zum Datenladen
        private static void LoadData(SQLiteDataAccess acc, List<string> titles, SeriesCollection crimes, string selyear = "2019")
        {
            crimes.Clear();

            string[][] titlesearch = new string[2][];
            titlesearch[0] = acc.Delikte.ToArray();
            titlesearch[1] = titles.ToArray();


            foreach (string s in acc.Delikte)
            {
                int index = Array.IndexOf(titlesearch[0], s);

                crimes.Add(new StackedRowSeries
                {
                    Title = titlesearch[1].GetValue(index).ToString(),
                    Values = new ChartValues<double>(SQLiteDataAccess.SelectBerlinData(s, selyear)),
                    DataLabels = true
                });

            }
        }

        private void CartesianChart_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
