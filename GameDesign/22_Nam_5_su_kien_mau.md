# Năm 5 — chuỗi phục hồi mẫu v0.1

Fixture giả lập để author; không là runtime hay lịch trường thật. Trigger Y5 chỉ khi Y4 transition đã commit do thiếu yêu cầu, không áp dụng nếu graduation chưa xác định vì thiếu data. E/M, mốc và tác động là giả thuyết chưa playtest. Save/transaction theo `02`, course theo `03`, career theo `04`.

## Fixture

`Y5_FIXTURE_A`: Y4 snapshot đã commit `graduated=false`, `C_NMT=failed`, 3 credit môn chưa được cấp, `capstone=passed`, Long đã gặp nhưng `conflict_light=open`; knowledge T_BASIC=30, T_CONTROL=20 được giữ, wallet=30M, E=75. Đây không phải state mặc định của mọi player; fixture B có `capstone=partial`, course đã pass, dùng để kiểm tra nhánh khác. Việc không có data program dẫn `blocked_data`, không tự tạo fixture.

## EV_Y5_PLAN — Kế hoạch năm thêm

- Major, W01 `[08:00,20:00)`, H6 hoặc ROOM, trigger khi player mở dashboard và Y5 snapshot hợp lệ. Danh sách phải lấy từng requirement thiếu thật, số credit, course attempts, nguồn evidence, recovery slot đã author và deadline; không biến course đã pass thành fail. Panel pause, 0 phút.
- Fixture A biết `RET_NMT_Y5` registration W03 `[08:00,12:00)`, fee 5M, course entry W03 13:00–15:00, exam W10 `[08:00,09:00)`; fixture B không hiển thị slot môn đã pass. `view` 0 phút, `plan` ghi calendar reminder 0 phút, `later` 0 phút. Việc plan không tự đăng ký/trả phí.
- Nếu thiếu slot tương ứng, hiển thị “chưa có lịch phục hồi” và `unresolved`; không hứa tốt nghiệp. Player không mở panel vẫn có thể xem calendar đã biết. Không tự tiết lộ thông tin của Long/Minh.

## EV_Y5_REGISTER_RETAKE — Đăng ký học lại

- Scheduled, W03 `[08:00,12:00)`, LOC_H6, C_NMT chưa pass, player biết slot. `register`: 15 phút E −2, reserve 5M khi bắt đầu, commit khoản phí và attempt mới khi hoàn tất; kết thúc đúng 12:00 được tính kịp. Thiếu available money → disable và hiển thị lựa chọn `fee_assistance` đã author trong fixture: đăng ký hỗ trợ miễn phí 30 phút E −2 tại cùng window; completion được cấp voucher non-transferable, redeem vào registration 15 phút.
- Nếu không đủ thời gian để hoàn tất bước chọn, UI cảnh báo; dừng giữa bước không thu phần fee chưa commit, thời gian/E đã dùng không hoàn. Không áp dụng hỗ trợ tự động. `later` 0 phút; hết window → missed registration, không tạo attempt hay thu phí; slot kế tiếp nếu có phải lấy từ calendar author.
- Attempt mới giữ knowledge và course record cũ; chỉ pass ở resolver mới cộng credit một lần. Không cấp credit lúc đăng ký. Fixture không chứng minh course còn lại hay schedule toàn năm.

## EV_Y5_FRESHMAN — Người hỏi đường

- Medium, W02 `[09:00,11:00)`, H6, S05 available và player có mặt. Student hỏi phòng lớp, không yêu cầu người chơi đã có memory đi tìm lớp Y1.
- `walk_together`: 15 phút E −2, đưa S05 tới bảng lớp, memory `helped_freshman` sau completion; `point_board`: 5 phút E −1, cùng memory nhưng outcome `directions`; `decline`: 0 phút, không giảm trust hoặc gán thái độ tiêu cực. Nêu nguy cơ trùng lịch đã biết trước khi chọn.
- Nếu `first_404` hoặc `first_class` có thật, có thể hiện một dòng suy nghĩ player nhớ lúc mới đến; không tạo memory mới từ việc chưa xảy ra. Không grant skill nghề chỉ vì giúp. Miss/leave: event expired, không ảnh hưởng graduation. S05 chỉ callback tốt nghiệp nếu gặp lại và biết event.

## EV_Y5_CAPSTONE_REPAIR — Sửa đồ án

- Conditional major, W04–W09, LOC_PROJECT/ROOM; snapshot có capstone partial/failed và program definition cho phép sửa; không mở cho fixture A nếu capstone đã pass. Fixture B còn milestone implementation 90 phút E −15; mentor review W09 có window cần được author. Project revision giữ `parentEvidenceId`.
- `continue_task`: hoàn tất implementation thì tạo revision `task_done`, không tự pass capstone; `ask_review`: 20 phút E −3 nếu mentor assigned/available, feedback sau completion; `later` 0 phút. Tại deadline nộp phải có rubric/assessor; thiếu → `blocked_data`, giữ artifact và status unresolved.
- Nếu deadline lỡ, lịch khác cần author; không auto nộp hay reset tiến độ. Nam không tự là mentor và không tự biết outcome; portfolio revision không sửa snapshot CV đã nộp.

## EV_Y5_LONG_CHECKIN — Nhịp sống khác

- Conditional W07 `[18:00,20:00)`, LOC_CANTEEN/ROOM, met Long, Long available; không giả định Long tốt nghiệp/đi làm. Nếu chưa gặp hoặc Long bận, không trigger và không tạo memory.
- Long hỏi “Dạo này ổn không?”. `share`: 15 phút E −1, ghi Long biết `player_studying_extra_year` khi completion; không tự tiết lộ điểm. `change_topic`: 15 phút E −1, không chia sẻ; `later`: 0 phút, event có thể expire trung tính. Nếu `conflict_light=open`, thoại có nhắc sự việc mà cả hai đã biết, nhưng không tự giải quyết.
- `discuss_conflict` chỉ xuất hiện khi conflict open, cả hai available: 15 phút E −2; fixture Long đồng ý một thỏa thuận sau khi player xác nhận, mark resolved, resentment −3 clamp 0. `defer` giữ open. Callback sau này chỉ đọc memory đã commit và knowledge của NPC.

## EV_Y5_RESOLVE — Chốt năm

- Scheduled W12, program configuration và tất cả requirement đã author. `eligible` khi toàn bộ predicates true, `missing` khi có ít nhất một false, `unresolved` khi thiếu dữ liệu cần đánh giá; UI liệt kê từng reason và nguồn. Không dùng `unresolved` như `missing`.
- Eligible: preview graduation, player confirm, atomic save rồi mở graduation. Missing: preview Y6 và recovery entries thực có, player confirm, atomic save rồi mở Y6. Cancel/save lỗi giữ state cũ. Nếu thiếu dữ liệu để tạo Y6 calendar/recovery thì giữ unresolved và báo, không xóa state hoặc cấp bằng.
- Credit ledger idempotent; course pass trước không được cộng lại; không tự giải quyết conflict hoặc gán first job.

## Kiểm tra cần làm

Fee trợ cấp + registration đúng 12:00; mất kết nối/save lỗi; course đã pass không bị đăng ký lại; player chưa gặp Long; conflict vẫn mở nếu không thỏa thuận; fixture B sửa đồ án không đổi CV cũ; `unresolved` không rơi vào nhánh Y6. Chưa có event records cho những thiếu hụt khác và chưa mô phỏng/playtest.
