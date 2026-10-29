# Năm 2 — fixture môn học v0.1

Dữ liệu game hư cấu, không phải chuẩn học thuật thực. Các course lấy ID/prerequisite từ `31`; chưa có calendar tuyệt đối, đề retake, lesson content hoặc runtime. Cần review chuyên môn trước production. Không gán skill khi pass nếu không có effect author riêng.

## Quy tắc cho 5 môn bài tập + đề

C_OOP, C_DSA, C_DB, C_MATH2, C_WEB: 3 credit; prereq course theo `31` phải pass trước khi enroll. Assignment hai milestone ×45 phút E−7, submit 5 phút; 0/1/2 milestone → 0/50/100 điểm; submission immutable. Không auto nhận late nếu calendar thiếu. Final ba câu 20 giây standard/câu, profile `40`; unanswered sai. Điểm cuối `floor((7*finalPoints+3*assignmentPoints+5)/10)`, finalPoints `floor((200*correct+3)/6)`, pass ≥50; missed final =0 có reason, missing data = unknown. Tất cả phải có attempt và window authored. Award 3 credit một lần/course khi pass; failed giữ topic evidence.

## C_OOP

Assignment `ASG_OOP`: thiết kế class đơn giản; thêm method. Topics T_OOP_CLASS, T_OOP_OBJECT, T_OOP_COMPOSE.
- OOP_Q1 CLASS: class mô tả gì? A một giá trị duy nhất, **B cấu trúc/hành vi của đối tượng**, C một ngày lịch.
- OOP_Q2 OBJECT: object thường là gì? **A một instance của class**, B tên khác của màn hình, C một mạng Wi-Fi.
- OOP_Q3 COMPOSE: composition có thể mô tả gì? A biến mọi object thành số, **B object chứa hoặc dùng object khác**, C xóa mọi thuộc tính.

## C_DSA

Assignment `ASG_DSA`: biểu diễn danh sách sinh viên; so sánh hai cách truy cập. Topics T_DSA_ARRAY, T_DSA_LIST, T_DSA_TREE.
- DSA_Q1 ARRAY: với mảng 0-based, phần tử đầu ở index? **A 0**, B 1, C −1.
- DSA_Q2 LIST: danh sách liên kết dùng gì nối node? A mật khẩu, **B tham chiếu/liên kết**, C màu nền.
- DSA_Q3 TREE: một tree có node gốc thường gọi là? A tail, **B root**, C cursor.

## C_DB

Assignment `ASG_DB`: phác bảng sinh viên; viết truy vấn lọc. Topics T_DB_MODEL, T_DB_QUERY, T_DB_INDEX.
- DB_Q1 MODEL: khóa chính dùng để? A tô màu dòng, **B định danh bản ghi**, C tăng độ sáng.
- DB_Q2 QUERY: câu lệnh SQL thường đọc dữ liệu? **A SELECT**, B DELETE, C DROP.
- DB_Q3 INDEX: index thường giúp gì? A bảo đảm mọi truy vấn nhanh tuyệt đối, **B hỗ trợ tìm kiếm theo cột được lập chỉ mục**, C thay toàn bộ dữ liệu.

## C_MATH2

Assignment `ASG_MATH2`: nhân ma trận đơn giản; tính xác suất một sự kiện. Topics T_MATH2_MATRIX, T_MATH2_PROB, T_MATH2_OPT.
- MATH2_Q1 MATRIX: vector (2,3) cộng (1,1)? A (2,3), **B (3,4)**, C (2,4).
- MATH2_Q2 PROB: đồng xu công bằng, xác suất mặt ngửa? **A 1/2**, B 1/3, C 1.
- MATH2_Q3 OPT: tìm giá trị nhỏ nhất trong {2,5,8}? **A 2**, B 5, C 8.

## C_WEB

Assignment `ASG_WEB`: tạo cấu trúc trang; thêm style và tương tác có thể truy cập. Topics T_WEB_HTML, T_WEB_STYLE, T_WEB_CLIENT.
- WEB_Q1 HTML: thẻ nào biểu diễn nút? A div, **B button**, C title.
- WEB_Q2 STYLE: CSS thường dùng để? A lưu trữ tài khoản, **B mô tả cách trình bày**, C thay server database.
- WEB_Q3 CLIENT: thuộc tính nào giúp mô tả nút biểu tượng khi không có chữ nhìn thấy? **A aria-label phù hợp**, B một màu bất kỳ, C xóa tên nút.

## C_TEAM — dự án nhóm

Theo `EV_Y2_GROUP_FORM` trong `16`: bốn milestone ×90 phút E−15, deadline W08 20:00 khi calendar author; player có thể làm solo hoặc theo nhóm. `submission` snapshot chứa task và actor contribution thực tế, không tự cộng milestone của NPC vắng. Mỗi milestone hoàn thành và được assessor chấp nhận =25/100; hoàn tất chưa đồng nghĩa accepted. Tổng ≥50 pass khi cả bốn milestone đã được đánh giá hoặc đánh dấu missing sau deadline; assessor/rubric thiếu → unresolved. Không có final quiz riêng trong fixture C_TEAM. Review milestone không tự resolve conflict. Pass course credit một lần, tạo project evidence theo task thực có, không tự grant skill/job.

## Chưa hoàn thiện

Chưa có gợi ý/hint cho các câu trên, lịch/route/NPC, retake questions, đánh giá qualitative C_TEAM và cách tính điểm thiếu assessment; các câu cần review và variants. Đề chưa được chạy, chưa chứng minh đủ lịch cho 6 môn hoặc recovery. Không gán 18 credits chỉ từ danh mục.
