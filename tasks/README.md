# Task tracker — Phòng 404

> **Reset 27/10/2026:** Gameplay, scene `Room404_MVP` và `Production_Opening` cùng editor cũ đã được đưa ra khỏi project theo yêu cầu làm lại. Các trạng thái và mô tả runtime bên dưới là lịch sử, không phản ánh project hiện tại. T-001 cần được lập lại theo UI/asset mới trước khi triển khai.

Trang này là nơi duy nhất để xác định **việc kế tiếp**. Tài liệu thiết kế nói game nên trở thành gì; tài liệu implementation/review nói runtime nào đã được kiểm tra; task tracker chuyển chúng thành thứ tự công việc.

## Việc kế tiếp

| ID | Task | Phạm vi | Trạng thái |
|---|---|---|---|
| T-001 | Hoàn thiện flow gameplay Năm 1 | `Room404_MVP` | **Đang triển khai và rà flow — chưa nghiệm thu thủ công** |
| T-002 | Hoàn thiện flow gameplay Năm 2 | M09–M16, bàn giao Năm 1 → Năm 3 | Chờ T-001 và gate T-007 |
| T-003 | Hoàn thiện flow gameplay Năm 3 | M17–M23, bridge M24 | Chờ T-002, T-006 và gate T-007 |
| T-006 | Author thời điểm sự kiện và làm transition event-driven | Production campaign | Chờ T-007 |
| T-004 | Hoàn thiện flow gameplay Năm 4 | M24–M28 → M29; học/tiền/môn nợ/đồ án nhỏ, chưa tốt nghiệp | Chờ T-002, T-003, T-006 và gate T-007 |
| T-005 | Hoàn thiện flow gameplay Năm 5 | M29–M35-A; áp lực sinh viên, internship/repair; bàn giao môn cuối Năm 6 | Chờ T-004, T-006 và gate T-007 |
| T-007 | Vertical slice 8–12 phút và playtest | Fixture cô lập, production | Chờ T-001 |

Mở [T-001](active/T-001-year-one-gameplay-plan.md) để bắt đầu task kế tiếp; xem thêm [T-002 — Năm 2](backlog/T-002-year-two-gameplay-plan.md), [T-003 — Năm 3](backlog/T-003-year-three-gameplay-plan.md), [T-006 — transition](backlog/T-006-event-driven-transitions.md), [T-004 — Năm 4](backlog/T-004-year-four-gameplay-plan.md), [T-005 — Năm 5](backlog/T-005-year-five-gameplay-plan.md) và [T-007 — slice/playtest](backlog/T-007-vertical-slice-playtest.md).
Audit production M01–M09 đã hoàn thành và được lưu riêng: [task](done/audit-m01-m09.md) · [kết quả](done/audit-m01-m09-results.md).

## Nguồn chuẩn và phạm vi

- **Trải nghiệm, scope và agency:** `story/GAME_DESIGN_PILLARS.md`, `story/GAMEPLAY_SYSTEMS_PLAN.md`.
- **Canon/missions:** `story/PRODUCTION_FLOW_BASELINE.md`.
- **Transition:** `story/GAMEPLAY_TRANSITIONS_PLAN.md`.
- **Gates/playtest:** `story/PLAYTEST_AND_ACCEPTANCE_PLAN.md`.
- **Runtime đã triển khai:** `story/PRODUCTION_CAMPAIGN_IMPLEMENTATION.md` và review mới nhất theo phạm vi mission.

Thứ tự triển khai: T-001 Năm 1 → gate T-007 slice/playtest → T-002 Năm 2 → T-003 Năm 3; T-006 timestamp hỗ trợ các năm → T-004 Năm 4 → T-005 Năm 5. Năm 6 theo [payoff plan](../story/YEAR_SIX_PAYOFF_PLAN.md): M35-B/M36 → M37–M45; việc làm chỉ epilogue sau credits. Triển khai phần campaign còn lại cần task riêng; T-004 không còn là task mở rộng M10–M45. T-002 kế thừa Năm 1 đã triển khai; task card Năm 2 hiện là plan, chưa phải bằng chứng runtime.

## Tình trạng đã xác minh trong tài liệu

- Production campaign có 45 mission/212 event trong model. M01–M09 đã được nối vào scene riêng `Production_Opening`; implementation report ghi nhận 819 PlayMode checks. Đây là bằng chứng runtime prototype, không phải bằng chứng trải nghiệm đã đạt các design pillar.
- M10–M45 chưa được nghiệm thu thành tuyến runtime hoàn chỉnh.
- `Room404_MVP` chạy flow Năm 1 v2 riêng. `NEXT_STEPS_HANDOVER.md` và `MANUAL_CHECKPOINTS.md` chỉ áp dụng cho scene/runtime đó; F8 không phải checkpoint production mặc định.
- `30 ngày` là ví dụ trong mockup, không phải giới hạn transition. Campaign M10–M45 chưa có timestamp đầy đủ; cần author mốc event đích rồi mới có thể hiển thị đúng ngày/giờ và thời lượng trôi.

## Format task

Mỗi file task chỉ có ba phần: **TLDR**, **Sơ đồ ASCII**, **Definition of Done**. Cập nhật trạng thái ở bảng tracker; giữ evidence/kết quả ở artifact review riêng khi task hoàn thành.
