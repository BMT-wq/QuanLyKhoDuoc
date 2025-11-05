using QuanLyKhoDuoc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ConsoleApp1
{
    public class KhoManager
    {
        private const string FILENAME = "duocpham_kho.txt";

        #region Fields (Cấu trúc dữ liệu)

        private readonly Dictionary<string, Thuoc> _thuocTheoMa;
        private readonly SortedList<DateTime, List<Thuoc>> _thuocTheoHanDung;

        #endregion

        public KhoManager()
        {
            _thuocTheoMa = new Dictionary<string, Thuoc>();
            _thuocTheoHanDung = new SortedList<DateTime, List<Thuoc>>();
        }

        #region Thao tác CRUD (O(1) và O(log N))

        public bool ThemThuoc(Thuoc thuocMoi)
        {
            if (_thuocTheoMa.ContainsKey(thuocMoi.MaThuoc)) return false;

            _thuocTheoMa.Add(thuocMoi.MaThuoc, thuocMoi);

            if (!_thuocTheoHanDung.ContainsKey(thuocMoi.HanSuDung))
            {
                _thuocTheoHanDung.Add(thuocMoi.HanSuDung, new List<Thuoc>());
            }
            _thuocTheoHanDung[thuocMoi.HanSuDung].Add(thuocMoi);
            return true;
        }

        public bool CapNhatSoLuong(string maThuoc, int soLuongMoi)
        {
            if (_thuocTheoMa.TryGetValue(maThuoc, out Thuoc thuocCanSua))
            {
                thuocCanSua.SoLuongTon = soLuongMoi;
                return true;
            }
            return false;
        }

        public bool XoaThuoc(string maThuoc)
        {
            if (!_thuocTheoMa.TryGetValue(maThuoc, out Thuoc thuocCanXoa))
            {
                return false;
            }

            _thuocTheoMa.Remove(maThuoc);

            List<Thuoc> danhSachCungHSD = _thuocTheoHanDung[thuocCanXoa.HanSuDung];
            danhSachCungHSD.Remove(thuocCanXoa);

            if (danhSachCungHSD.Count == 0)
            {
                _thuocTheoHanDung.Remove(thuocCanXoa.HanSuDung);
            }
            return true;
        }

        #endregion

        #region Search Operations

        public Thuoc TimTheoMa(string maThuoc)
        {
            if (_thuocTheoMa.TryGetValue(maThuoc, out Thuoc t))
            {
                return t;
            }
            return null;
        }

        public List<Thuoc> TimThuocSapHetHan(DateTime denNgay)
        {
            List<Thuoc> ketQua = new List<Thuoc>();
            DateTime homNay = DateTime.Today;

            foreach (var entry in _thuocTheoHanDung)
            {
                if (entry.Key >= homNay && entry.Key <= denNgay)
                {
                    ketQua.AddRange(entry.Value);
                }
                if (entry.Key > denNgay)
                {
                    break;
                }
            }
            return ketQua;
        }

        public List<Thuoc> LayTatCaThuoc()
        {
            return _thuocTheoMa.Values.ToList();
        }

        #endregion

        #region File Persistence (Lưu/Tải Dữ liệu)

        public void SaveData()
        {
            try
            {
                var lines = _thuocTheoMa.Values.Select(t => t.ToString());

                File.WriteAllLines(FILENAME, lines);
                MenuHelper.ShowMessage($"Đã lưu {lines.Count()} mục vào file {FILENAME}");
            }
            catch (Exception ex)
            {
                MenuHelper.ShowMessage($"LỖI LƯU FILE: {ex.Message}", true);
            }
        }

        public int LoadData()
        {
            if (!File.Exists(FILENAME)) return 0;

            int count = 0;
            _thuocTheoMa.Clear();
            _thuocTheoHanDung.Clear();

            try
            {
                string[] lines = File.ReadAllLines(FILENAME);
                foreach (string line in lines)
                {
                    string[] parts = line.Split(',');
                    if (parts.Length != 5) continue;

                    string ma = parts[0];
                    string ten = parts[1];
                    string dvt = parts[2];
                    DateTime hsd = DateTime.ParseExact(parts[3], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    int sl = int.Parse(parts[4]);

                    Thuoc thuocMoi = new Thuoc(ma, ten, dvt, hsd, sl);
                    ThemThuoc(thuocMoi);
                    count++;
                }
            }
            catch (Exception ex)
            {
                MenuHelper.ShowMessage($"LỖI TẢI FILE: Dữ liệu không hợp lệ ({ex.Message})", true);
            }
            return count;
        }

        #endregion
    }

}
