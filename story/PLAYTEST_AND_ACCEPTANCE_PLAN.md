# Playtest and acceptance plan — Phòng 404

Kế hoạch này kiểm tra xem vòng chơi và transition có dễ hiểu, thú vị và đủ ổn định để mở rộng campaign không. Nó không thay thế kiểm thử kỹ thuật trong [Production campaign implementation](PRODUCTION_CAMPAIGN_IMPLEMENTATION.md).

## 1. Vertical slice

Trước khi mở rộng mạnh M10–M45, làm một slice prototype cô lập dài 8–12 phút:

1. Bắt đầu 07:20; tiết học lúc 08:00; Long rủ chơi.
2. Đi học giữ điểm danh nhưng bỏ lỡ trận chơi. Chơi tới 08:10 tạo lần vắng nhưng có thời gian với Long.
3. Tối chọn học bù kiến thức hoặc giữ buổi ăn/chơi với Long. Học bù không xóa lần vắng nhưng giảm rủi ro kiến thức và chiếm buổi hẹn.
4. Cả hai đường tới sáng hôm sau; Long và state tóm tắt phản ánh lựa chọn.

Slice là fixture test, không phải mission canon: không thay mission ID, course graph, tín chỉ, ending hay save campaign. Dùng checkpoint/save directory riêng; restore phải bỏ coroutine, reservation, modal và state cũ trước khi bắt đầu lại.

## 2. Playtest sớm

Chạy tối thiểu với năm người chưa biết đoạn truyện. Quan sát trước, hỏi sau; không dẫn họ tới cách hiểu mong muốn.

Vòng đầu đạt khi:

- 4/5 tự giải thích đúng vì sao có hậu quả.
- 4/5 nhận ra trade-off trước khi chọn.
- 4/5 mô tả được chi phí của phương án phục hồi.
- 4/5 nhận ra phản ứng của Long khác do lựa chọn.
- 3/5 muốn chơi tiếp; đây là tín hiệu tò mò ban đầu, không phải bằng chứng market fit.
- Thời lượng trung vị 8–12 phút, không có softlock hoặc state sai sau retry/load.

Nếu fail, sửa comprehension, feedback hoặc nhịp trước khi nhân rộng system/content. Năm người đủ tìm lỗi hiểu/usability; chưa đủ để kết luận nhu cầu thị trường.

## 3. Acceptance cho choice và recovery

Choice quan trọng cần:

1. Báo trước trade-off có thể biết.
2. Cho thấy hậu quả truy được về lựa chọn.
3. Thay đổi gameplay, state hoặc phản ứng story quan sát được.
4. Có recovery nếu hậu quả có thể khóa tiến triển hoặc gây tổn thất dài hạn.

Flavor choice được phép không tạo nhánh gameplay, nhưng không được trình bày như lựa chọn làm thay đổi campaign. Recovery phải nói rõ chi phí trước khi commit và không đẩy người chơi vào vòng phạt vô hạn.

## 4. Acceptance cho transition

- Màn preview nêu nguồn/đích, thời gian trôi và các hậu quả đáng kể trước khi commit.
- Đích là timestamp của sự kiện kế tiếp cần người chơi chú ý/tương tác; ngày và thời lượng hiển thị được tính từ nguồn/đích đã author, không lấy một khoảng nhảy mặc định.
- Chuyển qua một đêm dùng ngủ; nhiều ngày dùng montage/thẻ thời gian, không biểu diễn như ngủ dài.
- Không có giới hạn 30 ngày mỗi transition: 30 ngày chỉ là ví dụ. Khoảng nhảy phải tới đúng event; không bỏ qua choice hoặc gameplay event nằm giữa.
- Mốc đích và state sau chuyển khớp nhau trên UI, save và scene.
- Transition dài không bỏ mất sự kiện quan trọng mà không báo; người chơi hiểu vì sao mình tới địa điểm/thời điểm mới.
- Tải lại trước commit cho phép hủy an toàn; tải lại sau commit không lặp event/effect.
- Tổng playtime campaign đo trên tuyến hoàn chỉnh, mục tiêu trung vị 60–90 phút; không suy từ `45 mission × thời lượng trung bình`.

## 5. Production gates

1. Audit M01–M09 theo [Game design pillars](GAME_DESIGN_PILLARS.md) và [Gameplay transitions plan](GAMEPLAY_TRANSITIONS_PLAN.md); chỉ sửa điểm yếu có bằng chứng.
2. Xây slice cô lập và đạt tiêu chí mục 2–4.
3. Nối M10–M45 theo baseline; mỗi mission transition có checkpoint/retry phù hợp và kiểm tra continuity.
4. Chạy tuyến campaign hoàn chỉnh, đo thời lượng, audit academic ledger/room occupancy/time skips, rồi chốt art/audio/device polish.

## 6. Báo cáo playtest

Mỗi vòng ghi: build/definition hash, người chơi, thời gian, lựa chọn, lỗi, điểm họ hiểu sai, nơi họ muốn nghỉ, và việc sửa tiếp theo. Không dùng pass của model/unit test làm bằng chứng cho sức hút hoặc khả năng hiểu của người chơi.
