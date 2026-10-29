# Năm 3 — fixture môn học v0.1

Các course và câu hỏi là nội dung thử cho game hư cấu, không phải tài liệu học tập đầy đủ. Prerequisite và credit trong `31`; chưa có ngày/giờ, rubric chi tiết, đề retake, localization hoặc runtime. Chưa review chuyên môn hay playtest.

## Quy tắc chung cho 5 môn bài tập + final

C_API, C_SYSTEM, C_NETWORK, C_DATA, C_TEST mỗi môn 3 credit; enrollment chỉ khi course prerequisites đã pass và registration window đã author. Assignment 2 milestones ×45 phút E−7, submit 5 phút: 0/1/2 milestones → 0/50/100, immutable per attempt. Missed submission =0 có reason nếu calendar author; unknown không đổi thành 0. Final 3 câu, standard 20 giây/câu, profile theo `40`, unanswered sai; `finalPoints=floor((200*correct+3)/6)`, `score=floor((7*finalPoints+3*assignmentPoints+5)/10)`, pass ≥50. Missed final =0 kèm reason; chưa có data = unresolved. Resolver đóng attempt một lần, pass award 3 credit unique/course; pass không tự tăng skill.

## C_API

`ASG_API`: xác định endpoint; mô tả response/error. Topics T_API_ROUTE, T_API_DATA, T_API_ERRORS.
- API_Q1 ROUTE: endpoint thường dùng để xác định? A màu màn hình, **B tài nguyên hoặc chức năng được yêu cầu**, C tiền học phí.
- API_Q2 DATA: response cần mô tả dữ liệu thế nào? **A cấu trúc rõ và nhất quán**, B thay đổi ngẫu nhiên, C không cần thông tin.
- API_Q3 ERRORS: đầu vào sai nên? A báo thành công, **B trả lỗi phù hợp và thông tin hữu ích**, C xóa toàn bộ dữ liệu.

## C_SYSTEM

`ASG_SYSTEM`: mô tả một process; ghi cách xử lý dữ liệu vào/ra. Topics T_SYSTEM_PROCESS, T_SYSTEM_MEMORY, T_SYSTEM_IO.
- SYSTEM_Q1 PROCESS: process là gì? **A chương trình đang thực thi**, B tên thư viện trường, C một màn hình cố định.
- SYSTEM_Q2 MEMORY: bộ nhớ dùng để? A thay địa chỉ mạng, **B lưu dữ liệu và trạng thái đang được xử lý**, C thay nguồn điện.
- SYSTEM_Q3 IO: đọc file là hoạt động? A chỉ hiển thị màu, **B nhập/xuất dữ liệu**, C đăng ký môn.

## C_NETWORK

`ASG_NETWORK`: vẽ tuyến request cơ bản; xác định thông tin mạng cần thiết. Topics T_NETWORK_ADDRESS, T_NETWORK_PROTOCOL, T_NETWORK_REQUEST.
- NET_Q1 ADDRESS: địa chỉ IP dùng để? **A nhận diện điểm trên mạng**, B đổi kích thước font, C tính tín chỉ.
- NET_Q2 PROTOCOL: protocol là? A tên một NPC, **B tập quy tắc giao tiếp**, C loại ghế.
- NET_Q3 REQUEST: client gửi request nhằm? A luôn xóa máy chủ, **B yêu cầu tài nguyên/chức năng**, C tự cấp bằng.

## C_DATA

`ASG_DATA`: kiểm tra giá trị thiếu; trình bày kết quả. Topics T_DATA_CLEAN, T_DATA_STATS, T_DATA_PRESENT.
- DATA_Q1 CLEAN: dữ liệu thiếu nên? A tự bịa không ghi chú, **B kiểm tra và nêu cách xử lý**, C xóa toàn bộ mà không xem.
- DATA_Q2 STATS: trung bình của 2 và 4? A 2, **B 3**, C 6.
- DATA_Q3 PRESENT: biểu đồ tốt cần? **A nhãn/trục rõ ràng**, B giấu đơn vị, C không nêu dữ liệu.

## C_TEST

`ASG_TEST`: viết test case; thiết kế kiểm tra một boundary. Topics T_TEST_CASE, T_TEST_UNIT, T_TEST_AUTOMATE.
- TEST_Q1 CASE: test case cần gì? **A điều kiện và kết quả mong đợi**, B tên màu không liên quan, C chỉ một emoji.
- TEST_Q2 UNIT: unit test thường tập trung? A cả công ty, **B một đơn vị hành vi nhỏ**, C giá vé xe.
- TEST_Q3 AUTOMATE: automation có lợi gì? A bảo đảm không còn lỗi, **B cho phép lặp kiểm tra nhất quán**, C thay việc hiểu yêu cầu.

## C_PROJECT — dự án cá nhân

Điều kiện C_TEAM AND C_OOP passed và enrollment. `P_LIBRARY_API` trong `18` là một artifact **cá nhân tự chọn**, không tự trở thành bài C_PROJECT: player cần chọn `link_project` trước deadline, snapshot evidence đúng version. Các project khác có thể link nếu rubric course author cho phép; chưa có data thì disabled.

Course fixture có ba hạng mục `scope`, `build`, `demo`; completion task được assessor đánh giá accepted/rejected/unknown; không có assessor/rubric -> unknown. Điểm lần lượt 20/50/30, accepted đủ điểm, rejected 0 có reason; tổng ≥50 pass sau khi cả ba resolved và course attempt đóng. Missing deadline cần rule riêng trước khi xem là 0; nếu chưa author → unresolved. Pass +3 credit một lần, không biến project partial thành full hay sửa CV snapshot đã nộp.

Ví dụ: scope accepted + build accepted + demo rejected =70, pass; project evidence vẫn giữ `partial` nếu thiếu task của project definition. Nếu demo unknown, resolver chưa có kết quả dù hai phần trước đã accepted. Đây là tính tay, chưa chạy.

## Gaps

Chưa có hints, tình huống sâu/quiz variants, deadline/route từng course, retake/assessor calendars hoặc assessment interactions. Ba câu không đủ đại diện chương trình/hay bảo đảm trải nghiệm; cần review, simulation và playtest. Không cấp 18 credits chỉ vì fixture tồn tại.
