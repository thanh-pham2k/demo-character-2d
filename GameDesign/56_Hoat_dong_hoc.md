# Hoạt động học — thẻ ôn và quyết định v0.1

Đề xuất toy gameplay riêng cho life-sim, không sao chép giao diện hay cơ chế cụ thể của game tham khảo. Đây là đặc tả chưa triển khai, chưa chứng minh thú vị/cân bằng. Giữ A_STUDY trong `08`: 60 phút campaign, E−10, hoàn tất +10 topic evidence. Topic/assessment riêng trong `03`, timer policy `40`, transaction `43`.

## Lựa chọn trước phiên

Player chọn topic đã mở, địa điểm ROOM/LIBRARY, một trong `practice` hoặc `quick`. UI hiển thị hiện trạng topic, cost 60 phút/E−10/0M, giờ kết thúc và deadline đã biết có thể lỡ. Cả hai mode tạo cùng +10 evidence khi **activity hoàn tất**; practice cho feedback cụ thể, không cho thêm điểm vô hạn. Nếu chưa có content practice cho topic, disabled với lý do; quick cũng chỉ được mở khi topic definition đã tồn tại, không giả lập kiến thức.

Player có thể hủy trước start 0 cost. Sau start, activity dùng 60 phút campaign theo scheduler, E tiêu cumulative `29`; gặp event quan trọng thì pause và cho tiếp tục/dừng/replan. Dừng trước completion giữ elapsed/E đã tiêu, không cộng evidence. Rời practice UI không tự hoàn thành hoặc nộp; cung cấp lựa chọn tiếp tục practice, chuyển quick (không hoàn thời gian) hoặc dừng, xác nhận trước khi commit.

## Practice: sắp thứ tự các bước

Một bài gồm 3 thẻ và một mục tiêu, player xếp thứ tự rồi `Kiểm tra`. Đây là giao diện kéo thả **và** nút bàn phím/chọn lên xuống tương đương; không dùng timer thực, không phạt thử lại. Không làm thời gian campaign trôi riêng cho từng thao tác UI; 60 phút là cost của activity, không cộng thêm thời gian UI ngoài activity. Chốt kết quả sau một lần chọn `Hoàn thành`, lưu sequence và feedback; không cho farm evidence bằng cách mở lại cùng run.

Fixture `PRACTICE_NMT_BASIC_01`, topic T_BASIC: mục tiêu “Tính giá trị cuối cùng của x sau `x=2; x=x+1`”. Thẻ A “Bắt đầu với x=2”, B “Lấy x hiện tại cộng 1”, C “Ghi x=3”; thứ tự A→B→C. Thẻ hiển thị text, không chỉ màu/hình. Đúng: “Bạn đã theo dõi giá trị trước khi gán.” Sai: “Hãy xác định x ban đầu trước khi tính phép cộng”; `Xem gợi ý` hiển thị “Phép gán thứ hai dùng giá trị x hiện tại”. Player có thể sửa, hoặc hoàn thành với sequence bất kỳ; hoàn thành chưa đồng nghĩa trả lời đúng bài thi.

Mỗi `PracticeDefinition`: ID, topic, promptKey, card IDs/text, solution permutation, hintKey, feedback keys, version, accessibility order và localization. Content không đủ 3 thẻ/solution hợp lệ → `blocked_data`, không tự lấy đề thi hoặc hiện đáp án giả. Các topic khác cần practice definitions riêng; không khẳng định 72 topic đã có toy.

## Quick và outcome

`quick`: không chạy thao tác thẻ; hoạt động vẫn tiêu 60 phút/E−10 và +10 evidence khi hoàn tất; UI ghi rõ “Không có feedback bài tập”. Không dùng RNG hay đánh dấu player trả lời đúng. Practice sai/hủy trước completion không tạo memory kết quả đúng; activity completion vẫn +10 evidence để tránh khóa tiến trình, feedback lưu `practiceReviewed=true`, `lastSequenceCorrect` đúng với thực tế. Evidence không tự chuyển skill hoặc điểm thi; hint assessment vẫn theo `14`.

Dùng `sessionId` để commit evidence, cost và result một lần; nếu save lỗi, giữ snapshot trước completion và cho retry. Activity window/notification xảy ra trong 60 phút theo `02`; practice UI phải có state persist để resume khi event chen ngang. Mode quick/practice không tạo hai activityRun hoặc trừ cost hai lần khi chuyển.

## Quyết định và rủi ro

Player có quyết định **topic nào, khi nào, practice hay quick** trong lịch và có phản hồi về cách suy nghĩ; thử sai không phạt. Rủi ro: bộ thẻ quá dễ, rời rạc với thi, 60 phút cố định khuyến khích tối ưu lặp, nhiều topic tạo gánh content. Cần thử với player, đo thời gian thật, số lần dùng quick, số lỗi, mức hiểu feedback, ảnh hưởng tới lịch và tỷ lệ bỏ cuộc; không khẳng định vui trước playtest.

## QA cần có

Prerequisite, chọn practice missing content, keyboard/screen reader, solution không hợp lệ, check nhiều lần, complete sai, quick, pause event/restore, chuyển mode, timeout deadline, E=0, commit/retry và evidence clamp 100. Chưa có runtime, test, simulation hay playtest; minigame này không bao phủ project/schedule toys còn lại.
