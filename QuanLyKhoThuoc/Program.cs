using ConsoleApp1;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace QuanLyKhoDuoc
{
    /* * =======================================================================
     * PHẦN 1: CLASS Thuoc (MODEL DỮ LIỆU - TỐI ƯU TỐC ĐỘ)
     * =======================================================================
     */

    /// <summary>
    /// Class chứa thông tin thuốc - Tối ưu TỐC ĐỘ CẬP NHẬT/XÓA (O(1)).
    /// </summary>




    /* * =======================================================================
     * PHẦN 4: CLASS Benchmark (THỰC NGHIỆM TỐI ƯU)
     * =======================================================================
     */

    /// <summary>
    /// Lớp chạy thực nghiệm đo hiệu năng và bộ nhớ.
    /// </summary>


    /* * =======================================================================
     * PHẦN 5: CLASS MenuHelper (GIAO DIỆN CONSOLE ĐÃ LÀM SẠCH VÀ CÓ KHUNG)
     * =======================================================================
     */

    class Program
    {
        private static readonly KhoManager _kho = new KhoManager();
        private static readonly ThuocComboUtility _comboUtil = new ThuocComboUtility();
        private static readonly Benchmark _benchmark = new Benchmark();

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;
            Console.Title = "CyberTitans - Quản lý Kho Dược";

            if (_kho.LoadData() == 0)
            {
                TaoDuLieuMau();
                MenuHelper.ShowMessage("Đã tạo dữ liệu mẫu mới (chưa có file lưu).");
            }

            bool dangChay = true;
            while (dangChay)
            {
                MenuHelper.ShowMainMenuLogo();

                MenuHelper.DrawMenuBorder(10);

                string luaChon = MenuHelper.GetInput("Chọn chức năng");

                switch (luaChon)
                {
                    case "1": ThemThuocMoi(); _kho.SaveData(); break;
                    case "2": CapNhatThuoc(); _kho.SaveData(); break;
                    case "3": XoaThuoc(); _kho.SaveData(); break;
                    case "4": TimThuocTheoMa(); MenuHelper.Pause(); break;
                    case "5": TimThuocSapHetHan(); MenuHelper.Pause(); break;
                    case "6": LietKeTatCaThuoc(); MenuHelper.Pause(); break;
                    case "7": LietKeCombo(); MenuHelper.Pause(); break;
                    case "8": ChayBenchmark(); MenuHelper.Pause(); break;
                    case "9": _kho.SaveData(); MenuHelper.Pause(); break;
                    case "0":
                        if (MenuHelper.GetInput("Bạn có muốn lưu lại dữ liệu trước khi thoát? (y/n)").ToLower() == "y")
                        {
                            _kho.SaveData();
                        }
                        dangChay = false;
                        Console.WriteLine("\n      Cảm ơn đã sử dụng chương trình!");
                        break;
                    default:
                        MenuHelper.ShowMessage("Lựa chọn không hợp lệ!", true);
                        Thread.Sleep(1000);
                        break;
                }
            }
        }

        #region Helper Methods (Các hàm chức năng cho Menu)

        private static void TaoDuLieuMau()
        {
            _kho.ThemThuoc(new Thuoc("P001", "Paracetamol 500mg", "Viên", new DateTime(2026, 10, 1), 500));
            _kho.ThemThuoc(new Thuoc("A001", "Amoxicillin 250mg", "Hộp", new DateTime(2025, 12, 20), 100));
            _kho.ThemThuoc(new Thuoc("B001", "Berberin", "Viên", DateTime.Today.AddDays(10), 200));
            _kho.ThemThuoc(new Thuoc("C002", "Cefalexin 500mg", "Vỉ", new DateTime(2024, 11, 28), 75));
            _kho.ThemThuoc(new Thuoc("D003", "Vitamin C", "Lọ", new DateTime(2027, 5, 15), 300));
            _kho.ThemThuoc(new Thuoc("E004", "Erythromycin", "Viên", DateTime.Today.AddDays(45), 150));
            _kho.ThemThuoc(new Thuoc("G005", "Glucosamine", "Hộp", new DateTime(2028, 1, 1), 50));
        }

        private static void ThemThuocMoi()
        {
            MenuHelper.DrawBoxedTitle("CHỨC NĂNG THÊM THUỐC MỚI");
            try
            {
                string ma = MenuHelper.GetInput("Nhập mã thuốc");
                string ten = MenuHelper.GetInput("Nhập tên thuốc");
                string dvt = MenuHelper.GetInput("Nhập đơn vị tính");
                string hsdString = MenuHelper.GetInput("Nhập HSD (dd/MM/yyyy)");
                DateTime hsd = DateTime.ParseExact(hsdString, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                int sl = int.Parse(MenuHelper.GetInput("Nhập số lượng tồn"));

                Thuoc thuocMoi = new Thuoc(ma, ten, dvt, hsd, sl);
                if (_kho.ThemThuoc(thuocMoi))
                {
                    MenuHelper.ShowMessage("Thêm thuốc mới thành công!");
                }
                else
                {
                    MenuHelper.ShowMessage($"Mã thuốc {ma} đã tồn tại.", true);
                }
            }
            catch (Exception ex)
            {
                MenuHelper.ShowMessage($"Lỗi nhập liệu: {ex.Message}", true);
            }
        }

        private static void CapNhatThuoc()
        {
            MenuHelper.DrawBoxedTitle("CHỨC NĂNG CẬP NHẬT SỐ LƯỢNG");
            string ma = MenuHelper.GetInput("Nhập mã thuốc cần cập nhật");

            Thuoc t = _kho.TimTheoMa(ma);

            if (t == null)
            {
                MenuHelper.ShowMessage("Không tìm thấy thuốc!", true);
                return;
            }

            // HIỂN THỊ THÔNG TIN TRONG KHUNG
            MenuHelper.DrawContentBox(new List<string> {
                "THÔNG TIN HIỆN TẠI:",
                t.ToDisplayString()
            });

            try
            {
                int slMoi = int.Parse(MenuHelper.GetInput("Nhập số lượng tồn MỚI"));
                if (_kho.CapNhatSoLuong(ma, slMoi))
                {
                    MenuHelper.ShowMessage("Cập nhật thành công!");
                }
            }
            catch (Exception ex)
            {
                MenuHelper.ShowMessage($"Lỗi nhập liệu: {ex.Message}", true);
            }
        }

        private static void XoaThuoc()
        {
            MenuHelper.DrawBoxedTitle("CHỨC NĂNG XÓA THUỐC");
            string ma = MenuHelper.GetInput("Nhập mã thuốc cần xóa");

            if (_kho.TimTheoMa(ma) == null)
            {
                MenuHelper.ShowMessage("Không tìm thấy thuốc!", true);
                return;
            }

            // HIỂN THỊ CẢNH BÁO TRONG KHUNG
            MenuHelper.DrawContentBox(new List<string> {
                "CẢNH BÁO: Thao tác này sẽ xóa vĩnh viễn dữ liệu.",
                $"Bạn có chắc chắn muốn xóa thuốc [{ma}]? (y/n)"
            });

            if (MenuHelper.GetInput("Xác nhận (y/n)").ToLower() == "y")
            {
                if (_kho.XoaThuoc(ma))
                {
                    MenuHelper.ShowMessage("Xóa thuốc thành công!");
                }
            }
            else
            {
                MenuHelper.ShowMessage("Đã hủy thao tác xóa.", false);
            }
        }

        private static void TimThuocTheoMa()
        {
            MenuHelper.DrawBoxedTitle("CHỨC NĂNG TÌM THUỐC THEO MÃ");
            string ma = MenuHelper.GetInput("Nhập mã thuốc cần tìm");

            Thuoc t = _kho.TimTheoMa(ma);

            if (t != null)
            {
                // SỬ DỤNG KHUNG NỘI DUNG ĐỂ HIỂN THỊ KẾT QUẢ
                MenuHelper.DrawContentBox(new List<string> {
                    "KẾT QUẢ TÌM KIẾM:",
                    t.ToDisplayString()
                });
            }
            else
            {
                MenuHelper.ShowMessage("Không tìm thấy thuốc!", true);
            }
        }

        private static void TimThuocSapHetHan()
        {
            MenuHelper.DrawBoxedTitle("CHỨC NĂNG TÌM THUỐC SẮP HẾT HẠN");
            try
            {
                int soNgay = int.Parse(MenuHelper.GetInput("Tìm thuốc hết hạn trong bao nhiêu ngày tới?"));
                DateTime denNgay = DateTime.Today.AddDays(soNgay);
                List<Thuoc> ketQua = _kho.TimThuocSapHetHan(denNgay);

                Console.WriteLine($"\n      --- Tìm thấy {ketQua.Count} thuốc hết hạn từ nay đến {denNgay.ToShortDateString()} ---");
                if (ketQua.Count > 0)
                {
                    MenuHelper.DrawListHeader();
                    foreach (var t in ketQua)
                    {
                        MenuHelper.DrawListItem(t);
                    }
                    Console.WriteLine("      " + new string('=', MenuHelper.TOTAL_WIDTH_DISPLAY));
                }
                else
                {
                    MenuHelper.DrawContentBox(new List<string> {
                        "Không có thuốc nào sắp hết hạn trong thời gian này."
                    });
                }
            }
            catch (Exception ex)
            {
                MenuHelper.ShowMessage($"Lỗi nhập liệu: {ex.Message}", true);
            }
        }

        private static void LietKeTatCaThuoc()
        {
            MenuHelper.DrawBoxedTitle("DANH SÁCH TẤT CẢ THUỐC TRONG KHO");
            List<Thuoc> tatCa = _kho.LayTatCaThuoc();
            if (tatCa.Count == 0)
            {
                MenuHelper.DrawContentBox(new List<string> { "Kho rỗng." });
                return;
            }

            MenuHelper.DrawListHeader();
            foreach (var t in tatCa.OrderBy(t => t.HanSuDung))
            {
                MenuHelper.DrawListItem(t);
            }
            Console.WriteLine("      " + new string('=', MenuHelper.TOTAL_WIDTH_DISPLAY));
        }

        private static void LietKeCombo()
        {
            MenuHelper.DrawBoxedTitle("CHỨC NĂNG LIỆT KÊ COMBO THUỐC");
            try
            {
                List<Thuoc> tatCa = _kho.LayTatCaThuoc();
                Console.WriteLine($"      Kho hiện có {tatCa.Count} loại thuốc (n={tatCa.Count}).");
                if (tatCa.Count > 20)
                {
                    MenuHelper.ShowMessage("Số lượng thuốc (n) quá lớn (n > 20). Thuật toán có thể chạy rất lâu.", true);
                    if (MenuHelper.GetInput("Bạn vẫn muốn tiếp tục? (y/n)").ToLower() != "y")
                        return;
                }

                int m = int.Parse(MenuHelper.GetInput("Bạn muốn chọn bao nhiêu loại thuốc (m)"));

                if (m > tatCa.Count)
                {
                    MenuHelper.ShowMessage("Lỗi: m không thể lớn hơn n.", true);
                    return;
                }

                Console.WriteLine($"\n      --- Đang tìm tổ hợp C({tatCa.Count}, {m}) ---");
                List<List<Thuoc>> cacCombo = _comboUtil.LietKeCombo(tatCa, m);

                List<string> comboLines = new List<string>();
                comboLines.Add($"Tìm thấy {cacCombo.Count} combo.");

                foreach (var combo in cacCombo.Take(5))
                {
                    comboLines.Add("--- COMBO MỚI ---");
                    foreach (var thuoc in combo)
                    {
                        comboLines.Add($"   {thuoc.MaThuoc} - {thuoc.TenThuoc}");
                    }
                }
                if (cacCombo.Count > 5) comboLines.Add("... và còn nữa.");

                MenuHelper.DrawContentBox(comboLines);
            }
            catch (Exception ex)
            {
                MenuHelper.ShowMessage($"Lỗi: {ex.Message}", true);
            }
        }

        private static void ChayBenchmark()
        {
            MenuHelper.DrawBoxedTitle("CHỨC NĂNG THỰC NGHIỆM HỆ THỐNG");
            try
            {
                int soLuong = int.Parse(MenuHelper.GetInput("Bạn muốn test với bao nhiêu thuốc? (100000)"));
                if (soLuong > 500000)
                {
                    MenuHelper.ShowMessage("Số lượng rất lớn, có thể mất vài phút...", true);
                }
                _benchmark.RunTest(soLuong);
            }
            catch (Exception ex)
            {
                MenuHelper.ShowMessage($"Lỗi: {ex.Message}", true);
            }
        }

        #endregion
    }
}