# Thị trường việc làm và cơ hội phục hồi — fixture v0.1

Thiết kế hư cấu, số liệu không phản ánh thị trường thực, chưa mô phỏng/playtest. Mở sau `graduationCommitted` theo `25`; không dùng fixture để suy ra mọi career path đã hoàn chỉnh. Thu nhập giai đoạn chờ cần economy data riêng; thị trường không tự tạo tiền.

## Nhịp đợt tuyển

Thời điểm D1 09:00 là ngày đầu sau handoff. Lịch chỉ hiện khi người chơi biết job board; đọc board 0 phút. Các đợt có ID và khoảng cố định, không copy deadline bằng hệ thống random:

| Đợt / JD | Nhận hồ sơ | Nộp xong | Đánh giá | Offer và hạn |
|---|---|---|---|---|
| A1 `CO_A_API` | D1 09:00–D5 17:00 | trước hoặc đúng D5 17:00 | test D7 09:00–10:00; interview D9 09:00–10:00 | sau pass interview D9 12:00, hạn D12 17:00 |
| C1 `CO_C_GENERAL` | D1 09:00–D7 17:00 | trước hoặc đúng D7 17:00 | interview D10 09:00–10:00 | sau pass interview D10 12:00, hạn D13 17:00 |
| C2 `CO_C_GENERAL` | D15 09:00–D21 17:00 | trước hoặc đúng D21 17:00 | interview D24 09:00–10:00 | sau pass interview D24 12:00, hạn D27 17:00 |

Mỗi đợt chỉ áp dụng nếu company/JD data tồn tại; `CO_B` trong `26` chưa được đặc tả test nên không tính vào fixture này. Ngày, time zone là clock campaign, không phải thời gian thực. Mỗi đợt cho một attempt, nếu withdrawn/missed/rejected, player có thể nộp đợt khác khi hợp lệ. Không tự tạo vô hạn đợt; hết C2 mà không offer → `awaiting_next_authored_window`, cho phép hoạt động khác, không tự kết thúc hoặc offer. Đây là gap nội dung cần giải trước production.

## JD và kiểm tra hồ sơ

A1 điều kiện công khai API ≥30 AND database ≥30 AND evidence API full; C1/C2 programming_basics ≥20 AND evidence project bất kỳ được ghi đúng partial/full. Không đạt điều kiện cứng → không nộp được, UI chỉ rõ từng điều kiện thiếu; đây không phải rejection và không tạo attempt. Khi đủ, nộp 30 phút E −5, snapshot skill/evidence/CV; completion đúng deadline được chấp nhận. Screening fixture xác nhận requirements còn đúng trong snapshot → mời test/interview; không thêm điều kiện ẩn. Nếu snapshot thiếu data cần đánh giá → `blocked_data`, không tự reject.

Để thống nhất với `26`, ví dụ “CO_A công khai thiếu project full sau review” là feedback kiểm tra CV **trước submit**, không phải rejection sau screening của fixture này. Các công ty khác có thể có tiêu chí screening được author rõ.

## Test và interview fixture

A1 test 2 câu, mỗi câu 30 giây, cần ≥1 đúng; world clock pause, hết câu không chọn = sai; timer policy background và accessibility chưa chốt. Câu A1-1: REST request `GET` thường dùng để đọc dữ liệu hay xóa dữ liệu? **Đọc**. Câu A1-2: khi đầu vào không hợp lệ, nên trả thông báo lỗi rõ hay báo thành công giả? **Thông báo lỗi rõ**. Nếu không pass, record rejected `test_failed` và cung cấp điểm 0/2, không cho biết thêm tiêu chí ẩn. Câu hỏi là nội dung game hư cấu, không đại diện quy trình tuyển thật.

Interview A1/C1/C2 là hoạt động 30 phút E −5 tại WORK, bắt đầu trong cửa sổ và kết thúc không sau end. Mỗi interview có 2 lựa chọn: `show_evidence` nêu đúng scope của một project đã có trong snapshot; `invent_evidence` khai không đúng, bị từ chối với reason `unsupported_claim`. Nếu player chọn đúng và evidence đủ JD → offer tại giờ phát hành; nếu không tham dự → missed, không tự reject trước deadline. Không ép player khai sai; không có project khớp thì không nộp được từ đầu theo hard requirement. Rubric này là fixture đơn giản, chưa đánh giá sự hấp dẫn/công bằng.

Offer A: 100M/tháng, mentor được xác nhận, hạn D12 17:00. Offer C: 80M/tháng, mentor chưa biết, hạn theo bảng. Lương là đơn vị game. Confirm accept revalidate active + graduation, save atomically; không tự nhận việc nếu player không confirm. Sau accept, các offer khác withdraw theo `26`; first-day event được author theo startDate.

## Recovery và giới hạn

Sau A1 fail test: xem feedback 0 phút; ôn 60 phút E −10 là activity đã author và có thể nộp C1 nếu chưa hết hạn, hoặc C2. Sau C1 fail interview: C2 là đợt khác, cần apply mới và snapshot mới; không tự cấp offer. Không có project evidence: có thể tạo project theo data thực có; `P_LIBRARY_API` có quy định ở `18` nhưng prerequisites/skill source vẫn chưa có data đầy đủ, không được coi là recovery chắc chắn.

Policy sau D27, tiền sinh hoạt, các hướng ngoài API/generalist và lịch internship bắt buộc C_INTERN vẫn thiếu. Fixture không chứng minh mọi player đến được first job hoặc đủ tiền. Khi không có opportunity authored, hiển thị khoảng trống nội dung; không claim player bị khóa vĩnh viễn hay tự skip đến offer.

## QA yêu cầu

CV không đổi sau submit; 1/2 test đúng pass; bỏ trống 0/2 fail; background timer chưa có policy phải chặn runtime; interview hoàn tất đúng end; no-show; C1 rejected rồi nộp C2; cả hai offer active cùng lúc, accept một lần; rollback save; D27 không có offer không tự first job. Các ca này là expected, chưa được chạy.

**Policy đề xuất:** xem `40_Policy_timer_va_accessibility.md`; chưa được triển khai hoặc kiểm chứng, các ghi chú “chưa chốt” ở trên phản ánh trạng thái fixture.
