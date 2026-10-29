# Năm 3 — sự kiện project và hướng nghiệp v0.1

Fixture độc lập để thiết kế, không phải runtime hay dữ liệu campaign hoàn chỉnh. Giờ/chi phí/số điểm dưới đây giả lập; không xem là mức cân bằng. Áp dụng các quy tắc transaction, trạng thái NPC, event và skill trong `02`–`04`.

## Fixture

W04 player tại LOC_PROJECT, E 75, wallet 30 M, skill `api_fundamentals=30`, `database_fundamentals=30`; chưa có `backend_project` evidence. Đây là giá trị nạp fixture, không áp đặt cho playthrough thật. Nam/Minh chỉ tham gia khi đã gặp và available. Project `P_LIBRARY_API`: hai task bắt buộc `API`, `STORE`; một task tùy chọn `DOCS`. Mỗi task 90 phút, E −15; lưu tiến độ theo milestone, chỉ hoàn thành task khi đủ 90 phút. Deadline demo W06 18:00; hủy không hoàn thời gian/E đã tiêu. Cửa sổ và event cụ thể dưới đây là data thử.

## EV_Y3_PROJECT_PROPOSAL — Chọn phạm vi

- Major; W04 `[09:00,18:00)`, LOC_PROJECT hoặc ROOM; trigger xem bảng project. `P_LIBRARY_API` hiển thị khi cả API và database ≥30; nếu thiếu hiển thị node thiếu và cách học đã author, không khóa mọi project. `P_DATA_REPORT` là hướng thay thế nhưng chưa có data, hiển thị `chưa khả dụng` thay vì hứa có thể chơi.
- `start_backend`: 15 phút E −2, 0 M; sau completion tạo project draft với hai task bắt buộc và deadline đã nêu; `later`: 0 phút; `leave`: 0 phút. Player xem scope/cost, deadline và task trước xác nhận. Không tự khóa ngành/career.
- Miss: không tạo project nếu chưa start; có thể tự học hoặc project khác khi có data. Relationship không đổi, không tự có mentor. Không tự sinh evidence lúc tạo draft. Lưu transaction theo instance.

## EV_Y3_PROJECT_BLOCKER — Thiếu thời gian

- Conditional; W05 `[16:00,20:00)`, tại ROOM/PROJECT; project active, đã xong API, STORE chưa xong, còn ít nhất 90 phút đến deadline. Event chỉ phản ánh blocker do task còn lại, không giả định NPC bỏ việc.
- `work`: chạy task STORE 90 phút E −15, khi xong `STORE=done`; `cut_scope`: 15 phút E −2, điều chỉnh scope để chỉ API bắt buộc, ghi `scope_reduced=true`; `ask_nam`: 30 phút E −4, Nam đã gặp và available; sau trả lời mở gợi ý kỹ thuật, không tự hoàn thành task. `defer`: 0 phút.
- Cắt scope làm `P_LIBRARY_API` đủ điều kiện demo khi API xong nhưng evidence ghi reduced scope; không gọi đó là full backend project nếu JD yêu cầu đầy đủ API + STORE. Sau khi cắt vẫn có thể làm STORE và nâng evidence khi task hoàn thành. Deadline xử lý completion đúng mốc trước chốt.
- Miss: quá window hoặc thiếu điều kiện → không tạo conflict; player vẫn có thể làm task. Không thay trust chỉ vì hỏi giúp. Không tự bật referral.

## EV_Y3_HACKATHON — Cuối tuần bận rộn

