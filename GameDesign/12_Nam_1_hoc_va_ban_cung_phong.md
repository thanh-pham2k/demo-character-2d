# Năm 1 — bài tập, ôn thi và bạn cùng phòng v0.1

Bản authoring thử, số liệu chỉ dành cho fixture này. Không áp dụng như luật trường thực hoặc khẳng định đã chạy. Mọi event được lưu theo instanceId và áp dụng tác động một lần; thời gian/energy đã dùng không hoàn khi gián đoạn.

## Dữ liệu fixture

`C_NMT`: 3 credit; topic `T_BASIC` không prerequisite, `T_CONTROL` cần `T_BASIC >= 20`; cả hai bắt đầu 20 evidence. `ASG_01`: hai milestone 45 phút, E −7 mỗi milestone, submission mất 5 phút, E 0; mở W03 08:00, deadline W03 22:00. Nộp đủ milestone đúng hạn là complete; nộp bản chưa đủ là partial và không thể sửa bản đã nộp; nộp sau deadline trong cửa sổ W04 08:00–22:00 được late, không cộng phạt điểm trong fixture vì chưa có rubric. Quá W04 22:00 thì missed, vẫn giữ evidence; có lịch học lại do course author định nghĩa. Nhãn deadline, cách nộp và recovery hiển thị ngay khi assignment được biết.

Để không mâu thuẫn quy tắc activity liên tục, E −7 là mỗi milestone hoàn tất; nếu dừng giữa milestone thì E tiêu theo số phút đã chạy, mỗi milestone làm tròn xuống tới đơn vị E, phần chưa tiêu không trừ. Hoàn tất milestone thứ hai không tự nộp: player phải chọn submit.

## EV_Y1_ASSIGNMENT_RECEIVED — Bài đầu tiên

- Category scheduled academic; W03 08:00, entry `ASG_01`, course enrolled. Delivery là thông báo lịch, không yêu cầu player có mặt ở lớp; player đã biết deadline khi mở calendar. Priority 90; mỗi course attempt một instance.
- Player có thể `plan` (0 phút, mở activity list), `later` (0 phút) hoặc xem hướng dẫn (0 phút, UI pause). Không tự trừ E hay bắt đầu làm. Known flags: `assignment_known`; không sinh memory nếu chỉ đọc thông báo.
- Nếu course chưa enrolled thì không phát entry. Đã nhận nhưng không mở thông báo vẫn thấy entry trên calendar; deadline xử lý như bình thường. Cơ hội: `A_ASSIGNMENT` tại ROOM/LIBRARY; `A_SUBMIT` tại ROOM/CLASS.
- Rủi ro: chọn việc khác có thể làm quá deadline; preview hiển thị. Academic: mở assignment record; career/relationship: không đổi. Follow-up `EV_Y1_ASSIGNMENT_DEADLINE`, `EV_Y1_ASSIGNMENT_RECOVERY`.

## EV_Y1_ASSIGNMENT_DEADLINE — Hạn nộp

- Scheduler W03 22:00; trước khi đánh dấu hạn, xử lý submission hoàn tất chính xác 22:00 theo thứ tự đã quy định. Nếu đã nộp, `on_time`; nếu chưa nộp, `overdue`, mở cửa sổ late W04 08:00–22:00. Không tự sinh bài hoàn chỉnh.
- Player không chọn tại thời điểm deadline; có thể tắt thông báo và tiếp tục. Lỡ hạn không tự giảm trust của Hạnh. Deadline và recovery được ghi trên course card; Hạnh không tự biết lý do cá nhân.
- Nếu hoàn thành hai milestone nhưng chưa nộp, vẫn là overdue; nếu nộp đúng 22:00, không bị đánh dấu muộn. Nộp lại sau khi đã nộp bị từ chối để không ghi hai kết quả.

## EV_Y1_ASSIGNMENT_RECOVERY — Nộp muộn hoặc bỏ lỡ

- Category conditional; window `[W04 08:00,W04 22:00]`, `ASG_01=overdue`; ưu tiên 70. Không yêu cầu gặp Hạnh.
- `complete_then_submit`: tiếp tục milestone còn lại rồi submit 5 phút; cả bước submit phải kết thúc không muộn hơn 22:00. `submit_partial`: submit 5 phút nếu player chấp nhận; `defer`: 0 phút, vẫn overdue.
- Tại W04 22:00, xử lý submission kết thúc đúng mốc trước khi đóng, sau đó phần chưa nộp thành missed. Nếu không còn đủ thời gian hoàn tất + submit, UI không hứa nộp kịp; partial vẫn được nếu submit kết thúc kịp.
- Bài missed: hiển thị học lại/đăng ký lại theo course calendar khi có dữ liệu, không hứa ngay rằng slot tồn tại. Không xóa kiến thức hoặc khiến cả game over. Career/Long: không đổi tự động. Callback H04 chỉ khi Hạnh được phân công và có lý do biết record, không tự tiết lộ lý do cá nhân.

## EV_Y1_MIDTERM_PREP — Chọn cách ôn

