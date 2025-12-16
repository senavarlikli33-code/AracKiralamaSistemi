using System;
using System.Collections.Generic;

namespace AracKiralamaSistemi
{
    class Program
    {
        class Arac
        {
            public string Plaka;
            public string MarkaModel;
            public double GunlukFiyat;
        }

        class Rezervasyon
        {
            public string MusteriAdi;
            public string Plaka;
            public DateTime BaslangicTarihi;
            public DateTime BitisTarihi;
            public double Ucret;
        }

        static List<Arac> araclar = new List<Arac>();
        static List<Rezervasyon> rezervasyonlar = new List<Rezervasyon>();

        static void Main(string[] args)
        {
            OrnekAraclarEkle();

            bool devam = true;
            while (devam)
            {
                Console.Clear();
                Console.WriteLine("=== ARAC KIRALAMA SISTEMI ===");
                Console.WriteLine("1. Musait Araclari Goruntule");
                Console.WriteLine("2. Yeni Rezervasyon Olustur");
                Console.WriteLine("3. Rezervasyon Iptal Et");
                Console.WriteLine("4. Toplam Geliri Goruntule");
                Console.WriteLine("5. Musteri Rezervasyonlarini Goruntule");
                Console.WriteLine("6. En Cok Kiralanan Araci Goruntule");
                Console.WriteLine("0. Cikis");
                Console.Write("Seciminiz: ");

                string secim = Console.ReadLine();

                if (secim == "1")
                {
                    MusaitAraclariGoruntule();
                }
                else if (secim == "2")
                {
                    YeniRezervasyonOlustur();
                }
                else if (secim == "3")
                {
                    RezervasyonIptalEt();
                }
                else if (secim == "4")
                {
                    ToplamGeliriGoruntule();
                }
                else if (secim == "5")
                {
                    MusteriRezervasyonlariniGoruntule();
                }
                else if (secim == "6")
                {
                    EnCokKiralananAraciGoruntule();
                }
                else if (secim == "0")
                {
                    devam = false;
                    Console.WriteLine("Sistemden cikis yapildi.");
                }
                else
                {
                    Console.WriteLine("Gecersiz secim!");
                }

                if (devam)
                {
                    Console.WriteLine("\nDevam etmek icin bir tusa basin...");
                    Console.ReadKey();
                }
            }
        }

        static List<string> MusaitAraclariGetir(DateTime baslangic, DateTime bitis)
        {
            List<string> musaitAraclar = new List<string>();

            for (int i = 0; i < araclar.Count; i++)
            {
                if (AracMusaitMi(araclar[i].Plaka, baslangic, bitis))
                {
                    musaitAraclar.Add(araclar[i].Plaka);
                }
            }

            return musaitAraclar;
        }

