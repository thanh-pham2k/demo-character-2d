# Kiến trúc sáu năm — đề xuất v0.1

Phạm vi là khung kết nối và điều kiện chuyển tiếp; chưa phải content matrix, event database hay canon đã nghiệm thu. Các ID bên dưới là ID đề xuất cho thiết kế mới. Lịch từng năm, điều kiện cụ thể và nội dung chưa author không được coi là đã tồn tại.

## Mô hình thời gian và chuyển năm

Một năm gồm các tuần được author và các khoảng chuyển có preview. Tuần chỉ chứa các event đã khai báo; không tự tạo buổi học/vắng trong khoảng skip. Calendar cho biết học kỳ, registration, lớp, deadline, thi, cơ hội xã hội và resolver; entry là các khoảng `[start,end)` theo tài liệu thời gian. Chưa chốt số tuần chơi, độ dài campaign hoặc tần suất event. Mỗi transition phải hiển thị những việc sắp bị lỡ, cho hủy và commit atomically; state gốc được giữ nếu save lỗi.

`YearSnapshot`: year, `clock`, credits, course records, topic knowledge, energy, money, skill/evidence, relationships, memories, active/resolved conflicts, unlocked locations, event ledger, active project/application và RNG state. IDs ổn định, không làm lại relationship hoặc cấp lại credits khi bước sang năm tiếp. Event bắt buộc chưa resolve phải có nhánh miss hoặc chặn skip và nêu lý do; NPC không gặp thì mở biến thể thay thế thay vì tự sinh quan hệ.

## Năm 1 — Thích nghi

**Câu hỏi:** Tôi sẽ sống đời sinh viên như thế nào? Nhập học, đến KTX, làm quen Long, chuyến xe đầu, gặp bạn ở lớp, giữa kỳ, sinh hoạt chung và cuối kỳ. Hệ chính: lịch, travel, knowledge theo topic, quan hệ và lựa chọn thời gian. Location đề xuất: KTX/404, WC, hành lang/cầu thang, sảnh, căn tin, trạm xe, H6/lớp. Thời gian học và chơi cạnh tranh, nhưng lỡ xe hoặc thi kém mở recovery thay vì kết thúc game.

Đường nối: `Y1_ARRIVAL` → `Y1_FIRST_CLASS` → `Y1_LONG_GAME` → `Y1_MIDTERM` → `Y1_ROOM_CONFLICT` → `Y1_FINAL` → `Y1_HOTPOT`. Các mũi tên thể hiện khung thứ tự, không ngụ ý mọi event đã có bản ghi hoặc tất cả đều bắt buộc. Memory chỉ sinh nếu event thực sự xảy ra. Đến cuối năm, ghi điểm, credits, topic gaps, quan hệ và những việc còn dở; không yêu cầu mọi quan hệ phải đạt ngưỡng.

## Năm 2 — Khám phá

**Câu hỏi:** Tôi thích cuộc sống nào? Tuyển thành viên CLB, khả năng làm thêm, quan hệ mới, project nhóm và mâu thuẫn tiền/thời gian. Mở hoạt động/địa điểm theo event (CLB, nơi làm), không tự cho tất cả quyền truy cập. Lựa chọn CLB hoặc không tham gia tạo pool cơ hội khác, vẫn có đường khác để xây kỹ năng. Long có lịch riêng; mối quan hệ có thể tiến triển, xa dần hoặc chưa giải quyết bất đồng. Cuối năm giữ evidence của project và lịch sử chọn/không chọn.

## Năm 3 — Chọn hướng

**Câu hỏi:** Tôi muốn thử hướng nào? Môn nâng cao, dự án, mentoring, hackathon và chuẩn bị hồ sơ. Hướng nghề hình thành từ evidence và cơ hội, không gắn nhãn nghề độc quyền. Các cửa vào một vị trí có thể gồm project cá nhân, môn học hoặc CLB; bất kỳ yêu cầu ALL/ANY cụ thể phải có định nghĩa dữ liệu, hiển thị trên JD. Quan hệ bạn bè phản ánh lịch khác nhau; chưa tham gia hackathon không tự khóa mọi internship.

