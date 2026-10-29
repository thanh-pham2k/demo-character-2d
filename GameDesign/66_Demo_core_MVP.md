# Đề xuất demo core MVP — Một buổi sáng ở KTX

**Mục tiêu:** kiểm tra người chơi có thấy thú vị khi đánh đổi thời gian cho tình bạn và việc học, rồi chứng kiến hậu quả cùng ngày. Đây là demo độc lập, không thay thế Game Design Bible sáu năm.

## Trải nghiệm đề xuất: 10–15 phút (giả thuyết cần playtest)

07:00 trong phòng 404, Lươn biết tiết học 08:00 và xe 07:40/07:50. Long rủ chơi một trận 15 phút. Người chơi có thể đi ngay hoặc chơi, sau một trận quyết định có chơi tiếp. Lịch hiển thị giờ hiện tại, giờ xe và thời gian đến trạm. Người chơi tự di chuyển ra trạm, chọn xe; nếu đã lỡ hai chuyến, chọn hỗ trợ đi đến trường và vào lớp muộn. Tối về, Long hỏi chuyện theo những gì cậu thực sự biết; một màn recap nhắc lựa chọn và kết quả, sau đó demo kết thúc.

## Phạm vi triển khai

- Ba scene có thể chơi: phòng 404, trạm xe, lớp học; đường đi giữa scene bằng transition ngắn có thời gian. Long là NPC duy nhất có tương tác, giảng viên có một lời thoại khi player vào lớp muộn.
- Hai hoạt động có chi phí thời gian: chơi cùng Long (15 phút mỗi lượt) và đi đến địa điểm. Xe 3 M, Long +2 trust và memory sau lượt đầu hoàn tất; tối đa 3 lượt. Không cần mini-game, thanh năng lượng hay hệ thống tiền đầy đủ cho demo; giá vé là điều kiện cố định, khởi tạo đủ tiền. Không trình bày +trust như hệ thống quan hệ hoàn thiện.
- Chuyến 07:40/07:50, 5 phút tới H6, đi tiếp 5 phút tới lớp; phòng→trạm 10 phút. Hỗ trợ từ trạm tới H6 30 phút khi không còn xe. Đến lớp trước hoặc đúng 08:00 rồi chủ động vào học là đúng giờ; sau 08:00 và trước 10:00 là muộn; không vào trước 10:00 là vắng. Lên xe đúng giờ rời có lượt quyết định trước khi đóng cửa.
- Ba state tối thiểu: số lượt đã hoàn tất, kết quả đến lớp, Long biết gì. Chơi tạo memory; Long chỉ biết player đến muộn nếu player kể. Recap có biến thể dựa trên state thật, không tạo memory khi từ chối hoặc ngắt lượt.
- Lưu local tại điểm chuyển scene và lựa chọn đã commit; load tiếp đúng giờ, location và state, không nhân reward. Có nút chơi lại. F8 checkpoint tách save chính: trước lời mời, trạm sau chuyến cuối, lớp trước 08:00 và buổi tối với hai biến thể memory.

## Nhánh phải thấy được

| Chọn | Kết quả có thể thấy |
|---|---|
| Đi ngay | Kịp xe/lớp nếu người chơi tiếp tục đi đúng lịch; tối Long không nhắc trận game. |
| Một trận rồi đi | Có memory với Long và vẫn có thể kịp xe; buổi tối có callback trận game. |
| Ba trận | Đến trạm sau hai xe theo tuyến fixture; hỗ trợ đưa tới lớp 08:30, có thể chọn học muộn. Long chỉ nhắc việc đi muộn nếu được kể. |
| Bỏ học hoặc không đến lớp | Attendance vắng tại 10:00; demo tiếp tục và recap trung thực, không game over. |

Điều kiện: các ví dụ giả định player thực hiện các bước ngay, không tự xảy ra khi đứng yên hoặc chưa xác nhận hoạt động.

## Giao diện và cách đo

HUD nhỏ: giờ, mục tiêu «Đến lớp trước 08:00», chuyến xe tiếp theo và chi phí/thời gian của lựa chọn đang xem. Hướng dẫn điều khiển đặt góc màn hình; hội thoại không che nhân vật. Recap hiển thị những gì đã xảy ra, không chấm điểm đạo đức.

Quan sát 5–8 người chơi thử như mục tiêu ban đầu, chưa phải kết luận thống kê: họ có hiểu thời gian đang bị đánh đổi, dự đoán được nguy cơ lỡ xe, nhận ra callback có liên quan tới lựa chọn, và muốn thử lại nhánh khác không? Ghi hành vi, nhầm lẫn và thời lượng; sửa trước khi mở rộng.

## Không đưa vào demo đầu

Các năm 2–6, skill tree, nghề nghiệp, thi, tiền/energy đầy đủ, các NPC khác, open world và nhiều mini-game. Nếu lát cắt không tạo được lựa chọn thú vị và callback dễ nhận ra, chưa có cơ sở mở rộng số scene hay hệ thống.

## Checkpoint cho QA thủ công

1. F8 «Trước lời mời» → chọn một trận, đi trạm, lên 07:40 → có memory, tới lớp 07:50, chọn học đúng giờ.
2. F8 «Trước lời mời» → chơi ba trận, đi trạm, chọn hỗ trợ, vào lớp → tới 08:30, ghi muộn; tối Long không biết lý do nếu chưa kể.
3. F8 «Không chơi, buổi tối» → nói chuyện Long → thoại trung tính, không nhắc trận game.
4. F8 «Đã chơi, buổi tối» → nói chuyện Long → callback trận game; kể đến muộn chỉ khi attendance tương ứng, rồi thấy phản ứng phù hợp.
5. F8 «Sau hai chuyến» → dùng hỗ trợ, lưu/tải → giờ, vị trí, memory giữ đúng; không nhân chi phí.

Đây là kế hoạch thiết kế, chưa có game, checkpoints, test hoặc playtest. Quy mô thực tế và số phút cần xác minh với người chơi. Tham chiếu [10](10_Nam_1_su_kien_mau.md), [29](29_Boundary_va_reservation.md), [34](34_Lich_nam_1_fixture.md).
