# Rà soát logic lát cắt Năm 1 — v0.1

Phạm vi: đọc và tính thủ công fixture trong `10_Nam_1_su_kien_mau.md`, không thực thi game hay test suite, không chứng minh runtime hoặc toàn bộ Năm 1. Tất cả chi phí, thời gian và trạng thái là dữ liệu giả lập.

## Bảng tính tay

| Kịch bản | Dòng thời gian | Kết quả suy ra | Điều kiện để kết luận đúng |
|---|---|---|---|
| Đi thẳng | Rời 07:00 → tới trạm 07:10 → lên 07:40 → H6 07:45 → lớp 07:50 | Chờ đến 08:00, on_time nếu vẫn ở lớp và chọn học | Xe có mặt/cửa mở, có 3 M, player chọn lên; không tự điểm danh nếu rời trước 08:00 |
| Chơi 1 lượt | 07:00–07:15 → trạm 07:25 → xe 07:40 → lớp 07:50 | Tương tự đi thẳng, memory Long một lần, E −3 | Lượt game hoàn thành, player chọn lên xe; chưa tính E travel vì fixture chưa author |
| Chơi 2 lượt | 07:00–07:30 → trạm 07:40 → xe 07:40 → lớp 07:50 | Có thể on_time, không đảm bảo | Input boarding phải được xử lý tại đúng mốc trước khi đóng cửa |
| Chơi 3 lượt | 07:00–07:45 → trạm 07:55 → hỗ trợ đến H6 08:25 → lớp 08:30 | Late nếu vào học; Long chưa biết việc muộn | Hỗ trợ còn khả dụng; chỉ tạo missed_bus nếu đã chọn một chuyến đã rời |
| Rời lớp trước 08:00 | Đến lớp 07:50, rời trước 08:00 | Không on_time chỉ vì đã từng đến | Scheduler xóa trạng thái chờ khi rời lớp |
| Không đến lớp | Clock tiến đến 10:00 | absent một lần | Calendar đã author entry và player enrolled |

## Xung đột đã chỉnh trong event mẫu

1. **Memory lỡ xe:** trước đó rule chỉ ghi missed khi player có mặt tại trạm nhưng trigger lại yêu cầu player tới sau khi xe rời. Quy ước mới: ghi `missed_bus_id` nếu đã chọn chuyến và chuyến đó rời trước khi player lên, kể cả chưa tới trạm; chưa chọn chuyến thì ghi `no_bus_available`, không tạo memory lỡ xe.
2. **Điểm danh sớm:** trước đó tương tác trước 08:00 được ghi on_time vào 08:00 mà không yêu cầu player còn ở lớp. Đổi thành `waiting_for_class`, chỉ on_time nếu còn ở lớp và chọn học khi lớp bắt đầu.

## Vấn đề chưa giải

- Mốc 07:40 phụ thuộc scheduler dừng tại mốc để player chọn lên xe trước khi đóng cửa. Chưa có đặc tả input, tương tác khi đang travel hoặc save/load tại cùng timestamp; cần định nghĩa trước khi mô phỏng máy.
- Dự kiến chọn chuyến cần UI/record rõ: player đã chọn chuyến từ đâu, có thể đổi lựa chọn không, expiry và missed event lúc xe rời. Chưa author record này; không thể xác nhận kịch bản có `missed_bus_id` thực sự có thể xảy ra.
- `wait_until_bus`, `board`, `travel` và chuyển giữa location thiếu energy costs, giao diện, điểm ngắt, thời lượng của toàn route; không thể suy luận E cuối hoặc phí hỗ trợ toàn campaign.
- Hai lựa chọn `leave` và `not_now` trong invitation hiện gần như trùng, cần phân biệt bằng ý định/hành vi hoặc gộp sau playtest. Ba lượt game chưa tạo reward bổ sung và có thể không hấp dẫn; cần thiết kế outcome/giới hạn mà không khuyến khích grind quan hệ.
- Attendance có entry lớp nhưng evidence và số phút tham dự chưa được định nghĩa; không thể xác nhận kết quả học tập, không thể kiểm chứng nhánh bỏ lỡ đến giữa kỳ.
- Lịch NPC buổi tối, giờ mời, lựa chọn học/game, event học bù và phí hỗ trợ là placeholder hoặc fixture; không đủ để chứng minh flow qua W04.
- Cần mô phỏng chương trình nhiều môn, đủ đường hồi phục, transaction/idempotency và đánh giá độ vui từ người chơi; chưa có bằng chứng.
