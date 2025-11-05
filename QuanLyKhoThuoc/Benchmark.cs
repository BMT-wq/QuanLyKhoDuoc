using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Benchmark
    {
        public void RunTest(int soLuongThuoc = 100000)
        {
            KhoManager kho = new KhoManager();
            Random random = new Random();
            Stopwatch stopwatch = new Stopwatch();

            List<Thuoc> dataTest = new List<Thuoc>(soLuongThuoc);
            for (int i = 0; i < soLuongThuoc; i++)
            {
                string ma = "T" + i.ToString("D7");
                DateTime hsd = DateTime.Today.AddDays(random.Next(1, 3650));
                dataTest.Add(new Thuoc(ma, "Thuoc " + i, "Vien", hsd, 100));
            }

            MenuHelper.DrawBoxedTitle($"BENCHMARK VỚI {soLuongThuoc:N0} THUỐC (CHỈ DÙNG CLASS)");

            long memoryTruoc = GC.GetTotalMemory(true);
            stopwatch.Start();
            foreach (var item in dataTest)
            {
                kho.ThemThuoc(item);
            }
            stopwatch.Stop();
            long memorySau = GC.GetTotalMemory(true);
            Console.WriteLine($"Thời gian Thêm: {stopwatch.ElapsedMilliseconds} ms");
            Console.WriteLine($"Tổng bộ nhớ dữ liệu: {(memorySau - memoryTruoc) / (1024.0 * 1024.0):F2} MB");

            string maCanTim = "T" + (soLuongThuoc / 2).ToString("D7");

            stopwatch.Restart();
            kho.TimTheoMa(maCanTim);
            stopwatch.Stop();
            Console.WriteLine($"Thời gian Tìm theo Mã (O(1)): {stopwatch.Elapsed.TotalMilliseconds:F6} ms");

            stopwatch.Restart();
            kho.CapNhatSoLuong(maCanTim, 999);
            stopwatch.Stop();
            Console.WriteLine($"Thời gian Cập nhật (O(1)): {stopwatch.Elapsed.TotalMilliseconds:F6} ms");

            DateTime ngayCanTim = DateTime.Today.AddDays(60);
            stopwatch.Restart();
            kho.TimThuocSapHetHan(ngayCanTim);
            stopwatch.Stop();
            Console.WriteLine($"Thời gian Tìm theo HSD (O(log N)): {stopwatch.Elapsed.TotalMilliseconds:F6} ms");

            stopwatch.Restart();
            kho.XoaThuoc(maCanTim);
            stopwatch.Stop();
            Console.WriteLine($"Thời gian Xóa (O(log N)): {stopwatch.Elapsed.TotalMilliseconds:F6} ms");
            Console.WriteLine("--- BENCHMARK HOÀN TẤT ---");
        }
    }
}
