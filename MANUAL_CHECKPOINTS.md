# Checkpoint nhanh — Năm 1 v2

> **Phạm vi:** checkpoint F8 dành cho `Room404_MVP`/Năm 1 v2. Không dùng nó làm checkpoint production mặc định; xem [task tracker](tasks/README.md) để theo dõi gate và runtime production hiện tại.

Trong Unity: **KTX404 → Checkpoint nhanh** (**F8**). Tìm checkpoint rồi chọn **Nạp & Play**. Mỗi lần tải khởi động Play Mode mới, reset pose/reservation/coroutine và dùng save riêng trong `Application.temporaryCachePath/Room404-ManualCheckpoints/`.

Muốn trở lại save chơi chính: Stop → Play bình thường → Tiếp tục. Bản v2 dùng file riêng, giữ save v1; cần New Game nếu chưa có save v2.

| Nhóm | Đoạn có thể chọn |
| --- | --- |
| 10 milestone | Nhận phòng; học đầu tiên; học hay chơi; giữa HK1; chuẩn bị thi HK1; thi HK1; đầu HK2; đời sống KTX; chuẩn bị HK2; thi cuối năm |
| Chuyển ngày/skip | Nằm trước khi chuyển mỗi milestone; chọn Ngủ để xem hậu quả, Quay lại hoặc Tiếp tục |
| Phòng/Windows | Đứng, ghế trái/phải, giường trái/phải, Windows, lịch học và menu học |
| Đồng hồ/cảnh báo | Đang chơi trước giờ học 30/15/5 phút, đúng đầu tiết hoặc đang trễ |
| Điểm danh/bài tập | Vắng cả sáng chưa thành môn nợ; học bù; trước và ngay tại deadline |
| Thi/kết quả | Thi HK1 đang dở câu 2 với 11 giây còn lại; không đủ điều kiện dự thi |
| Tổng kết/Năm 2 | Cuối năm 0/26/32 TC; Năm 2 mang theo 0/26/32 TC và danh sách nợ; trước/sau ăn lẩu |
| KTX/bus | Ngày 8 chơi cùng Long; tắt đèn; ngày 7 sắp lỡ/đã lỡ xe; cổng H6 |

## Thao tác cần kiểm tra

- **Game:** ngồi → mở máy → chọn game; Windows ẩn, thấy phòng/animation; mở Windows thì giờ dừng, Về phòng thì tiếp tục. Thoát hoạt động trả giờ về x1, vẫn ngồi bàn.
- **Đi học:** tới cổng → vào lớp → ngồi; chờ nếu sớm. Điểm danh được ghi trước câu hỏi, không phải lúc hoàn thành bài.
- **Ngủ:** lên giường chỉ nghỉ; bấm Ngủ mở preview. Quay lại không chuyển mốc; Tiếp tục ghi hậu quả rồi chuyển ngày. Thiếu bài/FAIL vẫn ngủ được.
- **Thi:** ba câu có timer riêng. Nộp sớm phải xác nhận; câu trống sai. Không retry. Để kiểm tra Stop → Play → Tiếp tục giữa bài thi, dùng phiên chơi chính với save v2. F8 → Nạp & Play luôn dựng lại fixture cố định, không tiếp tục file sandbox cũ.
- **Skip:** kiểm tra số ngày trôi thực, bài mới quá hạn, buổi/kỳ thi bỏ lỡ, lịch sử và TC không tự được sửa.
- **Cuối năm:** 0 TC vẫn ăn lẩu được. Sau lẩu trở về phòng, chọn ngủ mới sang Năm 2. Nếu ngủ bỏ thi ngày cuối, game xử lý tới cuối ngày trước để còn dự lẩu.

Checkpoint đang chơi/thi và sắp lỡ xe chạy timer ngay sau tải; dùng Pause của Unity nếu muốn quan sát tại chỗ. Fixture lịch sử được dựng qua cùng model; không ghi vào save chính.
