# Tốt nghiệp và bàn giao — v0.1

Đề xuất thiết kế, không phải quy chế thật hoặc runtime. Dùng cho năm 4, 5 hoặc 6 khi có cấu hình chương trình và dữ liệu học tập đầy đủ. Các predicate, ngày và chi phí trong ví dụ cần author/kiểm chứng trước production.

## Dữ liệu và invariants

`GraduationCandidate`: programId, academicRevision, creditLedger, mandatoryCourseIds, capstoneRecord, internshipRequirementRecord, outstandingFlags. `GraduationEvaluation`: evaluationId, timestamp, inputRevision, từng predicate `true|false|unknown` kèm evidence/reason, tổng `eligible|missing|unresolved`. `GraduationCommit`: commitId, evaluationId, year, timestamp, inputRevision, degreeAwarded, snapshotId. `Ceremony`: invitation records, roster, participation, memories.

Predicate gốc do program definition; chưa có definition → unknown, không giả định đủ. Graduation credit tổng là tổng ledger từ các course pass, mỗi course ID một credit award; không cộng credit khi chỉ đạt assessment. Một graduate commit tối đa cho một campaign; yêu cầu `eligible`, đúng inputRevision, player đã xác nhận. Re-evaluate trước commit nếu hồ sơ thay đổi; không dùng preview stale. `degreeAwarded` chỉ true sau atomic save thành công, không phụ thuộc dự lễ.

## Thứ tự giao dịch

1. **Audit:** lấy snapshot, đánh giá predicates và hiển thị các điều kiện cùng evidence, unknown/missing và recovery đã author. UI đọc, 0 phút. Không gán graduation.
2. **Preparation tùy chọn:** người chơi có thể chuẩn bị/mời khách sau khi eligible. Invitation có actor, response, timestamp, availability và expiry; chi phí và thời gian hiển thị trước. Chưa có response thì unknown; không tự đồng ý. Không require khách, trang phục trả phí hay relation threshold để lấy bằng.
3. **Preview/confirm:** liệt kê năm, chương trình, credit đã xác nhận, điều kiện, deadline và trạng thái sự kiện được biết. Cancel không đổi state. Sau confirm, so sánh inputRevision, tạo graduation commit và handoff snapshot, ghi atomically một lần. Nếu lưu lỗi, giữ nguồn và cho retry, không hiển thị bằng hay mở job search.
4. **Lễ:** chỉ sau commit; người chơi có thể tham dự hoặc bỏ qua nếu calendar cho phép. Roster lấy từ invitations được xác nhận và availability tại thời điểm lễ; không có khách thì dùng biến thể một mình. Một invitation không tự thành memory dự lễ. Ceremony completion sinh memory với actor có mặt thực tế, idempotent. Bỏ qua không hủy bằng.
5. **Bàn giao:** sau ceremony completion hoặc skip, chuyển postgrad. Nếu ceremony đã hết window, ghi missed và cho tiếp tục job search. Handoff được tạo một lần; resume sau crash không chạy lại ceremony reward.

## Tình huống không tốt nghiệp

`missing` tại cuối Y4/Y5: hiển thị reason và route recovery trong calendar đã author, preview chuyển năm và cho cancel; nếu chưa có route dữ liệu thì giữ unresolved của transition, không xóa hồ sơ. `missing` tại cuối Y6 khi program data đầy đủ: dùng kết thúc thay thế trong `24_Nam_6_su_kien_mau.md`, chỉ sau confirm; bằng và first job vẫn false. `unresolved`: giữ hồ sơ, không phân loại failed hoặc good standing và không tạo ending giả.

## Snapshot bàn giao tới job search

Lưu version và IDs: graduation commit, program/year, course records/credit ledger, skill levels, immutable evidence versions, project/internship records, quan hệ/conflict, memory/knowledge, wallet/energy, calendar postgrad, application/offer states và RNG state. CV được tạo từ evidence thực; dữ liệu nhạy cảm hoặc các sự kiện NPC chưa biết không tự đưa vào CV. Offer trước graduation không tự được chấp nhận; nếu offer còn hạn, giữ trạng thái và điều kiện hợp đồng. Không tự đóng deadline trong thời gian pause/menu. Nếu calendar postgrad thiếu thì handoff có thể được lưu nhưng UI báo chưa có nội dung, không sinh việc làm.

## Nội dung lễ và callback

Nhân vật có thể chọn ảnh tập thể khi có ít nhất một khách, ảnh riêng, hoặc không chụp. Memory `graduation_photo` chỉ sinh nếu chọn chụp và xác nhận, với roster thật. Recap hiển thị năm, credits, môn đạt/chưa đạt, project, internship, quan hệ và ký ức đã xảy ra; không suy luận một nghề hay relationship ending từ một chỉ số đơn lẻ. Long nhắc chuyện xe chỉ khi Long biết; An không mặc định là người yêu. Memory selection có fallback không khách/không memory; nội dung thể hiện cả recovery và việc chưa hoàn thành mà không phán xét.

## QA/giới hạn

Các ca cần kiểm tra: true/false/unknown, academicRevision thay đổi sau preview, hủy, save lỗi trước commit, retry commit, load sau commit trước lễ, lễ không khách, invitation chưa có response, ceremony missed, credit từng môn chỉ cộng một lần, Y6 chưa đủ điều kiện không cấp bằng. Program configuration, roster policies, costs và job market chưa đủ dữ liệu; chưa có simulation hoặc playtest. Không tuyên bố graduation resolver đã hoàn chỉnh.
