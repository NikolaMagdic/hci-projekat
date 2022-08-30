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
using System.Collections.ObjectModel;
using Predmetni_zadatak.Model;
using Predmetni_zadatak.Dijalozi;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Predmetni_zadatak.Serijalizacija;
using System.Runtime.Serialization;
using System.Xml;
using System.ComponentModel;
using System.Threading;
using Predmetni_zadatak.Demo;
using Predmetni_zadatak.Pomoć;

namespace Predmetni_zadatak
{
    [Serializable]
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 



    public partial class MainWindow : Window
    {
        public static ObservableCollection<Spomenik> Spomenici
        {
            get;
            set;
        }

        public static ObservableCollection<Etiketa> Etikete
        {
            get;
            set;
        }

        public static ObservableCollection<TipSpomenika> TipoviSpomenika
        {
            get;
            set;
        }

        //Lista spomenika koji se nalaze na mapi
        public static ObservableCollection<Spomenik> spomeniciNaMapi
        {
            get;
            set;
        }

        public Point startPoint = new Point(); //Za drag&drop
        private bool isDragging = false;

        public MainWindow()
        {
            string putanja = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "repozitorijum");
            load(putanja);
            InitializeComponent();
            this.DataContext = this;

            prikazInformacijaOSpomeniku.Visibility = Visibility.Hidden;

            this.Closed += new EventHandler(MainWindowZatvoren); //Prilikom zatvaranja pozvaće se akcija za sačuvavanje
        }

