# Tìm việc sau tốt nghiệp — thiết kế v0.1

Bản thử thiết kế, không phải tư vấn nghề nghiệp hay mô hình tuyển dụng thực. Dữ liệu công ty, lương và chi phí là số giả lập để so sánh lựa chọn. Dùng `graduationCommitted` và `handoff` của `25_Tot_nghiep_va_ban_giao.md`; không mở nhánh tốt nghiệp nếu chưa có commit. Chưa có runtime hoặc playtest.

## State, lịch và quy trình

`JobMarket`: calendar, `knownJobIds`, seed; `Application`: attemptId, JD version, CV snapshot/evidence version, stage, result, deadline, response reason được phép hiển thị; `Offer`: id, role, pay, location, hours, mentor info biết được, expiresAt, status; `OfferAcceptance`: transactionId, acceptedOfferId, startDate. Nhiều hồ sơ có thể chờ cùng lúc; chỉ một hoạt động có time cost chạy tại một thời điểm. Chỉ có thể accept một offer trên campaign. Hồ sơ/offer không được tự sinh khi thiếu data.

`discover → JD known → draft → submitted → screened → test → interview → rejected | offer → accepted | declined | expired`. Mỗi transition có predicate, clock, idempotency key; CV snapshot không thay đổi nếu player chỉnh bản hiện tại. Nếu thiếu rubric/timer: `blocked_data`, không tự pass/fail. Deadline theo time system; completion đúng expiry được xử lý trước expiration. Confirm accept phải revalidate offer active, snapshot và atomic save; lỗi giữ state cũ. Khi accept thành công, đánh dấu các offer cạnh tranh `withdrawn_after_accept` và khóa lựa chọn thứ hai; không gán firstJob trước commit.

## Vòng chơi

1. Mở job board 0 phút; xem yêu cầu công khai, lương/giờ/địa điểm nếu công bố, deadline và quy trình. Lọc theo skill hoặc loại evidence, nhưng chưa biết không được tự coi là không đạt.
2. Sửa CV 30 phút, E −3; player chọn evidence thực và đúng version/quality, có thể nêu project partial với nhãn rõ. Review cùng mentor 45 phút, E −5 khi NPC available; không đảm bảo được tuyển.
3. Apply 30 phút, E −5, 0 M; hoàn tất trước hoặc đúng deadline. Nếu điều kiện cứng không đạt, UI nêu thiếu và không nộp. Các tiêu chí không công khai không được hứa chắc tỷ lệ đậu.
4. Khi có invitation, player chọn ngày trong windows đã author. Test và interview có duration, data, rubric, thời hạn và hậu quả được công bố phù hợp. Rejection cho reason công ty có thể chia sẻ, giữ hồ sơ lịch sử.
5. Người chơi có thể ôn, nâng project, tìm cơ hội khác hoặc nghỉ. Không giới hạn số lần nộp trừ rule công ty; không tự xóa evidence hoặc ép chọn offer đầu tiên. Calendar không có cơ hội → hiển thị thiếu content thay vì tự sinh offer.
6. So sánh các offer active, chấp nhận một hoặc từ chối. Ngày đầu là scene riêng sau accepted + graduation, có thể liên hệ ký ức player tự biết; không tự nhận quan hệ/role khác.

## Công ty fixture

| ID | Loại / role | Requirements công khai | Nội dung fixture | Offer giả lập |
|---|---|---|---|---|
| CO_A | Startup / junior backend | API ≥30 AND database ≥30 AND project full hoặc evidence tương đương | deadline D5 17:00; screening, test, interview; rubric chưa author | 100M/tháng, gần trường, mentor nếu công ty xác nhận; expiry D12 17:00 |
| CO_B | Product / junior frontend | frontend ≥30 AND project frontend evidence | deadline D6 17:00; screening, test, interview; rubric chưa author | 120M/tháng, xa hơn, giờ ổn định; expiry D12 17:00 |
| CO_C | Công ty dịch vụ / junior generalist | programming_basics ≥20 AND bất kỳ project evidence (partial được ghi đúng) | deadline D7 17:00; screening, interview; rubric chưa author | 80M/tháng, gần nhà, mentor chưa biết; expiry D13 17:00 |

Các con số trên **không phải giá thị trường**. Các JD cho thấy ý định có nhiều cổng vào, nhưng hiện thiếu course/skill graph, test và công ty khác; không chứng minh mọi player đều có cơ hội. Không tạo offer chỉ vì thỏa requirement; deadline offer chỉ được áp dụng nếu offer thực sự phát hành.

## Sự kiện và recovery

`POST_JD_DISCOVERY`: board có deadline, 0 phút; `POST_CV_EDIT`: chọn evidence, commit sau hoạt động; `POST_APPLY`: immutable snapshot + reservation, save/commit một lần; `POST_TEST`: assessment data và timer, thiếu data `blocked_data`; `POST_INTERVIEW`: invitation và rubric, không tự đoán câu trả lời; `POST_REJECT`: hiện reason và hoạt động recovery, không xóa lịch sử; `POST_COMPARE`: chỉ active offers; `POST_ACCEPT`: revalidate + atomic commit; `POST_FIRST_DAY`: sau startDate với accepted offer, không chạy hai lần.

Ví dụ feedback trước khi nộp: CO_A công khai thiếu project full sau khi kiểm tra CV; player có thể sửa project sau deadline rồi tìm đợt khác nếu được author, hoặc nộp CO_C nếu đủ điều kiện. Không hứa luôn có đợt thứ hai. Nếu mọi cơ hội hết hạn, fixture một đợt phục hồi C2 được mô tả trong `33_Thi_truong_viec_lam_phuc_hoi.md`; sau đợt đó vẫn thiếu policy và content, do đó gap soft-lock chưa được giải hoàn toàn.

## QA cần có

Kiểm tra snapshot CV không thay khi evidence đổi; apply đúng deadline; thiếu rubric không tạo outcome; rejection/expired không tạo offer; song song hai offer và accept một lần; save lỗi lúc accept; offer hết hạn trong activity; load trước/sau deadline; không có mentor/không internship; hồ sơ partial không bị gọi full; graduation false không vào first day. Chưa mô phỏng hay playtest và chưa có dữ liệu đủ để xác nhận đường tới first job.
