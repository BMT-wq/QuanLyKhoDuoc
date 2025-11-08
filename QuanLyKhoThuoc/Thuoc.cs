using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Thuoc
    {
        public string MaThuoc { get; set; }
        public string TenThuoc { get; set; }
        public string DonViTinh { get; set; }
        public DateTime HanSuDung { get; set; }
        public int SoLuongTon { get; set; }

        public Thuoc(string ma, string ten, string dvt, DateTime hsd, int sl)
        {
            MaThuoc = ma;
            TenThuoc = ten;
            DonViTinh = dvt;
            HanSuDung = hsd;
            SoLuongTon = sl;
        }
        public string ToDisplayString()
        {
            return $"[{MaThuoc}] {TenThuoc} | DVT: {DonViTinh} | HSD: {HanSuDung.ToShortDateString()} | SL Ton: {SoLuongTon}";
        }
        public override string ToString()
        {
            // Định dạng dùng cho việc lưu file: Ma,Ten,DVT,HSD(dd/MM/yyyy),SL
            return $"{MaThuoc},{TenThuoc},{DonViTinh},{HanSuDung.ToString("dd/MM/yyyy")},{SoLuongTon}";
        }
    }
}
