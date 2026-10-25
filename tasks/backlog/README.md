# Backlog — Phòng 404

## T-002 — Vertical slice và playtest

Sau khi T-001 xác định các gap chính, dựng slice cô lập 8–12 phút theo `story/PLAYTEST_AND_ACCEPTANCE_PLAN.md`: lựa chọn đi học hoặc chơi với Long, hậu quả attendance/relationship, lựa chọn phục hồi buổi tối, và sáng hôm sau phản ứng theo state. Dùng checkpoint/save sandbox riêng. Playtest với tối thiểu năm người mới; ghi thời lượng, mức hiểu trade-off, nguyên nhân hậu quả và ý muốn chơi tiếp. Thẻ transition qua đêm phải preview nguồn/đích/hậu quả và có hủy trước commit.

## T-003 — Author thời điểm sự kiện và làm transition event-driven

Sau khi có timestamp cho các event đích cần montage, triển khai preview → montage → summary theo `story/GAMEPLAY_TRANSITIONS_PLAN.md`. Transition đi tới đúng thời điểm sự kiện kế tiếp; số ngày/giờ trôi được tính từ mốc nguồn và đích. “30 ngày” chỉ là ví dụ, không đặt giới hạn hoặc mốc dừng giả. Không vượt qua choice/gameplay event ở giữa; tóm tắt các biến cố không cần tương tác. Kiểm tra một mốc ngắn và một mốc dài; nếu có sự kiện thật cách hơn 30 ngày thì phải tới đúng sự kiện đó, không chia/dừng giả tạo. Kiểm tra hủy trước commit, load/retry sau commit và bảo đảm event không lặp. Không giả định M10–M45 đã có timestamp chỉ vì bridge text tồn tại.

## T-004 — Mở rộng M10–M45

Chỉ sau khi T-001 audit và T-002 slice đạt gate, nối các mission theo `story/PRODUCTION_FLOW_BASELINE.md`. Từng transition phải có dữ liệu nguồn/đích/thời lượng và checkpoint; từng activity phải qua rule gameplay systems. Nghiệm thu cả tuyến trước khi gọi campaign hoàn chỉnh.
