# Năm 4 — thực tập và đồ án v0.1

Fixture độc lập; số liệu là giả thuyết, không phải luật thật hay game đã chạy. Áp dụng schema thời gian/transaction, academic và career trong `02`–`04`. Các mốc W là ví dụ; cần calendar thực trước runtime.

## Fixture

Player E 75, wallet 30 M, chưa có hợp đồng; công ty thử `CO_STARTUP_A`, vị trí intern API `JD_A`, W02 `[09:00,17:00)`; requirement `api_fundamentals >=30` AND (`project_library_api` full OR evidence project tương đương được author). Không có referral vẫn ứng tuyển được. Phí 0; CV snapshot và thời lượng 30 phút E −5. Đây là cấu hình thử, không mặc định player thực đã đạt.

## EV_Y4_APPLY — Đơn thực tập

- Major, W02 LOC_CAREER hoặc ROOM, JD đã biết. UI trình bày yêu cầu, lịch test/phỏng vấn và ca dự kiến, time/energy, thời hạn; điều kiện chưa đạt hiển thị rõ và không nhận đơn. `apply`: snapshot CV/evidence, 30 phút E −5; `prepare`: chọn activity tự học/project riêng; `later`: 0 phút.
- Submission kết thúc đúng 17:00 được nhận; sau đó hết hạn. Một attemptId, retry mạng không tạo hai đơn. Screening chỉ kiểm tra predicate của JD; đúng → test invitation W03, sai → không cho submit trong fixture. Screen pass không phải offer.
- Miss: hết window không tạo rejection; đường khác cần công ty/calendar thật, hiện chưa author. Không tự cho Nam referral hoặc Long biết.

## EV_Y4_INTERVIEW — Đánh giá và phản hồi

- Scheduled W03 `[09:00,10:00)`, invited và LOC_WORK; test trong fixture gồm hai câu chưa author, thời lượng 30 phút E −5. Thiếu câu hỏi/rubric → `blocked_data`, không quyết định outcome. UI cho thấy deadline và quy trình trước khi bắt đầu.
- Khi data có: `start` lưu attempt và timer, `submit` lưu câu trả lời; fail → rejection + lý do công ty được phép chia sẻ; pass → interview invitation theo calendar. `withdraw`: đóng attempt và ghi `withdrawn`, không nhận rejection giả. Đến sau 10:00 → `missed`, cho xem lịch ứng tuyển khác nếu author.
- Offer chỉ sau interview outcome được author, có deadline và lương/giờ làm/mentor đã biết. Acceptance là transaction duy nhất, tạo internship contract; từ chối hoặc hết hạn không khóa nghề. Không đủ data thì không tạo offer.

## EV_Y4_BUG — Báo sự cố

- Conditional major W05 tại LOC_WORK, player có active contract, đã hoàn thành task đưa thay đổi vào staging và phát hiện issue `BUG_A`; mentor available trong fixture. Không gán lỗi cho player nếu task chưa xảy ra.
- `report`: 15 phút E −2, tạo `bug_reported`, mentor biết sự cố; mở buổi xử lý 45 phút E −6 nếu cả hai available. `ask_colleague`: 30 phút E −4, đồng nghiệp available; tạo investigation note, không tự giải quyết. `investigate_alone`: 60 phút E −10, tạo note khi xong; tự fix chỉ khi có task và rubric. `defer`: 0 phút, bug vẫn open, deadline theo contract được hiển thị.
- Khi deadline tới, nếu bug chưa đóng thì ghi incident `unresolved`, không tự sa thải. Callback của mentor dựa trên note/report họ thực biết; không chia sẻ với Long nếu player không kể. `incident`/`resolution` idempotent, chưa định nghĩa reputation score.
- Không có internship hoặc mentor vắng → không xuất hiện cảnh; học và capstone vẫn có đường tiếp tục nếu calendar author.

## EV_Y4_CAPSTONE_PROPOSAL — Chọn phạm vi

- Major W06 LOC_PROJECT, player đăng ký capstone và course predicate hợp lệ. Fixture chỉ author đường solo `CAP_A`: `proposal`, `implementation`, `review` ba milestone; mỗi milestone 90 phút E −15; deadline W10 17:00. Nhóm/research/industry hiển thị chưa có data, không chọn được trong fixture.
- `start_solo`: 15 phút E −2, commit draft với task/deadline; `review_requirements`: 0 phút; `later`: 0 phút. Sau mỗi milestone hoàn tất ghi progress; interruption không tự hoàn milestone. Không có tiền phí trong fixture.
- Giảng viên Hạnh chỉ xuất hiện nếu được phân công và available; player có thể nộp proposal qua kênh được author. Chưa có assignment mentor → unresolved, không ép Hạnh làm mentor.

## EV_Y4_CAPSTONE_REVIEW — Chọn cách hoàn thiện

- Conditional W07, CAP_A active, proposal done; review event có rubric/giảng viên được author. Giả lập nhận feedback “implementation chưa đủ”, không tự gán điểm.
- `work`: 90 phút E −15 cho milestone; `reduce_scope`: 30 phút E −5, yêu cầu giảng viên chấp nhận, nếu không thì scope cũ giữ; `ask`: 20 phút E −3 nếu giảng viên rảnh, nhận feedback, không tự hoàn milestone; `later`: 0 phút.
- Hết window mà chưa review: project giữ tiến độ, được nộp theo rule course. Deadline: snapshot completed/partial; partial không tự fail trước rubric. Conflict NPC chỉ khi dialogue outcome tạo record rõ.

## EV_Y4_GRAD_AUDIT — Kiểm tra điều kiện

- Scheduled W11; resolver đọc `requiredCredits`, `mandatoryCourseIds`, `capstone predicate`, `internship predicate` từ cấu hình khóa và các course/evidence đã commit. Fixture chưa có program configuration đầy đủ; `blocked_data` là kết quả hợp lệ khi thiếu, không giả định 120 credits hay graduation.
- Khi data đủ, `eligible` chỉ khi mọi predicate true; false trả reason từng điều kiện, link tới recovery entry thực có và deadline. `review`: 0 phút; `graduate`: chỉ enabled nếu eligible, tạo preview; `cancel`: giữ state; `confirm`: atomically save `graduationCommitted` và mở job search.
- Nếu không đủ, player chọn xem kế hoạch Y5 sau khi author calendar; không ép commit Y5 nếu save thất bại. Nếu eligibility unresolved vì missing data, không tự chuyển nhánh fail hoặc grant degree.
- Năm 4 chưa thực tập không tự làm fail nếu program không yêu cầu internship; nếu program yêu cầu, UI phải công bố và recovery phải có calendar.

## Chưa đạt

Chưa có rubric, đề interview, offer, company alternatives, program requirements, lịch retake/capstone và bản ghi chuyển năm đầy đủ. Cần kiểm tra completion đúng deadline, task interruption, save fail, no-internship và data missing; chưa chạy simulation hay playtest.
