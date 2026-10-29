# Chương trình học giả lập — đề xuất v0.1

Dữ liệu thiết kế cho game hư cấu, không phải chương trình/điều kiện tốt nghiệp của trường thực. Tất cả môn và topic trừ fixture `C_NMT` đều là định nghĩa sơ bộ, chưa có bài/đề/rubric hoặc calendar. Không xem các ID này là đã triển khai.

## Program và credit

`PROGRAM_IT_A`: 24 môn bắt buộc, mỗi môn 3 credit, tổng 72 credit, `requiredCredits=72`, `mandatoryCourseIds` là toàn bộ 24 ID dưới đây; `capstonePredicate=passed(C_CAP)`; `internshipPredicate=passed(C_INTERN)` trong cấu hình thử này. `C_CAP` và `C_INTERN` cũng là môn 3 credit, không cộng credit riêng cho requirement. Không có môn tự chọn trong program thử; mở rộng sau cần cấu hình degree khác, không sửa bất ngờ save cũ. 72 là con số thiết kế nhỏ để thử resolver, không đại diện bằng đại học thật.

`passed(courseId)` chỉ true nếu ledger ghi pass; thiếu record = unknown cho tới khi có registration/attempt data, failed/missed thực tế = false. `credits` tổng các credit unique của course passed, không tính lần pass trùng. `eligible` khi đủ 72, tất cả mandatory passed, CAP/INTERN passed; `missing` nếu có false và toàn bộ dữ liệu cần thiết đã biết; `unresolved` nếu không đủ data để quyết định. Academic revisions phải được kiểm tra lại khi confirm như `25`.

## Danh mục 24 môn / 72 topic seed

Topic thể hiện tên nội dung, chưa phải question banks. Mỗi course 3 topic, `C_NMT` giữ hai topic đã author cộng `T_NMT_TRACE` là seed; không tự thay đổi fixture cũ. Prerequisite là course đã pass trừ khi ghi `—`; chưa pass thì không đăng ký, nhưng học bù/retake theo calendar được author.

| Năm dự kiến | Course ID · tên | Prerequisite | Topic IDs (seed) |
|---|---|---|---|
| 1 | C_NMT · Nhập môn tư duy lập trình | — | T_BASIC, T_CONTROL, T_NMT_TRACE |
| 1 | C_MATH1 · Toán cơ sở | — | T_MATH1_FUNC, T_MATH1_LIMIT, T_MATH1_RATE |
| 1 | C_PHYS1 · Vật lý nền tảng | — | T_PHYS1_MOTION, T_PHYS1_FORCE, T_PHYS1_ENERGY |
| 1 | C_COMM · Giao tiếp | — | T_COMM_LISTEN, T_COMM_WRITE, T_COMM_PRESENT |
| 1 | C_DIGITAL · Kỹ năng số | — | T_DIGITAL_FILES, T_DIGITAL_WEB, T_DIGITAL_SAFE |
| 1 | C_ENGLISH · Tiếng Anh chuyên ngành cơ sở | — | T_ENGLISH_READ, T_ENGLISH_TERM, T_ENGLISH_WRITE |
| 2 | C_OOP · Lập trình hướng đối tượng | C_NMT | T_OOP_CLASS, T_OOP_OBJECT, T_OOP_COMPOSE |
| 2 | C_DSA · Cấu trúc dữ liệu | C_NMT | T_DSA_ARRAY, T_DSA_LIST, T_DSA_TREE |
| 2 | C_DB · Cơ sở dữ liệu | C_NMT | T_DB_MODEL, T_DB_QUERY, T_DB_INDEX |
| 2 | C_MATH2 · Toán ứng dụng | C_MATH1 | T_MATH2_MATRIX, T_MATH2_PROB, T_MATH2_OPT |
| 2 | C_TEAM · Dự án nhóm | C_COMM | T_TEAM_SCOPE, T_TEAM_PLAN, T_TEAM_REVIEW |
| 2 | C_WEB · Web cơ bản | C_NMT | T_WEB_HTML, T_WEB_STYLE, T_WEB_CLIENT |
| 3 | C_API · Thiết kế API | C_OOP AND C_DB | T_API_ROUTE, T_API_DATA, T_API_ERRORS |
| 3 | C_SYSTEM · Hệ điều hành | C_NMT | T_SYSTEM_PROCESS, T_SYSTEM_MEMORY, T_SYSTEM_IO |
| 3 | C_NETWORK · Mạng máy tính | C_DIGITAL | T_NETWORK_ADDRESS, T_NETWORK_PROTOCOL, T_NETWORK_REQUEST |
| 3 | C_DATA · Phân tích dữ liệu | C_MATH2 AND C_DB | T_DATA_CLEAN, T_DATA_STATS, T_DATA_PRESENT |
| 3 | C_TEST · Kiểm thử phần mềm | C_OOP | T_TEST_CASE, T_TEST_UNIT, T_TEST_AUTOMATE |
| 3 | C_PROJECT · Dự án cá nhân | C_TEAM AND C_OOP | T_PROJECT_SCOPE, T_PROJECT_BUILD, T_PROJECT_DEMO |
| 4 | C_SECURITY · An toàn ứng dụng | C_NETWORK AND C_API | T_SECURITY_AUTH, T_SECURITY_INPUT, T_SECURITY_PRIVACY |
| 4 | C_CLOUD · Triển khai | C_SYSTEM AND C_NETWORK | T_CLOUD_DEPLOY, T_CLOUD_LOG, T_CLOUD_MONITOR |
| 4 | C_PRODUCT · Sản phẩm số | C_TEAM | T_PRODUCT_USER, T_PRODUCT_METRIC, T_PRODUCT_ITERATE |
| 4 | C_LAW · Đạo đức và pháp lý | C_COMM | T_LAW_PRIVACY, T_LAW_LICENSE, T_LAW_RESPONSIBILITY |
| 4 | C_INTERN · Thực tập | C_PROJECT | T_INTERN_ONBOARD, T_INTERN_TASK, T_INTERN_REVIEW |
| 4 | C_CAP · Đồ án cuối | C_PROJECT AND C_TEAM | T_CAP_PROPOSE, T_CAP_BUILD, T_CAP_DEFEND |

