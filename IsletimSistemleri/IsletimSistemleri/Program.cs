using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IsletimSistemleri
{
   
    internal class Program
    {
        static object ciftLock = new object();
        static object tekLock = new object();
        static object asalLock = new object();
        static void Main(string[] args)
        {

            ArrayList sayilar = new ArrayList();
            for (int i = 1; i <= 1000000; i++)
            {
                sayilar.Add(i);
            }

            // Sayıları 4 eşit parçaya bölerek her bir parçayı ayrı bir ArrayList'e ata
            int boyut = 250000;
            List<ArrayList> Listeler = new List<ArrayList>();

            for (int i = 0; i < sayilar.Count; i += boyut)
            {
                ArrayList yeniArrayList = new ArrayList(sayilar.GetRange(i, Math.Min(boyut, sayilar.Count - i)));
                Listeler.Add(yeniArrayList);
            }

            ArrayList ciftSayilar = new ArrayList();
            ArrayList tekSayilar = new ArrayList();
            ArrayList asalSayilar = new ArrayList();

            //4`er adet thread oluşturduk
            for(int i = 0; i < 4; i++)
            {
                Thread ciftThread = new Thread(() => CiftSayiBul(Listeler[i], ciftSayilar));
                ciftThread.Start();
                ciftThread.Join();

            }

            for (int i = 0; i < 4; i++)
            {
                Thread tekThread = new Thread(() => TekSayilBul(Listeler[i], tekSayilar));
                tekThread.Start();
                tekThread.Join();

            }

            for (int i = 0; i < 4; i++)
            {
                Thread asalThread = new Thread(() => AsalSayiBul(Listeler[i], asalSayilar));
                asalThread.Start();
                asalThread.Join();

            }   

            // Sonuçları yazdır
            Console.WriteLine("Çift Sayılar: " + string.Join(", ", ciftSayilar.Cast<int>()));
            Console.WriteLine("\nTek Sayılar: " + string.Join(", ", tekSayilar.Cast<int>()));          
            Console.WriteLine("\nAsal Sayılar: " + string.Join(", ", asalSayilar.Cast<int>()));

            Console.WriteLine("Çift Sayı Adeti: " + ciftSayilar.Count+ "\nTek Sayı Adeti: " +
                tekSayilar.Count + "\nAsal Sayı Adeti: " + asalSayilar.Count );           

            Console.ReadLine();
        }

        // Çift sayıları bulan fonksiyon
        private static void CiftSayiBul(ArrayList list, ArrayList result)
        {
            foreach (int sayi in list)
            {
                if (sayi % 2 == 0)
                {
                    lock (ciftLock)
                    {
                        result.Add(sayi);
                    }
                }
            }
        }

        // Tek sayıları bulan fonksiyon
        private static void TekSayilBul(ArrayList list, ArrayList result)
        {
            foreach (int sayi in list)
            {
                if (sayi % 2 != 0)
                {
                    lock (tekLock)
                    {
                        result.Add(sayi);
                    }
                }
            }
        }

        // Asal sayıları bulan fonksiyon
        private static void AsalSayiBul(ArrayList list, ArrayList result)
        {
            foreach (int sayi in list)
            {
                if (AsalMi(sayi))
                {
                    lock (asalLock)
                    {
                        result.Add(sayi);
                    }
                }
            }
        }

        // Bir sayının asal olup olmadığını kontrol eden fonksiyon
        private static bool AsalMi(int number)
        {
            if (number < 2)
                return false;

            for (int i = 2; i <= Math.Sqrt(number); i++)
            {
                if (number % i == 0)
                    return false;
            }
            return true;
        }
    }
 }
