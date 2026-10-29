# Học tập và quan hệ — đề xuất v0.1

Các ngưỡng, thời lượng và công thức dưới đây là giả thuyết mô phỏng, không phải cam kết cân bằng hay luật của trường thực tế. Thiết kế cần dữ liệu course/calendar, UI và thử nghiệm trước khi đưa vào game.

## Academic: state và quyết định

`CourseDefinition`: id, credit, prerequisite IDs, academic windows, topics, assignment definitions, attendance rule, eligibility rule, assessment weights và recovery options. `CourseRecord`: status `unavailable | eligible | enrolled | passed | failed`, attendance theo entry, submissions theo assignment, assessment results, topic proficiency và creditAwarded. `Topic`: id, courseId, prerequisite IDs, evidence points 0–100; nhãn `UNKNOWN` 0, `EXPOSED` 1–19, `WEAK` 20–44, `COMFORTABLE` 45–74, `MASTERED` 75–100. Không giảm nhãn chỉ vì qua ngày trừ khi một rule suy giảm đã được công bố; giới hạn 0–100.

Lượt học: chọn topic đã mở, hình thức (tự học/nhóm/hỏi giảng viên), thời lượng và play/quick resolve; UI cho biết thời gian, điều kiện, chi phí và dải kết quả. Khi hoàn thành, ghi evidence vào topic mục tiêu; phần còn yếu được hiển thị để player tự chọn ưu tiên. Chỉ topic đã mở theo prerequisite mới có thể học; prerequisite cần định nghĩa bằng ID và mức, không dựa vào tên môn. Môn trượt không xóa evidence; đăng ký học lại có thể tái sử dụng kiến thức nhưng không tự cộng credit.

Attendance theo từng tiết `on_time | late | absent | excused`; đến khi tiết đang diễn ra thì late, sau end là absent. Bỏ lỡ bài hoặc deadline ghi lý do; recovery tùy định nghĩa môn, cho biết thời gian/chi phí và mốc kết thúc. Không có recovery thì phải nêu trước khi player commit action có rủi ro. Assessment ghi question IDs, câu trả lời, điểm, timestamp và submitted flag; question mapping dùng topic ID. Khi nộp, đáp án chưa trả lời tính sai; load giữa thi giữ câu và thời gian; commit kết quả đúng một lần. Nhánh fail cho phép retake theo calendar nếu chương trình còn cơ hội, không reset story.

**Ví dụ cân bằng giả lập**: một môn 3 credit có 4 topic và 2 assignments, bài cuối yêu cầu điều kiện attendance do course author quy định. Tự học 60 phút tăng 10 evidence trên topic đủ prerequisite; đây là giá trị kiểm thử, phải hiệu chỉnh theo số lượng slot thực. Final 3 câu, 20 giây/câu là phương án MVP có thể đổi; điểm, weighting và ngưỡng đậu phải được course định nghĩa đầy đủ và UI hiển thị trước kỳ thi. Credit chỉ được cộng một lần khi chuyển `enrolled → passed`.

UI course card: lịch, tiến độ điểm danh, deadline, topic/độ chắc chắn, eligibility và chi phí recovery. Không hiển thị điểm ẩn như xác suất chắc chắn; mô tả tác động có thể xảy ra. Cần chứng minh đường tốt nghiệp hợp lệ, đường trượt/cứu được và không thể trùng credit bằng mô phỏng nhiều lịch; chưa thực hiện.

## Quan hệ: state đa chiều

`Relationship[npc]` gồm `trust`, `respect`, `resentment` (0–100), `stage`, memory IDs, active conflict IDs, lastInteractionAt; quan hệ gần gũi và mâu thuẫn có thể cùng tồn tại. Ngưỡng gợi ý stage: stranger khi chưa gặp; acquaintance sau gặp; friend khi trust ≥40 và có ≥2 shared memories; close khi trust ≥70 và có ≥4 shared memories. Stage biểu thị mức gần gũi, không tự giải quyết mâu thuẫn và không đơn độc quyết định NPC đồng ý. Ngưỡng là thử nghiệm, không hiển thị như luật sản phẩm đã cân bằng.

NPC availability và mục tiêu độc lập: mỗi NPC có calendar, lịch ưu tiên và khoảng trống; invitation có window. `conflict` gồm origin, severity, deadline nếu có, resolution options và trạng thái open/resolved/expired. Khi trust cao nhưng resentment cao, NPC vẫn có thể quan tâm nhưng chọn đối thoại dè dặt; một câu trả lời hòa giải không tự xóa xung đột nếu event chưa quy định.

`Memory`: id, event instance, actors, outcome, timestamp, visibility và tags. Dùng predicate cụ thể để chọn callback; callback chỉ chọn một lần trừ khi được định nghĩa là recurring. Không có memory nếu event không xảy ra. Với Long, chơi game cùng có thể tạo `long_played_before_class`; nếu player đến muộn và Long biết sự kiện, buổi tối dùng biến thể hỏi thăm; nếu không chơi, dùng lời thoại trung tính. Đây là dự kiến author, không suy diễn rằng Long biết mọi trạng thái ẩn.

## Kết nối và kiểm tra thiết kế

Chơi với Long lấy thời gian có thể làm lỡ xe nhưng không tự khiến học trượt. Học nhóm có thể tăng knowledge và tạo memory khi cả hai đồng ý; tốn thời gian và phụ thuộc lịch. Nợ môn mở recovery nhưng không khóa các hoạt động quan hệ chung nếu không có điều kiện cụ thể. Test cases cần bao gồm trust cao + conflict mở, NPC không có mặt, đúng/sai boundary topic, lưu giữa thi, cấp credit một lần, callback chỉ khi NPC biết. Chưa có simulation hoặc playtest, do đó các ngưỡng chưa được nghiệm thu.
