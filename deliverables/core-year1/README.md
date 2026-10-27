# Asset core Năm 1 — batch để duyệt

Tạo theo `YEAR_ONE_CORE_PARALLEL_PROMPTS.md`, bằng imagegen tích hợp. Đây là ảnh concept để duyệt, không phải asset production sẵn dùng; kích thước, PPU, pivot và tỉ lệ giữa các nhóm chưa chốt. Từng thư mục có sheet, ảnh cắt và metadata.

| Batch | Nội dung | Giới hạn chính |
|---|---|---|
| [01](batch-01/) | Phòng, quầy, hành lang, WC | Cửa/công tắc còn trong nền; ~824×459/ô |
| [02](batch-02/) | Căn tin, trạm bus, H6, lớp | Đạo cụ còn trong nền; ~824×459/ô |
| [03](batch-03/) | Luồn và Long đứng/đi | Alpha chạm mép, các pose đi gần giống nhau; chưa xác nhận loop |
| [04](batch-04/) | Nhân vật phụ | Alpha có quầng/chạm mép; chưa chuẩn hóa tỉ lệ |
| [05 v2](batch-05-loft-v2/) | Giường gác xép, bàn, thang, ghế | Một nệm trên, bàn đặt riêng phía dưới; nền trắng, chưa tách alpha |
| [06](batch-06/) | Đạo cụ nhỏ | Nền trắng; nồi/bếp gộp |
| [07](batch-07/) | Pose Luồn | Một số pose gộp bàn, laptop, thang hoặc gối; nền trắng |
| [08](batch-08/) | Icon, nút, khung thoại | Nền trắng; UI chưa có 9-slice hoặc kích thước sản xuất |

Trước khi sản xuất asset cuối, duyệt phòng và nhân vật cạnh nội thất để chốt kích thước. Sau đó tạo/chỉnh các asset tách lớp, alpha sạch, frame animation, pivot và PPU; không đưa trực tiếp các sheet này vào Unity như sprite hoàn chỉnh.

Batch 05 v2 là reference nội thất hiện tại. Các ảnh storyboard cũ có giường tầng dưới hoặc bàn không ở dưới giường chưa phù hợp và cần dựng lại khi duyệt flow.

Phòng bố cục mới: [room-layout-v2](room-layout-v2/). Cửa ở tường sau bên phải cửa sổ, hai giường gác xép ở hai bên; nền batch 01 cũ có cửa ở tường bên nên không dùng làm reference bố cục mới. Các ảnh mới vẫn là concept gộp lớp, cần kiểm tra tỉ lệ và lối đi trong Unity.
