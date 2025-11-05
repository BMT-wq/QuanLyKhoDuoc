using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public static class MenuHelper
    {
        // Hằng số cho căn lề bảng (đảm bảo thẳng hàng, không dùng Tab)
        public const int COL_MA = 12;
        public const int COL_TEN = 30;
        public const int COL_HSD = 12;
        public const int COL_SL = 10;
        public const int TOTAL_WIDTH_DISPLAY = 60; // Chiều rộng cho dòng kẻ

        public static void DrawBoxedTitle(string title)
        {
            Console.Clear();
            string line = new string('-', title.Length + 6);

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(line);
            Console.WriteLine($"|  {title.ToUpper()}  |");
            Console.WriteLine(line);
            Console.ResetColor();
            Console.WriteLine();
        }

        public static void ShowMainMenuLogo()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(@"
              |-----------------------------|
              |  CYBER TITANS WAREHOUSE     |
              |-----------------------------|                 
    ");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("      ===============================================");
            Console.ResetColor();
        }

        public static void DrawMenuBorder(int count)
        {
            int menuWidth = 45;
            Console.ForegroundColor = ConsoleColor.White;

            // 1. Khung trên
            Console.WriteLine($"      ╔{new string('═', menuWidth)}╗");

            // 2. Tiêu đề Menu
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("      ║ ");
            Console.Write("       CHỌN CHỨC NĂNG HỆ THỐNG", -menuWidth + 1); // Căn lề trái
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("              ║");

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"      ╠{new string('═', menuWidth)}╣");
            Console.ResetColor();

            // 3. Vẽ các tùy chọn bên trong khung
            for (int i = 1; i <= count; i++)
            {
                string optionText = "";
                switch (i)
                {
                    case 1: optionText = "Thêm thuốc mới"; break;
                    case 2: optionText = "Cập nhật số lượng thuốc"; break;
                    case 3: optionText = "Xóa thuốc"; break;
                    case 4: optionText = "Tìm thuốc theo Mã"; break;
                    case 5: optionText = "Tìm thuốc sắp hết hạn"; break;
                    case 6: optionText = "HIỂN THỊ DANH SÁCH THUỐC"; break;
                    case 7: optionText = "Liệt kê combo thuốc"; break;
                    case 8: optionText = "Chạy thực nghiệm"; break;
                    case 9: optionText = "LƯU DỮ LIỆU vào file TXT"; break;
                    case 10: optionText = "Thoát chương trình"; break;
                }

                string formattedOption = $"[{i % 10}] {optionText}";
                if (i == 10) formattedOption = "[0] Thoát chương trình";

                Console.Write("      ║ ");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(formattedOption);

                Console.ForegroundColor = ConsoleColor.White;
                // Căn lề phải
                Console.WriteLine($"{new string(' ', menuWidth - formattedOption.Length - 1)}║");
            }
            // 4. Khung dưới
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"      ╚{new string('═', menuWidth)}╝");
            Console.ResetColor();
        }

        public static void DrawContentBox(List<string> contentLines)
        {
            if (contentLines == null || contentLines.Count == 0) return;

            int maxWidth = contentLines.Max(l => (l?.Length ?? 0)) + 4;
            if (maxWidth < 40) maxWidth = 40;

            string borderLine = new string('-', maxWidth);

            Console.ForegroundColor = ConsoleColor.DarkGray;

            // Khung trên
            Console.WriteLine($"\n      +{borderLine}+");

            Console.ResetColor();
            foreach (string line in contentLines)
            {
                string paddedLine = $" {line}";

                // Sửa lỗi CS0150 bằng cách dùng String.Format cho định dạng căn lề
                Console.WriteLine(string.Format("      |{0,-" + maxWidth + "} |", paddedLine));
            }

            // Khung dưới
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"      +{borderLine}+");
            Console.ResetColor();
        }

        public static string GetInput(string prompt)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write($"\n      > {prompt}: ");
            Console.ResetColor();
            return Console.ReadLine();
        }

        public static void ShowMessage(string message, bool isError = false)
        {
            if (isError)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n      [LỖI] {message}");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\n      [OK] {message}");
            }
            Console.ResetColor();
        }

        // Dùng căn lề cứng
        public static void DrawListHeader()
        {
            string separator = new string('=', TOTAL_WIDTH_DISPLAY);

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n      " + separator);
            // Căn lề theo hằng số
            Console.WriteLine($"      | {"Mã Thuốc",-COL_MA} | {"Tên Thuốc",-COL_TEN} | {"HSD",-COL_HSD} | {"SL Tồn",-COL_SL} |");
            Console.WriteLine("      " + separator);
            Console.ResetColor();
        }

        public static void DrawListItem(Thuoc t)
        {
            Console.ForegroundColor = t.HanSuDung < DateTime.Today.AddDays(30) ? ConsoleColor.Red : ConsoleColor.White;

            // Căn lề theo hằng số
            Console.WriteLine($"      | {t.MaThuoc,-COL_MA} | {t.TenThuoc,-COL_TEN} | {t.HanSuDung.ToShortDateString(),-COL_HSD} | {t.SoLuongTon,-COL_SL} |");

            Console.ResetColor();
        }

        public static void Pause()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\n      Nhấn phím bất kỳ để quay lại menu...");
            Console.ReadKey();
            Console.ResetColor();
        }
    }
}
