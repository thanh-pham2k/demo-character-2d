# C_CAP — đồ án và bảo vệ fixture v0.1

Môn C_CAP 3 credit trong program hư cấu `31`, prerequisite C_PROJECT và C_TEAM passed. Dữ liệu dưới đây là thử thiết kế; không đại diện quy định thật. Đề xuất một route solo `CAP_A` theo `20`, các route khác chưa author. Yêu cầu program và calendar phải hiển thị rõ cho player trước khi chọn.

## State và phạm vi

`CapstoneAttempt`: attemptId, courseId, route, assignedAssessorId, proposal/implementation/review milestones, immutable submissions, defenseAttempt, result. `CAP_A` có ba milestone mỗi cái 90 phút E −15, 0 M; `proposal` → `implementation` → `review` theo thứ tự; chỉ commit completion sau đủ thời gian, interruption theo `29`. Sự kiện phát hiện thiếu implementation không tự sửa task. Review milestone là chuẩn bị nội dung, **không thay cho** đánh giá của giảng viên/hội đồng.

`C_CAP_Y4`: registration W05, proposal/build/review W06–W10, deadline nộp W10 17:00; bảo vệ W10 `[18:00,19:00)` tại H6. W là nhãn, ngày tuyệt đối/location routes và assessor availability phải author trước runtime. Start project 15 phút E −2 như `20`. Môn tạo courseAttemptId riêng; personal project API và C_CAP là hai hồ sơ khác nhau, có thể tham chiếu evidence nhưng không dùng task API/STORE thay milestone capstone.

## Nộp và đánh giá

Player chọn submit một snapshot ở ROOM/PROJECT, 5 phút E 0; submit completion đúng 17:00 hợp lệ. Nếu không submit trước deadline thì `missed_submission`, không tự tạo bản nộp hay pass; lưu lại progress và thông báo lịch phục hồi đã author. Submission snapshot lưu IDs/milestone/evidence version, không đổi khi player tiếp tục chỉnh; sau deadline chỉ được tạo revision cho lần đánh giá tiếp theo.

Giả lập rubric: proposal accepted 20, implementation accepted 50, defense accepted 30; mỗi phần **đã được assessor đánh giá** nhận đủ điểm hoặc 0 với reason, chưa đánh giá = unknown. Đủ cả ba result và tổng ≥50 là pass; thiếu response = unresolved. Ba milestone hoàn tất không tự bảo đảm assessor accepted. Đánh giá phải ghi assessorId, rubricVersion, timestamp và reason được công khai; thiếu assessor hoặc rubric → blocked_data. Cho phép feedback/sửa trước deadline nếu window author.

Defense: chỉ mở khi đã submit và có lịch/hội đồng; bắt đầu `[18:00,19:00)` trong fixture, player có thể trình bày 30 phút E −5. Nhánh hoàn thành đúng 19:00 hợp lệ; hội đồng phản hồi theo rubric tại thời điểm đã author, không tự pass khi chỉ tới phòng. Không tham gia trước end → `missed_defense`; không tự ghi 0 trừ khi rubric quy định. Player có thể xem phản hồi hoặc hoãn nếu có window được author.

## Course resolver và credit

Resolver chỉ chạy khi mọi thành phần có đánh giá hoặc có rule `missed` tường minh; case missing deadline trong fixture ghi course result `failed`, reason `missed_submission|missed_defense`; case assessor unknown là unresolved. Pass C_CAP → creditLedger +3 đúng một lần; program capstone predicate đọc `passed(C_CAP)`, không cộng thêm credit. Failed giữ project progress và evidence; không tự cấp bằng.

## Recovery Y5/Y6 fixture

Calendar đề xuất: Y5 W04–W09 sửa, W10 17:00 submit, W10 18:00–19:00 defense; Y6 có window tương tự nếu program author. `retryAttempt` tạo courseAttemptId mới với tham chiếu progress cũ, không sửa submission/history của attempt trước. Player có thể copy từng milestone hoàn tất làm starting artifact, nhưng assessor vẫn đánh giá snapshot mới; tiến độ không tự thành accepted. Không thu phí trong fixture, trừ thời gian/E của milestone sửa. Nếu assessor/calendar không đủ data → unresolved; không hứa pass.

Ví dụ Y5 fixture B: capstone cũ có proposal completed, implementation chưa hoàn thành, review chưa hoàn thành. Player tiếp tục implementation 90 phút E −15, review 90 phút E −15, submit 5 phút rồi defense 30 phút E −5 trong cửa sổ; sau đó vẫn phải chờ đủ assessor results. Đây là ví dụ tính thời gian, không chứng minh hợp lịch/có đủ E.

## QA cần thiết

Prerequisite chưa pass; submit completion đúng deadline; không submit; assessor vắng; defense kết thúc đúng end; interrupt/resume; partial review; failed Y4 rồi Y5 pass; source artifact không bị sửa; course credit một lần; graduation unresolved nếu thiếu assessor. Chưa chạy mô phỏng hay playtest; các route nhóm/research/industry, calendar thực và tiêu chí qualitative còn thiếu.
