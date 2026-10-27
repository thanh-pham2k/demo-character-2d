# Bộ phòng 404 — đang kiểm tra ghép

- `batch-01`: nền, cửa sổ, cửa, công tắc; nền tường vẫn có phản chiếu cửa sổ trên sàn dù cửa sổ được tách.
- `batch-02`: khung gác xép, nệm, thang, lan can. `*-alpha.png` là ảnh do imagegen tách nền, không phải alpha được xác nhận sạch hoàn toàn.
- `batch-03`: bàn, ghế, laptop, đèn; `batch-04`: đồ cá nhân.
- `preview.html`: ghép thử các ảnh alpha thật với Luồn, không phải scene Unity. Đã xem trong browser: nệm không khớp khung về phối cảnh, giường bị cắt ở hai bên và lối đi/cửa chưa xác nhận; không dùng bố cục này làm thông số sản xuất. Các lớp foreground chưa được căn khớp. Cần chỉnh asset và ghép lại trước khi chốt PPU, pivot, sorting.

Các sheet/crop được giữ nguyên, không đưa vào Unity. Chưa chạy Unity, Play Mode, build hoặc kiểm tra gameplay.
