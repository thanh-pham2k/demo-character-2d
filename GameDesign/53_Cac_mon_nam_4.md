# Năm 4 — bốn môn lý thuyết fixture v0.1

Dữ liệu hư cấu để thiết kế game; không phải hướng dẫn pháp luật hoặc khóa học chuyên môn. C_INTERN và C_CAP có fixture riêng `37`/`38`. Chưa có calendar, content lessons, đề retake, rubric hoàn chỉnh hay review chuyên gia. Các câu chỉ kiểm tra nguyên tắc cơ bản, không đại diện kiến thức nghề.

## Quy tắc chung

C_SECURITY, C_CLOUD, C_PRODUCT, C_LAW mỗi môn 3 credit; enroll khi prerequisites trong `31` đã pass và có registration window author. Assignment hai milestone ×45 phút E−7, submit 5 phút; 0/1/2 milestone = 0/50/100. Snapshot theo attempt, missed =0 chỉ khi course rule/calendar author, unknown không thành 0. Final 3 câu, standard 20 giây/câu, extended/untimed theo `40`; unanswered sai. `finalPoints=floor((200*correct+3)/6)`, `score=floor((7*finalPoints+3*assignmentPoints+5)/10)`, pass ≥50. Missed final =0 kèm reason; thiếu rubric/data → unresolved. Pass award 3 credit một lần/course; không tự tăng skill.

## C_SECURITY — an toàn ứng dụng

`ASG_SECURITY`: xác định đầu vào cần kiểm tra; mô tả quyền truy cập tối thiểu. Topics T_SECURITY_AUTH, T_SECURITY_INPUT, T_SECURITY_PRIVACY.
- SEC_Q1 AUTH: cấp quyền nên theo nguyên tắc? A ai cũng có quyền cao nhất, **B chỉ quyền cần cho nhiệm vụ**, C chia sẻ tài khoản chung.
- SEC_Q2 INPUT: dữ liệu từ client nên? **A kiểm tra trước khi sử dụng**, B luôn mặc định đáng tin, C bỏ qua định dạng.
- SEC_Q3 PRIVACY: dữ liệu cá nhân không cần thiết nên? A thu thật nhiều, **B hạn chế thu thập theo nhu cầu**, C công khai mặc định.

## C_CLOUD — triển khai

`ASG_CLOUD`: mô tả bước triển khai; tạo danh sách thông tin cần theo dõi. Topics T_CLOUD_DEPLOY, T_CLOUD_LOG, T_CLOUD_MONITOR.
- CLOUD_Q1 DEPLOY: trước khi triển khai thay đổi, nên? **A có kế hoạch kiểm tra và khôi phục**, B xóa bản cũ không kiểm tra, C bỏ qua môi trường.
- CLOUD_Q2 LOG: log hữu ích cho? A tự sửa mọi lỗi, **B điều tra sự kiện và lỗi**, C thay thế sao lưu.
- CLOUD_Q3 MONITOR: metric giúp? A bảo đảm hệ thống không bao giờ hỏng, **B quan sát trạng thái và phát hiện bất thường**, C xóa nhu cầu kiểm thử.

## C_PRODUCT — sản phẩm số

`ASG_PRODUCT`: xác định nhu cầu người dùng; chọn một chỉ số và cách xem xét. Topics T_PRODUCT_USER, T_PRODUCT_METRIC, T_PRODUCT_ITERATE.
- PRODUCT_Q1 USER: trước khi đề xuất tính năng nên? **A tìm hiểu nhu cầu người dùng**, B chỉ đoán tính năng, C bỏ qua bối cảnh.
- PRODUCT_Q2 METRIC: một chỉ số hữu ích cần? A không liên quan mục tiêu, **B gắn với câu hỏi cần đánh giá**, C không có định nghĩa.
- PRODUCT_Q3 ITERATE: sau thử nghiệm nên? A giữ nguyên dù bằng chứng trái ngược, **B xem kết quả và điều chỉnh giả thuyết**, C bỏ qua phản hồi.

## C_LAW — đạo đức và pháp lý

`ASG_LAW`: nhận diện dữ liệu cá nhân trong tình huống giả lập; kiểm tra quyền dùng một tài liệu. Topics T_LAW_PRIVACY, T_LAW_LICENSE, T_LAW_RESPONSIBILITY.
- LAW_Q1 PRIVACY: trước khi dùng dữ liệu cá nhân trong tình huống game, nên? **A xác định mục đích và quyền sử dụng**, B công khai mặc định, C bỏ qua người liên quan.
- LAW_Q2 LICENSE: trước khi dùng tài nguyên của người khác, nên? A tự cho phép mọi mục đích, **B kiểm tra giấy phép và điều kiện sử dụng**, C xóa thông tin nguồn.
- LAW_Q3 RESPONSIBILITY: nếu phát hiện thông tin có thể gây hại, nên? A che giấu bất kể hoàn cảnh, **B báo qua kênh phù hợp và xem xét tác động**, C đăng dữ liệu riêng tư công khai.

Đây là tình huống giáo dục giả lập, không kết luận quy định pháp luật hoặc thay tư vấn chuyên môn. Cần người có chuyên môn review trước phát hành.

## Chưa hoàn thiện

Chưa có hint, câu hỏi biến thể, lịch và đề retake; chưa tính được quỹ thời gian cùng C_INTERN/C_CAP. Đề chưa review, mô phỏng hay playtest. Không tự cấp 12 credit từ sự tồn tại của fixture.
