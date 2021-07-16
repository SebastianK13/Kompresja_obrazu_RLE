using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.IO;

namespace Kompresja_obrazu_RLE
{
    public struct Rgb
    {
        #region pola
        public byte b, g, r;
        #endregion pola
        public Rgb Negatyw()
        {
            Rgb rob;
            rob.r = (byte)(255 - this.r);
            rob.g = (byte)(255 - this.g);
            rob.b = (byte)(255 - this.b);
            return rob;
        }
        public Rgb Jasnosc()
        {
            Byte rob = (byte)(0.299 * this.r + 0.587 * this.g + 0.114 * this.b);
            Rgb ret;
            ret.r = rob;
            ret.g = rob;
            ret.b = rob;
            return ret;
        }
    }
    static class Efekty
    {
        public static Bitmap Negatyw(Bitmap bitmapaWe)
        {
            int wysokosc = bitmapaWe.Height;
            int szerokosc = bitmapaWe.Width;

            Bitmap bitmapaWy = new Bitmap(szerokosc, wysokosc, PixelFormat.Format24bppRgb);

            BitmapData bmWeData = bitmapaWe.LockBits(new Rectangle(0, 0, szerokosc, wysokosc), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData bmWyData = bitmapaWy.LockBits(new Rectangle(0, 0, szerokosc, wysokosc), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int strideWe = bmWeData.Stride;
            int strideWy = bmWeData.Stride;

            IntPtr scanWe = bmWeData.Scan0;
            IntPtr scanWy = bmWyData.Scan0;

            unsafe
            {
                for (int y = 0; y < wysokosc; y++)
                {
                    byte* pWe = (byte*)(void*)scanWe + y * strideWe;
                    byte* pWy = (byte*)(void*)scanWy + y * strideWy;

                    for (int x = 0; x < szerokosc; x++)
                    {
                        //todo
                        ((Rgb*)pWy)[x] = ((Rgb*)pWe)[x].Negatyw();
                        if (pWe == pWe + 1)
                        {
                            pWe = (byte*)(void*)scanWe + y * strideWe;
                            pWy = (byte*)(void*)scanWy + y * strideWy;
                        }
                    }

                }
            }
            bitmapaWy.UnlockBits(bmWyData);
            bitmapaWe.UnlockBits(bmWeData);

            return bitmapaWy;
        }
        public static Bitmap Jasnosc(Bitmap bitmapaWe)
        {
            int wysokosc = bitmapaWe.Height;
            int szerokosc = bitmapaWe.Width;

            Bitmap bitmapaWy = new Bitmap(szerokosc, wysokosc, PixelFormat.Format24bppRgb);

            BitmapData bmWeData = bitmapaWe.LockBits(new Rectangle(0, 0, szerokosc, wysokosc), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData bmWyData = bitmapaWy.LockBits(new Rectangle(0, 0, szerokosc, wysokosc), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int strideWe = bmWeData.Stride;
            int strideWy = bmWeData.Stride;

            IntPtr scanWe = bmWeData.Scan0;
            IntPtr scanWy = bmWyData.Scan0;

            unsafe
            {
                for (int y = 0; y < wysokosc; y++)
                {
                    byte* pWe = (byte*)(void*)scanWe + y * strideWe;
                    byte* pWy = (byte*)(void*)scanWy + y * strideWy;

                    for (int x = 0; x < szerokosc; x++)
                    {

                        ((Rgb*)pWy)[x] = ((Rgb*)pWe)[x].Jasnosc();
                    }
                }
            }
            bitmapaWy.UnlockBits(bmWyData);
            bitmapaWe.UnlockBits(bmWeData);

            return bitmapaWy;
        }
        public static Bitmap Kompresja(Bitmap bitmapaWe)
        {

            int wysokosc = bitmapaWe.Height;
            int szerokosc = bitmapaWe.Width;
            byte l_wyst = 1;
            List<byte> Wy = new List<byte>();


            int l = 0;
            int b = 0;
            int rozmiar_l = 0;

            string desktop;

            Bitmap bitmapaWy = new Bitmap(szerokosc, wysokosc, PixelFormat.Format24bppRgb);

            BitmapData bmWeData = bitmapaWe.LockBits(new Rectangle(0, 0, szerokosc, wysokosc), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData bmWyData = bitmapaWy.LockBits(new Rectangle(0, 0, szerokosc, wysokosc), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int strideWe = bmWeData.Stride;
            int strideWy = bmWyData.Stride;

            IntPtr scanWe = bmWeData.Scan0;
            IntPtr scanWy = bmWyData.Scan0;

            unsafe
            {
                desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                desktop += "\\ piksele.txt ";

                StreamWriter piksele = new StreamWriter(desktop);

                for (int y = 0; y < wysokosc; y++)
                {

                    byte* pWe = (byte*)(void*)scanWe + y * strideWe;

                    Rgb kolor;
                    kolor.r = ((Rgb*)pWe)[0].r;
                    kolor.g = ((Rgb*)pWe)[0].g;
                    kolor.b = ((Rgb*)pWe)[0].b;
                    l_wyst = 1;

                    for (int x = 1; x < szerokosc; x++)
                    {

                        if (kolor.r == ((Rgb*)pWe)[x].r && kolor.g == ((Rgb*)pWe)[x].g && kolor.b == ((Rgb*)pWe)[x].b)
                        {
                            l_wyst++;

                            if (l_wyst == 255)
                            {
                                Wy.Add(l_wyst);
                                Wy.Add(kolor.r);
                                Wy.Add(kolor.g);
                                Wy.Add(kolor.b);

                                piksele.Write(l_wyst);
                                piksele.Write(" ");
                                piksele.Write(kolor.r);
                                piksele.Write(" ");
                                piksele.Write(kolor.g);
                                piksele.Write(" ");
                                piksele.Write(kolor.b);
                                piksele.Write(" ");

                                rozmiar_l += 4;

                                l_wyst = 1;
                                b = 1;

                            }
                        }
                        else
                        {

                            Wy.Add(l_wyst);
                            Wy.Add(kolor.r);
                            Wy.Add(kolor.g);
                            Wy.Add(kolor.b);

                            piksele.Write(l_wyst);
                            piksele.Write(" ");
                            piksele.Write(kolor.r);
                            piksele.Write(" ");
                            piksele.Write(kolor.g);
                            piksele.Write(" ");
                            piksele.Write(kolor.b);
                            piksele.Write(" ");

                            rozmiar_l += 4;

                            l_wyst = 1;
                            b = 1;
                        }

                        kolor.r = ((Rgb*)pWe)[x].r;
                        kolor.g = ((Rgb*)pWe)[x].g;
                        kolor.b = ((Rgb*)pWe)[x].b;
                        if (x == szerokosc - 1)
                            b = 0;

                    }
                    if (l_wyst != 1)
                    {
                        Wy.Add(l_wyst);
                        Wy.Add(kolor.r);
                        Wy.Add(kolor.g);
                        Wy.Add(kolor.b);

                        piksele.Write(l_wyst);
                        piksele.Write(" ");
                        piksele.Write(kolor.r);
                        piksele.Write(" ");
                        piksele.Write(kolor.g);
                        piksele.Write(" ");
                        piksele.Write(kolor.b);
                        piksele.Write(" ");

                        rozmiar_l += 4;
                    }
                    else if (l_wyst == 1 && b == 0)
                    {
                        Wy.Add(l_wyst);
                        Wy.Add(kolor.r);
                        Wy.Add(kolor.g);
                        Wy.Add(kolor.b);

                        piksele.Write(l_wyst);
                        piksele.Write(" ");
                        piksele.Write(kolor.r);
                        piksele.Write(" ");
                        piksele.Write(kolor.g);
                        piksele.Write(" ");
                        piksele.Write(kolor.b);
                        piksele.Write(" ");

                        rozmiar_l += 4;
                    }


                }
                piksele.Close();

                desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                desktop += "\\ sz_wys_rozTab.txt ";

                StreamWriter sze_wys = new StreamWriter(desktop);
                sze_wys.Write(szerokosc);
                sze_wys.Write(" ");
                sze_wys.Write(wysokosc);
                sze_wys.Write(" ");
                sze_wys.Write(rozmiar_l);
                sze_wys.Close();


                int c = 0;
                int p = 0;


                for (int y = 0; y < wysokosc; y++)
                {
                    byte* pWy = (byte*)(void*)scanWy + y * strideWy;


                    for (int x = 0; x < szerokosc; x++)
                    {

                        if (c >= szerokosc)
                        {
                            break;
                        }

                        if (x > 0 || y > 0)
                        {
                            l += 4;
                        }

                        if (l >= rozmiar_l)
                        {
                            break;
                        }

                        for (int i = 0; i < Wy[l]; i++)
                        {


                            ((Rgb*)pWy)[c].r = Wy[l + 1];
                            ((Rgb*)pWy)[c].g = Wy[l + 2];
                            ((Rgb*)pWy)[c].b = Wy[l + 3];

                            c++;
                            if (c >= szerokosc)
                            {
                                break;
                            }

                        }
                        p++;
                    }
                    c = 0;



                }


                bitmapaWe.UnlockBits(bmWeData);
                bitmapaWy.UnlockBits(bmWyData);

                return bitmapaWy;
            }
        }
        public static Bitmap Dekompresja(string p_path, string p_path2)
        {
            string sze_wyso_rT;
            string pix;
            int l_zn = 0;
            int dl_s = 1;
            int l = 0;

            string desktop_2;

            /* desktop_2 = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
             desktop_2 += "\\ sz_wys_rozTab.txt ";*/
            desktop_2 = p_path;

            string[] plik = File.ReadAllLines(desktop_2);

            FileStream fs_2 = new FileStream(desktop_2,
                FileMode.Open, FileAccess.Read);
            StreamReader sr_2 = new StreamReader(fs_2);

            sze_wyso_rT = sr_2.ReadToEnd();
            sr_2.Close();
            fs_2.Close();

            l_zn = sze_wyso_rT.Length;



            for (int i = 0; i < l_zn; i++)
            {
                if (sze_wyso_rT[i] == ' ')
                {
                    dl_s++;
                }
            }

            int[] swr = new int[dl_s];

            for (int i = 0; i < 1; i++)
            {
                string[] tmp = plik[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                for (int j = 0; j < tmp.Length; j++)
                {
                    swr[j] = int.Parse(tmp[j]);
                }
            }

            string desktop;
            dl_s = 0;

            /*desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            desktop += "\\ piksele.txt ";*/

            desktop = p_path2;

            string[] plik_2 = File.ReadAllLines(desktop);

            FileStream fs = new FileStream(desktop,
                FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs);

            pix = sr.ReadToEnd();
            sr.Close();
            fs.Close();

            l_zn = pix.Length;



            for (int i = 0; i < l_zn; i++)
            {
                if (pix[i] == ' ')
                {
                    dl_s++;
                }
            }

            List<byte> L_pix = new List<byte>();

            for (int i = 0; i < 1; i++)
            {
                string[] tmp = plik_2[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                for (int j = 0; j < tmp.Length; j++)
                {
                    L_pix.Add(byte.Parse(tmp[j]));
                }
            }

            int szerokosc = swr[0];
            int wysokosc = swr[1];
            int roz_T = swr[2];

            Bitmap bitmapaWy = new Bitmap(szerokosc, wysokosc, PixelFormat.Format24bppRgb);
            BitmapData bmWyData = bitmapaWy.LockBits(new Rectangle(0, 0, szerokosc, wysokosc), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            int strideWy = bmWyData.Stride;
            IntPtr scanWy = bmWyData.Scan0;

            unsafe
            {
                int c = 0;
                int p = 0;


                for (int y = 0; y < wysokosc; y++)
                {
                    byte* pWy = (byte*)(void*)scanWy + y * strideWy;


                    for (int x = 0; x < szerokosc; x++)
                    {

                        if (c >= szerokosc)
                        {
                            break;
                        }

                        if (x > 0 || y > 0)
                        {
                            l += 4;
                        }

                        if (l >= roz_T)
                        {
                            break;
                        }

                        for (int i = 0; i < L_pix[l]; i++)
                        {


                            ((Rgb*)pWy)[c].r = L_pix[l + 1];
                            ((Rgb*)pWy)[c].g = L_pix[l + 2];
                            ((Rgb*)pWy)[c].b = L_pix[l + 3];

                            c++;
                            if (c >= szerokosc)
                            {
                                break;
                            }

                        }
                        p++;
                    }
                    c = 0;
                }
            }
            bitmapaWy.UnlockBits(bmWyData);
            return bitmapaWy;
        }
    }
}
