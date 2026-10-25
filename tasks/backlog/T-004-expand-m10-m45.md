# T-004 — Mở rộng M10–M45 theo baseline

## TLDR

Sau khi T-001 audit và T-002 slice đạt gate, nối gameplay M10–M45 theo mission baseline, systems plan và transition data đã author.

## Sơ đồ

```text
M01–M09 audit đạt ─► Slice + playtest đạt ─► M10–M45 gameplay/adapters
                                                │
                           ┌────────────────────┼─────────────────────┐
                           ▼                    ▼                     ▼
                     Mission actions       Event timestamps       Checkpoint/retry
                           └────────────────────┼─────────────────────┘
                                                ▼
                                  Full-route acceptance
```

## Definition of Done

- [ ] M10–M45 triển khai theo đúng thứ tự, điều kiện và ending của `PRODUCTION_FLOW_BASELINE.md`.
- [ ] Mỗi activity có hành động/kỹ năng/quyết định và feedback phù hợp; không thêm minigame chỉ để đổi giao diện.
- [ ] Mỗi transition có mốc event đích, thời gian trôi, continuity và checkpoint/retry theo transition plan.
- [ ] Choice quan trọng làm đổi state, cách giải quyết hoặc phản ứng story; recovery không khóa campaign vô lý.
- [ ] Save/load/retry không bỏ hoặc commit lặp event, effect, tiền, tín chỉ hay thay đổi phòng.
- [ ] Chạy nghiệm thu tuyến campaign hoàn chỉnh; ghi playtime, lỗi continuity, academic ledger, room occupancy và time-skip; chỉ gọi full campaign đạt khi các cổng này có bằng chứng.
