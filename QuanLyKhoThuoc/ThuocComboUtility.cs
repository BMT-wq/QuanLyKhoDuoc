using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class ThuocComboUtility
    {
        private List<List<Thuoc>> _tatCaCombo;

        public List<List<Thuoc>> LietKeCombo(List<Thuoc> danhSachThuoc, int m)
        {
            _tatCaCombo = new List<List<Thuoc>>();
            TimCombo(danhSachThuoc, m, 0, new List<Thuoc>());
            return _tatCaCombo;
        }

        private void TimCombo(List<Thuoc> danhSachThuoc, int m, int batDau, List<Thuoc> comboHienTai)
        {
            if (comboHienTai.Count == m)
            {
                _tatCaCombo.Add(new List<Thuoc>(comboHienTai));
                return;
            }

            for (int i = batDau; i < danhSachThuoc.Count; i++)
            {
                comboHienTai.Add(danhSachThuoc[i]);
                TimCombo(danhSachThuoc, m, i + 1, comboHienTai);
                comboHienTai.RemoveAt(comboHienTai.Count - 1);
            }
        }
    }
}
