# Task tracker — Phòng 404

Trang này là nơi duy nhất để xác định **việc kế tiếp**. Tài liệu thiết kế nói game nên trở thành gì; tài liệu implementation/review nói runtime nào đã được kiểm tra; task tracker chuyển chúng thành thứ tự công việc.

## Việc kế tiếp

| ID | Task | Phạm vi | Trạng thái |
|---|---|---|---|
| T-001 | Audit M01–M09 theo design mới | `Production_Opening` | **Kế tiếp** |
| T-002 | Làm vertical slice 8–12 phút và playtest | Fixture cô lập, production | Chờ T-001 |
| T-003 | Author thời điểm sự kiện và làm transition event-driven | Production campaign | Chờ T-001, T-002 |
| T-004 | Nối tiếp M10–M45 theo baseline | Production campaign | Chờ các gate trước |

Mở [T-001](active/T-001-audit-m01-m09.md) để bắt đầu; xem [T-002](backlog/T-002-vertical-slice-playtest.md), [T-003](backlog/T-003-event-driven-transitions.md) và [T-004](backlog/T-004-expand-m10-m45.md).

## Nguồn chuẩn và phạm vi

- **Trải nghiệm, scope và agency:** `story/GAME_DESIGN_PILLARS.md`, `story/GAMEPLAY_SYSTEMS_PLAN.md`.
- **Canon/missions:** `story/PRODUCTION_FLOW_BASELINE.md`.
- **Transition:** `story/GAMEPLAY_TRANSITIONS_PLAN.md`.
- **Gates/playtest:** `story/PLAYTEST_AND_ACCEPTANCE_PLAN.md`.
- **Runtime đã triển khai:** `story/PRODUCTION_CAMPAIGN_IMPLEMENTATION.md` và review mới nhất theo phạm vi mission.

Thứ tự này khớp production gates: audit M01–M09 → slice và playtest → mở rộng M10–M45 → nghiệm thu tuyến đầy đủ.

## Tình trạng đã xác minh trong tài liệu

- Production campaign có 45 mission/212 event trong model. M01–M09 đã được nối vào scene riêng `Production_Opening`; implementation report ghi nhận 819 PlayMode checks. Đây là bằng chứng runtime prototype, không phải bằng chứng trải nghiệm đã đạt các design pillar.
- M10–M45 chưa được nghiệm thu thành tuyến runtime hoàn chỉnh.
- `Room404_MVP` chạy flow Năm 1 v2 riêng. `NEXT_STEPS_HANDOVER.md` và `MANUAL_CHECKPOINTS.md` chỉ áp dụng cho scene/runtime đó; F8 không phải checkpoint production mặc định.
- `30 ngày` là ví dụ trong mockup, không phải giới hạn transition. Campaign M10–M45 chưa có timestamp đầy đủ; cần author mốc event đích rồi mới có thể hiển thị đúng ngày/giờ và thời lượng trôi.

## Format task

Mỗi file task chỉ có ba phần: **TLDR**, **Sơ đồ ASCII**, **Definition of Done**. Cập nhật trạng thái ở bảng tracker; giữ evidence/kết quả ở artifact review riêng khi task hoàn thành.
