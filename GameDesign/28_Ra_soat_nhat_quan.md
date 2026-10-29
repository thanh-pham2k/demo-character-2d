# Rà soát nhất quán thiết kế — v0.1

Phạm vi: đối chiếu thủ công các tài liệu hiện có, không chạy game, test hay simulation. Danh sách này không chứng minh đã tìm mọi lỗi. Các thay đổi là quyết định đề xuất, phải được kiểm chứng khi có runtime.

## Điểm đã điều chỉnh

1. **Xe khởi hành đúng mốc:** `02` trước đây đặt đóng cửa trước interaction, còn `10` cho player tới đúng giờ được lên; đã định nghĩa một lượt quyết định boarding tại mốc, sau completion travel và trước close. Trạng thái pause, save/load, cancel, timeout của lượt này vẫn cần dữ liệu/UI cụ thể.
2. **Game với Long:** catalogue `08` mô tả game thông thường 30 phút/E −5, fixture D2 `10` dùng mỗi lượt 15 phút/E −3. Đã tách `A_GAME_LONG_QUICK` cho fixture; không ngầm đổi giá trị của phiên thông thường.
3. **Kết thúc Y6 chưa tốt nghiệp:** `23` ghi chưa có policy, trong khi `24` đã đề xuất một fixture kết thúc ngoài campaign. Đã làm rõ đó mới là fixture, policy sản phẩm chưa duyệt.

## Điểm cần xử lý tiếp

| ID | Vấn đề | Ảnh hưởng | Trạng thái |
|---|---|---|---|
| C01 | Nhiều Wxx không gắn ngày tuyệt đối | Không xếp lịch hay chứng minh cơ hội phục hồi | Có fixture NMT trong `34`, calendar toàn game thiếu |
| C02 | Chưa có lựa chọn `select_bus` và UI timeout tại mốc boarding | Không thể tái hiện cờ missed_bus hay boundary một cách xác định | Spec đề xuất trong `29`, chưa kiểm chứng |
| C03 | NMT retake được nói là có lịch nhưng đề/rubric chưa có | Không thể chứng minh đường pass và credit Y1/Y5/Y6 | Đã có fixture trong `30`, thiếu calendar/policy và kiểm chứng |
| C04 | Energy khi gián đoạn dùng round theo milestone, nhưng A_STUDY/game/ca chưa thống nhất | Dễ hao E khác nhau khi pause/resume | Spec đề xuất trong `29`, chưa kiểm chứng |
| C05 | Cơ hội ứng tuyển thay thế chưa có calendar/rubric | Có thể hết mọi JD mà không tới first job | Có fixture C2 trong `33`; còn thiếu policy sau C2 và data các hướng khác |
| C06 | Curriculum và program requirements chưa có | Không thể đánh giá graduation và đường 4/5/6 | Có program seed trong `31`, thiếu rubric/calendar và kiểm chứng |
| C07 | Mời ăn Long thu phí khi hoàn thành trong khi money spec nói reserve/commit rõ | Có thể tiêu số dư trong lúc activity chạy | Spec đề xuất trong `29`, chưa kiểm chứng |
| C08 | Bạn cùng phòng đến muộn và quyền chọn giường chưa có dialog agreement cụ thể | Có thể player bị ép giường còn lại | Spec đề xuất trong `29`, chưa kiểm chứng |
| C09 | W04 ôn giữa kỳ cùng recovery deadline cần slot & route | Chưa chứng minh có thể đi đúng giờ | Cần fixture theo ngày |
| C10 | 5 NPC, 15 địa điểm và activity catalogue mới là bản nháp | Không chứng minh content density, scene độc lập hoặc replay | Thiếu records, playtest |
| C11 | Sau clean repo, tài liệu mới nằm ngoài repo | Không được version control cùng dự án | Theo yêu cầu giữ repo sạch; không tự copy vào repo |

## Bước tiếp theo

Đặc tả C02–C08 với boundary, state và ví dụ; sau đó author program/course/calendar/companies có đủ alternative, rồi mô phỏng ít nhất các đường on-time/late/missed, year 4/5/6 và job search. Chỉ chấm score sau khi có evidence; hiện không có cơ sở cho điểm 9/10.
