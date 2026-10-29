# Kiểm toán thời gian và tài nguyên — fixture v0.1

Kiểm toán thủ công các số đã đề xuất, không phải simulation, playtest hay bảo đảm cân bằng. M là đơn vị tiền hư cấu; không suy ra chi phí sống thật. Tài liệu dùng quy tắc `04` và `29`; bất kỳ dữ liệu thiếu nào đều ghi rõ.

## S1: tự học backend từ 0

`35` yêu cầu 10 session × 60 phút, E −10 mỗi session để đạt BASIC 20, OOP 20, DB 30, API 30. Project `18` cần start 15 phút E −2 + 2 task 90 phút E −15/task: tổng **795 phút và 132 E**, chưa tính travel, bữa, deadline hay việc khác.

Ví dụ không tối ưu, giả sử player có bốn buổi `[18:00,22:00)` mỗi ngày, E ban đầu 75, sleep mỗi đêm 480 phút +60 E đúng theo fixture `08`, không có event ngắt:

| Buổi | Hoạt động | Phút trong 18–22 | E cuối trước ngủ |
|---|---|---:|---:|
| 1 | 4 skill session | 240 | 35 |
| 2 | 4 skill session | 240 | 55 (35+60 clamp 95−40) |
| 3 | 2 skill + start project + 1 task | 225 | 63 (55+60 clamp 100−20−2−15) |
| 4 | 1 task | 90 | 85 (63+60 clamp 100−15) |

Schedule chỉ hợp lệ nếu session tương ứng đã mở năm/địa điểm; thứ tự skill BASIC→OOP→DB→API và project requirements được giữ. Tính tay này không bao gồm bữa, việc học, chi phí sinh hoạt, hoặc xác định đêm ngủ có trùng thời gian lớp/ca không. Nó chứng minh một trace giả định không âm E, không chứng minh trace có thể xảy ra trong calendar campaign.

## S2: thực hành tại trường

`37` R_CAMPUS gồm 3 sessions ×240 phút, E −30/session, 0M, trước deadline review W08. Giả sử ba ngày riêng đều có khung 4 giờ, E đầu ngày ≥30, instructor/session available, và không có travel. 3 sessions là **720 phút, E −90 tổng**, nhưng phải nghỉ/ngủ giữa ca nếu E không đủ. Nộp artifact/reflection và chờ instructor review có duration chưa định nghĩa, nên đây là mức tối thiểu chứ chưa chứng minh pass.

Ba session trong cùng một ngày giả định E=75: sau hai session E=15, session thứ ba không được bắt đầu nếu activity không gián đoạn; cần nghỉ 30 phút +15 E để đủ 30, đưa tổng lên ít nhất 750 phút. Fixture chưa nêu mỗi session interruptible hay không; không được khẳng định quy tắc nào đã được thiết lập. Cần author. Nếu instructor vắng và không có response, result unresolved, không coi là fail.

## S3: tiền và cơ hội

Từ fixture 30M, một bữa 8M và vé bus 3M còn 19M nếu cả hai hoàn tất, không có chi phí khác. Một ca làm +40M chỉ khi contract/shift hoàn tất; cancel/no-show không được tự cộng tiền. Nếu thiếu 8M, hỗ trợ bữa trong `13` chỉ khả dụng khi event/data đã author; không mặc định cho mọi ngày. Chưa có tiền nhà, hỗ trợ gia đình, giá sinh hoạt hằng ngày, chu kỳ thanh toán hoặc ngân sách khởi đầu mỗi năm, nên không kết luận được khả năng sống hoặc đóng học phí qua campaign.

## Các rủi ro và quyết định cần author

1. **Môn pass nhưng skill không tự tăng:** 24 môn bắt buộc và tự học skill tranh cùng quỹ thời gian; cần content effect và dữ liệu lịch để tránh 10 session lặp gây grind. Đề xuất course activity có thể cấp skill theo effect tường minh, không tự suy từ status pass.
2. **Sleep cố định 480 phút:** làm mất ít nhất 8 giờ campaign; calendar cần ghi lịch ngày, time-skip và effect khi gián đoạn. Không giả định ngủ kết thúc đúng giờ nếu activity bị ngắt.
3. **Năm 4 có nhiều milestone:** C_INTERN tối thiểu 720 phút, C_CAP ít nhất 285 phút (start 15 + 3×90) trước submission/defense, cùng với môn/việc/đi lại. Không có calendar của các course khác, nên chưa chứng minh tốt nghiệp năm 4.
4. **Tài nguyên E=0:** rule cho nghỉ và travel để không kẹt, nhưng thời gian và E cho route/dùng trợ giúp chưa author đầy đủ; cần xác định route khẩn cấp.
5. **Bộ ba tiền/time/quan hệ:** không có model thu nhập/sinh hoạt đầy đủ nên chưa thể đánh giá lựa chọn làm thêm vs học có công bằng.

## Hướng QA

Mô phỏng theo tick và event boundaries với full calendar; kiểm tra min/max E, wallet, reservations, thời gian transit, missed deadlines, skill prerequisites, credit ledger và hàng loạt lựa chọn khác nhau. Chỉ thực hiện sau khi có dữ liệu course và event đầy đủ; các bảng trên là tính tay, chưa được kiểm chứng bằng chương trình.
