# Tài nguyên và nghề nghiệp — đề xuất v0.1

Thông số dưới đây là dữ liệu giả lập phục vụ thiết kế, không đại diện chi phí thực tế và chưa cân bằng qua playtest. Các hệ thống phải dùng transaction, event ordering và save đã nêu trong `02_He_thong_thoi_gian_va_su_kien.md`.

## Năng lượng

**Mục đích:** tạo lý do nghỉ và giới hạn chuỗi hoạt động, không bắt người chơi chăm một thanh nhu cầu liên tục. `energy` nguyên 0–100, khởi đầu thử nghiệm 75. `energyBand`: kiệt 0–19, mệt 20–39, ổn 40–79, khỏe 80–100. Nghỉ và ngủ tăng energy, hoạt động có `energyCost`; chi phí được hiển thị trước khi xác nhận. Thay đổi clamp [0,100] và ghi vào ledger theo transactionId.

Mỗi activity định nghĩa cost theo phút hoặc milestone; thời gian, energy đã tiêu không hoàn khi dừng, phần chưa đến không bị tính. Nếu đến 0 giữa một activity có thể gián đoạn, dừng an toàn tại milestone gần nhất, lưu phần đã làm và mở nghỉ; hoạt động không thể gián đoạn phải công bố trước và chỉ được bắt đầu nếu đủ energy. Lựa chọn buộc lịch (ví dụ exam) không khóa vĩnh viễn khi thiếu energy: cho tham gia ở trạng thái mệt với phản hồi được giải thích, không tự trừ điểm thi nếu chưa có rule cụ thể. Mệt không tự khóa travel đến chỗ nghỉ.

Một cấu hình để thử: ngủ đủ theo lịch +60, nghỉ 30 phút +15, tự học 60 phút -10, game 30 phút -5, đi lại 20 phút -5; nhà thiết kế phải author riêng. UI cho thời gian, energy dự kiến và cảnh báo nếu activity có thể bị gián đoạn. Phục hồi: nghỉ/ngủ, nhờ hỗ trợ hoặc chọn activity nhẹ; không dùng tiền làm điều kiện duy nhất để phục hồi. Năm 2+ công việc và dự án tranh thời gian nghỉ, nhưng không cộng thêm chỉ số mới theo từng năm nếu không tạo quyết định.

## Tiền

`wallet`: số nguyên đơn vị tiền trong game; ledger ghi id, thời điểm, nguồn, amount, loại và transactionId. Không cho số dư âm ngoài khoản hỗ trợ/nợ được author và player đồng ý rõ. Hiển thị số dư, khoản đã cam kết, giá/thu nhập chắc chắn, thời điểm thanh toán và điều kiện hủy; dùng `available = balance - reservations`. Không được trả tiền hai lần sau load. Hoạt động liên tục có thể chia cost thành các milestone; nêu trước phần không hoàn lại.

Thu nhập có thể từ gia đình, trợ cấp, ca làm, freelance hoặc thực tập khi sự kiện/mốc đã mở; chi phí gồm di chuyển, ăn, tài liệu, hoạt động xã hội. Thiếu tiền: ẩn hoặc disable lựa chọn có phí, nhưng hiển thị lý do và gợi ý phương án miễn phí/chi phí thấp được author; không tự thêm khoản vay. Khi các nhu cầu thiết yếu phải trả phí, lịch phải bảo đảm một đường phục hồi khả dụng trong cửa sổ hợp lệ hoặc cung cấp hỗ trợ rõ. Thu nhập và sinh hoạt trong năm 1 do content author cụ thể quyết định; part-time mở từ năm 2 nếu có event, không bắt buộc mọi run.

## Năng lực và chứng cứ nghề nghiệp

`SkillNode`: id, prerequisites (nhóm ALL/ANY với mức), sources, progress 0–100, unlocks, year window. `Evidence`: id, loại course/project/internship/portfolio, skill tags, quality, date, visibility; không tự suy ra kinh nghiệm làm việc từ một lần xem tài liệu. Một activity ghi progress vào node liên quan và/hoặc evidence; không có menu chọn nghề một lần khóa vĩnh viễn. Kỹ năng có thể học qua môn, project hoặc mentor theo điều kiện riêng, tạo nhiều đường tới yêu cầu nghề.

