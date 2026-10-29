# Hướng nghề và graph kỹ năng — v0.1

Tám hướng dưới đây là cấu trúc thiết kế đề xuất, không phải danh sách vị trí thực, dữ liệu tuyển dụng hay nội dung đã triển khai. Môn tham chiếu `31`; skill là state riêng, không tự tăng khi môn pass trừ effect cụ thể được author. Các threshold, cost, công ty và timeline chưa cân bằng.

## Graph dùng chung

`S_BASIC` → `S_OOP`; `S_BASIC` → `S_WEB`, `S_DATA`, `S_QA`; `S_OOP` + `S_DB` → `S_API`; `S_BASIC` + `S_OOP` → `S_MOBILE`/`S_GAME`; `S_MATH` + `S_DATA` → `S_ML`; `S_NETWORK` + `S_SYSTEM` → `S_CLOUD`. Các mũi tên là prerequisite đề xuất, cần map từng node thành AST `all/any`, source activity, threshold, chi phí, availability và downstream. Không tự khóa học skill vì career khác; nhiều node có thể đạt từ course, project hoặc hướng dẫn đã author.

Bảng viết `P_x` là project fixture chưa có record, `I_x` là internship fixture chưa có company/contract, `J_x` là junior job fixture chưa có JD/rubric. `mentor` chỉ là cơ hội khi đã gặp và rảnh; không là yêu cầu duy nhất. Mỗi mục cần thành data có thể thực thi trước khi coi là hoàn chỉnh.

| Hướng | Core / optional | Môn có thể tạo cơ hội | Project / portfolio | CLB, mentor, internship | Phỏng vấn / company pool | Event và biến thể kết |
|---|---|---|---|---|---|---|
| Backend | S_API, S_DB; optional S_CLOUD | C_OOP, C_DB, C_API | P_API có endpoint + lưu dữ liệu; evidence full hoặc partial | Coding club/Nam, I_BACKEND | API, data, errors; startup/product/outsource | bug/review; role backend theo offer accepted |
| Frontend | S_WEB, S_UI; optional S_TEST | C_WEB, C_PRODUCT, C_TEST | P_UI giao diện + tương tác | CLB/Nam hoặc tự học; I_FRONTEND | web, accessibility, test; product/outsource | demo responsive; role frontend theo offer |
| Mobile | S_MOBILE, S_OOP; optional S_UI | C_OOP, C_PRODUCT | P_MOBILE prototype + lưu dữ liệu | CLB/mobile meetup; I_MOBILE | state, UI, data; product/startup | demo thiết bị; role mobile theo offer |
| AI/ML | S_MATH, S_DATA, S_ML; optional S_CLOUD | C_MATH2, C_DATA | P_ML xử lý dữ liệu + đánh giá | Hạnh/nhóm nghiên cứu hoặc tự học; I_ML | dữ liệu, đánh giá, giới hạn; research/product | chọn chất lượng dữ liệu; role ML theo offer |
| Data | S_DATA, S_DB; optional S_MATH | C_DB, C_DATA | P_DATA làm sạch + báo cáo | thư viện/mentor hoặc tự học; I_DATA | query, thống kê, giải thích; product/services | báo cáo thiếu dữ liệu; role analyst theo offer |
| Game | S_GAME, S_OOP; optional S_UI | C_OOP, C_PROJECT | P_GAME prototype có gameplay | CLB game hoặc tự học; I_GAME | loop, state, debug; indie/studio | prototype bị trễ; role game theo offer |
| QA/Automation | S_QA, S_TEST; optional S_API | C_TEST, C_API | P_TEST test plan + automation | mentor/CLB hoặc tự học; I_QA | test case, lỗi, automation; outsource/product | lỗi tái hiện không ổn định; role QA theo offer |
| DevOps/Cloud | S_CLOUD, S_SYSTEM, S_NETWORK; optional S_SECURITY | C_SYSTEM, C_NETWORK, C_CLOUD | P_DEPLOY build + logs | mentor hoặc tự học; I_CLOUD | deploy, logs, incident; product/services | sự cố staging; role cloud theo offer |

## Các điều kiện và đường vào

Một JD định nghĩa rõ ALL/ANY của skill levels, evidence type/version, requirement thực tập nếu có, application window, assessment và terms. Không có CLB/mentor không phải khóa toàn bộ hướng; mỗi hướng cần ít nhất một đường self-study + project được author và một cơ hội công ty khác nếu lỡ đợt đầu. Hiện mới là yêu cầu thiết kế, chưa được chứng minh bằng calendar/data.

Đổi hướng: giữ skills/evidence cũ, học node còn thiếu, tạo revision/project mới; CV snapshot đã nộp không thay. Một evidence có thể gắn nhiều tags nhưng phải mô tả đúng scope, không coi partial là full. Career ending chỉ dùng `acceptedOffer.role`; việc học `S_API` không tự nhận chức danh backend.

## Các record cần author

Cho mỗi `P_x`: scope, task IDs, duration, cost, blocker, quality rubric, deadline, interrupted/partial/revision, evidence tags và ít nhất một đường tự làm. Cho mỗi `I_x`: company, contract, pay, shift, eligibility, alternatives, assessment, mentor availability, completion và recovery. Cho mỗi `J_x`: public/hidden predicates, curriculum/evidence alternatives, test/interview questions/rubric, feedback, offer/expiry và retry calendar. Không xem tên trong bảng là event database hoặc job có thể chơi.

C_INTERN của `PROGRAM_IT_A` là bắt buộc; cần có lựa chọn internship và lịch phục hồi phù hợp cho cả tám hướng hoặc điều chỉnh program có phương án tương đương. Fixture campus practical trong `37_Hoc_phan_thuc_tap.md` đề xuất một đường thay thế, nhưng chưa có calendar/assessment đủ để chứng minh điều này. Cần rà prerequisite graph, tránh soft-lock và đo scarcity trước khi chốt độ khó; chưa chạy simulation/playtest.
