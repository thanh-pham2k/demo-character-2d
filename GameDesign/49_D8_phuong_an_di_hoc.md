# D8 — đường tới buổi học đầu, fixture v0.1

Thêm dữ liệu cho lịch D8 của `34`/`48`, không tự áp dụng lên D9 hoặc các năm khác. Thời gian/E/M là giả lập; không đại diện giao thông thật. State clock, boarding, tiền và idempotency theo `29`/`43`.

## Tuyến và giờ

- `ROOM→BUS`: đi bộ 10 phút, E−1, 0M; ngược lại 10 phút, E−1. Có thể dừng trên đường, vị trí trung gian/tiếp tục cần runtime data; chưa có thì không triển khai interrupt.
- `BUS_D8_0740`: cửa sẵn sàng 07:35, rời 07:40, phí 3M, tới H6 07:45. `BUS_D8_0750`: cửa 07:45, rời 07:50, phí 3M, tới H6 07:55. Boarding tại đúng giờ rời theo decision trong `29`.
- `BUS→H6` đi bộ: 40 phút, E−4, 0M. `H6→CLASS`: 5 phút, E−1, 0M. Tất cả có điều kiện route known/open; trong fixture route được thông báo sau enrollment. Không hứa route cho ngày khác.
- `D8_SUPPORT_DESK`: tại BUS `[07:35,09:00)`, player biết từ biển trạm, hỗ trợ xe tới H6 mất 30 phút E−1, 0M; một lần/ngày. Đây là cơ chế hỗ trợ hư cấu cần playtest, không giả định dịch vụ thật.

## EV_D8_TRAVEL_RECOVERY

Trigger: player tại BUS, chưa lên xe, ít nhất một chuyến đã rời hoặc player thiếu 3M, và lớp còn mở. Player xem giờ/chi phí và chọn: `wait_next` nếu trip chưa rời, `walk` nếu route known, `support` nếu bàn hỗ trợ mở và chưa dùng, `return` theo route. UI không tự nhận rằng player đã chọn chuyến; `missed_bus_id` chỉ ghi cho selected trip đã rời theo `29`.

Travel activity có runId, origin/destination, elapsed và cost; completion commit vị trí một lần; save lỗi giữ snapshot trước commit. UI cảnh báo nếu dự kiến đến sau 08:00 hoặc 10:00, không dự đoán chắc attendance trước khi player chọn attend. Nếu lỡ cả hai chuyến, không tự sinh chuyến thứ ba. Nếu E không đủ cho route có thể chọn support E−1 nếu đủ hoặc nghỉ theo data tại trạm; fixture nghỉ tại trạm được đề xuất trong `55_Phu_hoi_tai_nguyen.md`, chưa có runtime hoặc kiểm chứng.

## Trace tính tay

1. 07:30 ROOM → BUS 07:40, chọn board đúng boundary → H6 07:45 → CLASS 07:50 → wait/attend 08:00 → on_time; M−3, E−2 từ hai đoạn đi bộ (chưa tính các hành động khác).
2. 07:45 tới BUS, chọn chuyến 07:50 → H6 07:55 → CLASS 08:00 → on_time nếu chọn attend tại start; M−3, E−1 cho H6→CLASS, chưa tính route tới BUS.
3. 07:55 tới BUS, chọn support → H6 08:25 → CLASS 08:30 → late nếu attend; 0M, E−2, chưa tính route tới BUS.
4. 07:55 tới BUS, chọn đi bộ → H6 08:35 → CLASS 08:40 → late nếu attend; 0M, E−5, chưa tính route tới BUS.

Các trace không bao gồm sleep, route thực tế trước trạm, điều kiện calendar/NPC, save hay interruption; chưa chạy simulation.

## Học bù sau vắng

Nếu D8 absent, `first_class` không sinh. Calendar author `D8_REVIEW` D8 `[17:00,20:00)`, LIBRARY, T_BASIC đã mở: `A_LIBRARY_STUDY` 60 phút E−10 tạo +10 topic evidence khi hoàn tất. Nếu thư viện chưa biết/đóng, không tự cho chạy; D9 ROOM tự học là activity riêng cần calendar/địa điểm, không tự tăng điểm attendance. Học bù không chuyển absent thành on_time/late và không sinh memory đã dự buổi đầu. Nếu user không học bù, tiếp tục campaign với kết quả thật.

## Thiếu dữ liệu

Route còn lại, nghỉ tại trạm khi E=0, availability hỗ trợ, discovery và save/restore chưa triển khai; chưa chứng minh mọi trạng thái đều có recovery. Các giá/giờ chưa được playtest hoặc xác nhận cân bằng.
