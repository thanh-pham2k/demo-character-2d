# Năm 1 — năm môn nền, fixture đề và bài v0.1

Dữ liệu hư cấu để phát triển game, không đại diện curriculum thật. Bổ sung 5 course IDs của `31`, cùng C_NMT (`14`/`30`) thành 6 môn seed Năm 1. Chưa có calendar không chồng nhau, đề thi lại riêng, content lessons, route hoặc simulation; không coi là sáu môn playable hoàn chỉnh.

## Quy tắc fixture dùng chung

Mỗi môn 3 credit, prerequisite course NONE, cần enroll trong registration window được author. Mỗi môn có một assignment hai milestones ×45 phút, E−7/milestone; submit 5 phút E0, immutable snapshot 0/1/2 milestones = 0/50/100. Nộp muộn chỉ được nhận nếu course calendar có late window, không tự tạo window; missed theo calendar fixture tính 0 với reason, unresolved không tính 0. Các nhiệm vụ được mô tả riêng bên dưới; completion là kết quả abstraction, chưa có interaction đồ họa.

Final gồm ba câu, mỗi câu 20 giây profile standard; extended/untimed theo `40`. `finalPoints=floor((200*correct+3)/6)`, `courseScore=floor((7*finalPoints+3*assignmentPoints+5)/10)`, pass ≥50; unanswered sai. Eligibility: enrolled trong fixture, không thêm attendance rule ẩn. Mỗi course resolver chỉ chốt khi assignment và final đã resolved/missed, định nghĩa đầy đủ và attempt đã đóng; pass award 3 credits unique per courseId, không nhân đôi khi retake. Question IDs prefix course để tránh collision. Không tự tăng skill khi pass: effect skill nếu có phải author riêng.

Attendance là narrative/feedback theo class entry thực tế, không dùng làm ngưỡng trong fixture. Cần thêm timeline cho từng môn, missed-assignment rule trên UI, review assessment và bộ reexam khác trước khi vận hành.

## C_MATH1 — Toán cơ sở

Assignment `ASG_MATH1`: milestone 1 lập bảng giá trị hàm, milestone 2 giải thích thay đổi giữa hai giá trị. Topics: T_MATH1_FUNC, T_MATH1_LIMIT, T_MATH1_RATE.

- `FIN_MATH1_01` FUNC: f(x)=2x, f(3)=? A 5, **B 6**, C 8. Hint: nhân 2 với 3.
- `FIN_MATH1_02` LIMIT: Khi x tiến gần 2, x+1 tiến gần? A 2, **B 3**, C 4. Hint: xét giá trị x gần 2.
- `FIN_MATH1_03` RATE: Giá trị tăng từ 4 lên 7 thì tăng bao nhiêu? A 2, **B 3**, C 4. Hint: lấy giá trị sau trừ trước.

## C_PHYS1 — Vật lý nền tảng

Assignment `ASG_PHYS1`: milestone 1 ghi vị trí theo thời gian, milestone 2 giải thích lực trong ví dụ. Topics: T_PHYS1_MOTION, T_PHYS1_FORCE, T_PHYS1_ENERGY.

- `FIN_PHYS1_01` MOTION: Đi 10 m trong 2 s với vận tốc không đổi, tốc độ? A 2 m/s, **B 5 m/s**, C 20 m/s. Hint: quãng đường chia thời gian.
- `FIN_PHYS1_02` FORCE: Đẩy một vật về phía phải, lực đẩy hướng? A trái, **B phải**, C không hướng. Hint: hướng lực theo chiều đẩy.
- `FIN_PHYS1_03` ENERGY: Một vật nâng lên cao có dạng năng lượng nào tăng trong mô hình cơ bản? **A thế năng trọng trường**, B nhiệt độ chắc chắn, C điện tích chắc chắn. Hint: xét độ cao.

## C_COMM — Giao tiếp

Assignment `ASG_COMM`: milestone 1 soạn thư hỏi lịch, milestone 2 chuẩn bị phản hồi cho bạn trong nhóm. Topics: T_COMM_LISTEN, T_COMM_WRITE, T_COMM_PRESENT.

- `FIN_COMM_01` LISTEN: Khi chưa hiểu bạn, cách hữu ích? **A hỏi lại để xác nhận**, B ngắt lời, C giả vờ hiểu. Hint: đảm bảo hiểu ý người kia.
- `FIN_COMM_02` WRITE: Thư xin lịch cần có gì? A chỉ biểu tượng cảm xúc, **B mục đích và thời gian đề xuất**, C không có lời chào. Hint: người nhận cần biết yêu cầu.
- `FIN_COMM_03` PRESENT: Trước khi trình bày, nên làm gì? A không cần chuẩn bị, **B xác định ý chính và người nghe**, C đọc nhanh nhất có thể. Hint: ưu tiên người nghe hiểu.

## C_DIGITAL — Kỹ năng số

Assignment `ASG_DIGITAL`: milestone 1 tổ chức thư mục bài học, milestone 2 lập kế hoạch bảo vệ tài khoản. Topics: T_DIGITAL_FILES, T_DIGITAL_WEB, T_DIGITAL_SAFE.

- `FIN_DIGITAL_01` FILES: Tên file nào dễ tìm bản nộp hơn? A final-final, **B baitap_mon_ngay_v2**, C x. Hint: tên có nội dung và phiên bản.
- `FIN_DIGITAL_02` WEB: Muốn biết một thông tin trên web còn đúng không, nên? **A xem nguồn và ngày cập nhật**, B tin mọi tiêu đề, C chỉ đọc URL. Hint: tìm nguồn gốc và thời gian.
- `FIN_DIGITAL_03` SAFE: Khi nhận yêu cầu cung cấp mật khẩu bất ngờ, nên? A gửi ngay, **B xác minh qua kênh chính thức**, C đăng công khai. Hint: không chia sẻ mật khẩu qua yêu cầu lạ.

## C_ENGLISH — Tiếng Anh chuyên ngành cơ sở

Assignment `ASG_ENGLISH`: milestone 1 đọc hướng dẫn ngắn, milestone 2 viết câu hỏi làm rõ. Topics: T_ENGLISH_READ, T_ENGLISH_TERM, T_ENGLISH_WRITE.

- `FIN_ENGLISH_01` READ: “Submit before Friday” nghĩa là? A nộp sau thứ Sáu, **B nộp trước thứ Sáu**, C không cần nộp. Hint: before = trước.
- `FIN_ENGLISH_02` TERM: “deadline” gần nghĩa nào? A đồng đội, **B hạn chót**, C phòng học. Hint: liên quan thời gian phải hoàn thành.
- `FIN_ENGLISH_03` WRITE: Câu hỏi lịch sự để làm rõ yêu cầu? **A Could you clarify the requirement?**, B I no need details, C Ignore requirement. Hint: chọn câu hỏi rõ ràng.

## Giới hạn và QA

Các câu là ví dụ ngắn, chưa review bởi chuyên gia hoặc thiết kế distractor/độ khó; một bộ ba câu không chứng minh học phần sâu hay replayability. Chưa author lịch, question variants, retake và policy missed/excused từng course; không dùng lại đề y hệt vô hạn. Kiểm tra IDs/topic mapping, đáp án, boundary, untimed, save/retry, credit và tất cả `unresolved` trước khi triển khai. Chưa chạy simulation hay playtest.