- Major; W05 `[09:00,13:00)`, LOC_PROJECT; event đã biết trước từ bảng tin, phí 0 M, dự kiến E −30. Hai lựa chọn khác: học 60 phút E −10 hoặc ca làm đã có hợp đồng; UI hiển thị lịch thi và ca đã biết. Tự đi tới location mất thời gian theo route khi có data.
- `join`: hoạt động 240 phút; tạo attempt với team đã chọn, E phân bổ theo thời gian. Sau 120 phút là checkpoint `prototype`; nếu dừng sau checkpoint giữ artifact partial, dừng trước checkpoint không có artifact. Khi hoàn tất tạo `hackathon_demo` evidence với `scope=prototype`, memory có actor thực tham gia; không tự cho thắng.
- `study`, `work`, `rest`, `skip` dùng activity tương ứng, không có memory hackathon. Nếu đã chọn ca và hackathon trùng giờ, UI yêu cầu hủy ca hoặc không join trước khi commit; không tạo hai activity đồng thời. Conflict event trong team chỉ mở khi có record được author, không tự áp dụng.
- Miss: cửa sổ hết hoặc thiếu E → bỏ lỡ event, vẫn có project cá nhân và cơ hội nghề khác khi dữ liệu cho phép. Nộp demo không tự cấp offer hoặc referral.

## EV_Y3_PROJECT_DEMO — Đóng project

- Scheduled W06 18:00, `P_LIBRARY_API` tồn tại. Tại deadline lấy snapshot task và scope: `full` nếu API + STORE xong; `reduced` nếu API xong và scope_reduced; `partial` nếu không thỏa. Tạo evidence `project_library_api` một lần với task list và quality trạng thái; không đổi course credit nếu project là cá nhân.
- Player có thể `submit_now` trước deadline hoặc để hệ thống chốt snapshot tại deadline đã thông báo; chưa tạo project → không có evidence. Sau deadline có thể sửa artifact cá nhân nhưng không sửa snapshot deadline; khi sửa hoàn tất tạo revision mới với `parentEvidenceId`, không ghi đè lịch sử ứng tuyển.
- Nam có thể review chỉ nếu được mời và available, feedback riêng không tự tăng quality. Nếu project partial, UI cho thấy task thiếu và cơ hội tiếp tục sau deadline.

## EV_Y3_CV_AND_APPLICATION — Hồ sơ đầu tiên

- Major; W08–W10, LOC_CAREER/ROOM. `CV`: chọn evidence mà player sở hữu, có thể bao gồm project partial được ghi đúng nhãn; không cho claim internship chưa có. `review`: 45 phút E −5, reviewer available; `self_edit`: 30 phút E −3; `later`: 0 phút. Review tạo feedback, không tự sửa skill.
- Fixture JD `INTERNSHIP_API_A`: W10 `[09:00,17:00)`; requirements API ≥30 AND database ≥30 AND evidence `project_library_api` full (bao gồm revision full), không yêu cầu Nam. UI liệt kê điều kiện thiếu. `apply`: 30 phút E −5, dự kiến kết thúc phải ≤17:00; snapshot CV/skills/evidence tại lúc submit. Cùng attemptId không nộp hai lần.
- Screening outcome fixture: đủ điều kiện và snapshot có full evidence → mời technical test vào W11 theo lịch; thiếu bất kỳ điều kiện nào → không cho nộp và hiện lý do. Pass screening không phải offer. Nếu người chơi có reduced/partial, đường recovery là hoàn thiện task STORE và tạo revision full, rồi nộp nếu còn window; sau window, cần một JD/đợt khác đã author, không tự tái mở.
- Cơ hội khác: career center đăng `INTERNSHIP_GENERAL_B` về sau nhưng chưa có requirements/calendar, phải đánh dấu chưa author; vì vậy fixture chưa chứng minh player có đường apply sau W10.

## EV_Y3_TEST_FEEDBACK — Kết quả bài test

- Conditional W11 theo lịch `INTERNSHIP_API_A`, application đã qua screening. Cần test question IDs, rubric và timer để thực sự tính; nếu thiếu data → `blocked_data`, không tạo rejection/offer giả.
- Khi có data: `pass` chuyển tới interview, `fail` ghi rejection kèm lý do được công ty công khai, giữ CV snapshot; player có thể review kỹ năng, sửa CV và nộp cơ hội khác nếu có. Không tạo mất quan hệ với Nam nếu Nam không liên quan.

## Chưa đạt

Chưa có dữ liệu đề technical test, công ty thay thế, event khác của Year 3, chuẩn E khi interrupt, route và timing cụ thể theo ngày. Bản này chưa chứng minh đường recovery sau expiry hay qua Năm 4. Các điều kiện, chi phí và consequences cần simulation, playtest và QA.
