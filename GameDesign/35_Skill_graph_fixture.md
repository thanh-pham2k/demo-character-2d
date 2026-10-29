# Skill graph và activity fixture — v0.1

Đề xuất giả lập, chưa phải data runtime hoặc mô hình năng lực thực tế. Tên skill canonical trong tài liệu này: `S_BASIC=programming_basics`, `S_OOP=oop`, `S_DB=database_fundamentals`, `S_API=api_fundamentals`, `S_WEB=frontend_fundamentals`; alias chỉ dùng khi migration/import, không lưu hai bản riêng. Skill khác dùng ID trong bảng. Quy tắc E/tick/reservation tại `29`.

## Rule cho mỗi node

State `locked|available`, progress nguyên [0,100], unlock khi predicate đủ tại thời điểm hiển thị; khóa/reject không xóa điểm cũ. `skill_session`: chọn node available, tự học 60 phút E −10, tiền 0; completion +10 progress và tạo ledger theo `runId`; dừng sớm không cộng điểm (elapsed/E vẫn tiêu). Không cần NPC. Session có thể lặp, nhưng lịch/energy hạn chế và player chọn thủ công; chưa có auto resolve. Mỗi completion chỉ cập nhật một skill được chọn; không tự cộng topic hoặc credit. Session là fixture, không khẳng định đã có animation/minigame.

Threshold entry cho một hướng project là ≥30 của những node nêu tại project data, không đồng nghĩa đã giỏi hay đủ điều kiện JD. Các node dưới đây có 0 khi chưa học; `NONE` mở từ đầu, chỉ năm và location được biết để thực hiện session. Prereq dạng `ALL` và giá trị là progress tối thiểu, không yêu cầu pass môn để tự học; nếu chọn học trong course thì course prerequisite vẫn áp dụng độc lập.

| Skill ID | Predicate session | Nguồn course/topic gợi ý; không tự cộng | Self-study | Downstream |
|---|---|---|---|---|
| S_BASIC | NONE | C_NMT | ROOM/LIBRARY, Y1–6 | S_OOP, S_WEB, S_DATA, S_QA |
| S_MATH | NONE | C_MATH1/C_MATH2 | LIBRARY, Y1–6 | S_ML |
| S_DB | S_BASIC ≥20 | C_DB | ROOM/LIBRARY, Y2–6 | S_API, P_DATA |
| S_OOP | S_BASIC ≥20 | C_OOP | ROOM/LIBRARY, Y2–6 | S_API/S_MOBILE/S_GAME |
| S_WEB | S_BASIC ≥20 | C_WEB | ROOM/LIBRARY, Y2–6 | S_UI, P_UI |
| S_UI | S_WEB ≥20 | C_PRODUCT | ROOM/LIBRARY, Y2–6 | P_UI, P_MOBILE |
| S_DATA | S_BASIC ≥20 | C_DATA | LIBRARY, Y2–6 | S_ML, P_DATA |
| S_QA | S_BASIC ≥20 | C_TEST | ROOM/LIBRARY, Y2–6 | S_TEST |
| S_TEST | S_QA ≥20 | C_TEST | ROOM/LIBRARY, Y2–6 | P_TEST |
| S_SYSTEM | S_BASIC ≥20 | C_SYSTEM | LIBRARY, Y3–6 | S_CLOUD |
| S_NETWORK | NONE | C_NETWORK | LIBRARY, Y3–6 | S_CLOUD, S_SECURITY |
| S_SECURITY | S_NETWORK ≥20 | C_SECURITY | LIBRARY, Y3–6 | optional P_DEPLOY |
| S_API | S_OOP ≥20 AND S_DB ≥20 | C_API | ROOM/LIBRARY, Y3–6 | P_LIBRARY_API |
| S_MOBILE | S_BASIC ≥20 AND S_OOP ≥20 | C_OOP | ROOM/LIBRARY, Y3–6 | P_MOBILE |
| S_GAME | S_BASIC ≥20 AND S_OOP ≥20 | C_PROJECT | ROOM/LIBRARY, Y3–6 | P_GAME |
| S_ML | S_MATH ≥20 AND S_DATA ≥20 | C_DATA | LIBRARY, Y3–6 | P_ML |
| S_CLOUD | S_SYSTEM ≥20 AND S_NETWORK ≥20 | C_CLOUD | LIBRARY, Y3–6 | P_DEPLOY |

Nếu chưa biết LIBRARY, H6 phải cung cấp cách tìm thấy nơi này theo calendar đã author; graph này chưa chứng minh khả năng tiếp cận. Tên P_UI, P_MOBILE, P_DATA, P_TEST, P_GAME, P_ML, P_DEPLOY có fixture sơ bộ trong `36_Project_career_fixture.md`; vẫn không tạo nút chạy hoặc evidence giả khi thiếu calendar/rubric thực. `P_LIBRARY_API` fixture nằm trong `18`, cần S_API ≥30 AND S_DB ≥30; alias `api_fundamentals` và `database_fundamentals` ánh xạ tới hai canonical ID trên.

## Trace tính tay — backend tự học

Giả sử năm 3, ROOM/LIBRARY đã mở, E đủ và không có deadline trùng, tất cả skill ban đầu 0: 2 session S_BASIC →20; 2 S_OOP →20; 3 S_DB →30; 3 S_API →30. Tổng 10×60 = 600 phút và E −100 trước nghỉ, nên player phải nghỉ/ngủ giữa các session theo `04`; không có cơ chế năng lượng âm. Đạt ngưỡng chỉ cho project; API/STORE thêm 2×90 phút theo `18` để full evidence. Trace bỏ qua thời gian đi lại, lịch deadline và availability, vì thế không chứng minh có thể hoàn thành trong campaign.

## Invariants / unresolved

- Prerequisite evaluation chặn session trước khi trừ cost, ledger delta idempotent theo runId; `unknown` không coi là đủ. Mỗi node có entry/source phù hợp năm/location.
- Nếu E không đủ hoặc session bị ngắt, trừ năng lượng theo cumulative rule; skill progress chỉ commit completion, không cộng hai lần khi reload.
- Evidence gắn task và revision; không tự tạo khi skill đạt 30. Chọn hướng khác không xóa skill/evidence trước.
- Chưa có activity records dùng schema thống nhất, course effects, đồ án của 7 hướng, lịch skill sau tốt nghiệp, mức tiêu hao/tốc độ học qua playtest. JD alias và project requirement phải được tích hợp thật trước khi fixture `33` có thể chạy. Các case trên chưa được thực thi.