## Năm 4 — Trách nhiệm

**Câu hỏi:** Tôi đã sẵn sàng bước ra ngoài chưa? Lịch thực tập, khóa luận/capstone và học phần cạnh tranh thời gian; sự cố công việc cho player chọn báo cáo, hỏi giúp hoặc xử lý. Graduation resolver kiểm tra credits, môn bắt buộc, capstone và điều kiện author khác; nếu đủ, player xác nhận tốt nghiệp rồi tìm việc. Nếu thiếu, hiển thị từng thiếu hụt cùng recovery và chuyển vào năm 5; không ép đậu hoặc hủy toàn bộ tiến độ.

## Năm 5 — Hồi phục và so sánh

**Câu hỏi:** Nếu đi chậm hơn bạn bè thì sao? Chỉ mở nếu graduation chưa commit ở năm 4; lý do thiếu điều kiện được mang sang. Bạn cũ có lịch/địa điểm khác, sinh viên mới có thể hỏi đường, người chơi có cơ hội hỗ trợ hoặc tập trung học bù. Lịch recovery phù hợp từng thiếu hụt; project hoặc thực tập chưa đạt được cơ hội sửa nếu author cho phép. Cuối năm đủ điều kiện có thể tốt nghiệp; nếu không, resolver đưa từng việc còn lại sang năm 6.

## Năm 6 — Khép lại

**Câu hỏi:** Tôi khép lại hành trình thế nào? Chỉ mở nếu chưa tốt nghiệp năm 5. Ưu tiên lịch hoàn thành điều kiện cuối, cơ hội hàn gắn và thăm lại KTX, căn tin, H6 khi chúng còn khả dụng. Callback có điều kiện về memory thật và kiến thức của NPC; không bắt buộc mọi NPC xuất hiện. Nếu đủ điều kiện thì graduation; nếu không đủ phải có kết thúc chưa tốt nghiệp và phương án tương lai được author, không gán bằng hoặc công việc giả. Chính sách cơ hội sau năm 6 chưa được chốt; không thể gọi nhánh này hoàn thiện.

## Sau tốt nghiệp → tìm việc → ngày đầu

Tốt nghiệp chốt snapshot, sau đó job market / CV / screening / test / interview / reject hoặc offer / so sánh / chấp nhận. Rejection có cơ hội cải thiện và nộp nơi khác; không phát sinh offer chỉ để ép đến ending. Ending ngày đầu chỉ xảy ra khi có offer đã chấp nhận. Các trường hợp trì hoãn hoặc chưa nhận offer cần policy và nội dung cụ thể; hiện chưa xác định giới hạn thời gian hậu tốt nghiệp.

## Luồng cổng và chống khóa mềm

| Cổng | Điều kiện nguồn | Kết quả | Cần dữ liệu bổ sung |
|---|---|---|---|
| Vào năm 2–4 | Year resolver commit | Mang snapshot; mở calendar năm tiếp | Calendar và tồn đọng từng năm |
| Graduate năm 4/5/6 | Resolver đủ điều kiện và player xác nhận | Chốt credit, kết quả, memories; mở job search | Yêu cầu khóa, điều kiện cụ thể và UI |
| Vào năm 5/6 | Chưa graduate; resolver có hướng recovery | Mang hồ sơ thiếu hụt, mở sự kiện phù hợp | Lịch học lại/đồ án và chi phí |
| First job | Đã graduate và offer được accept | Chốt job và mở scene ngày đầu | Company, offer, scene |
| Chưa graduate sau năm 6 | Có thiếu hụt tại resolver | Nhánh kết thúc thay thế; không gán bằng | Recovery/policy và narrative chưa author |

Tất cả điều kiện là thiết kế đề xuất. Không thể chứng minh đường hợp lệ từ năm 1 tới first job cho đến khi có credit graph, số cơ hội retake, calendar và mô phỏng; nội dung năm 5–6 cũng chưa thể xác nhận đủ sâu.
