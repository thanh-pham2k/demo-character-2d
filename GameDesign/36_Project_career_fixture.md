# Project cho tám hướng nghề — fixture v0.1

Đề xuất hư cấu, chưa triển khai hoặc cân bằng. `P_LIBRARY_API` tiếp tục dùng `18`; bảng này bổ sung 7 project. Skill/session theo `35`, time/energy theo `29`; tất cả tên/task sau là dữ liệu thử, không chứng minh nghề thực tế. Project cá nhân không cấp credit trừ khi course definition riêng author hiệu lực.

## Quy tắc chung

- UI hiển thị prerequisite, scope, 2 task bắt buộc, time/E, deadline đã author, risk và điều kiện tạo evidence. Start 15 phút E −2, 0M, tại ROOM/PROJECT. Player chủ động chọn, không require CLB/mentor. Y3–Y6 khi đã biết địa điểm; sau tốt nghiệp cần postgrad calendar/location được author mới mở tiếp, không giả định.
- Mỗi task 90 phút E −15, 0M; `runId`, elapsed và cumulative E như `29`. Chỉ completion commit task, interruption giữ elapsed nếu definition cho resume; có thể nghỉ và resume mà không sinh runId mới. Chọn task nào trước tùy player nếu bảng không nêu prerequisite nội bộ.
- Project có `deadline` từ calendar đã author; thiếu deadline → `blocked_data`, không ngầm tạo deadline. Submission trước deadline hoặc auto snapshot tại deadline nếu player đã được thông báo: đủ hai task bắt buộc = `full`, một hoặc không task = `partial`. `reduced` chỉ khi có thỏa thuận scope author riêng, không được coi là full. Snapshot evidence immutable với task list, skill tags, source and quality; sau deadline sửa tạo revision mới có parentEvidenceId.
- Một lần tự làm blocker hoặc nhờ mentor là lựa chọn tùy event, không tự xuất hiện/giải quyết. Nếu mentor unavailable, self-recovery dùng chính task còn thiếu khi thời gian cho phép. Không mặc định evidence được chấp nhận trong mọi JD; JD predicate quyết định.

## Định nghĩa project

| ID / hướng | Điều kiện start (ALL) | Task bắt buộc | Blocker / choice seed | Evidence tags sau submission |
|---|---|---|---|---|
| P_UI / frontend | S_WEB ≥30, S_UI ≥30 | layout, interaction | chữ tràn: sửa layout hoặc giảm scope; cần event/rubric | web, ui, actual_tasks |
| P_MOBILE / mobile | S_MOBILE ≥30, S_UI ≥30 | screen, persistence | state bị mất: sửa persistence hoặc demo partial | mobile, ui, actual_tasks |
| P_ML / ML | S_ML ≥30, S_DATA ≥30 | clean_data, evaluate | dữ liệu thiếu: ghi giới hạn hoặc bổ sung dữ liệu trong window | ml, data, actual_tasks |
| P_DATA / data | S_DATA ≥30, S_DB ≥30 | query, report | kết quả truy vấn thiếu: kiểm tra hoặc công bố giới hạn | data, database, actual_tasks |
| P_GAME / game | S_GAME ≥30, S_OOP ≥30 | loop, state | lỗi chuyển cảnh: sửa state hoặc nộp prototype partial | game, oop, actual_tasks |
| P_TEST / QA | S_QA ≥30, S_TEST ≥30 | plan, automation | test không ổn định: viết lại hoặc trình bày test thủ công | qa, test, actual_tasks |
| P_DEPLOY / cloud | S_CLOUD ≥30, S_NETWORK ≥30 | deploy, logging | thiếu log: thêm logging hoặc giữ scope partial | cloud, network, actual_tasks |

Mỗi blocker ở bảng hiện chỉ là seed: chưa có trigger, duration, choice result hay dialogue, vì vậy chưa được scheduler tự chạy. Các project bắt đầu khi user chọn và đủ ngưỡng; không tự bốc blocker ngẫu nhiên. Sự khác biệt về task và evidence là giả thuyết thiết kế, cần writer, programmer và playtest đánh giá.

## Thời gian, tài nguyên và ví dụ

`P_UI`: từ S_WEB=30, S_UI=30, E=75, ROOM đã mở, bắt đầu 09:00 và calendar đặt deadline 14:00: start 09:00–09:15 (E−2), layout 09:15–10:45 (E−15), interaction 10:45–12:15 (E−15), nộp 0 phút → full evidence với task list. Còn E=43 nếu không có activity khác; số này là tính tay, không mô phỏng. Nếu 10:45 player đi hoạt động khác qua deadline, snapshot chỉ có layout → partial. Khi trở lại, tạo revision sau khi interaction hoàn tất; không sửa CV snapshot đã nộp trước đó.

## Phụ thuộc còn thiếu

Không có calendar chung Y3–postgrad, JD cho bảy project, rubric nội dung, công ty internship và năng lực truy cập location đã author. Không có danh sách event blocker thực thi hoặc số dư ban đầu/chi phí sinh hoạt bảo đảm thời gian. Vì thế bảng này không chứng minh tất cả hướng có thể hoàn thành hoặc đủ C_INTERN. Cần QA predicate/alias, deadline chính xác, interruption, cost, immutable revision và đúng phân biệt partial/full; chưa chạy test hay playtest.
