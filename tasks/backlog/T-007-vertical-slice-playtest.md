# T-007 — Vertical slice và playtest

## TLDR

Tạo slice cô lập 8–12 phút để kiểm tra một lựa chọn thời gian có hậu quả dễ hiểu, có recovery phải trả giá, và story phản ứng khác nhau. Dùng các gap đã ưu tiên trong audit M01–M09 và T-001 Năm 1. Task slice từng mang ID T-002 rồi T-005; nay là T-007 để nhường T-005 cho Năm 5.

## Sơ đồ ASCII

```text
07:20 — Long rủ chơi
       │
       ├── Đi học ──────────────► giữ attendance, lỡ trận chơi
       │
       └── Chơi tới 08:10 ──────► có thời gian với Long, vắng học
                                      │
                         Buổi tối: recovery hay giữ hẹn?
                              ┌───────┴────────┐
                              ▼                ▼
                         Học bù          Gặp/chơi với Long
                       giảm rủi ro        giữ relationship
                              └───────┬────────┘
                                      ▼
                    Sáng hôm sau: state/story phản ứng
                                      ▼
                        Playtest tối thiểu 5 người
```

## Definition of Done

- [ ] Slice chạy 8–12 phút, tách khỏi mission ID, course graph, ending và save campaign.
- [ ] Checkpoint/save sandbox khôi phục đúng state và dọn modal, coroutine, reservation cũ.
- [ ] Choice báo trước trade-off; hậu quả attendance/knowledge/relationship truy được về choice.
- [ ] Recovery nói rõ chi phí trước commit; hai nhánh dẫn tới sáng hôm sau với phản ứng/state tương ứng.
- [ ] Playtest tối thiểu 5 người mới; ít nhất 4/5 hiểu hậu quả, trade-off và chi phí recovery; ít nhất 3/5 muốn chơi tiếp.
- [ ] Trung vị thời lượng 8–12 phút; không softlock hoặc sai state sau retry/load; ghi phản hồi và quyết định sửa/đạt.
