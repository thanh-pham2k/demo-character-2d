# Năm 6 — sự kiện mẫu v0.1

Thiết kế thử, không phải runtime hay luật trường thật. Chỉ mở sau khi snapshot Y5 chuyển Y6 đã commit; các mốc và cost là giả lập, chưa được cân bằng. Áp dụng scheduler/atomic save tại `02`, học tập tại `03`, resolver tại `04`.

## EV_Y6_REQUIREMENTS — Điều còn lại

- Major W01, ROOM/H6, Y6 transition committed. Dashboard đọc program configuration và records; phân biệt `satisfied`, `missing` và `unresolved`, liệt kê số credit còn thiếu, mandatory course/capstone và link slot có thật. UI 0 phút; không tự quyết định học hay nộp phí.
- `plan`: đặt reminder cho slot đã biết, không tự đăng ký; `view_evidence`: xem tiến độ; `later`: đóng dashboard. Thiếu course data hoặc calendar → hiển thị unresolved, không tạo slot giả.
- Không sửa kết quả course đã pass hoặc credit ledger; không gán NPC biết lý do. Nếu không mở dashboard, deadline thực vẫn chạy và calendar đã biết vẫn hiển thị.

## EV_Y6_RETAKE — Cơ hội học lại

- Conditional W02 `[08:00,12:00)` LOC_H6, fixture `C_NMT=failed` và slot `RET_NMT_Y6` author. Nếu đã pass không hiển thị. `register`: 15 phút E −2, phí 5M; kết thúc đúng 12:00 kịp. Voucher hỗ trợ miễn phí 30 phút E −2 rồi đăng ký 15 phút nếu player thiếu tiền và còn thời gian. Cancel không trừ phí chưa commit; thời gian/E đã tiêu không hoàn.
- Registration tạo attempt, không cấp credit, kiến thức trước giữ. Khi hoàn thành assessment, course resolver theo rubric; credit chỉ cộng một lần khi pass. Cửa sổ thi W09 là placeholder cho đến khi đề/rubric author; missing → blocked_data, không tạo kết quả giả.
- Nếu lỡ registration, báo slot đã hết và cơ hội khác nếu calendar có. Không tự đẩy tới Y7 hoặc giả định chắc chắn được thi lại. Cần data cho mọi course chứ không chỉ fixture này.

## EV_Y6_GOODBYE_ROOM — Ghé phòng

- Medium W07, LOC_ROOM nếu player còn quyền vào, hoặc ngưỡng cửa nếu không còn. Trigger khi tương tác địa điểm; không require Long. `look`: 10 phút E 0, hiển thị tối đa hai ký ức theo thứ tự preference và đúng predicate: `first_404`, `memory_long_game_d2`, `light_discussion`, `first_meal_long` chỉ nếu tồn tại. `leave`: 0 phút, không tạo memory mới.
- Nếu không có ký ức hợp lệ, dùng mô tả trung tính về căn phòng. Nếu Long available và player chọn nói chuyện, dùng event riêng; không tự xuất hiện hoặc tự giải quyết conflict. Không đổi học tập/tiền/career.

## EV_Y6_RECONCILE — Nói chuyện còn dang dở

- Conditional W06 `[18:00,20:00)`, ROOM/CANTEEN, `met_long=true`, conflict `light` open và Long available. Không trigger nếu chưa từng gặp hoặc conflict đã resolved. UI cho thấy thời gian 15 phút, E −2 và cảnh báo deadline đã biết.
- `discuss`: 15 phút; Long có quyền không đồng ý theo predicate/availability của event. Với fixture `light` severity 1 và Long available, hai người đồng ý thỏa thuận → mark conflict resolved, resentment −3 clamp 0, tạo memory có outcome. `later` hoặc `decline`: 0 phút, conflict giữ nguyên. Completion sau interruption chỉ commit khi đủ thời gian.
- Nếu lỡ window, conflict giữ nguyên, có cơ hội khác chỉ khi calendar author; graduation không require hòa giải. Không tự gán Long biết điểm/việc làm.

## EV_Y6_FINAL_REQUIREMENTS — Chốt trạng thái cuối

- Scheduled W10, resolver tính từng program predicate từ records và config. Truth table: tất cả true → `eligible`; có false với đủ dữ liệu → `missing`; thiếu dữ liệu cần thiết và không thể quyết định → `unresolved`. UI trình bày từng reason và recovery slot còn hiệu lực; không cấp degree.
- Khi `eligible`, cho preview ceremony và lựa chọn `confirm_graduation`; khi `missing`, cho preview outcome chưa tốt nghiệp ở W12 nếu policy được author; khi `unresolved`, giữ state, nêu data thiếu và không đoán outcome. `cancel` giữ nguyên; save thất bại giữ source snapshot.

## EV_Y6_NON_GRADUATE — Kết thúc chưa hoàn tất

- Fixture policy đề xuất, chỉ xuất hiện ở W12 khi program data đầy đủ và vẫn `missing`. Player xem yêu cầu còn thiếu, kết quả đã đạt và thông tin liên hệ hỗ trợ, có thể `review` hoặc `confirm`. Confirm atomically ghi `ending=continuing_outside_campaign`, `graduated=false`; không cấp bằng, credit hoặc việc làm. Đây là ending thay thế có tính kết thúc cho phạm vi campaign 6 năm, không mô phỏng năm 7.
- UI phải nói rõ story kết thúc tại đây và việc tiếp tục học bên ngoài campaign không được mô phỏng; không hứa chắc sẽ có slot học lại. Kết quả này khác mục tiêu chính first job và không được xem là đạt graduation. Không kích hoạt nếu state unresolved.

## EV_Y6_CEREMONY_PREP — Mời người tham dự

- Conditional W11 chỉ khi `eligible=true`; ceremony không bắt buộc có khách. Player chọn mời NPC đã gặp qua danh sách có availability, mỗi lời mời 5 phút E 0; NPC đồng ý/từ chối theo calendar và relation rule đã author, thiếu data hiện “chưa xác định”, không tự chấp nhận. `simple`: 0 phút và không mời; `later`: đóng.
- Lời mời không tự tạo ký ức đã tham dự. Sau graduation committed, ceremony xác định roster từ response đã commit và availability; không có khách dùng biến thể độc lập. Không tự gán An romance hoặc người thân đi dự.

## QA còn lại

Không có program/course toàn game, calendar thực, policy NPC hay rubric đánh giá; các predicate và outcome chưa được mô phỏng/playtest. Cần kiểm tra thiếu data không rơi vào `missing`, voucher chỉ dùng một lần, NPC vắng, memory trống, conflict đã đóng, save thất bại và outcome chưa tốt nghiệp không cấp bằng.