Graph thử nghiệm: `programming_basics` → `oop` → `api_fundamentals`; `programming_basics` → `database_fundamentals`; `api_fundamentals` AND `database_fundamentals` → `backend_project`. Project hoàn thành sinh evidence có quality và skill tags. Một internship backend có thể yêu cầu `(backend_project completed OR equivalent_project evidence)` AND `api_fundamentals >= threshold`; threshold phải nằm ở company definition và được giải thích trong JD. Không khóa các nghề khác khi player học node này; skill dùng chung vẫn giữ khi đổi hướng.

`Project`: scope, deadline, task IDs, effort required/completed, blockers, collaborators, quality and evidence state. Mỗi session tiêu thời gian/energy; khi blocker xuất hiện, chọn hỏi giúp đỡ, giảm scope hoặc làm thêm, mỗi lựa chọn có cost và rủi ro được biết. Deadline chốt artifact theo effort và scope đã hoàn tất; thiếu feature không xóa thành quả, vẫn có thể tạo portfolio với trạng thái partial. Mỗi project cần author quy tắc quality cụ thể trước production; không suy từ một điểm học tổng quát.

## Thực tập và tìm việc

`CompanyDefinition`: id, archetype, role, location, eligibility predicates, application windows, interview steps, compensation range, hours, mentor/culture tags và offer expiry. `Application`: companyId, CV snapshot, phase, submittedAt, responseAt, outcome, visible reasons, attemptId. Trình tự `draft → submitted → screening → test → interview → rejected | offer → accepted | expired | declined`. Mỗi transition kiểm tra điều kiện, chỉ commit một lần và lưu trước khi hiển thị kết quả; khóa application đã nộp để thay CV không làm thay đổi đánh giá lịch sử. Người chơi được sửa CV trước hồ sơ mới.

Mỗi bước phải nêu thời gian thực hiện, deadline và dữ liệu được đánh giá trước khi bắt đầu; kết quả từ company-specific rule, không chỉ GPA. Khi từ chối có lý do tường minh mà công ty có thể cung cấp, đường tiếp theo gồm sửa CV, cải thiện evidence, mock interview hoặc nộp công ty khác. Không tự cấp offer hay cho game over khi bị từ chối. So sánh offer bằng vai trò, lương, địa điểm, giờ làm, mentor, hạn phản hồi và thông tin player đã biết; có thể từ chối. Acceptance là commit duy nhất, đóng các offer cạnh tranh và tạo first-job event sau graduation.

## Tốt nghiệp / năm thêm / ending

`GraduationRequirements` là cấu hình của khóa: requiredCredits, mandatoryCourseIds, capstone predicate, internship predicate nếu có, deadline và attempts. `resolveGraduation` trả `eligible` hoặc danh sách thiếu từng điều kiện kèm cách/khung thời gian recovery. Môn pass chỉ cộng một lần; fail không xóa credit đã có. Cuối năm 4/5 nếu đủ thì player có thể xác nhận tốt nghiệp, nếu chưa đủ tạo kế hoạch năm tiếp với lý do và cơ hội đã author; năm 6 là hạn cuối theo campaign, nhưng đường không đủ điều kiện và kết thúc thay thế cần được author thay vì buộc cấp bằng giả. Khi khóa chưa có dữ liệu không tự tính là đủ.

`EndingSnapshot` đọc credit/course results, years studied, career evidence, firstJob, relationships, conflicts, memory IDs và milestones từ state đã commit. Resolver chọn nội dung theo predicate rõ và thứ tự ưu tiên, có fallback khi NPC chưa gặp hoặc player không có internship. Không kết luận close friend chỉ từ tổng điểm; không gán việc làm nếu chưa chấp nhận offer. Chuỗi mục tiêu chính hướng đến nhận việc đầu tiên; nhánh hết năm 6 chưa tốt nghiệp phải có hậu quả và hướng phục hồi, không giả vờ đã đạt ending đó. Cần author chính sách retry/giới hạn thời gian sau năm 6 trước khi chốt campaign.

## Rủi ro và gate

Cần ma trận dữ liệu cho tất cả course, paths, company, deadline, cơ hội cứu và công thức evaluation; hiện chỉ có một ví dụ graph. Chưa có 6–8 career paths, danh sách công ty, ending variants hoặc mô phỏng ngân sách; không thể tuyên bố hệ thống cân bằng hay game-ready. QA cần kiểm tra số dư sau load, energy = 0, đúng deadline offer, đơn xin trùng, credit trùng, đổi hướng nghề, thiếu evidence và nhánh năm 6.