        //Sačuvavanje - serijalizacija
        private void MainWindowZatvoren(object sender, EventArgs e)
        {
            string putanja = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "repozitorijum");
            save(putanja);
        }

        public void save(string putanja)
        {
            using (FileStream fs = new FileStream(putanja, FileMode.Create))
            {
                Repozitorijum r = new Repozitorijum();
                r.TipoviSpomenika = TipoviSpomenika;
                r.Etikete = Etikete;
                r.Spomenici = Spomenici;
                r.SpomeniciNaMapi = spomeniciNaMapi;
                var serijalizator = new DataContractSerializer(r.GetType(), null, 0x7FFF, false, true, null);
                serijalizator.WriteObject(fs, r);
                fs.Close();
            }

        }

        public void load(string putanja)
        {
            if (!File.Exists(putanja)) //Ako putanja ne postoji, fajl još nijednom nije sačuvan, inicijalizujemo prazne liste
            {
                TipoviSpomenika = new ObservableCollection<TipSpomenika>();
                Etikete = new ObservableCollection<Etiketa>();
                Spomenici = new ObservableCollection<Spomenik>();
                spomeniciNaMapi = new ObservableCollection<Spomenik>();
                return;
            }

            using (FileStream fs = new FileStream(putanja, FileMode.Open))
            {
                if (fs.Length == 0) //Ako je strim prazan pravimo prazne liste
                {
                    TipoviSpomenika = new ObservableCollection<TipSpomenika>();
                    Etikete = new ObservableCollection<Etiketa>();
                    Spomenici = new ObservableCollection<Spomenik>();
                    spomeniciNaMapi = new ObservableCollection<Spomenik>();
                    return;
                }

                DataContractSerializer serijalizator = new DataContractSerializer(typeof(Repozitorijum));
                XmlDictionaryReader citac = XmlDictionaryReader.CreateTextReader(fs, new XmlDictionaryReaderQuotas());
                Repozitorijum r = (Repozitorijum)serijalizator.ReadObject(citac);

                TipoviSpomenika = r.TipoviSpomenika;
                Etikete = r.Etikete;
                Spomenici = r.Spomenici;
                spomeniciNaMapi = r.SpomeniciNaMapi;

                fs.Close();
            }
        }

        //Na dupli na stavku u stablu se otvara prozor za izmenu
        private void treeView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (treeView.SelectedItem is TipSpomenika)
            {
                TipSpomenika tip = (TipSpomenika)treeView.SelectedItem;
                var s = new DodajTipSpomenika(tip);
                s.ShowDialog();
            }
            else if (treeView.SelectedItem is Spomenik)
            {
                Spomenik spomenik = (Spomenik)treeView.SelectedItem;
                var s = new DodajSpomenik(spomenik);
                statusBar.Text = "Izmena spomenika " + spomenik.Ime;
                s.ShowDialog();
                statusBar.Text = "Izmenjen spomenik";
            }
        }

        //Drag&drop

        //Preuzimanje trenutne tačke kada se klikne na levi taster miša
        private void treeView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(null);
        }

        private void TreeViewItem_OnItemSelected(object sender, RoutedEventArgs e)
        {
            treeView.Tag = e.OriginalSource;
            
            //za statusBar
            if(treeView.SelectedItem is TipSpomenika)
            {
                statusBar.Text = "Izabran tip spomenika " + ((TipSpomenika)treeView.SelectedItem).Ime + " u stablu";
            }
            else if (treeView.SelectedItem is Spomenik)
            {
                statusBar.Text = "Izabran spomenik " + ((Spomenik)treeView.SelectedItem).Ime + " u stablu";
            }
        }

        private void treeView_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && !isDragging)
            {
                Point position = e.GetPosition(null);
                if (Math.Abs(position.X - startPoint.X) > SystemParameters.MinimumHorizontalDragDistance || Math.Abs(position.Y - startPoint.Y) > SystemParameters.MinimumVerticalDragDistance)
                {
                    StartDrag(e);
                }
            }
        }

        private void StartDrag(MouseEventArgs e)
        {
            //obavezno if! tipovi se ne mogu dragovati, a ovako ako je neko slucajno kliknuo negde na drvo ne moze ni to
            if (treeView.SelectedItem is Spomenik)
            {
                isDragging = true;

                Spomenik selectedItem = (Spomenik)treeView.SelectedItem;
                statusBar.Text = "Pomeranje spomenika " + selectedItem.Ime;
                TreeViewItem tvi = treeView.Tag as TreeViewItem;
                // Initialize the drag & drop operation                
                DataObject dragData = new DataObject("spomenikDrag", selectedItem);
                if (isDragging == true)
                    DragDrop.DoDragDrop(tvi, dragData, DragDropEffects.Move);
                //ovde se dođe tek kada se spusti objekat
                isDragging = false;
            }
        }

        //Ovo služi da prikaze da se na mapu može spustiti objekat
        private void Mapa_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent("spomenikDrag")) 
            {
                e.Effects = DragDropEffects.None;
            }
        }

        //ubacuje spomenik sa stabla na mapu
        private void Mapa_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("spomenikDrag"))
            {
                Spomenik s = e.Data.GetData("spomenikDrag") as Spomenik;
                s.Point = e.GetPosition(Mapa);
                if (!spomeniciNaMapi.Contains(s))
                {
                    spomeniciNaMapi.Add(s);
                }
                //observable vezana za listu iz mape
                isDragging = false;
                statusBar.Text = "Spomenik " + s.Ime + " spušten na mapu";
            }
        }


        //ovo sluzi za pokretanje draga sa mape
        private void Mapa_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && !isDragging)
            {
                Point position = e.GetPosition(null);
                if (Math.Abs(position.X - startPoint.X) > SystemParameters.MinimumHorizontalDragDistance || Math.Abs(position.Y - startPoint.Y) > SystemParameters.MinimumVerticalDragDistance)
                {
                    StartDragMap(e);
                }
            }
        }

        //funkcija koja pokreće drag sa mape
        private void StartDragMap(MouseEventArgs e)
        {
            if (Mapa.SelectedItem is Spomenik) //zbog null, ako je neko krenuo da vuce po mapi bezveze
            {
                isDragging = true;
                Spomenik selectedItem = (Spomenik)Mapa.SelectedItem;
                statusBar.Text = "Pomeranje spomenika " + selectedItem.Ime;
                ListBoxItem lwi = (ListBoxItem)Mapa.ItemContainerGenerator.ContainerFromItem(selectedItem);
                // Initialize the drag & drop operation
                DataObject dragData = new DataObject("spomenikDrag", selectedItem);
                if (isDragging == true)
                    DragDrop.DoDragDrop(lwi, dragData, DragDropEffects.Move);
                isDragging = false;
                //Mapa.SelectedItem = null; //Ako želim da deselektujem ikonicu posle otpuštanja, za sad sam isključio
            }
        }


        //Panel ispod stabla za prikaz informacija o spomeniku
        private void Mapa_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Mapa.SelectedItem is Spomenik)
            {
                Spomenik s = (Spomenik)Mapa.SelectedItem;
                prikazInformacijaOSpomeniku.Visibility = Visibility.Visible;
                oznakaInfo.Text = s.Oznaka;
                nazivInfo.Text = s.Ime;
                tipInfo.Text = s.Tip.Ime;
                statusBar.Text = "Izabran spomenik na mapi " + s.Ime;
            }
            else
            {
                prikazInformacijaOSpomeniku.Visibility = Visibility.Hidden;
            }

        }

        //Na dupli klik na spomenik na mapi se otvara prozor za izmenu
        private void Mapa_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            prikazInformacijaOSpomeniku.Visibility = Visibility.Hidden;
            Spomenik s = (Spomenik)Mapa.SelectedItem;
            DodajSpomenik ds = new DodajSpomenik(s);
            ds.ShowDialog();
        }

        //DEMO
        private bool demoMode = false;

        private void demo_Pocinje(object sender, RoutedEventArgs e)
        {
            statusBar.Text = "Pokrenut demo mod";
            demoMode = true;

            Point p = menuItemTip.PointToScreen(new Point(0d, 0d)); //Nalazi ciljnu tačku na ekranu, u ovom slučaju Tipovi spomenika meni
            p.X += 20; //Pošto prethodna naredba vraća gornji levi ugao komponente (dugmeta ili MenuItem-a) ovako ga postavljamo na sredinu
            p.Y += 10;

            LinearSmoothMove(p, 100);

            var pts = new PrikazTipovaSpomenika();
            statusBar.Text = "Pritisnite Space za izlazak iz demo moda";
            pts.Show();
            Thread.Sleep(500);

            pts.demo_PrikazPocinje(this, e);  //Prelazak na prikaz prozora PrikazTipovaSpomenika
            pts.Close();
        }

        //Fukcija koja omogućuje kontinualan izgled pomeranja strelice na ekranu sve do ciljne tačke
        public void LinearSmoothMove(Point newPosition, int steps)
        {
            if (demoMode)
            {
                Point start = Win32.GetMousePosition();
                Point iterPoint = start;

                // Pronalazi nagib linije između početne i krajnje pozicije
                Point slope = new Point(newPosition.X - start.X, newPosition.Y - start.Y);

                // Podeli sa brojem koraka
                slope.X = slope.X / steps;
                slope.Y = slope.Y / steps;

                //Pomera kursor iterativno - tačku po tačku - po koracima 
                for (int i = 0; i < steps; i++)
                {
                    if (!demoMode)
                    {
                        return;
                    }
                    iterPoint = new Point(iterPoint.X + slope.X, iterPoint.Y + slope.Y);
                    Win32.SetCursorPos((int)iterPoint.X, (int)iterPoint.Y);
                    Thread.Sleep(20);
                }

                //Stavlja kursor na krajnju tačku
                Win32.SetCursorPos((int)newPosition.X, (int)newPosition.Y);
            }
        }

        //U svakom trenutku se demo može prekinuti
        private void ExitDemo_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (demoMode == true)
            {
                e.CanExecute = true;
            }
        }

        //Kada se prekine demo postavlja se demoMode atribut na false, a pošto njega proveravamo pre svake iteracije demo će se zaustaviti 
        private void ExitDemo_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            demoMode = false;
        }

        //PREČICE I STATUS BAR

        //Za demo mod informaciju o završetku
        private void Mapa_MouseEnter(object sender, MouseEventArgs e)
        {
            if(demoMode == true)
            {
                statusBar.Text = "Završen demo mod"; //OVO RADIIII, JA SAM GENIJE! :D ŠALA MALA DA SAM GENIJE NE BIH 'VAKO RADIOO naprotiv debilčina sam :'(
                demoMode = false;
            }
        }

        private void DodajSpomenik_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void DodajSpomenik_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var s = new DodajSpomenik();
            statusBar.Text = "Otvoren prozor za dodavanje spomenika";
            s.ShowDialog();
            statusBar.Text = "Završeno dodavanje spomenika";
        }

        private void DodajTip_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void DodajTip_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var s = new DodajTipSpomenika();
            statusBar.Text = "Otvoren prozor za dodavanje tipa spomenika";
            s.ShowDialog();
            statusBar.Text = "Završeno dodavanje tipa spomenika";
        }

        private void DodajEtiketu_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void DodajEtiketu_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var s = new DodajEtiketu();
            statusBar.Text = "Otvoren prozor za dodavanje etikete";
            s.ShowDialog();
            statusBar.Text = "Završeno dodavanje etikete";
        }

        private void ObrisiSpomenik_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void ObrisiSpomenik_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (treeView.SelectedItem is Spomenik)
            {
                MainWindow.spomeniciNaMapi.Remove((Spomenik)treeView.SelectedItem);
                statusBar.Text = "Obrisan spomenik" + ((Spomenik)treeView.SelectedItem).Ime;
                MainWindow.Spomenici.Remove((Spomenik)treeView.SelectedItem);
                ((Spomenik)treeView.SelectedItem).Tip.SpomeniciTipa.Remove((Spomenik)treeView.SelectedItem);
            }//Briše u tabeli, na mapi i u stablu
        }
        private void ObrisiTip_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void ObrisiTip_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (treeView.SelectedItem is TipSpomenika)
            {
                MessageBoxResult izabran = MessageBox.Show("Brisanjem tipa spomenika biće obrisani i svi spomenici koji pripadaju tom tipu. Da li želite da nastavite?", "Upozorenje", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                switch (izabran)
                {
                    case MessageBoxResult.Yes:
                        //Prilikom brisanja tipa spomenika svi spomenici sa tim tipom se takođe brišu; zato dodajemo upozorenje korisniku
                        TipSpomenika t = (TipSpomenika)treeView.SelectedItem;
                        MainWindow.TipoviSpomenika.Remove(t);
                        for (int i = MainWindow.Spomenici.Count - 1; i >= 0; i--)
                        {  //foreach baca exception pa sam zato upotrebio ovaj oblik
                            Spomenik s = MainWindow.Spomenici.ElementAt(i);
                            if (s.Tip.Oznaka == t.Oznaka)
                            {
                                MainWindow.Spomenici.Remove(s);
                                MainWindow.spomeniciNaMapi.Remove(s);
                            }
                        }
                        statusBar.Text = "Obrisan tip " + t.Ime;
                        break;
                    case MessageBoxResult.No:
                        break;
                }

            }
        }

        private void PrikaziSpomenik_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void PrikaziSpomenik_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            prikazInformacijaOSpomeniku.Visibility = Visibility.Hidden; //Za prikaz informacija HARDKOD DO BOLA
            var s = new PrikazSpomenika();
            statusBar.Text = "Prikaz spomenika";
            s.ShowDialog();
            statusBar.Text = "Završeno prikazivanje spomenika";
        }
        private void PrikaziTip_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void PrikaziTip_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var s = new PrikazTipovaSpomenika();
            statusBar.Text = "Prikaz tipova";
            s.ShowDialog();
            statusBar.Text = "Završeno prikazivanje tipova";
        }
        private void PrikaziEtiketu_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void PrikaziEtiketu_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var s = new PrikazEtiketa();
            statusBar.Text = "Prikaz etiketa";
            s.ShowDialog();
            statusBar.Text = "Završeno prikazivanje etiketa";
        }

        private void Pomoc_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Pomoc_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var s = new ProzorZaPomoc("index");
            statusBar.Text = "Otvoren prozor za pomoć";
            s.ShowDialog();
            statusBar.Text = "Zatvoren prozor za pomoć";
        }

        //Brisanje ikonica sa mape na desni klik
        private void ObrisiNaMapi_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void ObrisiNaMapi_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (Mapa.SelectedItem is Spomenik)
            {
                Spomenik s = (Spomenik)Mapa.SelectedItem;
                spomeniciNaMapi.Remove(s);
                prikazInformacijaOSpomeniku.Visibility = Visibility.Hidden;
                statusBar.Text = "Sa mape obrisan spomenik " + s.Ime;
            }
        }
    }
}
