# T-003 — Author mốc sự kiện và làm transition event-driven

## TLDR

Transition phải tới đúng thời điểm của sự kiện đích đã author. UI tính thời gian đã trôi từ mốc nguồn đến mốc sự kiện; “30 ngày” chỉ là ví dụ, không phải giới hạn hay mốc dừng.

## Sơ đồ

```text
Mốc hiện tại
    │
    ▼
Sự kiện kế tiếp cần người chơi chú ý/tương tác
    │  timestamp đã author
    ▼
Preview: nguồn → sự kiện đích + thời lượng tính được
    │
    ├── Hủy ─────────► state giữ nguyên
    │
    └── Tiếp tục ────► montage các biến cố không cần tương tác
                           │
                           ▼
                Commit đúng event + summary/checkpoint
```

## Definition of Done

- [ ] Mỗi transition cần time-skip có nguồn, event đích và timestamp/nhãn thời gian rõ trong dữ liệu hoặc tài liệu.
- [ ] Preview nêu tên/mốc event đích, ngày/giờ và khoảng thời gian tính từ nguồn; không dùng thời lượng nhảy mặc định.
- [ ] Không tự dừng/chia tại 30 ngày; nếu event thật ở xa hơn, transition vẫn tới đúng event đích.
- [ ] Không bỏ qua choice/gameplay event giữa hai mốc; montage/summary thể hiện biến cố không cần tương tác.
- [ ] Hủy trước commit không đổi state; load/retry sau commit không lặp event/effect.
- [ ] Có kiểm tra một khoảng ngắn và một khoảng dài theo timestamp event; ghi rõ evidence runtime.