Năm dự kiến không cưỡng ép đăng ký nếu prerequisite chưa đạt. Nếu môn nợ làm chậm chuỗi, cần slot học lại và mốc có thể học các môn khác; chưa có calendar không được hứa có đường phục hồi. Khóa bắt buộc internship là **giả thuyết của program này**, không mâu thuẫn rule hệ thống: khóa khác có thể không yêu cầu internship.

## Liên kết skill / career đề xuất

C_NMT, C_OOP → `programming_basics`/`oop`; C_DB → `database_fundamentals`; C_API → `api_fundamentals`; C_WEB → `frontend_fundamentals`; C_DSA/C_TEST/C_NETWORK hỗ trợ nhiều hướng. Topic evidence và skill progress là hai records khác nhau, một activity chỉ cập nhật cả hai nếu effect author rõ; pass môn không tự xác định nghề. Skill thresholds và điều kiện chuyển đổi chưa có; không thể dùng graph này như production data.

## Điều kiện chưa đủ để chạy

Năm môn nền Y1 có đề và bài fixture trong `50_Cac_mon_nen_nam_1.md`; 6 môn Y2 có fixture trong `51_Cac_mon_nam_2.md`; 6 môn Y3 có fixture trong `52_Cac_mon_nam_3.md`; bốn môn Y4 có fixture trong `53_Cac_mon_nam_4.md`, còn thiếu dữ liệu đầy đủ, và lịch/registration/fee chưa đầy đủ; topic names chưa tương đương 72 nodes playable. C_NMT fixture có 2 topic đánh giá, topic thứ ba chỉ là placeholder. C_INTERN có fixture đường thay thế tại `37_Hoc_phan_thuc_tap.md` nhưng chưa có calendar/assessment hoàn chỉnh; C_CAP có rubric fixture trong `38_Do_an_va_bao_ve.md`, chưa có calendar/assessor thực và kiểm chứng. Chưa có đầy đủ skill graph/career paths, kế hoạch Y5–Y6 hoặc simulation để kiểm tra tính khả thi 72 credits. Score/duration ở C_NMT vẫn là giả lập.

## QA cần thiết

Kiểm tra prerequisite graph không vòng; không cộng credit trùng qua retake; pass đủ 24 môn mới đạt program thử; failed/unknown được phân biệt; thay cấu hình sau preview vô hiệu hóa preview; data chưa author không tự thành passed/failed; có đường phục hồi từng prerequisite và access internship/capstone. Các kiểm tra này chưa chạy.
