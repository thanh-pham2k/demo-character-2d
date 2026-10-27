# Danh mục ảnh tham chiếu Năm 1

> **Bố cục mới:** mỗi người dùng giường gác xép với bàn bên dưới, không có nệm dưới. Các ảnh nhánh 04B, ảnh bổ sung giường dưới và những cảnh thể hiện giường hai nệm không còn phù hợp; chưa được thay thế toàn bộ. Quy tắc chọn giường mới chưa author.

40 ảnh chính theo thứ tự spec; ảnh lặp là chủ ý khi cùng layout đại diện nhiều mốc. Mã nguồn `bNN-XX` chỉ ô XX trong sheet batch NN; các bản cắt trùng đã được dọn, ảnh chọn vẫn nằm trong thư mục này. Các bước bổ sung trong `bo-sung/` cần ghép vào sequence tương ứng. Chữ, giờ, lựa chọn và kết quả được thêm khi dựng GIF; ảnh không chứa UI chính xác.

| ID | Nội dung | Nguồn |
|---|---|---|
| 01 | Quầy nhận khóa | b01-01 |
| 02 | Mở cửa 404 | b01-02 |
| 03 | Chọn giường | b06-02 |
| 04A | Giường trên | b06-03 |
| 04B | Giường dưới | b09-01 |
| 05 | Làm quen Long | b09-02 |
| 06 | Căn tin | b08-03 |
| 07 | Về giường và preview | b09-04 |
| 08 | Chuẩn bị sách | b07-06 |
| 09 | Trạm xe | b01-10 |
| 10 | Lớp học | b01-11 |
| 11 | Long mời chơi | b06-06 |
| 12A | Hai laptop | b09-05 |
| 12B | Đi học | b09-06 |
| 13 | Tới lớp | b02-03 |
| 14 | Lựa chọn buổi tối | b09-07 |
| 15 | Dashboard giữa kỳ | b05-01 |
| 16 | Ôn bài | b05-03 |
| 17 | Lớp Day 29 | b02-07 |
| 18 | Thi giữa kỳ | b02-08 |
| 19 | Feedback giữa kỳ | b05-02 |
| 20 | Chuẩn bị cuối kỳ | b05-07 |
| 21 | Deadline | b08-09 |
| 22 | Thi HK1 | b02-12 |
| 23 | Kết quả HK1 | b05-09 |
| 24A | Kịp bus | b03-02 |
| 24B | Lỡ bus | b03-03 |
| 25 | Chờ xe sau | b08-07 |
| 26 | Lớp HK2 | b03-05 |
| 27 | Lớp Day 155 | b03-06 |
| 28 | Hai laptop buổi tối | b09-05 |
| 29A | Trao đổi tắt đèn | b07-10 |
| 29B | Đèn đã tắt | b09-10 |
| 30 | Nghỉ tại giường | b07-02 |
| 31 | Lịch Day 204 | b05-10 |
| 32 | Chuẩn bị HK2 | b05-07 |
| 33 | Thi HK2 | b04-01 |
| 34 | Kết quả năm | b04-02 |
| 35 | Chuẩn bị lẩu | b08-10 |
| 36 | Ăn lẩu | b06-12 |

## Các ảnh nối cần dùng

- Day 0 sau chọn giường: `bo-sung/transition-01.png` đặt balo, `transition-02.png` dọn phòng, `transition-03.png` chọn món, `transition-04.png` về phòng.
- Routine: `bo-sung/03.png` thức dậy, `04.png` đánh răng, `05.png` rửa mặt, `06.png` lấy sách/balo.
- Lỡ xe: sau 24B dùng 25 và `bo-sung/transition-08.png` để thể hiện đến lớp; giờ/attendance thêm bằng overlay theo state.
- Cuối năm: sau 35 dùng 36, tiếp `bo-sung/transition-12.png`; thêm preview Day 212, hủy/xác nhận và summary khi dựng GIF.
- Các nhánh 04A/04B, 12A/12B, 24A/24B và 29A/29B là lựa chọn thay thế, không phát như một đường tuyến tính duy nhất.

- Sau bữa lẩu, dùng `bo-sung/final-11.png` rồi `final-12.png` cho cảnh về giường và sáng hôm sau. Đây là một biến thể giường dưới; nhánh giường trên cần dựng theo lựa chọn.
- Nếu chọn giường trên, dùng `bo-sung/giuong-tren/02.png` và `03.png` lúc chọn, `05.png`–`06.png` sáng hôm sau, `11.png`–`12.png` sau bữa lẩu. Các ảnh này là keyframe, không phải animation liên tục.
