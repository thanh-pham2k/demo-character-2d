# C_INTERN — fixture thực tập và phương án tương đương v0.1

Program hư cấu `PROGRAM_IT_A` trong `31` yêu cầu pass C_INTERN (3 credits). Không phải quy định trường thực; data, thời gian và tiền chỉ để thiết kế. Chưa thực thi, chưa chứng minh cân bằng hoặc có đủ lịch.

## Đăng ký và lựa chọn

Prerequisite `C_PROJECT=passed`, chưa từng pass C_INTERN, registration window được author. Mỗi courseAttemptId là một lần đăng ký, có `route=external|campus`, activity ledger, evidence và `course_status`. UI trước khi chọn hiển thị yêu cầu, phí, deadline, thời gian tối thiểu, tiền/energy, assessment và recovery của từng route. Không có dữ liệu nghĩa là `blocked_data`, không tự cho chọn.

### Route external: `R_EXT`

Cần internship contract đã accepted, được course công nhận cho cùng attempt, có người hướng dẫn và calendar. Một contract không tự thành course pass. Fixture gồm 3 shifts, mỗi shift 240 phút, E −30; thu nhập +60 M sau completion theo contract nếu có. `onboarding`, `task`, `review` được hoàn tất tương ứng với 3 shifts; mỗi entry có ID riêng và phải đúng cửa sổ. Mentor nhận xét theo rubric, không mặc định positive.

Evidence external phải bao gồm contractId, 3 completed shift IDs, task artifact, mentor review và player reflection đã nộp. Player có thể xin review khi mentor available; không tự tạo mentor response khi người đó vắng. Thiếu shift, artifact hoặc review là incomplete, không pass.

### Route campus: `R_CAMPUS`

Practical lab của trường, không giả là internship công ty. Được program này cho phép thay thế yêu cầu học phần, nhưng ending/CV ghi chính xác là campus practical, không phải company internship. C_PROJECT passed, đăng ký trong window; không yêu cầu đã gặp Nam hay có offer. Có 3 sessions 240 phút, E −30, không trả lương, 0 M phí trong fixture. Sessions tạo `onboarding`, `task`, `review`; instructor có calendar. Hoàn thành và nộp artifact/reflection + instructor review để được evaluate.

## Rubric fixture và assessment

`onboarding` 20 điểm, `task artifact` 50, `reflection/review` 30. Mỗi thành phần được evaluator đánh giá theo data author, `unknown` nếu chưa có rubric hoặc review; tổng ≥50 và đủ 3 thành phần = pass. Dữ liệu v0.1 chỉ cho phép hai điểm cho mỗi phần: completed & accepted = tối đa phần đó; missing/rejected = 0 với reason; chưa đánh giá = unknown. Player được xem feedback và có cơ hội sửa trước deadline được author; completion session không tự thành accepted.

Lưu submission snapshot, evaluator ID/availability, assessment version, reason, response time. Khi đủ data resolver commit pass/fail một lần; pass cấp 3 credits qua course ledger, không cộng thêm vì program có internshipPredicate. `failed|missed` giữ records/evidence; retake phải tạo attemptId mới, không nhân credit; `unknown` không bị biến thành fail hay pass.

## Calendar fixture

Y4 W02–W03 nhận đăng ký course, W04–W06 sessions, W07 review, W08 deadline; Y5 W02–W03 và Y6 W02–W03 là các cửa sổ phục hồi **đề xuất**. Cần gán ngày/giờ cụ thể, phòng, capacity và lịch evaluator trước runtime. Route external có thể không có offer; route campus là cơ hội thay thế nhưng chưa thể coi bảo đảm cho đến khi calendar, capacity và program data author/được mô phỏng.

Nếu rớt review, feedback cho biết phần chưa đạt và slot sửa nếu còn; nếu bỏ lỡ W08, course attempt ghi missed và xem slot năm sau đã author. Nếu phí/vị trí chưa có data hoặc E không đủ, UI nêu rõ thay vì chạy tự động. Campus 0 M không bảo đảm player đủ sinh hoạt phí.

## Liên kết và tình huống

- Player chưa có CLB/mentor/offer, C_PROJECT passed → có thể chọn R_CAMPUS khi slot và instructor hợp lệ; hoàn thành course không sinh external internship evidence.
- Player có contract nhưng bỏ ca → route external incomplete; được xin chuyển route **chỉ nếu** chương trình author rule chuyển, chưa được xác định ở fixture này; không tự di chuyển progress giữa routes.
- Player pass external → award 3 credit một lần, evidence chỉ dùng trong CV nếu player chọn; accepted offer công việc full-time không tự sinh.
- Thiếu evaluator/rubric → unresolved, không chuyển năm dựa trên giả định pass/fail.

## Chưa đạt / QA

Cần ngày tuyệt đối, capacity và availability, assessment text, accessibility, route switch, data contract và rubric cụ thể cho 8 hướng. Kiểm tra save/restore giữa ca, trả lương một lần, teacher vắng, fee, external vs campus evidence, review rejected, pass 3 credits một lần và retake. Chưa chạy simulation hay playtest; chưa chứng minh đường tốt nghiệp.
