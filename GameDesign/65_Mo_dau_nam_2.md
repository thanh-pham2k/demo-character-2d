# Mở đầu Năm 2 — tiếp nhận trạng thái Năm 1 v0.1

Dữ liệu hư cấu cho fixture chuyển năm, nối `64` và matrix `15`. Không khẳng định toàn calendar Y2 đã có hoặc cho phép bỏ qua các entry chưa author. Số liệu là giả định chưa playtest.

## Điểm đến và chính sách

`Y2_START` = D85 08:00, year=2. Trong fixture, khi source ROOM hoặc H6, điểm đến giữ nguyên location; các location khác chưa có mapping nên preview transition `blocked_data`. Nếu source time ≤ D85 08:00 và interval tới destination không chứa occurrence được author khác, không có accrual E, tiền, điểm hoặc di chuyển; nếu có occurrence phải dùng policy của occurrence đó theo `02`, không silently drop. Nếu source time > D85 08:00, giữ source time/location và chạy xử lý các entry Y2 đã author từ D85 tới source; thiếu miss/skip policy thì chặn preview confirm theo `64`. Các pending decision/running activity phải resolve hoặc có skip policy riêng trước transition.

Fixture chỉ định nghĩa các entry trong tài liệu này; để chạy full campaign vẫn cần calendar Y2 đầy đủ, các môn và event còn lại. Không dùng sự tồn tại của fixture để coi mọi khoảng trống trong campaign là không có hoạt động.

## EV_Y2_RETURN — Trở lại với hồ sơ thật

`Y2_RETURN_CARD`: D85 [08:00,20:00), ROOM/H6; thông tin có sẵn khi transition commit, hiển thị trong calendar/notification đã biết. Player chọn mở card 0 phút; hết cửa sổ thì thông báo hết hạn, course records vẫn đọc được qua academic panel; không ghi memory hoặc trừ quan hệ vì không mở. Card không yêu cầu Long/Minh có mặt.

Card gồm credit ledger, trạng thái từng course, evidence topic, các deadline đã biết và conflict người chơi biết. Không đưa `unresolved` thành fail, không tiết lộ private NPC knowledge. Long có thoại chỉ khi NPC có mặt/available theo entry riêng; hiện chưa author availability, nên không tự phát thoại. Ba lựa chọn 0 phút: `view_courses` mở course panel; `view_opportunities` xem thông báo đã biết; `close` quay lại. Không có lựa chọn tự enroll, reset tiền/E hoặc cấp credit.

## Y2_NMT_RETAKE_NOTICE — Nhìn lại môn nợ

D85 08:00, nếu `C_NMT=failed`, hiển thị “Môn chưa đạt; lịch học lại chưa được công bố” trong course card. Nếu `passed`, không hiển thị nợ; nếu `unresolved`, hiển thị lý do thiếu dữ liệu và không gán nợ. Không tạo slot học lại hoặc hứa một ngày cụ thể. Để đảm bảo soft failure trong toàn campaign cần author một cơ hội đủ sớm với attempt, assignment, đề, prereq và chi phí thực tế; hiện vẫn thiếu và chưa thể chứng minh recovery.

## Các bất biến chuyển tiếp

1. Event/notice 0 phút không thay đổi resources, knowledge, relationship, attendance hay credit. Tình trạng Long/Minh, memories và conflicts tiếp tục theo snapshot.
2. `Y2_RETURN_CARD` có occurrence ID riêng; nhìn card không tạo event memory. Lưu instance/knownAt để reload không nhân notice hoặc thay đổi kết quả.
3. Nếu Y1 chưa resolved hoặc destination thiếu data bắt buộc, `64` nêu `blocked_data` và không phát Y2 notice. Preview không tiết lộ NPC chưa gặp hoặc hội CLB trước khi thông tin được author.
4. Một môn passed không được award lại khi mở card; một retake chưa tồn tại không được enroll. Không tự mở C_OOP/DSA nếu prerequisites `31` chưa pass.

## Trace và QA

A: D84 18:00 ROOM, NMT passed, +3 credit đã ledger, E 40, wallet 12M, Long conflict open; với interval và destination data đầy đủ, transition tới D85 08:00 ROOM, state giữ nguyên; mở card không thay đổi state. B: NMT failed và thiếu lịch retake → card nói chưa công bố, không cấp credit, không tạo registration. C: NMT unresolved → nêu nguyên nhân, không gọi là nợ. D: source H6 sau D85 08:00 → chỉ được commit khi policy những entry Y2 đã qua đầy đủ, không lùi clock.

Các trace tính tay, chưa chạy. Cần author calendar thật và recovery NMT Y2, kiểm thử save lỗi, stale revision, entry trùng mốc destination, location khác ROOM/H6 và NPC knowledge trước khi xem transition là playable.