        static bool AracMusaitMi(string plaka, DateTime bas, DateTime bit)
        {
            for (int i = 0; i < rezervasyonlar.Count; i++)
            {
                if (rezervasyonlar[i].Plaka == plaka)
                {
                    if (!(bit < rezervasyonlar[i].BaslangicTarihi || bas > rezervasyonlar[i].BitisTarihi))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        static double AracGunlukFiyatiniGetir(string plaka)
        {
            for (int i = 0; i < araclar.Count; i++)
            {
                if (araclar[i].Plaka == plaka)
                {
                    return araclar[i].GunlukFiyat;
                }
            }
            return 0;
        }

        static void RezervasyonEkle(string musteri, string plaka, DateTime bas, DateTime bit)
        {
            if (!AracMusaitMi(plaka, bas, bit))
            {
                Console.WriteLine("HATA: Bu arac secilen tarihler arasinda musait degil!");
                return;
            }

            double ucret = RezervasyonUcretiHesapla(plaka, bas, bit);

            Rezervasyon yeni = new Rezervasyon();
            yeni.MusteriAdi = musteri;
            yeni.Plaka = plaka;
            yeni.BaslangicTarihi = bas;
            yeni.BitisTarihi = bit;
            yeni.Ucret = ucret;

            rezervasyonlar.Add(yeni);

            Console.WriteLine("Rezervasyon basariyla olusturuldu!");
            Console.WriteLine("Musteri: " + musteri);
            Console.WriteLine("Plaka: " + plaka);
            Console.WriteLine("Baslangic: " + bas.ToShortDateString());
            Console.WriteLine("Bitis: " + bit.ToShortDateString());
            Console.WriteLine("Toplam Ucret: " + ucret + " TL");
        }

        static double RezervasyonUcretiHesapla(string plaka, DateTime bas, DateTime bit)
        {
            double gunlukFiyat = AracGunlukFiyatiniGetir(plaka);
            int gunSayisi = (bit - bas).Days + 1;
            return gunlukFiyat * gunSayisi;
        }

        static void RezervasyonIptal(string plaka)
        {
            bool bulundu = false;

            for (int i = 0; i < rezervasyonlar.Count; i++)
            {
                if (rezervasyonlar[i].Plaka == plaka)
                {
                    rezervasyonlar.RemoveAt(i);
                    bulundu = true;
                    break;
                }
            }

            if (bulundu)
            {
                Console.WriteLine(plaka + " plakalı arac icin rezervasyon iptal edildi.");
            }
            else
            {
                Console.WriteLine("HATA: " + plaka + " plakalı arac icin aktif rezervasyon bulunamadi.");
            }
        }

        static double ToplamGelir()
        {
            double toplam = 0;

            for (int i = 0; i < rezervasyonlar.Count; i++)
            {
                toplam = toplam + rezervasyonlar[i].Ucret;
            }

            return toplam;
        }

        static List<string> MusteriRezervasyonlariniGetir(string musteri)
        {
            List<string> musteriRezervasyonlari = new List<string>();

            for (int i = 0; i < rezervasyonlar.Count; i++)
            {
                if (rezervasyonlar[i].MusteriAdi == musteri)
                {
                    string bilgi = "Plaka: " + rezervasyonlar[i].Plaka +
                                  ", Tarih: " + rezervasyonlar[i].BaslangicTarihi.ToShortDateString() +
                                  " - " + rezervasyonlar[i].BitisTarihi.ToShortDateString() +
                                  ", Ucret: " + rezervasyonlar[i].Ucret + " TL";
                    musteriRezervasyonlari.Add(bilgi);
                }
            }

            return musteriRezervasyonlari;
        }

        static string EnCokKiralananArac()
        {
            if (rezervasyonlar.Count == 0)
            {
                return "Henuz rezervasyon yok";
            }

            string enCokKiralanan = "";
            int maxKiralama = 0;

            for (int i = 0; i < araclar.Count; i++)
            {
                int sayac = 0;

                for (int j = 0; j < rezervasyonlar.Count; j++)
                {
                    if (rezervasyonlar[j].Plaka == araclar[i].Plaka)
                    {
                        sayac++;
                    }
                }

                if (sayac > maxKiralama)
                {
                    maxKiralama = sayac;
                    enCokKiralanan = araclar[i].Plaka;
                }
            }

            return enCokKiralanan + " (" + maxKiralama + " kez)";
        }

        static void OrnekAraclarEkle()
        {
            Arac arac1 = new Arac();
            arac1.Plaka = "34ABC123";
            arac1.MarkaModel = "Toyota Corolla";
            arac1.GunlukFiyat = 500;
            araclar.Add(arac1);

            Arac arac2 = new Arac();
            arac2.Plaka = "06XYZ456";
            arac2.MarkaModel = "Honda Civic";
            arac2.GunlukFiyat = 550;
            araclar.Add(arac2);

            Arac arac3 = new Arac();
            arac3.Plaka = "35DEF789";
            arac3.MarkaModel = "Volkswagen Passat";
            arac3.GunlukFiyat = 650;
            araclar.Add(arac3);

            Arac arac4 = new Arac();
            arac4.Plaka = "16GHI012";
            arac4.MarkaModel = "Renault Megane";
            arac4.GunlukFiyat = 450;
            araclar.Add(arac4);

            Arac arac5 = new Arac();
            arac5.Plaka = "41JKL345";
            arac5.MarkaModel = "BMW 3.20i";
            arac5.GunlukFiyat = 900;
            araclar.Add(arac5);
        }

        static void MusaitAraclariGoruntule()
        {
            Console.WriteLine("\n--- MUSAIT ARACLARI GORUNTULE ---");
            Console.Write("Baslangic tarihi (gg.aa.yyyy): ");
            DateTime baslangic = DateTime.Parse(Console.ReadLine());

            Console.Write("Bitis tarihi (gg.aa.yyyy): ");
            DateTime bitis = DateTime.Parse(Console.ReadLine());

            List<string> musaitAraclar = MusaitAraclariGetir(baslangic, bitis);

            if (musaitAraclar.Count == 0)
            {
                Console.WriteLine("Bu tarihler arasinda musait arac bulunmamaktadir.");
            }
            else
            {
                Console.WriteLine("Musait Arac Sayisi: " + musaitAraclar.Count);

                for (int i = 0; i < musaitAraclar.Count; i++)
                {
                    for (int j = 0; j < araclar.Count; j++)
                    {
                        if (araclar[j].Plaka == musaitAraclar[i])
                        {
                            int gunSayisi = (bitis - baslangic).Days + 1;
                            double toplamFiyat = araclar[j].GunlukFiyat * gunSayisi;
                            Console.WriteLine("Plaka: " + araclar[j].Plaka +
                                            " | Model: " + araclar[j].MarkaModel +
                                            " | Gunluk: " + araclar[j].GunlukFiyat + " TL" +
                                            " | Toplam (" + gunSayisi + " gun): " + toplamFiyat + " TL");
                        }
                    }
                }
            }
        }

        static void YeniRezervasyonOlustur()
        {
            Console.WriteLine("\n--- YENI REZERVASYON OLUSTUR ---");
            Console.Write("Musteri adi: ");
            string musteri = Console.ReadLine();

            Console.Write("Arac plakasi: ");
            string plaka = Console.ReadLine();

            Console.Write("Baslangic tarihi (gg.aa.yyyy): ");
            DateTime baslangic = DateTime.Parse(Console.ReadLine());

            Console.Write("Bitis tarihi (gg.aa.yyyy): ");
            DateTime bitis = DateTime.Parse(Console.ReadLine());

            RezervasyonEkle(musteri, plaka, baslangic, bitis);
        }

        static void RezervasyonIptalEt()
        {
            Console.WriteLine("\n--- REZERVASYON IPTAL ---");
            Console.Write("Iptal edilecek aracin plakasi: ");
            string plaka = Console.ReadLine();

            RezervasyonIptal(plaka);
        }

        static void ToplamGeliriGoruntule()
        {
            Console.WriteLine("\n--- TOPLAM GELIR RAPORU ---");
            double gelir = ToplamGelir();
            Console.WriteLine("Toplam Rezervasyon Sayisi: " + rezervasyonlar.Count);
            Console.WriteLine("Toplam Gelir: " + gelir + " TL");
        }

        static void MusteriRezervasyonlariniGoruntule()
        {
            Console.WriteLine("\n--- MUSTERI REZERVASYONLARI ---");
            Console.Write("Musteri adi: ");
            string musteri = Console.ReadLine();

            List<string> rezervasyonListesi = MusteriRezervasyonlariniGetir(musteri);

            if (rezervasyonListesi.Count == 0)
            {
                Console.WriteLine(musteri + " adli musteriye ait rezervasyon bulunamadi.");
            }
            else
            {
                Console.WriteLine(musteri + " adli musterinin rezervasyonlari:");

                for (int i = 0; i < rezervasyonListesi.Count; i++)
                {
                    Console.WriteLine(rezervasyonListesi[i]);
                }
            }
        }

        static void EnCokKiralananAraciGoruntule()
        {
            Console.WriteLine("\n--- EN COK KIRALANAN ARAC ---");
            string enCok = EnCokKiralananArac();
            Console.WriteLine("En cok kiralanan arac: " + enCok);
        }
    }
}