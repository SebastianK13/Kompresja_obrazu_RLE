using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Kompresja_obrazu_RLE
{
    public partial class Form1 : Form
    {
        private Bitmap _bitmapa;
        private string path = "puste";
        private string path2 = "puste";

        public Form1()
        {
            InitializeComponent();
        }

        private void otworzToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog.ShowDialog() == DialogResult.OK)
            {
                _bitmapa = new Bitmap(this.openFileDialog.FileName);
                this.mainPictureBox.Image = _bitmapa;
            }
        }

        private void negatywToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap bitmapaWynikowa = Efekty.Negatyw(_bitmapa);
            this.mainPictureBox.Image = bitmapaWynikowa;
        }

        private void jasnośćToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap bitmapaWynikowa = Efekty.Jasnosc(_bitmapa);
            this.mainPictureBox.Image = bitmapaWynikowa;
        }

        private void kompresjaToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (this.mainPictureBox.Image != null)
            {
                Bitmap bitmapaWynikowa = Efekty.Kompresja(_bitmapa);
                this.mainPictureBox.Image = bitmapaWynikowa;
                MessageBox.Show("Kompresja zakończona powodzeniem pliki zostały zapisane na pulpicie");
            }
            else
            {
                if (this.openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    _bitmapa = new Bitmap(this.openFileDialog.FileName);
                    Bitmap bitmapaWynikowa = Efekty.Kompresja(_bitmapa);
                    this.mainPictureBox.Image = bitmapaWynikowa;
                    MessageBox.Show("Kompresja zakończona powodzeniem pliki zostały zapisane na pulpicie");
                }
                else
                    MessageBox.Show("Wystąpił błąd podczas wczytywania pliku");


            }

        }

        private void zapiszToolStripMenuItem_Click(object sender, EventArgs e)
        {


            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "All Files|*.*|Bitmap File(*)|*.bmp";
            saveFileDialog1.FileName = "skompresowane";
            saveFileDialog1.FilterIndex = 2;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {

                mainPictureBox.Image.Save(saveFileDialog1.FileName);
            }

        }

        private void zakończToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void dekompresjaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Wybierz plik zawierający dane dotyczące wysokości, szerokości");
            //OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "txt file(*)|*.txt|All Files|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                path = (this.openFileDialog.FileName);
            }

            MessageBox.Show("Wybierz plik zawierający dane dotyczące pikseli");
            //OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog.Filter = "txt file(*)|*.txt|All Files|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                path2 = (this.openFileDialog.FileName);
            }

            if (path != "puste" && path2 != "puste")
            {
                Bitmap bitmapaWynikowa_2 = Efekty.Dekompresja(path, path2);
                _bitmapa = bitmapaWynikowa_2;
                this.mainPictureBox.Image = _bitmapa;
                MessageBox.Show("Dekompresja zakończona powodzeniem");

            }
            else
                MessageBox.Show("Problem z plikiem");

        }

        private void oProgramieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Aplikacja służy do kompresji obrazów w formacie - bitmap. " +
                "Za kompresje w tej aplikacji odpowiedzialny jest algorytm bezstratnej kompresji " +
                "RLE. Wynikiem działania kompresji jest plik w formacie txt. Aby zamienić plik tekstowy " +
                "w obraz, należy użyć opcji dekompresja. Program posiada również inne funkcjonalności " +
                "poza wymienionymi. " +
                "\n\n Aplikacja została stworzona w ramach przedmiotu Projekt programistyczny. " +
                "\n\n Autor : Sebastian Knych. ");
        }

        
    }
}