- Category major academic; fixture W04 18:00–22:00 tại ROOM/LIBRARY, enrolled C_NMT. Panel hiển thị `T_BASIC`, `T_CONTROL`, nhãn hiện tại, prerequisite và thời gian. Conflict: assignment có thể còn overdue trong cùng cửa sổ.
- A: chọn `A_STUDY` 60 phút, E −10, topic đã mở +10 evidence khi hoàn tất; B: làm/nộp ASG_01 theo record; C: nghỉ 30 phút +15 E. UI preview thời gian kết thúc và rủi ro deadline. Panel không tự trôi thời gian; có thể quay lại khi cửa sổ còn mở.
- Knowledge sau ôn là input cho feedback và assessment được author, không tự xác định điểm thi; memory hoặc quan hệ chỉ phát sinh nếu có group event riêng. Hết window: panel đóng, topic giữ nguyên; thi vẫn có nhánh tham gia hoặc bỏ lỡ. Recovery: `EV_Y1_POST_MIDTERM_REVIEW` nếu calendar còn cửa sổ.

## EV_Y1_MIDTERM — Bài giữa kỳ

- Category scheduled assessment; W05 08:00–09:00 LOC_CLASS, enrolled C_NMT. Nội dung 3 câu, tối đa 20 giây thực/câu trong UI; world clock pause trong lần làm bài, timer riêng vẫn chạy và được lưu. Các câu, đáp án, mapping topic cần được author trước runtime; không tính điểm nếu chưa có question data.
- `start`: bắt đầu attempt, save câu/timer/answers; `submit`: chưa trả lời = sai; `exit`: yêu cầu xác nhận, giữ attempt hoặc nộp tùy lựa chọn, không tự làm lại. Kết quả sau submit được lưu một lần và tạo feedback theo topic, không cấp credit hay tự cho PASS/FAIL course.
- `miss`: khi hết cửa sổ mà chưa bắt đầu, trạng thái missed; không tạo điểm giả. Đối với attempt đã bắt đầu, timer/submit tiếp tục theo policy assessment trong UI, không dùng world time để âm thầm hủy. Kết quả/thông tin chỉ chia sẻ với Long khi player chọn kể hoặc Long có mặt tại cảnh được author.

## EV_Y1_POST_MIDTERM_REVIEW — Phục hồi

- Conditional: W06, khi player có feedback topic hoặc missed midterm. Panel biết tên topic đã được lộ, cho chọn tự học `A_STUDY`, hỏi giảng viên trong office hour nếu available, hoặc bỏ qua. Với missed không tạo feedback strong/weak giả; cho xem lại course outline và lịch thay thế nếu có.
- `study` +10 evidence cho topic 60 phút, E −10; `ask` 20 phút, E −3, +5 evidence khi giảng viên chấp nhận; `skip` 0 phút. Nếu Hạnh bận, không hiển thị đang có mặt; vẫn có tự học. Không tự thay điểm giữa kỳ đã submit.

## EV_Y1_ROOM_LIGHT — Đèn khi Long muốn ngủ

- Category major relationship; W07, 22:00–23:00 LOC_ROOM, required met Long, Long có mặt, đèn phòng đang bật. Player biết Long muốn ngủ khi event mở; không tự gán nhân vật đang tức.
- A `desk_lamp`: 0 phút, tắt đèn phòng và bật đèn bàn nếu có nguồn; tạo `agreement_light` khi Long đồng ý theo dialog, conflict chưa mở. B `ask_30`: 5 phút trò chuyện; Long đồng ý nếu lịch ngày sau cho phép, tạo window đến 22:35; nếu từ chối, player được chọn lại A/C. C `keep_light`: 0 phút, đèn vẫn sáng, mở `conflict_light` với severity 1 và resentment +3; không tự hạ trust hay phá quan hệ. D `stop_work`: 0 phút, tắt đèn phòng; không tạo conflict.
- Khi window `ask_30` kết thúc mà đèn vẫn sáng, mở `conflict_light` và +3 resentment một lần; nếu tắt kịp, ghi agreement fulfilled. Nếu rời phòng/hết cửa sổ, đèn và conflict do state thực tế quyết định, không tự giả sử đã tắt. No academic/career change trực tiếp; tiếp tục học tốn thời gian/E qua activity riêng.
- Memory chỉ khi dialog được resolve: `light_discussion`, actor Lươn/Long, outcome choice. Nếu Long không có mặt event không trigger. Callback `EV_Y1_LIGHT_FOLLOWUP` không tự resolve.

## EV_Y1_LIGHT_FOLLOWUP — Sáng hôm sau

- Conditional: sáng W08 khi `light_discussion` đã commit và hai người có mặt/available. Nếu có conflict mở: Long phản ánh sự việc, player chọn `apologize_and_agree` 5 phút (Long đồng ý theo predicate author; resolve conflict, resentment −3 clamp 0), `discuss_later` hoặc `dismiss` (0 phút, conflict giữ nguyên). Nếu không có conflict: thoại trung tính, không cộng điểm chỉ vì hiển thị.
- Miss nếu NPC bận hoặc player không gặp: conflict tiếp tục, có thể bàn khi có cửa sổ khác được author; không âm thầm hòa giải. Không cho Long nói về deadline hay điểm mà player chưa chia sẻ.

## Giới hạn và kiểm tra cần làm

Cần dữ liệu calendar thực tế, đề thi/đáp án, attendance/điểm course, lịch Long và event hồi phục sau W04. Các nhánh cửa sổ trùng W04 22:00, interruption, save/load, partial submit, conflict một lần và NPC vắng phải được mô phỏng. Chưa chạy test hoặc playtest; chưa có đủ event Năm 1.
