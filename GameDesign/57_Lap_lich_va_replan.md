# Lập lịch và replan — toy gameplay đề xuất v0.1

Thiết kế cho quyết định thời gian, không phải giao diện đã triển khai. Calendar/event ordering theo `02`, state/transaction theo `43`, chỉ dùng thông tin player biết. Giá trị và ví dụ là fixture chưa playtest.

## Vòng tương tác

Mở lịch **không tốn thời gian**. Màn hình gồm trục thời gian/ngày, entry đã biết và thẻ hoạt động. Player có thể đặt tối đa ba `planItem` tạm vào các khoảng trống, kéo thả hoặc dùng bàn phím/nút lên xuống. Đây là bản kế hoạch, không đặt lịch NPC, không đặt chỗ xe và không thực hiện activity. Vùng unavailable và sự kiện chưa biết không được tiết lộ qua dự đoán.

Mỗi plan item `{planId, activityId, plannedStart, locationId, duration, energyCost, moneyCost, status}`. Khi thêm/sửa: hiển thị kết thúc, cost chắc chắn, trùng lịch đã biết, E/tiền dự tính dựa trên state và các item trước đó; giả định phải được gắn nhãn. Không có route/calendar data → “chưa đủ thông tin đi lại”, không tô xanh chắc chắn. Player có thể bỏ item hoặc đổi thứ tự trước khi save plan. Không cho hai item dự tính chạy cùng lúc; có thể lưu kế hoạch xung đột với cảnh báo rõ, vì lịch là dự tính chứ không phải lời hứa tự thực hiện.

`save_plan` chỉ lưu bản kế hoạch và source revision; không trừ E, tiền, không ghi completion/memory. `start_next` chỉ bắt đầu một activity tại `now` khi điều kiện thật còn đúng; nếu plannedStart ở tương lai thì hỏi player chờ hoặc sửa plan, chờ là activity tiêu thời gian theo `02`. Luôn revalidate state, NPC availability, cost và location; UI không tự teleport hoặc đặt chỗ. Player có thể bỏ plan bất kỳ lúc nào không phạt, nhưng thời gian đã dùng trong activity trước không hoàn.

## Thông tin mới và replan

Nếu scheduled event chen trong activity, scheduler pause tại timestamp, persist activityRun + event instance; UI cho biết điều vừa xảy ra và những phần kế hoạch bị ảnh hưởng. Các lựa chọn: `continue` tiếp activity (nếu còn hợp lệ), `stop` giữ elapsed/E đã tiêu và bỏ cost chưa commit, hoặc `revise_plan` (0 phút UI, không tự hủy activity cho đến khi player xác nhận stop). Không hiển thị nội dung event chưa được player biết trước trigger. NPC invitation hết window sẽ expire theo `02`, không tự giảm quan hệ.

## Fixture D22 — bài nộp và ôn thi

D22 18:00 tại ROOM, ASG_01 còn hai milestone 90 phút + submit 5 phút, late deadline 22:00; study một topic 60 phút; E giả định đủ. Kế hoạch A: làm + nộp 18:00–19:35, học 19:35–20:35; kế hoạch B: học 18:00–19:00 rồi làm/nộp tới 20:35. Cả hai hợp lệ trong giả định không có event chen, cùng location; UI không nhận xét A tốt hơn B vì kết quả phụ thuộc ý định player.

Fixture `D22_LONG_HELP` được author tại [58](58_Long_giup_replan.md), gồm lịch cục bộ, thoại, chi phí và hợp đồng tạm ngắt/resume ASG_01. Khi trigger lúc 18:30, milestone đã chạy 30/45 phút và tiêu 4 E; player có thể giúp Long rồi tự xác nhận resume cùng runId. Nếu player chưa gặp Long hoặc không cùng location, không offer và không tiết lộ sự kiện. Đây vẫn là fixture chưa triển khai hoặc playtest.

## State, UX và QA

Trạng thái plan `draft|saved|stale|completed|discarded`; state thay đổi ngoài plan làm `stale`, giữ dự định để player xem, không tự xóa. Đổi vị trí hoặc bỏ lỡ chuyến phải tính lại route khi data có. Screen reader đọc timeline thứ tự thời gian và warning bằng chữ; không phụ thuộc màu/kéo thả. Không cần auto-resolve cả ngày; nếu thêm về sau, phải preview từng interruption và không tự chọn thay player.

Expected cases: plan không trừ tiền/E; save plan rồi NPC bận; stale revision; xung đột optional; hidden event không bị lộ; pause assignment và resume cùng runId; deadline đúng mốc; cancel/restore; không double cost/memory; accessible keyboard. Chưa có runtime, simulation hoặc playtest; mục tiêu ba plan items và cách trình bày là giả thuyết.
