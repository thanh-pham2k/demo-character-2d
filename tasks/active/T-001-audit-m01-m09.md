# T-001 — Audit M01–M09 theo design mới

- **Trạng thái:** Kế tiếp
- **Runtime:** `mobile-2d/Assets/Scenes/Production_Opening.unity`
- **Mục đích:** Xác định prototype M01–M09 có thực sự thực hiện lời hứa của bản design mới không, trước khi tiếp tục mở rộng nội dung.

## Checklist audit

Với từng mission M01–M09, ghi bằng chứng từ scene/review và trả lời:

1. Người chơi thực sự làm/chọn gì? Động từ gameplay có khác mission liền trước không?
2. Choice quan trọng có trade-off nhìn thấy trước khi chọn không?
3. Hậu quả có truy được về choice và xuất hiện trong state, gameplay hoặc phản ứng NPC không?
4. Có recovery hợp lý khi thất bại/đến trễ không; chi phí có rõ và có tránh vòng phạt không?
5. Transition có nguồn, đích, thời gian trôi, hậu quả bị lỡ, nơi trả quyền điều khiển và checkpoint không?
6. Phòng 404/NPC/lịch học cho thấy mốc thời gian đã trôi như thế nào?
7. Có đoạn nào chỉ là tương tác lặp/timer/giữ nút mà nên rút gọn hoặc cần kỹ năng/quyết định rõ hơn?

## Deliverable

Tạo bảng M01–M09 với các cột: mission, gameplay verb, choice/trade-off, state/story consequence, recovery, transition, bằng chứng runtime, verdict (`đạt` / `sửa` / `chưa kiểm chứng`). Mỗi mục `sửa` cần ghi biểu hiện cụ thể và thay đổi tối thiểu đề xuất. Không đổi canon trong task này.

Kết thúc bằng danh sách tối đa 3 vấn đề có ảnh hưởng lớn nhất để đưa vào T-002/slice; không mở thêm content hoặc asset chỉ từ cảm giác.

## Điều kiện hoàn tất

- Đủ cả M01–M09, không suy rộng từ số lượng PlayMode checks.
- Phân biệt rõ model pass, runtime pass và bằng chứng người chơi hiểu/thấy hậu quả.
- Đối chiếu `GAME_DESIGN_PILLARS`, `GAMEPLAY_SYSTEMS_PLAN`, `GAMEPLAY_TRANSITIONS_PLAN` và production baseline.
- Ghi nguồn/scene/ngày cho mọi nhận định về trạng thái.
- Đầu ra lưu tại `tasks/active/T-001-audit-results.md`; sau đó cập nhật ưu tiên T-002.

## Nguồn hiện trạng

- [Production campaign implementation](../../story/PRODUCTION_CAMPAIGN_IMPLEMENTATION.md): M01–M09 đã nối scene và có report 819 checks; M10–M45 chưa hoàn chỉnh.
- [Production year-one review](../../story/PRODUCTION_YEAR_ONE_REVIEW.md): bằng chứng chi tiết M01–M08, không thay report M09 mới hơn.
- [Production registration review](../../story/PRODUCTION_REGISTRATION_REVIEW.md): M09 và giới hạn art/motion/audio/build.
- `Room404_MVP`/F8 không thuộc runtime audit này.

## Nhật ký

- **2026-10-26:** Đặt làm task kế tiếp theo production gates. Cần audit trải nghiệm, không chạy lại 819 checks chỉ để lặp bằng chứng kỹ thuật đã có trong report.
