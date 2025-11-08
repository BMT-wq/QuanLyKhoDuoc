# Đề tài 05: Quản lý kho dược phẩm trong bệnh viện

Dự án môn Cấu trúc dữ liệu & Giải thuật.

## Thành viên Nhóm
- **Nhóm trưởng:** Bùi Minh Tự
- **Thành viên 1:** Nguyễn Phạm Minh Thi
- **Thành viên 2:** Cao Trần Anh Thy
- **Thành viên 3:** Đỗ Tấn Quốc
- **Thành viên 4:** Nguyễn Hoàng Phúc

---

## Hướng dẫn chạy code
1.  Clone repository: `git clone [link_repo]`
2.  Chuyển sang nhánh develop: `git checkout develop`
3.  Mở file `.sln` bằng Visual Studio 2022.
4.  Nhấn **F5** (Start Debugging) để chạy.
5.  Dữ liệu kho được lưu tại file `duocpham_kho.txt` (trong thư mục .../bin/Debug/net...).

---

## Phân tích Kỹ thuật 

Dự án sử dụng 2 cấu trúc dữ liệu chính để tối ưu hiệu năng:

1.  **Dictionary<string, Thuoc> (Tìm theo Mã O(1))
    - **File:** `QuanLyKhoDuoc.cs`
    - **Giải thích:** Sử dụng Dictionary (Hash Table) cho phép tìm kiếm, cập nhật, và xóa thuốc theo Mã (Key) với tốc độ O(1) (gần như ngay lập tức), bất kể kho có 100 hay 1 triệu loại thuốc. 

2.  **SortedList<DateTime, List<Thuoc>> (Tìm theo HSD O(log N)):**
    - **File:** `QuanLyKhoDuoc.cs`
    - **Giải thích:** Sử dụng SortedList (Cây nhị phân) để tự động sắp xếp thuốc theo Hạn sử dụng. Điều này cho phép tìm kiếm thuốc sắp hết hạn (ví dụ: trong 30 ngày tới) cực kỳ nhanh với tốc độ O(log N). Đảm bảo thuốc luôn được sắp xếp mà không cần thao tác sắp xếp thủ công.

3.  **Tối ưu Cập nhật (O(1)):**
    - **File:** `Thuoc.cs`
    - **Giải thích:** Dùng `class Thuoc` (kiểu tham chiếu). Khi cập nhật số lượng, hệ thống chỉ cần tìm O(1) trong Dictionary và thay đổi giá trị `SoLuongTon` tại 1 địa chỉ duy nhất. Thay đổi này tự động áp dụng cho cả hai cấu trúc do cùng tham chiếu, không cần cập nhật thủ công
4.  **Đảm bảo tính nhất quán:**
    - **File:** `QuanLyKhoDuoc.cs`
    - ** Giải thích:** Mọi thao tác thêm, xóa, cập nhật thuốc đều được thực hiện đồng thời trên cả hai cấu trúc dữ liệu để đảm bảo tính nhất quán và tránh lỗi dữ liệu.

---

## Kết quả Thực nghiệm
--- BẮT ĐẦU BENCHMARK ---
Thời gian Thêm: 0 ms
Tổng bộ nhớ dữ liệu: 0.01 MB
Thời gian Tìm theo Mã (O(1)): 0.159700 ms
Thời gian Cập nhật (O(1)): 0.101000 ms
Thời gian Tìm theo HSD (O(log N)): 2.862100 ms
Thời gian Xóa (O(log N)): 0.764600 ms
--- BENCHMARK HOÀN TẤT ---